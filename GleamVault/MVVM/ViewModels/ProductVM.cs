using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using GleamVault.TestData;
using Shared.Models;
using Shared.Models.Enums;

namespace GleamVault.MVVM.ViewModels
{
    public partial class ProductVM : INotifyPropertyChanged
    {
        #region Fields
        private ObservableCollection<Product> _allProducts = new();
        private ObservableCollection<Product> _filteredProducts = new();
        private ObservableCollection<Category> _allCategories = new();
        private Category selectedCategory = new();
        private ObservableCollection<HallmarkType> allHallmarks = new();
        private HallmarkType? selectedHallmark;
        private int sortIndex = 2;
        #endregion

        #region Properties
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

        public void ClearALL()
        {
            AllProducts.Clear();
            AllCategories.Clear();
            FilteredProducts.Clear();
            AllHallmarks.Clear();
            SelectedCategory = new Category();
        }
        public async Task LoadDataAsync()
        {
            ClearALL();
            GetHallmarks();
            await Task.Delay(100);
            TestProducts.GetProducts().ForEach(p => AllProducts.Add(p));
            TestProducts.GetCategories().ForEach(c => AllCategories.Add(c));
            FilteredProducts = new ObservableCollection<Product>(AllProducts);

        }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
