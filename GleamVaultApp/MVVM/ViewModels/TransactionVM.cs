using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Maui.Views;
using GleamVault.MVVM.Views.Popups;
using GleamVault.TestData;
using Microsoft.Maui.Controls;
using PropertyChanged;
using Shared.Models;
using Shared.Models.Enums;
using Syncfusion.Maui.Calendar;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using System.IO;
using Microsoft.Maui.Storage;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace GleamVault.MVVM.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public partial class TransactionVM : INotifyPropertyChanged
    {
        #region Fields
        private ObservableCollection<Transaction> _allTransactions = new();
        private ObservableCollection<Transaction> _filteredTransactions = new();
        private ObservableCollection<Transaction> _paginatedTransactions = new();

        private int _currentPage = 1;
        private const int ItemsPerPage = 10;

        private DateTime? _startDate;
        private DateTime? _endDate;
        private TransactionType? _selectedTransactionType;
        private CalendarDateRange? _selectedDateRange;
        private SaleChannel? _selectedChannel;
        private bool shimmerLoading = true;
        private bool shimmerNotLoading = false;
        private readonly ObservableCollection<object> _shimmerItems = new();
        #endregion

        #region Properties
        public ObservableCollection<Transaction> AllTransactions
        {
            get => _allTransactions;
            set
            {
                if (_allTransactions == value) return;
                _allTransactions = value;
                OnPropertyChanged();
                FilterTransactions();
            }
        }

        public ObservableCollection<Transaction> FilteredTransactions
        {
            get => _filteredTransactions;
            set
            {
                if (_filteredTransactions == value) return;
                _filteredTransactions = value;
                OnPropertyChanged();
                _currentPage = 1;
                OnPropertyChanged(nameof(CurrentPage));
                OnPropertyChanged(nameof(TotalPages));
                OnPropertyChanged(nameof(CanGoToPreviousPage));
                OnPropertyChanged(nameof(CanGoToNextPage));
                UpdatePagination();
            }
        }

        public ObservableCollection<Transaction> PaginatedTransactions
        {
            get => _paginatedTransactions;
            set
            {
                if (_paginatedTransactions == value) return;
                _paginatedTransactions = value;
                OnPropertyChanged();
            }
        }

        public int CurrentPage
        {
            get => _currentPage;
            set
            {
                var newValue = value;
                if (FilteredTransactions != null && FilteredTransactions.Count > 0)
                {
                    var maxPage = TotalPages;
                    if (newValue > maxPage) newValue = maxPage;
                    if (newValue < 1) newValue = 1;
                }
                else
                {
                    newValue = 1;
                }

                if (_currentPage == newValue) return;
                _currentPage = newValue;
                OnPropertyChanged();
                UpdatePagination();
            }
        }

        public int TotalPages
        {
            get
            {
                var count = FilteredTransactions?.Count ?? 0;
                return count == 0 ? 0 : (int)Math.Ceiling((double)count / ItemsPerPage);
            }
        }

        public bool CanGoToPreviousPage => CurrentPage > 1;
        public bool CanGoToNextPage => CurrentPage < TotalPages;

        public List<TransactionType> AllTransactionTypes => new List<TransactionType>
        {
            TransactionType.Sell,
            TransactionType.CustomeOrder,
            TransactionType.Repairement,
            TransactionType.Buy
        };

        public TransactionType? SelectedTransactionType
        {
            get => _selectedTransactionType;
            set
            {
                if (_selectedTransactionType == value) return;
                _selectedTransactionType = value;
                OnPropertyChanged();
                FilterTransactions();
            }
        }

        public SaleChannel? SelectedChannel
        {
            get => _selectedChannel;
            set
            {
                if (_selectedChannel == value) return;
                _selectedChannel = value;
                OnPropertyChanged();
                FilterTransactions();
            }
        }

        public List<SaleChannel> AllChannels => new List<SaleChannel>
        {
            SaleChannel.InStore,
            SaleChannel.Online
        };

        private DateTime _startDateValue = DateTime.Now.AddDays(-30);
        private DateTime _endDateValue = DateTime.Now;

        public DateTime? StartDate
        {
            get => _startDate;
            set
            {
                if (_startDate == value) return;
                _startDate = value;
                if (value.HasValue) _startDateValue = value.Value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(StartDateValue));
                OnPropertyChanged(nameof(DateRangeText));
                FilterTransactions();
            }
        }

        public DateTime StartDateValue
        {
            get => _startDateValue;
            set
            {
                if (_startDateValue == value) return;
                _startDateValue = value;
                if (_startDate != null)
                {
                    StartDate = value;
                }
                OnPropertyChanged();
                OnPropertyChanged(nameof(DateRangeText));
            }
        }

        public DateTime? EndDate
        {
            get => _endDate;
            set
            {
                if (_endDate == value) return;
                _endDate = value;
                if (value.HasValue) _endDateValue = value.Value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(EndDateValue));
                OnPropertyChanged(nameof(DateRangeText));
                FilterTransactions();
            }
        }

        public DateTime EndDateValue
        {
            get => _endDateValue;
            set
            {
                if (_endDateValue == value) return;
                _endDateValue = value;
                if (_endDate != null)
                {
                    EndDate = value;
                }
                OnPropertyChanged();
                OnPropertyChanged(nameof(DateRangeText));
            }
        }

        public string DateRangeText
        {
            get
            {
                if (StartDateValue == default || EndDateValue == default)
                    return "Select Date Range";
                return $"{StartDateValue:MMM dd} - {EndDateValue:MMM dd, yyyy}";
            }
        }


        public Transaction? SelectedTransaction { get; set; }

        public ObservableCollection<object> ShimmerItems
        {
            get => _shimmerItems;
        }

        public bool ShimmerNotLoading
        {
            get => shimmerNotLoading;
            set
            {
                if (shimmerNotLoading == value) return;
                shimmerNotLoading = value;
                OnPropertyChanged();
            }
        }

        public bool ShimmerLoading
        {
            get => shimmerLoading;
            set
            {
                if (shimmerLoading == value) return;
                shimmerLoading = value;
                OnPropertyChanged();
                ShimmerNotLoading = !value;
            }
        }

        public CalendarDateRange? SelectedDateRange
        {
            get => _selectedDateRange;
            set
            {
                if (_selectedDateRange == value) return;
                _selectedDateRange = value;
                OnPropertyChanged();

                if (value != null && value.StartDate != null && value.EndDate != null)
                {
                    StartDate = value.StartDate;
                    EndDate = value.EndDate;
                }
                else
                {
                    StartDate = null;
                    EndDate = null;
                }
            }
        }
        #endregion

        #region Commands
        public ICommand ExportToPdfCommand => new Command(async () => await ExportToPdfAsync());
        public ICommand ViewTransactionCommand => new Command<Transaction>(async (t) => await ViewTransactionAsync(t));
        public ICommand ShowTransactionDetailsCommand => new Command<Transaction>(async (t) => await ShowTransactionDetailsAsync(t));
        public ICommand ClearDateFilterCommand => new Command(() =>
        {
            SelectedDateRange = null;
            SelectedChannel = null;
            SelectedTransactionType = null;
            StartDate = null;
            EndDate = null;
            _startDateValue = DateTime.Now.AddDays(-30);
            _endDateValue = DateTime.Now;
            OnPropertyChanged(nameof(StartDateValue));
            OnPropertyChanged(nameof(EndDateValue));
            OnPropertyChanged(nameof(DateRangeText));
        });
        public ICommand ShowDateRangePopupCommand => new Command(async () => await ShowDateRangePopupAsync());

        public ICommand FirstPageCommand => new Command(() => { CurrentPage = 1; });
        public ICommand PreviousPageCommand => new Command(() => { if (CanGoToPreviousPage) CurrentPage--; });
        public ICommand NextPageCommand => new Command(() => { if (CanGoToNextPage) CurrentPage++; });
        public ICommand LastPageCommand => new Command(() => { CurrentPage = TotalPages; });
        #endregion

        #region Methods
        public void FilterTransactions()
        {
            if (AllTransactions == null || AllTransactions.Count == 0)
            {
                ReplaceCollection(FilteredTransactions, Array.Empty<Transaction>());
                return;
            }

            IEnumerable<Transaction> query = AllTransactions;

            if (StartDate.HasValue)
            {
                query = query.Where(t => t.CreatedAt.Date >= StartDate.Value.Date);
            }

            if (EndDate.HasValue)
            {
                query = query.Where(t => t.CreatedAt.Date <= EndDate.Value.Date);
            }

            if (SelectedChannel.HasValue)
            {
                query = query.Where(t => t.Channel == SelectedChannel.Value);
            }

            if (SelectedTransactionType.HasValue)
            {
                query = query.Where(t => t.Type == SelectedTransactionType.Value);
            }

            var results = query.OrderByDescending(t => t.CreatedAt).ToList();
            ReplaceCollection(FilteredTransactions, results);

            _currentPage = 1;
            OnPropertyChanged(nameof(CurrentPage));
            OnPropertyChanged(nameof(TotalPages));
            OnPropertyChanged(nameof(CanGoToPreviousPage));
            OnPropertyChanged(nameof(CanGoToNextPage));
            UpdatePagination();
        }

        private void ReplaceCollection(ObservableCollection<Transaction> target, IEnumerable<Transaction> source)
        {
            var list = source is IList<Transaction> l ? l : source.ToList();
            target.Clear();
            for (int i = 0; i < list.Count; i++)
                target.Add(list[i]);
        }

        private void UpdatePagination()
        {
            if (FilteredTransactions == null || FilteredTransactions.Count == 0)
            {
                ReplaceCollection(PaginatedTransactions, Array.Empty<Transaction>());
                _currentPage = 1;
                OnPropertyChanged(nameof(CurrentPage));
                OnPropertyChanged(nameof(TotalPages));
                OnPropertyChanged(nameof(CanGoToPreviousPage));
                OnPropertyChanged(nameof(CanGoToNextPage));
                return;
            }

            var total = TotalPages;

            if (total > 0 && CurrentPage > total)
            {
                _currentPage = total;
                OnPropertyChanged(nameof(CurrentPage));
            }
            else if (CurrentPage < 1)
            {
                _currentPage = 1;
                OnPropertyChanged(nameof(CurrentPage));
            }

            var startIndex = (CurrentPage - 1) * ItemsPerPage;
            var count = Math.Min(ItemsPerPage, FilteredTransactions.Count - startIndex);

            if (count <= 0 || startIndex >= FilteredTransactions.Count)
            {
                ReplaceCollection(PaginatedTransactions, Array.Empty<Transaction>());
            }
            else
            {
                var filteredList = FilteredTransactions.ToList();
                var paginatedItems = filteredList.GetRange(startIndex, count);
                ReplaceCollection(PaginatedTransactions, paginatedItems);
            }

            OnPropertyChanged(nameof(TotalPages));
            OnPropertyChanged(nameof(CanGoToPreviousPage));
            OnPropertyChanged(nameof(CanGoToNextPage));
        }

        public async Task LoadDataAsync()
        {
            ShimmerLoading = true;
            ShimmerNotLoading = false;

            ShimmerItems.Clear();

            for (int i = 0; i < 5; i++)
            {
                ShimmerItems.Add(new object());
            }

            await Task.Delay(500);

            if (TestTransactions.Transactions.Count == 0)
            {
                var products = TestProducts.GetProducts();
                var customers = TestProducts.GetCustomers();
                TestTransactions.GenerateTestTransactions(products, customers);
            }

            AllTransactions = new ObservableCollection<Transaction>(TestTransactions.Transactions);

            var endDate = DateTime.Now;
            var startDate = endDate.AddDays(-90);
            SelectedDateRange = new CalendarDateRange(startDate, endDate);
            StartDateValue = startDate;
            EndDateValue = endDate;

            FilterTransactions();

            await Task.Delay(100);
            ShimmerLoading = false;
            ShimmerNotLoading = true;
        }

        public async Task ShowDateRangePopupAsync()
        {
            await Shell.Current.ShowPopupAsync(new DateRangePopup(this));
        }

        public async Task ViewTransactionAsync(Transaction transaction)
        {
            if (transaction == null) return;
            SelectedTransaction = transaction;
            await Task.CompletedTask;
        }

        public async Task ShowTransactionDetailsAsync(Transaction transaction)
        {
            if (transaction == null) return;
            var popup = new GleamVault.MVVM.Views.Popups.TransactionDetailsPopup(transaction);
            await Shell.Current.ShowPopupAsync(popup);
        }

        public async Task ExportToPdfAsync()
        {
            try
            {
                var transactionsToExport = FilteredTransactions.Any() ? FilteredTransactions : AllTransactions;

                if (!transactionsToExport.Any())
                {
                    await Shell.Current.DisplayAlert("No Data", "There are no transactions to export.", "OK");
                    return;
                }

                var pdfContent = GeneratePdfContent(transactionsToExport.ToList());
                var fileName = $"Transactions_{DateTime.Now:yyyyMMdd_HHmmss}.txt";

                var filePath = Path.Combine(FileSystem.CacheDirectory, fileName);
                await File.WriteAllTextAsync(filePath, pdfContent);

                await Shell.Current.DisplayAlert("Export Complete",
                    $"Transactions exported to:\n{filePath}", "OK");

                var snackbar = Snackbar.Make("Transactions exported successfully!",
                    duration: TimeSpan.FromSeconds(2));
                await snackbar.Show();
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Export Error",
                    $"Failed to export transactions: {ex.Message}", "OK");
            }
        }

        private string GeneratePdfContent(List<Transaction> transactions)
        {
            var content = new System.Text.StringBuilder();
            content.AppendLine("GLEAM & CO. - TRANSACTION REPORT");
            content.AppendLine("=".PadRight(50, '='));
            content.AppendLine($"Generated: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            content.AppendLine($"Total Transactions: {transactions.Count}");
            content.AppendLine();

            if (StartDate.HasValue || EndDate.HasValue)
            {
                content.AppendLine("Date Range:");
                content.AppendLine($"  From: {(StartDate?.ToString("yyyy-MM-dd") ?? "All")}");
                content.AppendLine($"  To: {(EndDate?.ToString("yyyy-MM-dd") ?? "All")}");
                content.AppendLine();
            }

            content.AppendLine($"Transaction Type: {SelectedTransactionType}");
            content.AppendLine();
            content.AppendLine("-".PadRight(50, '-'));

            foreach (var transaction in transactions)
            {
                content.AppendLine($"Transaction ID: {transaction.Id}");
                content.AppendLine($"Date: {transaction.CreatedAt:yyyy-MM-dd HH:mm:ss}");
                content.AppendLine($"Type: {transaction.Type}");
                content.AppendLine($"Channel: {transaction.Channel}");
                content.AppendLine($"Customer: {transaction.Customer?.FullName ?? "N/A"}");
                content.AppendLine($"Description: {transaction.Description ?? "N/A"}");
                content.AppendLine();
                content.AppendLine("Items:");
                if (transaction.Items != null)
                {
                    foreach (var item in transaction.Items)
                    {
                        content.AppendLine($"  - {item.Name} (Qty: {item.Quantity})");
                        content.AppendLine($"    SKU: {item.Sku}");
                        content.AppendLine($"    Unit Price: ${item.UnitPrice:F2}");
                        content.AppendLine($"    Subtotal: ${item.UnitPrice * item.Quantity:F2}");
                    }
                }
                content.AppendLine();
                content.AppendLine($"Subtotal: ${transaction.SubTotalAmount:F2}");
                content.AppendLine($"Discount: ${transaction.DiscountValue ?? 0:F2}");
                content.AppendLine($"Total: ${transaction.TotalAmount:F2}");
                content.AppendLine("-".PadRight(50, '-'));
                content.AppendLine();
            }

            var totalAmount = transactions.Sum(t => t.TotalAmount);
            var totalDiscount = transactions.Sum(t => t.DiscountValue ?? 0);
            var totalSubtotal = transactions.Sum(t => t.SubTotalAmount);

            content.AppendLine("SUMMARY");
            content.AppendLine("=".PadRight(50, '='));
            content.AppendLine($"Total Subtotal: ${totalSubtotal:F2}");
            content.AppendLine($"Total Discount: ${totalDiscount:F2}");
            content.AppendLine($"Total Amount: ${totalAmount:F2}");
            content.AppendLine();

            return content.ToString();
        }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
