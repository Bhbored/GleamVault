using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GleamVault.TestData;
using PropertyChanged;
using Shared.Models;
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
        private ObservableCollection<Product> _allProducts = new();
        private ObservableCollection<Product> _filteredProducts = new();
        private ObservableCollection<Product> _cartItems = new();
        private ObservableCollection<Category> _allCategories = new();
        private float _cartSubtotal;
        private float _cartTotal;
        private float _discountAmount;
        private bool _hasItemsInCart;
        private readonly Dictionary<Product, Product> _mapInventoryToCart = new();
        private readonly Dictionary<Product, Product> _mapCartToInventory = new();
        #endregion


        #region Properties
        public IList<object> SelectedProducts { get; set; } = [];
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
            set { if (_discountAmount == value) return; _discountAmount = value; OnPropertyChanged(); RecalculateTotals(); }
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
            await Task.CompletedTask;
        }

        #endregion

        #region methods
        private void RecalculateTotals()
        {
            CartSubtotal = CartItems.Sum(p => p.UnitPrice * p.CurrentStock);
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

        #endregion


        public async Task LoadDataAsync()
        {
            AllProducts.Clear();
            AllCategories.Clear();
            FilteredProducts.Clear();
            CartItems.Clear();
            _mapCartToInventory.Clear();
            _mapInventoryToCart.Clear();
            DiscountAmount = 0;
            RefreshCart();
            await Task.Delay(100);
            TestProducts.GetProducts().ForEach(p => AllProducts.Add(p));
            TestProducts.GetCategories().ForEach(c => AllCategories.Add(c));
            FilteredProducts = new ObservableCollection<Product>(AllProducts);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
