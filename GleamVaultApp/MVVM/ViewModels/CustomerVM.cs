using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Maui.Views;
using GleamVault.MVVM.Views.Popups;
using GleamVault.TestData;
using Shared.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace GleamVault.MVVM.ViewModels
{
    public class CustomerVM : INotifyPropertyChanged
    {
        #region Fields
        private ObservableCollection<Customer> _allCustomers = new();
        private ObservableCollection<Customer> _filteredCustomers = new();
        private Customer _selectedCustomer = new();
        private bool _isCustomerSelected;
        private bool shimmerLoading = true;
        private bool shimmerNotLoading = false;
        private readonly ObservableCollection<object> _shimmerItems = new();
        private int sortIndex = 2;
        private int _currentIndex = 0;
        private Customer? _deletedCustomer;
        private int _deletedCustomerIndex;
        public Func<string, Func<Task>, Task>? ShowDeleteSnackbar;
        public Func<string, Task>? ShowSuccessSnackbar;
        public IList<object> SelectedCustomers { get; set; } = [];
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

        public ObservableCollection<Customer> AllCustomers
        {
            get => _allCustomers;
            set { if (_allCustomers == value) return; _allCustomers = value; OnPropertyChanged(); }
        }

        public ObservableCollection<Customer> FilteredCustomers
        {
            get => _filteredCustomers;
            set { if (_filteredCustomers == value) return; _filteredCustomers = value; OnPropertyChanged(); }
        }

        public Customer SelectedCustomer
        {
            get => _selectedCustomer;
            set
            {
                if (_selectedCustomer == value) return;
                _selectedCustomer = value;
                IsCustomerSelected = value != null;
                OnPropertyChanged();
            }
        }

        public bool IsCustomerSelected
        {
            get => _isCustomerSelected;
            set
            {
                if (_isCustomerSelected == value) return;
                _isCustomerSelected = value;
                OnPropertyChanged();
            }
        }

        private Customer _newCustomer = new Customer();
        public Customer NewCustomer
        {
            get => _newCustomer;
            set
            {
                if (_newCustomer == value) return;
                _newCustomer = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Commands
        public ICommand SelectCustomerCommand => new Command<Customer>(c => SelectCustomer(c));
        public ICommand SaveCustomerCommand => new Command(async () => await SaveCustomerAsync());
        public ICommand CancelEditCommand => new Command(() => CancelEdit());
        public ICommand LoadMoreCommand => new Command(() => LoadMore());
        public ICommand DeleteCustomerCommand => new Command<Customer>(async (c) => await DeleteCustomerAsync(c));
        public ICommand AddCustomerCommand => new Command(async () => await AddCustomerAsync());
        #endregion

        #region Tasks
        public async Task SaveCustomerAsync()
        {
            if (SelectedCustomer == null)
            {
                await Shell.Current.DisplayAlert("Error", "No customer selected for editing.", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(SelectedCustomer.FullName))
            {
                await Shell.Current.DisplayAlert("Validation Error", "Customer name is required.", "OK");
                return;
            }

            if (SelectedCustomer.FullName.Length < 3)
            {
                await Shell.Current.DisplayAlert("Validation Error", "Customer name must be at least 3 characters long.", "OK");
                return;
            }

            var customerToUpdate = AllCustomers.FirstOrDefault(c => c.Id == SelectedCustomer.Id);
            if (customerToUpdate == null)
            {
                await Shell.Current.DisplayAlert("Error", "Customer not found.", "OK");
                return;
            }

            try
            {
                customerToUpdate.FullName = SelectedCustomer.FullName.Trim();
                customerToUpdate.PhoneNumber = SelectedCustomer.PhoneNumber?.Trim();
                customerToUpdate.Email = SelectedCustomer.Email?.Trim();
                customerToUpdate.Address = SelectedCustomer.Address?.Trim();
                customerToUpdate.DateOfBirth = SelectedCustomer.DateOfBirth;
                customerToUpdate.Notes = SelectedCustomer.Notes?.Trim();
                customerToUpdate.LoyaltyPoints = SelectedCustomer.LoyaltyPoints;

                FilterCustomers();
                SelectedCustomer = null;

                await Shell.Current.DisplayAlert("Success", "Customer updated successfully!", "OK");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"Failed to save customer: {ex.Message}", "OK");
            }
        }

        public void CancelEdit()
        {
            SelectedCustomer = null;
        }

        public async Task DeleteCustomerAsync(Customer customer)
        {
            if (customer == null) return;

            bool confirm = await Shell.Current.DisplayAlert(
                "Delete Customer",
                $"Are you sure you want to delete '{customer.FullName}'?",
                "Delete",
                "Cancel");

            if (!confirm) return;

            var customerName = customer.FullName;

            _deletedCustomer = new Customer
            {
                Id = customer.Id,
                FullName = customer.FullName,
                PhoneNumber = customer.PhoneNumber,
                Email = customer.Email,
                Address = customer.Address,
                DateOfBirth = customer.DateOfBirth,
                Notes = customer.Notes,
                LoyaltyPoints = customer.LoyaltyPoints,
                ImageUrl = customer.ImageUrl
            };

            _deletedCustomerIndex = AllCustomers.IndexOf(customer);

            AllCustomers.Remove(customer);
            FilteredCustomers.Remove(customer);

            if (SelectedCustomer?.Id == customer.Id)
            {
                SelectedCustomer = null;
            }

            if (ShowDeleteSnackbar != null)
            {
                await ShowDeleteSnackbar(customerName, async () => await UndoDeleteAsync(customerName));
            }
        }

        public async Task UndoDeleteAsync(string customerName)
        {
            UndoDeleteCustomer();

            if (ShowSuccessSnackbar != null)
            {
                await ShowSuccessSnackbar($"'{customerName}' restored");
            }
        }

        public async Task AddCustomerAsync()
        {
            NewCustomer = new Customer { DateOfBirth = DateTime.Now.AddYears(-25) };
            await Shell.Current.ShowPopupAsync(new AddCustomerPopup(this));
        }

        public async Task AddNewCustomerAsync(
            string fullName,
            string? phoneNumber,
            string? email,
            string? address,
            DateTime? dateOfBirth,
            int loyaltyPoints,
            string? notes)
        {
            if (string.IsNullOrWhiteSpace(fullName))
            {
                await Shell.Current.DisplayAlert("⚠️ Validation Error", "Customer name is required.", "OK");
                return;
            }

            if (fullName.Length < 3)
            {
                await Shell.Current.DisplayAlert("⚠️ Validation Error", "Customer name must be at least 3 characters long.", "OK");
                return;
            }

            var customer = new Customer
            {
                Id = Guid.NewGuid(),
                FullName = fullName.Trim(),
                PhoneNumber = phoneNumber?.Trim(),
                Email = email?.Trim(),
                Address = address?.Trim(),
                DateOfBirth = dateOfBirth,
                LoyaltyPoints = loyaltyPoints,
                Notes = notes?.Trim(),
                ImageUrl = "user.png"
            };

            AllCustomers.Add(customer);
            FilterCustomers();
            await Shell.Current.DisplayAlert("✓ Success", $"Customer '{fullName}' added successfully!", "OK");
        }
        #endregion

        #region Methods
        public void SelectCustomer(Customer customer)
        {
            if (customer == null) return;
            SelectedCustomer = new Customer
            {
                Id = customer.Id,
                FullName = customer.FullName,
                PhoneNumber = customer.PhoneNumber,
                Email = customer.Email,
                Address = customer.Address,
                DateOfBirth = customer.DateOfBirth,
                Notes = customer.Notes,
                LoyaltyPoints = customer.LoyaltyPoints,
                ImageUrl = customer.ImageUrl
            };
        }

        public void FilterCustomers()
        {
            if (SelectedCustomers.Count == 0)
            {
                FilteredCustomers.Clear();
                _currentIndex = 0;
                var nextItems = AllCustomers.Skip(_currentIndex).Take(10);
                foreach (var item in nextItems)
                {
                    FilteredCustomers.Add(item);
                }
                _currentIndex += 10;
            }
            else
            {
                var filteredList = new List<Customer>();
                foreach (var selectedItem in SelectedCustomers)
                {
                    if (selectedItem is Customer selectedCustomer)
                    {
                        var matchingCustomer = AllCustomers.FirstOrDefault(c => c.FullName == selectedCustomer.FullName);
                        if (matchingCustomer != null)
                        {
                            filteredList.Add(matchingCustomer);
                        }
                    }
                }
                ReplaceCollection(FilteredCustomers, filteredList);
                _currentIndex = filteredList.Count;
            }
        }

        public void SortNow()
        {
            if (FilteredCustomers == null || FilteredCustomers.Count == 0) return;
            List<Customer> sortedList = SortIndex switch
            {
                0 => FilteredCustomers.OrderBy(c => c.FullName).ToList(),
                1 => FilteredCustomers.OrderByDescending(c => c.FullName).ToList(),
                2 => FilteredCustomers.ToList(),
                _ => FilteredCustomers.ToList(),
            };
            ReplaceCollection(FilteredCustomers, sortedList);
        }

        private static void ReplaceCollection(ObservableCollection<Customer> target, IEnumerable<Customer> source)
        {
            var list = source is IList<Customer> l ? l : source.ToList();
            MainThread.BeginInvokeOnMainThread(() =>
            {
                target.Clear();
                for (int i = 0; i < list.Count; i++)
                    target.Add(list[i]);
            });
        }

        public void UndoDeleteCustomer()
        {
            if (_deletedCustomer == null) return;

            if (_deletedCustomerIndex >= 0 && _deletedCustomerIndex <= AllCustomers.Count)
            {
                AllCustomers.Insert(_deletedCustomerIndex, _deletedCustomer);
            }
            else
            {
                AllCustomers.Add(_deletedCustomer);
            }

            FilterCustomers();

            _deletedCustomer = null;
            _deletedCustomerIndex = -1;
        }

        public async Task LoadData()
        {
            TestProducts.GetCustomers().ForEach(c => AllCustomers.Add(c));
            await Task.Delay(500);
            LoadMore();
        }

        public void LoadMore()
        {
            if (SelectedCustomers.Count > 0)
            {
                return; 
            }
            var source = AllCustomers.Skip(_currentIndex).Take(10);
            foreach (var item in source)
            {
                FilteredCustomers.Add(item);
            }
            _currentIndex += 10;
        }

        public void ClearAll()
        {
            AllCustomers.Clear();
            FilteredCustomers.Clear();
            SelectedCustomers.Clear();
            SelectedCustomer = null;
            ShimmerLoading = true;
            ShimmerNotLoading = false;
            _currentIndex = 0;
            _shimmerItems.Clear();
            for (int i = 0; i < 4; i++)
            {
                _shimmerItems.Add(new object());
            }
            OnPropertyChanged(nameof(ShimmerItems));
        }

        public async Task LoadDataAsync()
        {
            ClearAll();
            await LoadData();
            await Task.Delay(3000);
            ShimmerLoading = false;
            ShimmerNotLoading = true;
        }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
