using CommunityToolkit.Maui.Extensions;
using GleamVault.MVVM.Views.Popups;
using GleamVault.TestData;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;
using PropertyChanged;
using Shared.Models;
using Shared.Models.Enums;
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
    public partial class DiscountVM : INotifyPropertyChanged
    {
        public DiscountVM()
        {
        }

        #region Fields
        private ObservableCollection<Product> _allProducts = new();
        private ObservableCollection<Product> _filteredProducts = new();
        private ObservableCollection<Product> _filteredProductsForPopup = new();
        private ObservableCollection<Category> _allCategories = new();
        private Category selectedCategory = new();
        private ObservableCollection<HallmarkType> allHallmarks = new();
        private HallmarkType? selectedHallmark;
        private int sortIndex = 2;
        private bool shimmerLoading = true;
        private bool shimmerNotLoading = false;
        private readonly ObservableCollection<object> _shimmerItems = new();
        private int _currentIndex = 0;
        #endregion

        #region Commands
        public ICommand MoreCommand => new Command<Product>(async (product) =>
        {
            if (product == null) return;
            await Shell.Current.ShowPopupAsync(new ProductDetailsPopup(product));
        });

        public ICommand ShowDiscountPromptCommand => new Command<Product>(async (product) =>
        {
            if (product == null) return;
            await ShowDiscountPromptForProductAsync(product);
        });

        public ICommand RemoveDiscountCommand => new Command<Product>(async (product) =>
        {
            if (product == null) return;
            await RemoveProductDiscountAsync(product);
        });

        public ICommand ShowAddDiscountPopupCommand => new Command(async () => await ShowAddDiscountPopupAsync());

        public ICommand LoadMoreCommand => new Command(() => LoadMore());
        #endregion

        #region Properties
        public int DiscountCount => AllProducts?.Count(p => p.OfferPrice > 0) ?? 0;
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

        public IList<object> SelectedProducts { get; set; } = [];
        public IList<object> PopupSelectedProducts { get; set; } = [];
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

        public ObservableCollection<Product> FilteredProductsForPopup
        {
            get => _filteredProductsForPopup;
            set { if (_filteredProductsForPopup == value) return; _filteredProductsForPopup = value; OnPropertyChanged(); }
        }

        public ObservableCollection<Category> AllCategories
        {
            get => _allCategories;
            set { if (_allCategories == value) return; _allCategories = value; OnPropertyChanged(); }
        }
        #endregion

        #region Tasks

        public async Task ShowAddDiscountPopupAsync()
        {
            // Initialize popup filtered products with all products (not just discounted)
            FilteredProductsForPopup = new ObservableCollection<Product>(AllProducts);
            PopupSelectedProducts.Clear();
            await Shell.Current.ShowPopupAsync(new AddDiscountPopup(this));
            // Refresh the main page list after popup closes to show newly discounted products
            RefreshFilteredProducts();
        }

        public void FilterPopupProducts()
        {
            if (PopupSelectedProducts.Count == 0)
            {
                FilteredProductsForPopup = new ObservableCollection<Product>(AllProducts);
            }
            else
            {
                var filteredList = new List<Product>();
                foreach (var selectedItem in PopupSelectedProducts)
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
                FilteredProductsForPopup = new ObservableCollection<Product>(filteredList);
            }
        }

        public async Task ShowDiscountPromptForProductAsync(Product product)
        {
            if (product == null) return;
            var initialValue = product.OfferPrice > 0 ? product.OfferPrice.ToString("F2") : "";
            var result = await Shell.Current.DisplayPromptAsync(
                "Enter Discount Price",
                $"Enter the discount price for {product.Name} (must be >= 50% of unit price ${product.UnitPrice:F2}):",
                "OK",
                "Cancel",
                initialValue,
                -1,
                Keyboard.Numeric);

            if (string.IsNullOrWhiteSpace(result))
            {
                return;
            }

            if (!float.TryParse(result, out float discountPrice))
            {
                await Shell.Current.DisplayAlert("⚠️ Invalid Input", "Please enter a valid number.", "OK");
                return;
            }

            if (discountPrice <= 0)
            {
                await Shell.Current.DisplayAlert("⚠️ Invalid Price", "Discount price must be greater than 0.", "OK");
                return;
            }

            var minPrice = product.UnitPrice * 0.5f;
            if (discountPrice < minPrice)
            {
                await Shell.Current.DisplayAlert("⚠️ Invalid Price", $"Discount price must be at least 50% of unit price (${minPrice:F2}).", "OK");
                return;
            }

            await UpdateProductDiscount(product, discountPrice);
        }

        public async Task UpdateProductDiscount(Product product, float discountPrice)
        {
            if (product == null) return;
            var targetProduct = AllProducts.FirstOrDefault(p => p.Id == product.Id);
            if (targetProduct != null && discountPrice > 0)
            {
                targetProduct.OfferPrice = discountPrice;
                await Task.CompletedTask;
                // Update the product in popup list if it exists there
                var popupProduct = FilteredProductsForPopup.FirstOrDefault(p => p.Id == product.Id);
                if (popupProduct != null)
                {
                    popupProduct.OfferPrice = discountPrice;
                }
                OnPropertyChanged(nameof(DiscountCount));
                // Don't refresh main page here - let it refresh when popup closes
            }
        }

        public async Task RemoveProductDiscountAsync(Product product)
        {
            if (product == null) return;
            var targetProduct = AllProducts.FirstOrDefault(p => p.Id == product.Id);
            if (targetProduct != null)
            {
                targetProduct.OfferPrice = 0;
                await Task.CompletedTask;
                // Update the product in popup list if it exists there
                var popupProduct = FilteredProductsForPopup.FirstOrDefault(p => p.Id == product.Id);
                if (popupProduct != null)
                {
                    popupProduct.OfferPrice = 0;
                }
                OnPropertyChanged(nameof(DiscountCount));
                // Don't refresh main page here - let it refresh when popup closes
            }
        }

        private void RefreshFilteredProducts()
        {
            // Remove the product from filtered list if it no longer has a discount
            var productsToRemove = FilteredProducts.Where(p => p.OfferPrice <= 0).ToList();
            foreach (var product in productsToRemove)
            {
                FilteredProducts.Remove(product);
            }

            // If we're not filtering by search, refresh the list
            if (SelectedProducts.Count == 0)
            {
                // Check if we need to add new discounted products
                var discountedProducts = AllProducts.Where(p => p.OfferPrice > 0).ToList();
                var existingIds = FilteredProducts.Select(p => p.Id).ToHashSet();
                var newProducts = discountedProducts.Where(p => !existingIds.Contains(p.Id)).ToList();

                foreach (var newProduct in newProducts)
                {
                    FilteredProducts.Add(newProduct);
                }

                // Re-sort if needed
                SortNow();
            }
            else
            {
                // If filtering, re-filter
                FilterProducts();
            }

            OnPropertyChanged(nameof(DiscountCount));
        }

        #endregion

        #region Methods
        public void FilterProducts()
        {
            if (SelectedProducts.Count == 0)
            {
                FilteredProducts.Clear();
                _currentIndex = 0;
                var discountedProducts = AllProducts.Where(p => p.OfferPrice > 0).ToList();
                var nextItems = discountedProducts.Skip(_currentIndex).Take(12);
                foreach (var item in nextItems)
                {
                    FilteredProducts.Add(item);
                }
                _currentIndex += 12;
            }
            else
            {
                var filteredList = new List<Product>();

                foreach (var selectedItem in SelectedProducts)
                {
                    if (selectedItem is Product selectedProduct)
                    {
                        var matchingProduct = AllProducts.FirstOrDefault(p => p.Name == selectedProduct.Name && p.OfferPrice > 0);
                        if (matchingProduct != null)
                        {
                            filteredList.Add(matchingProduct);
                        }
                    }
                }

                ReplaceCollection(FilteredProducts, filteredList);
                _currentIndex = filteredList.Count;
            }
        }

        public void FilterProductsByCategory()
        {
            var discountedProducts = AllProducts.Where(p => p.OfferPrice > 0);

            if (discountedProducts == null || !discountedProducts.Any())
            {
                ReplaceCollection(FilteredProducts, Array.Empty<Product>());
                return;
            }

            IEnumerable<Product> query = discountedProducts;

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
            FilteredProducts.Clear();
            _currentIndex = 0;
            var nextItems = results.Skip(_currentIndex).Take(12);
            foreach (var item in nextItems)
            {
                FilteredProducts.Add(item);
            }
            _currentIndex += 12;
        }

        public void FilterProductsByHallmark()
        {
            var discountedProducts = AllProducts.Where(p => p.OfferPrice > 0);

            if (discountedProducts == null || !discountedProducts.Any())
            {
                ReplaceCollection(FilteredProducts, Array.Empty<Product>());
                return;
            }

            IEnumerable<Product> query = discountedProducts;
            if (SelectedHallmark is HallmarkType hm)
            {
                query = query.Where(p => p != null && p.Hallmark == hm);
            }

            var results = query.ToList();
            FilteredProducts.Clear();
            _currentIndex = 0;
            var nextItems = results.Skip(_currentIndex).Take(12);
            foreach (var item in nextItems)
            {
                FilteredProducts.Add(item);
            }
            _currentIndex += 12;
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

        public void LoadMore()
        {
            if (SelectedProducts.Count > 0)
            {
                return; // Don't load more when filtering
            }

            var discountedProducts = AllProducts.Where(p => p.OfferPrice > 0).ToList();
            var source = discountedProducts.Skip(_currentIndex).Take(12);
            foreach (var item in source)
            {
                FilteredProducts.Add(item);
            }
            _currentIndex += 12;
        }

        #endregion

        public void ClearALL()
        {
            AllProducts.Clear();
            AllCategories.Clear();
            FilteredProducts.Clear();
            FilteredProductsForPopup.Clear();
            AllHallmarks.Clear();
            SelectedCategory = new Category();
            SelectedProducts.Clear();
            PopupSelectedProducts.Clear();
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
            var discountedProducts = AllProducts.Where(p => p.OfferPrice > 0).ToList();
            await Task.Delay(500);
            FilteredProducts.Clear();
            _currentIndex = 0;
            LoadMore();
        }

        public async Task LoadDataAsync()
        {
            ClearALL();
            GetHallmarks();
            TestProducts.GetProducts().ForEach(p => AllProducts.Add(p));
            TestProducts.GetCategories().ForEach(c => AllCategories.Add(c));
            await LoadData();
            await Task.Delay(3000);
            ShimmerLoading = false;
            ShimmerNotLoading = true;
            OnPropertyChanged(nameof(DiscountCount));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

