using CommunityToolkit.Mvvm.ComponentModel;
using GleamVault.TestData;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GleamVault.MVVM.ViewModels
{
    public partial class HomeVM : ObservableObject
    {


        #region properties
        [ObservableProperty]
        private ObservableCollection<Product> _allProducts = new();

        [ObservableProperty]
        private ObservableCollection<Product> _filteredProducts = new();

        [ObservableProperty]
        private ObservableCollection<Product> _cartItems = new();

        [ObservableProperty]
        private ObservableCollection<Category> _allCategories = new();
        #endregion

        #region fields
        #endregion

        #region methods
        #endregion

        #region commands
        #endregion

        #region tasks
        public async Task LoadDataAsync()
        {
            AllProducts.Clear();
            AllCategories.Clear();
            await Task.Delay(100);
            TestProducts.GetProducts().ForEach(p => AllProducts.Add(p));
            TestProducts.GetCategories().ForEach(c => AllCategories.Add(c));

        }
        #endregion
    }
}
