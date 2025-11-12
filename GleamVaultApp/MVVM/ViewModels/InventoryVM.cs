using CommunityToolkit.Maui.Extensions;
using GleamVault.TestData;
using Shared.Models;
using Shared.Models.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GleamVault.MVVM.ViewModels
{
    public class InventoryVM : INotifyPropertyChanged
    {

        #region Fields
        private ObservableCollection<Product> _allProducts = new();
        private ObservableCollection<Product> _filteredProducts = new();
        private ObservableCollection<Category> _allCategories = new();
        private Category selectedCategory = new();
        private ObservableCollection<HallmarkType> allHallmarks = new();
        private HallmarkType? selectedHallmark;
        private int sortIndex = 2;
        private bool shimmerLoading = true;
        private bool shimmerNotLoading = false;
        private readonly ObservableCollection<object> _shimmerItems = new();
        private Product _selectedProduct = new();
        private bool _isProductSelected;
        private ObservableCollection<WeightUnit> _allWeightUnits = new();
        private int _currentIndex = 0;
        private Product? _deletedProduct;
        private int _deletedProductIndex;
        public Func<string, Func<Task>, Task>? ShowDeleteSnackbar;
        public Func<string, Task>? ShowSuccessSnackbar;

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


        public ObservableCollection<Category> AllCategories
        {
            get => _allCategories;
            set { if (_allCategories == value) return; _allCategories = value; OnPropertyChanged(); }
        }

        public ObservableCollection<WeightUnit> AllWeightUnits
        {
            get => _allWeightUnits;
            set { if (_allWeightUnits == value) return; _allWeightUnits = value; OnPropertyChanged(); }
        }

        public Product SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                if (_selectedProduct == value) return;
                _selectedProduct = value;
                IsProductSelected = value != null;
                OnPropertyChanged();
            }
        }

        public bool IsProductSelected
        {
            get => _isProductSelected;
            set
            {
                if (_isProductSelected == value) return;
                _isProductSelected = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Commands
        public ICommand SelectProductCommand => new Command<Product>(p => SelectProduct(p));
        public ICommand SaveProductCommand => new Command(async () => await SaveProductAsync());
        public ICommand CancelEditCommand => new Command(() => CancelEdit());
        public ICommand LoadMoreCommand=> new Command(() => LoadMore());
        public ICommand DeleteProductCommand => new Command<Product>(async (p) => await DeleteProductAsync(p));
        #endregion

        #region Tasks
        public async Task SaveProductAsync()
        {
            if (SelectedProduct == null)
            {
                await Shell.Current.DisplayAlert("Error", "No product selected for editing.", "OK");
                return;
            }

            // Validate Product Name
            if (string.IsNullOrWhiteSpace(SelectedProduct.Name))
            {
                await Shell.Current.DisplayAlert("Validation Error", "Product name is required.", "OK");
                return;
            }

            if (SelectedProduct.Name.Length < 3)
            {
                await Shell.Current.DisplayAlert("Validation Error", "Product name must be at least 3 characters long.", "OK");
                return;
            }

            if (SelectedProduct.Name.Length > 200)
            {
                await Shell.Current.DisplayAlert("Validation Error", "Product name cannot exceed 200 characters.", "OK");
                return;
            }

            // Validate Category
            if (SelectedProduct.Category == null)
            {
                await Shell.Current.DisplayAlert("Validation Error", "Please select a category.", "OK");
                return;
            }

            // Validate Hallmark
            if (SelectedProduct.Hallmark == null || !Enum.IsDefined(typeof(HallmarkType), SelectedProduct.Hallmark))
            {
                await Shell.Current.DisplayAlert("Validation Error", "Please select a valid hallmark type.", "OK");
                return;
            }

            // Validate Unit Cost
            if (SelectedProduct.UnitCost <= 0)
            {
                await Shell.Current.DisplayAlert("Validation Error", "Unit cost must be greater than zero.", "OK");
                return;
            }

            if (SelectedProduct.UnitCost > 999999999)
            {
                await Shell.Current.DisplayAlert("Validation Error", "Unit cost is too high. Please enter a valid amount.", "OK");
                return;
            }

            // Validate Unit Price
            if (SelectedProduct.UnitPrice <= 0)
            {
                await Shell.Current.DisplayAlert("Validation Error", "Unit price must be greater than zero.", "OK");
                return;
            }

            if (SelectedProduct.UnitPrice > 999999999)
            {
                await Shell.Current.DisplayAlert("Validation Error", "Unit price is too high. Please enter a valid amount.", "OK");
                return;
            }

            // Validate Unit Price is greater than Unit Cost
            if (SelectedProduct.UnitPrice < SelectedProduct.UnitCost)
            {
                await Shell.Current.DisplayAlert("Validation Error", "Unit price should be greater than or equal to unit cost to avoid losses.", "OK");
                return;
            }

            // Validate Weight Unit
            if (!Enum.IsDefined(typeof(WeightUnit), SelectedProduct.WeightUnit))
            {
                await Shell.Current.DisplayAlert("Validation Error", "Please select a valid weight unit.", "OK");
                return;
            }

            // Validate Weight
            if (SelectedProduct.Weight <= 0)
            {
                await Shell.Current.DisplayAlert("Validation Error", "Weight must be greater than zero.", "OK");
                return;
            }

            if (SelectedProduct.Weight > 999999)
            {
                await Shell.Current.DisplayAlert("Validation Error", "Weight value is too high. Please enter a valid weight.", "OK");
                return;
            }

            // Validate Current Stock
            if (SelectedProduct.CurrentStock < 0)
            {
                await Shell.Current.DisplayAlert("Validation Error", "Current stock cannot be negative.", "OK");
                return;
            }

            if (SelectedProduct.CurrentStock > 999999)
            {
                await Shell.Current.DisplayAlert("Validation Error", "Stock quantity is too high. Please enter a valid quantity.", "OK");
                return;
            }

            // Validate Description (optional but length check)
            if (!string.IsNullOrWhiteSpace(SelectedProduct.Description) && SelectedProduct.Description.Length > 1000)
            {
                await Shell.Current.DisplayAlert("Validation Error", "Description cannot exceed 1000 characters.", "OK");
                return;
            }

            var productToUpdate = AllProducts.FirstOrDefault(p => p.Id == SelectedProduct.Id);
            if (productToUpdate == null)
            {
                await Shell.Current.DisplayAlert("Error", "Product not found in the inventory.", "OK");
                return;
            }

            try
            {
                productToUpdate.Name = SelectedProduct.Name.Trim();
                productToUpdate.Description = SelectedProduct.Description?.Trim();
                productToUpdate.CategoryId = SelectedProduct.CategoryId;
                productToUpdate.Category = SelectedProduct.Category;
                productToUpdate.Hallmark = SelectedProduct.Hallmark;
                productToUpdate.UnitCost = SelectedProduct.UnitCost;
                productToUpdate.UnitPrice = SelectedProduct.UnitPrice;
                productToUpdate.Weight = SelectedProduct.Weight;
                productToUpdate.CurrentStock = SelectedProduct.CurrentStock;
                productToUpdate.IsUniquePiece = SelectedProduct.IsUniquePiece;
                productToUpdate.ImageUrl = SelectedProduct.ImageUrl;

                FilterProducts();

                SelectedProduct = null;

                await Shell.Current.DisplayAlert("Success", "Product updated successfully!", "OK");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"Failed to save product: {ex.Message}", "OK");
            }
        }

        public void CancelEdit()
        {
            SelectedProduct = null;
        }

        public async Task DeleteProductAsync(Product product)
        {
            if (product == null) return;

            bool confirm = await Shell.Current.DisplayAlert(
                "Delete Product",
                $"Are you sure you want to delete '{product.Name}'?",
                "Delete",
                "Cancel");

            if (!confirm) return;

            var productName = product.Name;

            _deletedProduct = new Product
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Sku = product.Sku,
                CategoryId = product.CategoryId,
                Category = product.Category,
                ImageUrl = product.ImageUrl,
                Hallmark = product.Hallmark,
                WeightUnit = product.WeightUnit,
                Weight = product.Weight,
                UnitCost = product.UnitCost,
                UnitPrice = product.UnitPrice,
                OfferPrice = product.OfferPrice,
                CurrentStock = product.CurrentStock,
                IsUniquePiece = product.IsUniquePiece
            };

            _deletedProductIndex = AllProducts.IndexOf(product);

            AllProducts.Remove(product);
            FilteredProducts.Remove(product);

            if (SelectedProduct?.Id == product.Id)
            {
                SelectedProduct = null;
            }

            if (ShowDeleteSnackbar != null)
            {
                await ShowDeleteSnackbar(productName, async () => await UndoDeleteAsync(productName));
            }
        }

        public async Task UndoDeleteAsync(string productName)
        {
            UndoDeleteProduct();
            
            if (ShowSuccessSnackbar != null)
            {
                await ShowSuccessSnackbar($"'{productName}' restored");
            }
        }

        public void UndoDeleteProduct()
        {
            if (_deletedProduct == null) return;

            if (_deletedProductIndex >= 0 && _deletedProductIndex <= AllProducts.Count)
            {
                AllProducts.Insert(_deletedProductIndex, _deletedProduct);
            }
            else
            {
                AllProducts.Add(_deletedProduct);
            }

            FilterProducts();

            _deletedProduct = null;
            _deletedProductIndex = -1;
        }
        #endregion

        #region methods
        public void SelectProduct(Product product)
        {
            if (product == null) return;
            SelectedProduct = new Product
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Sku = product.Sku,
                CategoryId = product.CategoryId,
                Category = product.Category,
                ImageUrl = product.ImageUrl,
                Hallmark = product.Hallmark,
                WeightUnit = product.WeightUnit,
                Weight = product.Weight,
                UnitCost = product.UnitCost,
                UnitPrice = product.UnitPrice,
                OfferPrice = product.OfferPrice,
                CurrentStock = product.CurrentStock,
                IsUniquePiece = product.IsUniquePiece
            };
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

        public void GetWeightUnits()
        {
            AllWeightUnits.Clear();
            foreach (var v in Enum.GetValues<WeightUnit>())
                AllWeightUnits.Add(v);
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

        public async Task LoadData()
        {
            TestProducts.GetProducts().ForEach(p => AllProducts.Add(p));
            await Task.Delay(500);
            LoadMore();
        }

        public void LoadMore()
        {
            var nextItems = AllProducts.Skip(_currentIndex).Take(10);

            foreach (var item in nextItems)
            {
                FilteredProducts.Add(item);
            }
            _currentIndex += 10;
        }

        public void ClearALL()
        {
            AllProducts.Clear();
            AllCategories.Clear();
            FilteredProducts.Clear();
            AllHallmarks.Clear();
            AllWeightUnits.Clear();
            SelectedProducts.Clear();
            SelectedCategory = new Category();
            SelectedProduct = null;
            ShimmerLoading = true;
            ShimmerNotLoading = false;
            _shimmerItems.Clear();
            for (int i = 0; i < 8; i++)
            {
                _shimmerItems.Add(new object());
            }
            OnPropertyChanged(nameof(ShimmerItems));
        }
        public async Task LoadDataAsync()
        {
            ClearALL();
            GetHallmarks();
            GetWeightUnits();
            await LoadData();
            TestProducts.GetCategories().ForEach(c => AllCategories.Add(c));
            await Task.Delay(3000);
            ShimmerLoading = false;
            ShimmerNotLoading = true;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
