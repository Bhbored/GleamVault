using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GleamVault.TestData;
using PropertyChanged;
using Shared.Models;
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
        #region backing fields
        private ObservableCollection<Product> _allProducts = new();
        private ObservableCollection<Product> _filteredProducts = new();
        private ObservableCollection<Product> _cartItems = new();
        private float _cartSubtotal;
        private float _cartTotal;
        private bool _hasItemsInCart;
        private float _discountAmount;
        private ObservableCollection<Category> _allCategories = new();
        #endregion

        #region properties (explicit + OnPropertyChanged)
        public ObservableCollection<Product> AllProducts
        {
            get => _allProducts;
            set
            {
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Product> FilteredProducts
        {
            get => _filteredProducts;
            set
            {
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Product> CartItems
        {
            get => _cartItems;
            set
            {
                OnPropertyChanged();
                OnPropertyChanged(nameof(CartSubtotal));
                OnPropertyChanged(nameof(CartTotal));
                OnPropertyChanged(nameof(HasItemsInCart));
                OnPropertyChanged(nameof(AllProducts));
            }
        }

        public float CartSubtotal
        {
            get => _cartSubtotal;
            set
            {
                OnPropertyChanged();
            }
        }

        public float CartTotal
        {
            get => _cartTotal;
            set
            {
                OnPropertyChanged();
            }
        }

        public bool HasItemsInCart
        {
            get => _hasItemsInCart;
            set
            {
                OnPropertyChanged();
            }
        }

        public float DiscountAmount
        {
            get => _discountAmount;
            set
            {
                OnPropertyChanged();
                OnPropertyChanged(nameof(CartTotal));
            }
        }

        public ObservableCollection<Category> AllCategories
        {
            get => _allCategories;
            set
            {
                OnPropertyChanged();
            }
        }
        #endregion

        #region commands

        public ICommand AddToCartCommand => new Command<Product>(async (product) => await AddToCartAsync(product));
        public ICommand RemoveFromCartCommand => new Command<Product>(async (product) => await RemoveFromCartAsync(product));
        public ICommand IncreaseQuantityCommand => new Command<Product>(async (product) => await IncreaseQuantityAsync(product));
        public ICommand DecreaseQuantityCommand => new Command<Product>(async (product) => await DecreaseQuantityAsync(product));
        public ICommand ClearCartCommand => new Command(async () => await ClearCartAsync());

        #endregion
        #region tasks


        public async Task AddToCartAsync(Product product)
        {
            if (product is null) return;

            var index = AllProducts.IndexOf(product);
            if (index < 0) return;

            if (product.CurrentStock > 0)
            {
                if (!CartItems.Contains(product))
                {
                    CartItems.Add(product);
                    AllProducts[index].CurrentStock--;
                }
                else
                {
                    await IncreaseQuantityAsync(product);
                }

                HasItemsInCart = true;
                RefreshCart();
            }
        }

        public async Task RemoveFromCartAsync(Product product)
        {
            if (product is null) { await Task.CompletedTask; return; }

            var index = AllProducts.IndexOf(product);
            var cartIndex = CartItems.IndexOf(product);
            if (index < 0 || cartIndex < 0) { await Task.CompletedTask; return; }

            var stockToReturn = CartItems[cartIndex].CurrentStock;
            AllProducts[index].CurrentStock += stockToReturn;
            CartItems.Remove(product);

            RefreshCart();
            await Task.CompletedTask;
        }

        public async Task IncreaseQuantityAsync(Product product)
        {
            if (product is null) { await Task.CompletedTask; return; }

            var cartIndex = CartItems.IndexOf(product);
            var productIndex = AllProducts.IndexOf(product);
            if (cartIndex < 0 || productIndex < 0) { await Task.CompletedTask; return; }
            if (AllProducts[productIndex].CurrentStock > 0)
            {
                AllProducts[productIndex].CurrentStock--;
                CartItems[cartIndex].CurrentStock++;
                RefreshCart();
            }

            await Task.CompletedTask;
        }

        public async Task DecreaseQuantityAsync(Product product)
        {
            if (product is null) { await Task.CompletedTask; return; }

            var cartIndex = CartItems.IndexOf(product);
            var productIndex = AllProducts.IndexOf(product);
            if (cartIndex < 0 || productIndex < 0) { await Task.CompletedTask; return; }

            if (CartItems[cartIndex].CurrentStock > 1)
            {
                AllProducts[productIndex].CurrentStock++;
                CartItems[cartIndex].CurrentStock--;
                RefreshCart();
            }
            else
            {
                await RemoveFromCartAsync(product);
            }

            await Task.CompletedTask;
        }

        public async Task ClearCartAsync()
        {
            var items = new ObservableCollection<Product>(CartItems);
            foreach (var item in items)
            {
                await RemoveFromCartAsync(item);
            }

            CartItems.Clear();
            DiscountAmount = 0;
            RefreshCart();
            await Task.CompletedTask;
        }

        public async Task CompleteSaleAsync()
        {
            if (!HasItemsInCart)
                return;

            // TODO: Implement sale completion logic
            await Task.CompletedTask;
        }

        #endregion

        #region helpers

        private void RefreshCart()
        {
            CartSubtotal = CartItems.Sum(item => item.UnitPrice * item.CurrentStock);
            CartTotal = CartSubtotal - DiscountAmount;

            HasItemsInCart = CartItems.Count > 0;

            OnPropertyChanged(nameof(CartItems));
            OnPropertyChanged(nameof(AllProducts));
            OnPropertyChanged(nameof(CartSubtotal));
            OnPropertyChanged(nameof(CartTotal));
            OnPropertyChanged(nameof(HasItemsInCart));
            OnPropertyChanged(nameof(DiscountAmount));
        }

        public async Task LoadDataAsync()
        {
            AllProducts.Clear();
            AllCategories.Clear();
            CartItems.Clear();
            RefreshCart();
            await Task.Delay(100);
            TestProducts.GetProducts().ForEach(p => AllProducts.Add(p));
            TestProducts.GetCategories().ForEach(c => AllCategories.Add(c));
        }

        #endregion

        #region INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
