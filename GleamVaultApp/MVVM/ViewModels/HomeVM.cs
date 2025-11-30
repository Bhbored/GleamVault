using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GleamVault.MVVM.Views.Popups;
using GleamVault.TestData;
using PropertyChanged;
using Shared.Models;
using Shared.Models.Enums;
using Syncfusion.Maui.DataSource.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GleamVault.MVVM.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class HomeVM : INotifyPropertyChanged
    {

        #region Fields
        private int _currentIndex = 0;
        private ObservableCollection<Product> _allProducts = new();
        private ObservableCollection<Product> _filteredProducts = new();
        private ObservableCollection<Product> _cartItems = new();
        private ObservableCollection<Category> _allCategories = new();
        private float _cartSubtotal;
        private float _cartTotal;
        private float _discountAmount;
        private bool _hasItemsInCart;
        private Category selectedCategory = new();
        private ObservableCollection<HallmarkType> allHallmarks = new();
        private readonly Dictionary<Product, Product> _mapInventoryToCart = new();
        private readonly Dictionary<Product, Product> _mapCartToInventory = new();
        private HallmarkType? selectedHallmark;
        private int sortIndex = 2;
        private ObservableCollection<Customer> customers = new();
        private Customer selectedCustomer = new Customer();
        private bool useSavedCustomer = false;
        private string customerSearch = string.Empty;
        private DateTime completedAt = DateTime.Now;
        private int transactionTypeIndex = 0;
        private bool shimmerLoading = true;
        private bool shimmerNotLoading = false;
        private readonly ObservableCollection<object> _shimmerItems = new();
        #endregion


        #region Properties
        public ObservableCollection<object> ShimmerItems
        {
            get => _shimmerItems;
        }

        public bool ShimmerNotLoading
        {
            get => shimmerNotLoading;
            set
            {
                shimmerNotLoading = value;
                OnPropertyChanged();
            }
        }
        public bool ShimmerLoading
        {
            get => shimmerLoading;
            set
            {
                shimmerLoading = value;
                OnPropertyChanged();

            }
        }
        public string CustomerSearch
        {
            get => customerSearch;
            set { if (customerSearch == value) return; customerSearch = value; OnPropertyChanged(); }
        }
        public int TransactionTypeIndex
        {
            get => transactionTypeIndex;
            set
            {
                transactionTypeIndex = value;
            }
        }
        public DateTime CompletedAt
        {
            get => completedAt;
            set
            {
                completedAt = value;
                OnPropertyChanged();
            }
        }
        public bool UseSavedCustomer
        {
            get => useSavedCustomer;
            set
            {
                useSavedCustomer = value;
                OnPropertyChanged();
            }
        }
        public Customer SelectedCustomer
        {
            get => selectedCustomer;
            set
            {
                if (selectedCustomer == value) return; selectedCustomer = value; OnPropertyChanged();
            }
        }
        public IList<object> SelectedProducts { get; set; } = [];
        public int SortIndex
        {
            get => sortIndex;
            set
            {
                if (sortIndex == value) return;
                sortIndex = value;
                OnPropertyChanged();
                SortNow();
            }
        }
        public ObservableCollection<Customer> Customers
        {
            get => customers;
            set
            {
                customers = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<HallmarkType> AllHallmarks
        {
            get => allHallmarks;
            set
            {
                allHallmarks = value;
                OnPropertyChanged();
            }
        }
        public HallmarkType? SelectedHallmark
        {
            get => selectedHallmark;
            set
            {
                if (selectedHallmark == value) return;
                selectedHallmark = value;
                OnPropertyChanged();
                FilterProductsByCategory();
            }
        }
        public Category SelectedCategory
        {
            get => selectedCategory;
            set
            {
                if (selectedCategory == value) return;
                selectedCategory = value;
                OnPropertyChanged();
                FilterProductsByCategory();
            }
        }
        public ObservableCollection<Product> AllProducts
        {
            get => _allProducts;
            set { if (_allProducts == value) return; _allProducts = value; OnPropertyChanged(); }
        }

        public ObservableCollection<Product> FilteredProducts
        {
            get => _filteredProducts;
            set { if (_filteredProducts == value) return; _filteredProducts = value; OnPropertyChanged(); }
        }

        public ObservableCollection<Product> CartItems
        {
            get => _cartItems;
            set { if (_cartItems == value) return; _cartItems = value; OnPropertyChanged(); RefreshCart(); }
        }

        public ObservableCollection<Category> AllCategories
        {
            get => _allCategories;
            set { if (_allCategories == value) return; _allCategories = value; OnPropertyChanged(); }
        }

        public float CartSubtotal
        {
            get => _cartSubtotal;
            set { if (_cartSubtotal == value) return; _cartSubtotal = value; OnPropertyChanged(); }
        }

        public float CartTotal
        {
            get => _cartTotal;
            set { if (_cartTotal == value) return; _cartTotal = value; OnPropertyChanged(); }
        }

        public float DiscountAmount
        {
            get => _discountAmount;
            set
            {
                var v = float.IsNaN(value) ? 0 : Math.Max(0, value);
                if (Math.Abs(_discountAmount - v) < 0.0001f) return;
                _discountAmount = v; OnPropertyChanged();
                RecalculateTotals();
            }
        }

        public bool HasItemsInCart
        {
            get => _hasItemsInCart;
            set { if (_hasItemsInCart == value) return; _hasItemsInCart = value; OnPropertyChanged(); }
        }
        #endregion

        #region Commands
        public ICommand AddToCartCommand => new Command<Product>(async p => await AddToCartAsync(p));
        public ICommand RemoveFromCartCommand => new Command<Product>(async p => await RemoveFromCartAsync(p));
        public ICommand IncreaseQuantityCommand => new Command<Product>(async p => await IncreaseQuantityAsync(p));
        public ICommand DecreaseQuantityCommand => new Command<Product>(async p => await DecreaseQuantityAsync(p));
        public ICommand ClearCartCommand => new Command(async () => await ClearCartAsync());
        public ICommand CompleteSaleCommand => new Command(async () => await CompleteSaleAsync());
        public ICommand LoadMoreCommand => new Command(() =>  LoadMore());

        #endregion

        #region Tasks
        public async Task AddToCartAsync(Product product)
        {
            if (product == null) { await Task.CompletedTask; return; }
            var invIndex = AllProducts.IndexOf(product);
            if (invIndex < 0) { await Task.CompletedTask; return; }
            if (product.CurrentStock <= 0) { await Task.CompletedTask; return; }

            if (_mapInventoryToCart.TryGetValue(product, out var cartCopy))
            {
                cartCopy.CurrentStock += 1;
            }
            else
            {
                cartCopy = CloneForCart(product);
                cartCopy.CurrentStock = 1;
                CartItems.Add(cartCopy);
                _mapInventoryToCart[product] = cartCopy;
                _mapCartToInventory[cartCopy] = product;
            }

            product.CurrentStock -= 1;
            RefreshCart();
            await Task.CompletedTask;
        }

        public async Task RemoveFromCartAsync(Product product)
        {
            if (product == null) { await Task.CompletedTask; return; }

            Product inv = product;
            Product cart = product;

            if (_mapCartToInventory.TryGetValue(product, out var invFromCart))
            {
                inv = invFromCart;
            }
            else if (_mapInventoryToCart.TryGetValue(product, out var cartFromInv))
            {
                cart = cartFromInv;
            }
            else
            {
                await Task.CompletedTask; return;
            }

            var invIndex = AllProducts.IndexOf(inv);
            if (invIndex >= 0)
            {
                inv.CurrentStock += cart.CurrentStock;
            }

            _mapInventoryToCart.Remove(inv);
            _mapCartToInventory.Remove(cart);
            CartItems.Remove(cart);

            RefreshCart();
            await Task.CompletedTask;
        }

        public async Task IncreaseQuantityAsync(Product product)
        {
            if (product == null) { await Task.CompletedTask; return; }

            Product inv = product;
            Product cart = product;

            if (_mapCartToInventory.TryGetValue(product, out var invFromCart))
            {
                inv = invFromCart;
            }
            else if (_mapInventoryToCart.TryGetValue(product, out var cartFromInv))
            {
                cart = cartFromInv;
            }
            else
            {
                await AddToCartAsync(product);
                return;
            }

            var invIndex = AllProducts.IndexOf(inv);
            if (invIndex < 0) { await Task.CompletedTask; return; }
            if (inv.CurrentStock <= 0) { await Task.CompletedTask; return; }

            cart.CurrentStock += 1;
            inv.CurrentStock -= 1;

            RefreshCart();
            await Task.CompletedTask;
        }

        public async Task DecreaseQuantityAsync(Product product)
        {
            if (product == null) { await Task.CompletedTask; return; }

            Product inv = product;
            Product cart = product;

            if (_mapCartToInventory.TryGetValue(product, out var invFromCart))
            {
                inv = invFromCart;
            }
            else if (_mapInventoryToCart.TryGetValue(product, out var cartFromInv))
            {
                cart = cartFromInv;
            }
            else
            {
                await Task.CompletedTask; return;
            }

            if (cart.CurrentStock > 1)
            {
                cart.CurrentStock -= 1;
                inv.CurrentStock += 1;

                RefreshCart();
            }
            else
            {
                await RemoveFromCartAsync(cart);
            }

            await Task.CompletedTask;
        }

        public async Task ClearCartAsync()
        {
            foreach (var cart in CartItems.ToList())
            {
                if (_mapCartToInventory.TryGetValue(cart, out var inv))
                {
                    inv.CurrentStock += cart.CurrentStock;
                }
            }

            CartItems.Clear();
            _mapInventoryToCart.Clear();
            _mapCartToInventory.Clear();
            DiscountAmount = 0;
            RefreshCart();
            await Task.CompletedTask;
        }

        public async Task CompleteSaleAsync()
        {
            if (!HasItemsInCart) return;
            await Shell.Current.ShowPopupAsync(new ReceiptPopup(this));
            await Task.CompletedTask;
        }
        public async Task CompleteSaleConfirmationAsync()
        {

            if (!HasItemsInCart || CartItems.Count == 0)
            {
                await Shell.Current.DisplayAlert("🛒 Cart is empty.", "Your cart has no items.", "OK"); return;
            }

            if (UseSavedCustomer && SelectedCustomer is null)
            {
                await Shell.Current.DisplayAlert("🤔 No Customer Selected", "Please select a saved customer, or turn off the toggle.", "OK");
                return;
            }
            if (HasItemsInCart && CartItems.Count > 0 && UseSavedCustomer && SelectedCustomer != null)
            {
                Transaction tx = new Transaction
                {
                    CreatedAt = DateTime.Now,
                    Channel = TransactionTypeIndex == 0 ? SaleChannel.InStore : SaleChannel.Online,
                    SubTotalAmount = CartSubtotal,
                    DiscountValue = DiscountAmount,
                    Type = TransactionType.Sell,
                    TotalAmount = CartTotal,
                    CustomerId = SelectedCustomer.Id,
                    Customer = SelectedCustomer,

                };
                tx.Items = CartItems.Select(p => new TransactionItem
                {
                    TransactionId = tx.Id,
                    ProductId = p.Id,
                    Name = p.Name,
                    Quantity = p.CurrentStock,
                    UnitPrice = GetEffectivePrice(p),
                    Sku = p.Sku,
                    Description = p.Description,
                    CategoryId = p.CategoryId,
                    Category = p.Category,
                    ImageUrl = p.ImageUrl,
                    Hallmark = p.Hallmark,
                    WeightUnit = p.WeightUnit,
                    Weight = p.Weight,
                    OfferPrice = p.OfferPrice,
                    IsUniquePiece = p.IsUniquePiece,
                    UnitCost = p.UnitCost,
                }).ToList();
                TestTransactions.Transactions.Add(tx);
            }

            if (HasItemsInCart && CartItems.Count > 0 && !UseSavedCustomer)
            {
                var tx = new Transaction
                {
                    //need real api to adjust stock
                    CreatedAt = DateTime.Now,
                    Channel = TransactionTypeIndex == 0 ? SaleChannel.InStore : SaleChannel.Online,
                    SubTotalAmount = CartSubtotal,
                    DiscountValue = DiscountAmount,
                    Type = TransactionType.Sell,
                    TotalAmount = CartTotal,
                };
                tx.Items = CartItems.Select(p => new TransactionItem
                {
                    TransactionId = tx.Id,
                    ProductId = p.Id,
                    Name = p.Name,
                    Quantity = p.CurrentStock,
                    UnitPrice = GetEffectivePrice(p),
                    Sku = p.Sku,
                    Description = p.Description,
                    CategoryId = p.CategoryId,
                    Category = p.Category,
                    ImageUrl = p.ImageUrl,
                    Hallmark = p.Hallmark,
                    WeightUnit = p.WeightUnit,
                    Weight = p.Weight,
                    OfferPrice = p.OfferPrice,
                    IsUniquePiece = p.IsUniquePiece,
                    UnitCost = p.UnitCost,
                }).ToList();


                TestTransactions.Transactions.Add(tx);
            }
            await Shell.Current.DisplayAlert("Transaction saved.", "OK", "Success");

        }

        #endregion

        #region methods

        private float GetEffectivePrice(Product product)
        {
            if (product == null) return 0f;
            return product.OfferPrice > 0 ? product.OfferPrice : product.UnitPrice;
        }

        private void RecalculateTotals()
        {
            CartSubtotal = CartItems.Sum(p => GetEffectivePrice(p) * p.CurrentStock);
            CartTotal = CartSubtotal - DiscountAmount;
            if (CartTotal < 0) CartTotal = 0;
        }

        private void RefreshCart()
        {
            HasItemsInCart = CartItems.Count > 0;
            RecalculateTotals();
            OnPropertyChanged(nameof(CartItems));
            OnPropertyChanged(nameof(CartSubtotal));
            OnPropertyChanged(nameof(CartTotal));
            OnPropertyChanged(nameof(HasItemsInCart));
            OnPropertyChanged(nameof(DiscountAmount));
        }

        private Product CloneForCart(Product source)
        {
            var clone = Activator.CreateInstance<Product>();
            var props = typeof(Product).GetProperties().Where(pr => pr.CanRead && pr.CanWrite);
            foreach (var pr in props)
            {
                if (string.Equals(pr.Name, nameof(source.CurrentStock), StringComparison.Ordinal)) continue;
                pr.SetValue(clone, pr.GetValue(source));
            }
            return clone;
        }
        public void FilterProducts()
        {
            if (SelectedProducts.Count == 0)
            {
                FilteredProducts = new ObservableCollection<Product>(AllProducts);
            }
            else
            {
                var filteredList = new List<Product>();

                foreach (var selectedItem in SelectedProducts)
                {
                    if (selectedItem is Product selectedProduct)
                    {
                        var matchingProduct = AllProducts.FirstOrDefault(p => p.Name == selectedProduct.Name);
                        if (matchingProduct != null)
                        {
                            filteredList.Add(matchingProduct);
                        }
                    }
                }

                FilteredProducts = new ObservableCollection<Product>(filteredList);
            }
        }
        public void FilterProductsByCategory()
        {
            if (AllProducts is null || AllProducts.Count == 0)
            {
                ReplaceCollection(FilteredProducts, Array.Empty<Product>());
                return;
            }

            IEnumerable<Product> query = AllProducts;

            if (SelectedCategory != null)
            {
                var catId = SelectedCategory.Id;
                var catName = (SelectedCategory.Name ?? string.Empty).Trim();

                query = query.Where(p =>
                    p != null && (
                        p.CategoryId == catId ||
                        (p.Category?.Id == catId) ||
                        (!string.IsNullOrWhiteSpace(catName) &&
                         string.Equals((p.Category?.Name ?? string.Empty).Trim(), catName, StringComparison.OrdinalIgnoreCase))
                    ));
            }

            if (SelectedHallmark is HallmarkType hm)
            {
                query = query.Where(p => p != null && p.Hallmark == hm);
            }

            var results = query.ToList();
            ReplaceCollection(FilteredProducts, results);
        }

        public void FilterProductsByHallmark()
        {
            if (AllProducts is null || AllProducts.Count == 0)
            {
                ReplaceCollection(FilteredProducts, Array.Empty<Product>());
                return;
            }

            IEnumerable<Product> query = AllProducts;
            if (SelectedHallmark is HallmarkType hm)
            {
                query = query.Where(p => p != null && p.Hallmark == hm);
            }

            var results = query.ToList();
            ReplaceCollection(FilteredProducts, results);
        }


        private static void ReplaceCollection(ObservableCollection<Product> target, IEnumerable<Product> source)
        {
            var list = source is IList<Product> l ? l : source.ToList();
            MainThread.BeginInvokeOnMainThread(() =>
            {
                target.Clear();
                for (int i = 0; i < list.Count; i++)
                    target.Add(list[i]);
            });
        }
        public void GetHallmarks()
        {
            AllHallmarks.Clear();
            foreach (var v in Enum.GetValues<HallmarkType>())
                AllHallmarks.Add(v);

        }
        public void SortNow()
        {
            if (FilteredProducts == null || FilteredProducts.Count == 0) return;
            List<Product> sortedList = SortIndex switch
            {
                0 => FilteredProducts.OrderBy(p => p.Name).ToList(),
                1 => FilteredProducts.OrderByDescending(p => p.Name).ToList(),
                2 => FilteredProducts.ToList(),
                _ => FilteredProducts.ToList(),
            };
            ReplaceCollection(FilteredProducts, sortedList);
        }


        #endregion

        public void ClearALL()
        {
            AllProducts.Clear();
            AllCategories.Clear();
            FilteredProducts.Clear();
            CartItems.Clear();
            _mapCartToInventory.Clear();
            _mapInventoryToCart.Clear();
            AllHallmarks.Clear();
            DiscountAmount = 0;
            SelectedCategory = new Category();
            Customers.Clear();
            SelectedCustomer = new Customer();
            UseSavedCustomer = false;
            CompletedAt = DateTime.Now;
            CustomerSearch = string.Empty;
            TransactionTypeIndex = 0;
            ShimmerLoading = true;
            ShimmerNotLoading = false;
            _currentIndex = 0;
            _shimmerItems.Clear();
            for (int i = 0; i < 8; i++)
            {
                _shimmerItems.Add(new object());
            }
            OnPropertyChanged(nameof(ShimmerItems));

        }
        public async Task LoadData()
        {
            TestProducts.GetProducts().ForEach(p => AllProducts.Add(p));
            await Task.Delay(500);
            LoadMore();
        }

        public void LoadMore()
        {
            var nextItems = AllProducts.Skip(_currentIndex).Take(12);

            foreach (var item in nextItems)
            {
                FilteredProducts.Add(item);
            }
            _currentIndex += 12;
        }
        public async Task LoadDataAsync()
        {
            ClearALL();
            RefreshCart();
            GetHallmarks();
            await LoadData();
            TestProducts.GetCategories().ForEach(c => AllCategories.Add(c));
            TestProducts.GetCustomers().ForEach(cu => Customers.Add(cu));
            await Task.Delay(3000);
            ShimmerLoading = false;
            ShimmerNotLoading = true;

        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}