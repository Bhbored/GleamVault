using GleamVault.Services.Interfaces;
using GleamVault.TestData;
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
using Microsoft.Maui.Controls;
using System.Timers;
using Microsoft.Maui.ApplicationModel;

namespace GleamVault.MVVM.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public partial class ReportsVM : INotifyPropertyChanged
    {
        #region Fields
        private readonly IGoldPriceService _goldPriceService;
        private ObservableCollection<Transaction> _allTransactions = new();
        private ObservableCollection<Customer> _allCustomers = new();
        private GoldPriceData _currentGoldPrice = new();
        private ObservableCollection<GoldPriceHistoryPoint> _goldPriceHistory = new();
        private bool _isLoading = true;
        private System.Timers.Timer _priceUpdateTimer;
        private Random _priceRandom = new Random();
        #endregion

        #region Properties - Gold Price
        public GoldPriceData CurrentGoldPrice
        {
            get => _currentGoldPrice;
            set { _currentGoldPrice = value; OnPropertyChanged(); }
        }

        public ObservableCollection<GoldPriceHistoryPoint> GoldPriceHistory
        {
            get => _goldPriceHistory;
            set { _goldPriceHistory = value; OnPropertyChanged(); }
        }

        private string _selectedTimeframe = "1D";
        public string SelectedTimeframe
        {
            get => _selectedTimeframe;
            set
            {
                if (_selectedTimeframe != value)
                {
                    _selectedTimeframe = value;
                    OnPropertyChanged();
                    _ = LoadGoldPriceHistoryAsync();
                }
            }
        }

        public List<string> Timeframes { get; } = new List<string> { "1m", "5m", "15m", "30m", "1H", "4H", "1D", "1W", "1M" };
        #endregion

        #region Properties - Total Gold Weight Sold
        public float TotalGoldWeightSold { get; set; }
        public float TotalGoldWeightValue { get; set; }
        public ObservableCollection<SaleTypeData> GoldWeightBySaleType { get; set; } = new();
        #endregion

        #region Properties - Total Sales Revenue
        public float TotalSalesRevenue { get; set; }
        public ObservableCollection<SaleTypeData> RevenueBySaleType { get; set; } = new();
        #endregion

        #region Properties - Customer Metrics
        public int NewCustomers { get; set; }
        public int ReturningCustomers { get; set; }
        public int TotalCustomers { get; set; }
        public float OverallConversionRate { get; set; }
        public float NewCustomersChangePercent { get; set; }
        public float ReturningCustomersChangePercent { get; set; }
        public float TotalCustomersChangePercent { get; set; }
        public float ConversionRateChangePercent { get; set; }
        #endregion

        #region Properties - Customer Count by Sale Type
        public ObservableCollection<SaleTypeCountData> CustomerCountBySaleType { get; set; } = new();
        public int TotalCustomerCount { get; set; }
        #endregion

        #region Properties - E-Commerce Sales
        public float ECommerceRevenue { get; set; }
        public float ECommerceGoldWeight { get; set; }
        public ObservableCollection<ChannelData> ECommerceRevenueByChannel { get; set; } = new();
        public ObservableCollection<ChannelData> ECommerceGoldWeightByChannel { get; set; } = new();
        #endregion

        #region Constructor
        public ReportsVM(IGoldPriceService goldPriceService)
        {
            _goldPriceService = goldPriceService;
            InitializeCommands();
            InitializePriceTimer();
        }
        #endregion

        #region Commands
        public ICommand SelectTimeframeCommand { get; private set; }

        private void InitializeCommands()
        {
            SelectTimeframeCommand = new Command<string>(async (timeframe) =>
            {
                SelectedTimeframe = timeframe;
                await LoadGoldPriceHistoryAsync();
            });
        }
        #endregion

        #region Methods - Data Loading
        public async Task LoadDataAsync()
        {
            _isLoading = true;
            OnPropertyChanged(nameof(IsLoading));

            var products = TestProducts.GetProducts();
            var customers = TestProducts.GetCustomers();
            TestTransactions.GenerateTestTransactions(products, customers);

            _allTransactions = TestTransactions.Transactions;
            _allCustomers = new ObservableCollection<Customer>(customers);

            await LoadGoldPriceDataAsync();
            CalculateGoldWeightMetrics();
            CalculateRevenueMetrics();
            CalculateCustomerMetrics();
            CalculateCustomerCountBySaleType();
            CalculateECommerceMetrics();

            _isLoading = false;
            OnPropertyChanged(nameof(IsLoading));
        }

        private async Task LoadGoldPriceDataAsync()
        {
            try
            {
                var priceData = await _goldPriceService.GetGoldPriceAsync();
                CurrentGoldPrice = priceData;
                await LoadGoldPriceHistoryAsync();
            }
            catch
            {
                CurrentGoldPrice = new GoldPriceData { Price = 4000m, Currency = "USD", Timestamp = DateTime.Now, ChangePercent = 0.5m };
                GoldPriceHistory = new ObservableCollection<GoldPriceHistoryPoint>();
            }
        }

        private void InitializePriceTimer()
        {
            _priceUpdateTimer = new System.Timers.Timer(3000);
            _priceUpdateTimer.Elapsed += async (sender, e) => await UpdatePriceAsync();
            _priceUpdateTimer.AutoReset = true;
            _priceUpdateTimer.Start();
        }

        private async Task UpdatePriceAsync()
        {
            try
            {
                var newPriceData = await _goldPriceService.GetGoldPriceAsync();
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    if (CurrentGoldPrice != null && CurrentGoldPrice.Price > 0)
                    {
                        var variation = (decimal)(_priceRandom.NextDouble() * 0.004 - 0.002);
                        var newPrice = newPriceData.Price * (1 + variation);
                        var previousPrice = CurrentGoldPrice.Price;
                        var changePercent = ((newPrice - previousPrice) / previousPrice) * 100m;

                        CurrentGoldPrice = new GoldPriceData
                        {
                            Price = newPrice,
                            Currency = newPriceData.Currency,
                            Timestamp = DateTime.Now,
                            ChangePercent = changePercent,
                            Open = previousPrice,
                            High = Math.Max(newPrice, previousPrice) * 1.001m,
                            Low = Math.Min(newPrice, previousPrice) * 0.999m,
                            Close = newPrice
                        };

                        if (GoldPriceHistory.Count > 0)
                        {
                            var lastPoint = GoldPriceHistory[^1];
                            lastPoint.Date = DateTime.Now;
                            lastPoint.Close = newPrice;
                            lastPoint.Price = newPrice;
                            lastPoint.High = Math.Max(lastPoint.High, newPrice);
                            lastPoint.Low = Math.Min(lastPoint.Low, newPrice);
                        }
                    }
                    else
                    {
                        CurrentGoldPrice = newPriceData;
                    }
                });
            }
            catch
            {
            }
        }

        private async Task LoadGoldPriceHistoryAsync()
        {
            try
            {
                var history = await _goldPriceService.GetGoldPriceHistoryAsync(SelectedTimeframe, 100);
                GoldPriceHistory = new ObservableCollection<GoldPriceHistoryPoint>(history);
            }
            catch
            {
                GoldPriceHistory = new ObservableCollection<GoldPriceHistoryPoint>();
            }
        }
        #endregion

        #region Methods - Calculations
        private void CalculateGoldWeightMetrics()
        {
            var directSaleWeight = _allTransactions
                .Where(t => t.Description == "Direct Sale" && t.Type == TransactionType.Sell)
                .SelectMany(t => t.Items ?? new List<TransactionItem>())
                .Where(i => i.WeightUnit == WeightUnit.Grams)
                .Sum(i => (i.Weight ?? 0) * i.Quantity);

            var bespokeWeight = _allTransactions
                .Where(t => t.Description == "Bespoke" && t.Type == TransactionType.CustomeOrder)
                .SelectMany(t => t.Items ?? new List<TransactionItem>())
                .Where(i => i.WeightUnit == WeightUnit.Grams)
                .Sum(i => (i.Weight ?? 0) * i.Quantity);

            var goldBookingWeight = _allTransactions
                .Where(t => t.Description == "Gold Booking" && t.Type == TransactionType.Sell)
                .SelectMany(t => t.Items ?? new List<TransactionItem>())
                .Where(i => i.WeightUnit == WeightUnit.Grams)
                .Sum(i => (i.Weight ?? 0) * i.Quantity);

            TotalGoldWeightSold = directSaleWeight + bespokeWeight + goldBookingWeight;
            TotalGoldWeightValue = TotalGoldWeightSold * (float)CurrentGoldPrice.Price;

            GoldWeightBySaleType.Clear();
            GoldWeightBySaleType.Add(new SaleTypeData { Name = "Direct Sale", Value = directSaleWeight, Percentage = TotalGoldWeightSold > 0 ? (directSaleWeight / TotalGoldWeightSold) * 100 : 0 });
            GoldWeightBySaleType.Add(new SaleTypeData { Name = "Bespoke", Value = bespokeWeight, Percentage = TotalGoldWeightSold > 0 ? (bespokeWeight / TotalGoldWeightSold) * 100 : 0 });
            GoldWeightBySaleType.Add(new SaleTypeData { Name = "Gold Booking", Value = goldBookingWeight, Percentage = TotalGoldWeightSold > 0 ? (goldBookingWeight / TotalGoldWeightSold) * 100 : 0 });

            OnPropertyChanged(nameof(TotalGoldWeightSold));
            OnPropertyChanged(nameof(TotalGoldWeightValue));
            OnPropertyChanged(nameof(GoldWeightBySaleType));
        }

        private void CalculateRevenueMetrics()
        {
            var directSaleRevenue = _allTransactions
                .Where(t => t.Description == "Direct Sale" && t.Type == TransactionType.Sell)
                .Sum(t => t.TotalAmount);

            var bespokeRevenue = _allTransactions
                .Where(t => t.Description == "Bespoke" && t.Type == TransactionType.CustomeOrder)
                .Sum(t => t.TotalAmount);

            var repairRevenue = _allTransactions
                .Where(t => t.Description == "Repair" && t.Type == TransactionType.Repairement)
                .Sum(t => t.TotalAmount);

            var goldBookingRevenue = _allTransactions
                .Where(t => t.Description == "Gold Booking" && t.Type == TransactionType.Sell)
                .Sum(t => t.TotalAmount);

            var giftCardRevenue = _allTransactions
                .Where(t => t.Description == "Gift Card" && t.Type == TransactionType.Sell)
                .Sum(t => t.TotalAmount);

            TotalSalesRevenue = directSaleRevenue + bespokeRevenue + repairRevenue + goldBookingRevenue + giftCardRevenue;

            RevenueBySaleType.Clear();
            if (TotalSalesRevenue > 0)
            {
                RevenueBySaleType.Add(new SaleTypeData { Name = "Direct Sale", Value = directSaleRevenue, Percentage = (directSaleRevenue / TotalSalesRevenue) * 100 });
                RevenueBySaleType.Add(new SaleTypeData { Name = "Bespoke", Value = bespokeRevenue, Percentage = (bespokeRevenue / TotalSalesRevenue) * 100 });
                RevenueBySaleType.Add(new SaleTypeData { Name = "Repair", Value = repairRevenue, Percentage = (repairRevenue / TotalSalesRevenue) * 100 });
                RevenueBySaleType.Add(new SaleTypeData { Name = "Gold Booking", Value = goldBookingRevenue, Percentage = (goldBookingRevenue / TotalSalesRevenue) * 100 });
                RevenueBySaleType.Add(new SaleTypeData { Name = "Gift Cards", Value = giftCardRevenue, Percentage = (giftCardRevenue / TotalSalesRevenue) * 100 });
            }

            OnPropertyChanged(nameof(TotalSalesRevenue));
            OnPropertyChanged(nameof(RevenueBySaleType));
        }

        private void CalculateCustomerMetrics()
        {
            var last30Days = DateTime.Now.AddDays(-30);
            var previous30Days = DateTime.Now.AddDays(-60);

            var recentCustomers = _allTransactions
                .Where(t => t.CreatedAt >= last30Days && t.CustomerId.HasValue)
                .Select(t => t.CustomerId.Value)
                .Distinct()
                .ToList();

            var previousCustomers = _allTransactions
                .Where(t => t.CreatedAt >= previous30Days && t.CreatedAt < last30Days && t.CustomerId.HasValue)
                .Select(t => t.CustomerId.Value)
                .Distinct()
                .ToList();

            var allCustomerIds = _allTransactions
                .Where(t => t.CustomerId.HasValue)
                .Select(t => t.CustomerId.Value)
                .Distinct()
                .ToList();

            var newCustomerIds = recentCustomers.Except(previousCustomers).ToList();
            var returningCustomerIds = recentCustomers.Intersect(previousCustomers).ToList();

            NewCustomers = newCustomerIds.Count;
            ReturningCustomers = returningCustomerIds.Count;
            TotalCustomers = allCustomerIds.Count;

            var previousNewCustomers = previousCustomers.Except(
                _allTransactions
                    .Where(t => t.CreatedAt >= previous30Days.AddDays(-30) && t.CreatedAt < previous30Days && t.CustomerId.HasValue)
                    .Select(t => t.CustomerId.Value)
                    .Distinct()
            ).Count();

            var previousReturningCustomers = previousCustomers.Count;
            var previousTotalCustomers = _allTransactions
                .Where(t => t.CreatedAt < last30Days && t.CustomerId.HasValue)
                .Select(t => t.CustomerId.Value)
                .Distinct()
                .Count();

            NewCustomersChangePercent = previousNewCustomers > 0 ? ((NewCustomers - previousNewCustomers) / (float)previousNewCustomers) * 100 : 0;
            ReturningCustomersChangePercent = previousReturningCustomers > 0 ? ((ReturningCustomers - previousReturningCustomers) / (float)previousReturningCustomers) * 100 : 0;
            TotalCustomersChangePercent = previousTotalCustomers > 0 ? ((TotalCustomers - previousTotalCustomers) / (float)previousTotalCustomers) * 100 : 0;

            var totalVisits = _allTransactions.Count(t => t.CreatedAt >= last30Days);
            var conversions = _allTransactions.Count(t => t.CreatedAt >= last30Days && t.Type == TransactionType.Sell);
            OverallConversionRate = totalVisits > 0 ? (conversions / (float)totalVisits) * 100 : 0;

            var previousTotalVisits = _allTransactions.Count(t => t.CreatedAt >= previous30Days && t.CreatedAt < last30Days);
            var previousConversions = _allTransactions.Count(t => t.CreatedAt >= previous30Days && t.CreatedAt < last30Days && t.Type == TransactionType.Sell);
            var previousConversionRate = previousTotalVisits > 0 ? (previousConversions / (float)previousTotalVisits) * 100 : 0;
            ConversionRateChangePercent = previousConversionRate > 0 ? ((OverallConversionRate - previousConversionRate) / previousConversionRate) * 100 : 0;

            OnPropertyChanged(nameof(NewCustomers));
            OnPropertyChanged(nameof(ReturningCustomers));
            OnPropertyChanged(nameof(TotalCustomers));
            OnPropertyChanged(nameof(OverallConversionRate));
            OnPropertyChanged(nameof(NewCustomersChangePercent));
            OnPropertyChanged(nameof(ReturningCustomersChangePercent));
            OnPropertyChanged(nameof(TotalCustomersChangePercent));
            OnPropertyChanged(nameof(ConversionRateChangePercent));
        }

        private void CalculateCustomerCountBySaleType()
        {
            var directSaleCustomers = _allTransactions
                .Where(t => t.Description == "Direct Sale" && t.CustomerId.HasValue)
                .Select(t => t.CustomerId.Value)
                .Distinct()
                .Count();

            var bespokeCustomers = _allTransactions
                .Where(t => t.Description == "Bespoke" && t.CustomerId.HasValue)
                .Select(t => t.CustomerId.Value)
                .Distinct()
                .Count();

            var appraisalCustomers = _allTransactions
                .Where(t => t.Description == "Appraisal" && t.CustomerId.HasValue)
                .Select(t => t.CustomerId.Value)
                .Distinct()
                .Count();

            var repairCustomers = _allTransactions
                .Where(t => t.Description == "Repair" && t.CustomerId.HasValue)
                .Select(t => t.CustomerId.Value)
                .Distinct()
                .Count();

            var goldBookingCustomers = _allTransactions
                .Where(t => t.Description == "Gold Booking" && t.CustomerId.HasValue)
                .Select(t => t.CustomerId.Value)
                .Distinct()
                .Count();

            var giftCardCustomers = _allTransactions
                .Where(t => t.Description == "Gift Card" && t.CustomerId.HasValue)
                .Select(t => t.CustomerId.Value)
                .Distinct()
                .Count();

            TotalCustomerCount = directSaleCustomers + bespokeCustomers + appraisalCustomers + repairCustomers + goldBookingCustomers + giftCardCustomers;

            CustomerCountBySaleType.Clear();
            CustomerCountBySaleType.Add(new SaleTypeCountData { Name = "Direct Sale", Count = directSaleCustomers });
            CustomerCountBySaleType.Add(new SaleTypeCountData { Name = "Bespoke", Count = bespokeCustomers });
            CustomerCountBySaleType.Add(new SaleTypeCountData { Name = "Appraisal", Count = appraisalCustomers });
            CustomerCountBySaleType.Add(new SaleTypeCountData { Name = "Repair", Count = repairCustomers });
            CustomerCountBySaleType.Add(new SaleTypeCountData { Name = "Gold Booking", Count = goldBookingCustomers });
            CustomerCountBySaleType.Add(new SaleTypeCountData { Name = "Gift Card", Count = giftCardCustomers });

            OnPropertyChanged(nameof(TotalCustomerCount));
            OnPropertyChanged(nameof(CustomerCountBySaleType));
        }

        private void CalculateECommerceMetrics()
        {
            var onlineTransactions = _allTransactions
                .Where(t => t.Channel == SaleChannel.Online)
                .ToList();

            ECommerceRevenue = onlineTransactions.Sum(t => t.TotalAmount);

            ECommerceGoldWeight = onlineTransactions
                .SelectMany(t => t.Items ?? new List<TransactionItem>())
                .Where(i => i.WeightUnit == WeightUnit.Grams)
                .Sum(i => (i.Weight ?? 0) * i.Quantity);

            var websiteRevenue = onlineTransactions
                .Where(t => t.Description == "Direct Sale" || t.Description == "Bespoke")
                .Sum(t => t.TotalAmount);

            var appRevenue = onlineTransactions
                .Where(t => t.Description == "Gold Booking" || t.Description == "Gift Card")
                .Sum(t => t.TotalAmount);

            var websiteWeight = onlineTransactions
                .Where(t => t.Description == "Direct Sale" || t.Description == "Bespoke")
                .SelectMany(t => t.Items ?? new List<TransactionItem>())
                .Where(i => i.WeightUnit == WeightUnit.Grams)
                .Sum(i => (i.Weight ?? 0) * i.Quantity);

            var appWeight = onlineTransactions
                .Where(t => t.Description == "Gold Booking" || t.Description == "Gift Card")
                .SelectMany(t => t.Items ?? new List<TransactionItem>())
                .Where(i => i.WeightUnit == WeightUnit.Grams)
                .Sum(i => (i.Weight ?? 0) * i.Quantity);

            ECommerceRevenueByChannel.Clear();
            if (ECommerceRevenue > 0)
            {
                ECommerceRevenueByChannel.Add(new ChannelData { Name = "Website", Value = websiteRevenue, Percentage = (websiteRevenue / ECommerceRevenue) * 100 });
                ECommerceRevenueByChannel.Add(new ChannelData { Name = "App", Value = appRevenue, Percentage = (appRevenue / ECommerceRevenue) * 100 });
            }

            ECommerceGoldWeightByChannel.Clear();
            if (ECommerceGoldWeight > 0)
            {
                ECommerceGoldWeightByChannel.Add(new ChannelData { Name = "Website", Value = websiteWeight, Percentage = (websiteWeight / ECommerceGoldWeight) * 100 });
                ECommerceGoldWeightByChannel.Add(new ChannelData { Name = "App", Value = appWeight, Percentage = (appWeight / ECommerceGoldWeight) * 100 });
            }

            OnPropertyChanged(nameof(ECommerceRevenue));
            OnPropertyChanged(nameof(ECommerceGoldWeight));
            OnPropertyChanged(nameof(ECommerceRevenueByChannel));
            OnPropertyChanged(nameof(ECommerceGoldWeightByChannel));
        }
        #endregion

        #region Helper Classes
        public class SaleTypeData
        {
            public string Name { get; set; } = string.Empty;
            public float Value { get; set; }
            public float Percentage { get; set; }
        }

        public class SaleTypeCountData
        {
            public string Name { get; set; } = string.Empty;
            public int Count { get; set; }
        }

        public class ChannelData
        {
            public string Name { get; set; } = string.Empty;
            public float Value { get; set; }
            public float Percentage { get; set; }
        }
        #endregion

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public bool IsLoading
        {
            get => _isLoading;
            set { _isLoading = value; OnPropertyChanged(); }
        }
        #endregion
    }
}
