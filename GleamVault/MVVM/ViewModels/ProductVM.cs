using CommunityToolkit.Maui.Extensions;
using GleamVault.MVVM.Views.Popups;
using GleamVault.Services.Interfaces;
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
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GleamVault.MVVM.ViewModels
{
    [AddINotifyPropertyChangedInterface]

    public partial class ProductVM : INotifyPropertyChanged
    {

        public ProductVM(IImageService imageService)
        {
            _imageService = imageService;
        }

        #region Fields
        private ObservableCollection<Product> _allProducts = new();
        private ObservableCollection<Product> _filteredProducts = new();
        private ObservableCollection<Category> _allCategories = new();
        private Category selectedCategory = new();
        private ObservableCollection<HallmarkType> allHallmarks = new();
        private HallmarkType? selectedHallmark;
        private int sortIndex = 2;
        private bool isDataLoading = true;
        private bool shimmerLoading = true;
        private bool shimmerNotLoading = false;
        private readonly ObservableCollection<object> _shimmerItems = new();
        private readonly IImageService? _imageService;
        private string? _selectedImagePath;
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

        public ICommand ShowAddDiscountPopupCommand => new Command(async () => await ShowAddDiscountPopupAsync());

        public ICommand ShowAddProductPopupCommand => new Command(async () => await ShowAddProductPopupAsync());

        public ICommand PickImageCommand => new Command(async () => await PickImageAsync());

        public ICommand ClearImageCommand => new Command(() => ClearSelectedImage());
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

        public bool IsDataLoading
        {
            get => isDataLoading;
            set
            {
                isDataLoading = value;
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

        private Product _newProduct = new Product();
        public Product NewProduct
        {
            get => _newProduct;
            set
            {
                if (_newProduct == value) return;
                _newProduct = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<WeightUnit> _allWeightUnits = new();
        public ObservableCollection<WeightUnit> AllWeightUnits
        {
            get => _allWeightUnits;
            set
            {
                if (_allWeightUnits == value) return;
                _allWeightUnits = value;
                OnPropertyChanged();
            }
        }

        public string? SelectedImagePath
        {
            get => _selectedImagePath;
            set
            {
                if (_selectedImagePath == value) return;
                _selectedImagePath = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasSelectedImage));
            }
        }

        public bool HasSelectedImage => !string.IsNullOrWhiteSpace(_selectedImagePath);
        #endregion

        #region Tasks

        public async Task ShowAddDiscountPopupAsync()
        {
            await Shell.Current.ShowPopupAsync(new AddDiscountPopup(this));
        }

        public async Task ShowAddProductPopupAsync()
        {
            NewProduct = new Product();
            SelectedImagePath = null;
            GetWeightUnits();
            if (AllCategories.Count == 0)
            {
                TestProducts.GetCategories().ForEach(c => AllCategories.Add(c));
            }
            await Shell.Current.ShowPopupAsync(new AddProductPopup(this));
        }

        public async Task AddProductAsync(
            string name,
            string? description,
            Category? category,
            HallmarkType? hallmark,
            float unitCost,
            float unitPrice,
            WeightUnit weightUnit,
            float weight,
            bool isUniquePiece)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                await Shell.Current.DisplayAlert("⚠️ Validation Error", "Product name is required.", "OK");
                return;
            }

            if (category == null)
            {
                await Shell.Current.DisplayAlert("⚠️ Validation Error", "Category is required.", "OK");
                return;
            }

            if (hallmark == null)
            {
                await Shell.Current.DisplayAlert("⚠️ Validation Error", "Hallmark is required.", "OK");
                return;
            }

            string imageUrl = "default_product.gif";
            if (!string.IsNullOrWhiteSpace(_selectedImagePath))
            {
                if (_imageService != null)
                {
                    var imagesPath = @"C:\Users\Bhbored\Documents\C#\Maui\GleamVault\GleamVault\Resources\Images";
                    if (!Directory.Exists(imagesPath))
                        Directory.CreateDirectory(imagesPath);

                    var fileName = Path.GetFileName(_selectedImagePath);
                    var destPath = Path.Combine(imagesPath, fileName);

                    if (File.Exists(_selectedImagePath))
                    {
                        if (File.Exists(destPath))
                        {
                            var nameWithoutExt = Path.GetFileNameWithoutExtension(fileName);
                            var extension = Path.GetExtension(fileName);
                            var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                            fileName = $"{nameWithoutExt}_{timestamp}{extension}";
                            destPath = Path.Combine(imagesPath, fileName);
                        }

                        File.Copy(_selectedImagePath, destPath, true);
                        imageUrl = destPath;
                    }
                    else
                    {
                        imageUrl = _selectedImagePath;
                    }
                }
                else
                {
                    imageUrl = _selectedImagePath;
                }
            }

            var sku = GenerateSku(category.Name, hallmark.Value);
            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = name,
                Description = description,
                Category = category,
                CategoryId = category.Id,
                Sku = sku,
                Hallmark = hallmark.Value,
                UnitCost = unitCost,
                UnitPrice = unitPrice,
                ImageUrl = imageUrl,
                WeightUnit = weightUnit,
                Weight = weight,
                IsUniquePiece = isUniquePiece,
            };

            AllProducts.Add(product);
            await LoadDataAsync();
        }

        public async Task PickImageAsync()
        {
            try
            {
                var fileResult = await FilePicker.Default.PickAsync(new PickOptions
                {
                    PickerTitle = "Select Product Image",
                    FileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
                    {
                        { DevicePlatform.WinUI, new[] { ".jpg", ".jpeg", ".png", ".gif" } },
                        { DevicePlatform.Android, new[] { "image/*" } },
                        { DevicePlatform.iOS, new[] { "public.image" } },
                        { DevicePlatform.MacCatalyst, new[] { "public.image" } }
                    })
                });

                if (fileResult != null)
                {
                    SelectedImagePath = fileResult.FullPath;
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"Failed to pick image: {ex.Message}", "OK");
            }
        }

        public void ClearSelectedImage()
        {
            SelectedImagePath = null;
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
                await LoadDataAsync();
            }
        }

        #endregion

        #region Methods
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

        private string GenerateSku(string categoryName, HallmarkType hallmark)
        {
            var categoryPrefix = GetCategoryPrefix(categoryName);
            var existingProducts = AllProducts.Where(p => p.Sku != null && p.Sku.StartsWith(categoryPrefix)).ToList();
            var maxNumber = 0;

            foreach (var product in existingProducts)
            {
                if (product.Sku != null && product.Sku.Length > categoryPrefix.Length)
                {
                    var numberPart = product.Sku.Substring(categoryPrefix.Length);
                    if (int.TryParse(numberPart, out int num) && num > maxNumber)
                    {
                        maxNumber = num;
                    }
                }
            }

            string newSku;
            int attemptNumber = maxNumber + 1;
            do
            {
                newSku = $"{categoryPrefix}{attemptNumber:D3}";
                if (!AllProducts.Any(p => p.Sku == newSku))
                {
                    return newSku;
                }
                attemptNumber++;
            } while (attemptNumber < 1000);

            return $"{categoryPrefix}{attemptNumber:D3}";
        }

        private string GetCategoryPrefix(string categoryName)
        {
            return categoryName?.ToUpper() switch
            {
                "RINGS" => "RG",
                "NECKLACES" => "NK",
                "PENDANTS" => "PD",
                "CHAINS" => "CH",
                "EARRINGS" => "ER",
                "BRACELETS" => "BR",
                "ANKLETS" => "AK",
                "WATCHES" => "WT",
                "CUFFLINKS" => "CF",
                "BROOCHES" => "BC",
                "JEWELRY SETS" => "JS",
                "BRIDAL COLLECTIONS" => "BD",
                "CHARMS" => "CM",
                _ => "PR"
            };
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
            AllHallmarks.Clear();
            SelectedImagePath = null;
            SelectedCategory = new Category();
            IsDataLoading = true;
            ShimmerLoading = true;
            ShimmerNotLoading = false;
            _shimmerItems.Clear();
            for (int i = 0; i < 6; i++)
            {
                _shimmerItems.Add(new object());
            }
            OnPropertyChanged(nameof(ShimmerItems));

        }
        public async Task LoadDataAsync()
        {
            ClearALL();
            GetHallmarks();
            TestProducts.GetProducts().ForEach(p => AllProducts.Add(p));
            TestProducts.GetCategories().ForEach(c => AllCategories.Add(c));
            FilteredProducts = new ObservableCollection<Product>(AllProducts);
            await Task.Delay(3000);
            IsDataLoading = false;
            ShimmerLoading = false;
            ShimmerNotLoading = true;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}