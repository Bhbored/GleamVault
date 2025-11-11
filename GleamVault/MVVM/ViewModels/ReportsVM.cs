using GleamVault.Services.Interfaces;
using GleamVault.TestData;
using GleamVault.Utility;
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
using Microsoft.Maui.Graphics;

namespace GleamVault.MVVM.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public partial class ReportsVM : INotifyPropertyChanged
    {

        public ReportsVM(IGoldPriceService goldPriceService)
        {
            _goldPriceService = goldPriceService;
           
        }

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

        #region Properties 
        public bool IsLoading
        {
            get => _isLoading;
            set { _isLoading = value; OnPropertyChanged(); }
        }
        public ObservableCollection<Customer> AllCustomers
        {
            get => _allCustomers;
            set
            {
                _allCustomers = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<Transaction> AllTransactions
        {
            get => _allTransactions;
            set
            {
                _allTransactions = value;
                OnPropertyChanged();
            }
        }
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
        public float TotalGoldWeightSold { get; set; }
        public float TotalGoldWeightValue { get; set; }
        public ObservableCollection<SaleTypeData> GoldWeightBySaleType { get; set; } = new();
        public float TotalSalesRevenue { get; set; }
        public ObservableCollection<SaleTypeData> RevenueBySaleType { get; set; } = new();
        public int NewCustomers { get; set; }
        public int ReturningCustomers { get; set; }
        public int TotalCustomers { get; set; }
        public float OverallConversionRate { get; set; }
        public float NewCustomersChangePercent { get; set; }
        public float ReturningCustomersChangePercent { get; set; }
        public float TotalCustomersChangePercent { get; set; }
        public float ConversionRateChangePercent { get; set; }
        public ObservableCollection<SaleTypeCountData> CustomerCountBySaleType { get; set; } = new();
        public int TotalCustomerCount { get; set; }
        public float ECommerceRevenue { get; set; }
        public ObservableCollection<SaleTypeData> ECommerceRevenueByMaterial { get; set; } = new();
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

        #region Methods 
       

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

        #region Tasks
        private bool IsGoldHallmark(HallmarkType? hallmark)
        {
            if (!hallmark.HasValue) return false;
            return hallmark.Value == HallmarkType.Gold9K ||
                   hallmark.Value == HallmarkType.Gold10K ||
                   hallmark.Value == HallmarkType.Gold14K ||
                   hallmark.Value == HallmarkType.Gold18K ||
                   hallmark.Value == HallmarkType.Gold21K;
        }

        private float ConvertToGrams(float weight, WeightUnit? weightUnit)
        {
            if (!weightUnit.HasValue || weightUnit.Value == WeightUnit.Grams)
                return weight;

            return weightUnit.Value switch
            {
                WeightUnit.Carats => weight * 0.2f,
                WeightUnit.Ounces => weight * 28.3495f,
                WeightUnit.Pennyweight => weight * 1.55517f,
                WeightUnit.Kilograms => weight * 1000f,
                _ => weight
            };
        }

        private void CalculateGoldWeightMetrics()
        {
            var directSaleWeight = AllTransactions
                .Where(t => t.Type == TransactionType.Sell && t.Channel == SaleChannel.InStore)
                .SelectMany(t => t.Items ?? new List<TransactionItem>())
                .Where(i => IsGoldHallmark(i.Hallmark) && i.Weight.HasValue && i.Weight.Value > 0)
                .Sum(i => ConvertToGrams(i.Weight.Value, i.WeightUnit) * i.Quantity);

            var onlineWeight = AllTransactions
                .Where(t => t.Type == TransactionType.Sell && t.Channel == SaleChannel.Online)
                .SelectMany(t => t.Items ?? new List<TransactionItem>())
                .Where(i => IsGoldHallmark(i.Hallmark) && i.Weight.HasValue && i.Weight.Value > 0)
                .Sum(i => ConvertToGrams(i.Weight.Value, i.WeightUnit) * i.Quantity);

            var customOrderWeight = AllTransactions
                .Where(t => t.Type == TransactionType.CustomeOrder)
                .SelectMany(t => t.Items ?? new List<TransactionItem>())
                .Where(i => IsGoldHallmark(i.Hallmark) && i.Weight.HasValue && i.Weight.Value > 0)
                .Sum(i => ConvertToGrams(i.Weight.Value, i.WeightUnit) * i.Quantity);

            TotalGoldWeightSold = directSaleWeight + onlineWeight + customOrderWeight;
            TotalGoldWeightValue = TotalGoldWeightSold * (float)CurrentGoldPrice.Price;

            GoldWeightBySaleType.Clear();
            if (TotalGoldWeightSold > 0)
            {
                GoldWeightBySaleType.Add(new SaleTypeData
                {
                    Name = "Direct Sale",
                    Value = directSaleWeight,
                    Percentage = (directSaleWeight / TotalGoldWeightSold) * 100,
                    Brush = new SolidColorBrush(Color.FromArgb("#2D5016"))
                });
                GoldWeightBySaleType.Add(new SaleTypeData
                {
                    Name = "Online",
                    Value = onlineWeight,
                    Percentage = (onlineWeight / TotalGoldWeightSold) * 100,
                    Brush = new SolidColorBrush(Color.FromArgb("#7CB342"))
                });
                GoldWeightBySaleType.Add(new SaleTypeData
                {
                    Name = "Custom Order",
                    Value = customOrderWeight,
                    Percentage = (customOrderWeight / TotalGoldWeightSold) * 100,
                    Brush = new SolidColorBrush(Color.FromArgb("#8D6E63"))
                });
            }

            OnPropertyChanged(nameof(TotalGoldWeightSold));
            OnPropertyChanged(nameof(TotalGoldWeightValue));
            OnPropertyChanged(nameof(GoldWeightBySaleType));
        }

        private void CalculateRevenueMetrics()
        {
            var directSaleRevenue = AllTransactions
                .Where(t => t.Type == TransactionType.Sell && t.Channel == SaleChannel.InStore)
                .SelectMany(t => t.Items ?? new List<TransactionItem>())
                .Sum(i => i.UnitPrice * i.Quantity);

            var onlineRevenue = AllTransactions
                .Where(t => t.Type == TransactionType.Sell && t.Channel == SaleChannel.Online)
                .SelectMany(t => t.Items ?? new List<TransactionItem>())
                .Sum(i => i.UnitPrice * i.Quantity);

            var customOrderRevenue = AllTransactions
                .Where(t => t.Type == TransactionType.CustomeOrder)
                .SelectMany(t => t.Items ?? new List<TransactionItem>())
                .Sum(i => i.UnitPrice * i.Quantity);

            var repairementRevenue = AllTransactions
                .Where(t => t.Type == TransactionType.Repairement)
                .Sum(t => t.TotalAmount);

            TotalSalesRevenue = directSaleRevenue + onlineRevenue + customOrderRevenue + repairementRevenue;

            RevenueBySaleType.Clear();
            if (TotalSalesRevenue > 0)
            {
                RevenueBySaleType.Add(new SaleTypeData
                {
                    Name = "Direct Sale",
                    Value = directSaleRevenue,
                    Percentage = (directSaleRevenue / TotalSalesRevenue) * 100,
                    Brush = new SolidColorBrush(Color.FromArgb("#2D5016"))
                });
                RevenueBySaleType.Add(new SaleTypeData
                {
                    Name = "Online",
                    Value = onlineRevenue,
                    Percentage = (onlineRevenue / TotalSalesRevenue) * 100,
                    Brush = new SolidColorBrush(Color.FromArgb("#7CB342"))
                });
                RevenueBySaleType.Add(new SaleTypeData
                {
                    Name = "Custom Order",
                    Value = customOrderRevenue,
                    Percentage = (customOrderRevenue / TotalSalesRevenue) * 100,
                    Brush = new SolidColorBrush(Color.FromArgb("#8D6E63"))
                });
                RevenueBySaleType.Add(new SaleTypeData
                {
                    Name = "Repairement",
                    Value = repairementRevenue,
                    Percentage = (repairementRevenue / TotalSalesRevenue) * 100,
                    Brush = new SolidColorBrush(Color.FromArgb("#A1887F"))
                });
            }

            OnPropertyChanged(nameof(TotalSalesRevenue));
            OnPropertyChanged(nameof(RevenueBySaleType));
        }

        private void CalculateCustomerMetrics()
        {
            var now = DateTime.Now;
            var sevenDaysAgo = now.AddDays(-7);
            var thirtyDaysAgo = now.AddDays(-30);
            var previous30DaysStart = now.AddDays(-60);
            var previous30DaysEnd = now.AddDays(-30);

            var customerLatestTransactions = AllTransactions
                .Where(t => t.CustomerId.HasValue)
                .GroupBy(t => t.CustomerId.Value)
                .Select(g => new
                {
                    CustomerId = g.Key,
                    LatestTransactionDate = g.Max(t => t.CreatedAt)
                })
                .ToList();

            var newCustomerIds = customerLatestTransactions
                .Where(c => c.LatestTransactionDate >= sevenDaysAgo)
                .Select(c => c.CustomerId)
                .ToList();

            var returningCustomerIds = customerLatestTransactions
                .Where(c => c.LatestTransactionDate < thirtyDaysAgo)
                .Select(c => c.CustomerId)
                .ToList();

            var allCustomerIds = customerLatestTransactions
                .Select(c => c.CustomerId)
                .ToList();

            NewCustomers = newCustomerIds.Count;
            ReturningCustomers = returningCustomerIds.Count;
            TotalCustomers = allCustomerIds.Count;

            var previousCustomerLatestTransactions = AllTransactions
                .Where(t => t.CustomerId.HasValue && t.CreatedAt < previous30DaysEnd)
                .GroupBy(t => t.CustomerId.Value)
                .Select(g => new
                {
                    CustomerId = g.Key,
                    LatestTransactionDate = g.Max(t => t.CreatedAt)
                })
                .ToList();

            var previousSevenDaysAgo = previous30DaysEnd.AddDays(-7);
            var previousNewCustomers = previousCustomerLatestTransactions
                .Where(c => c.LatestTransactionDate >= previousSevenDaysAgo && c.LatestTransactionDate < previous30DaysEnd)
                .Count();

            var previousThirtyDaysAgo = previous30DaysEnd.AddDays(-30);
            var previousReturningCustomers = previousCustomerLatestTransactions
                .Where(c => c.LatestTransactionDate < previousThirtyDaysAgo)
                .Count();

            var previousTotalCustomers = previousCustomerLatestTransactions.Count;

            NewCustomersChangePercent = previousNewCustomers > 0 ? ((NewCustomers - previousNewCustomers) / (float)previousNewCustomers) * 100 : 0;
            ReturningCustomersChangePercent = previousReturningCustomers > 0 ? ((ReturningCustomers - previousReturningCustomers) / (float)previousReturningCustomers) * 100 : 0;
            TotalCustomersChangePercent = previousTotalCustomers > 0 ? ((TotalCustomers - previousTotalCustomers) / (float)previousTotalCustomers) * 100 : 0;

            var last30Days = now.AddDays(-30);
            var totalTransactions = AllTransactions.Count(t => t.CreatedAt >= last30Days);
            var salesTransactions = AllTransactions.Count(t => t.CreatedAt >= last30Days && (t.Type == TransactionType.Sell || t.Type == TransactionType.CustomeOrder));
            OverallConversionRate = totalTransactions > 0 ? (salesTransactions / (float)totalTransactions) * 100 : 0;

            var previousTotalTransactions = AllTransactions.Count(t => t.CreatedAt >= previous30DaysStart && t.CreatedAt < previous30DaysEnd);
            var previousSalesTransactions = AllTransactions.Count(t => t.CreatedAt >= previous30DaysStart && t.CreatedAt < previous30DaysEnd && (t.Type == TransactionType.Sell || t.Type == TransactionType.CustomeOrder));
            var previousConversionRate = previousTotalTransactions > 0 ? (previousSalesTransactions / (float)previousTotalTransactions) * 100 : 0;
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
            var directSaleCustomers = AllTransactions
                .Where(t => t.Type == TransactionType.Sell
                    && t.Channel == SaleChannel.InStore
                    && t.CustomerId.HasValue)
                .Select(t => t.CustomerId.Value)
                .Distinct()
                .Count();

            var onlineCustomers = AllTransactions
                .Where(t => t.Type == TransactionType.Sell
                    && t.Channel == SaleChannel.Online
                    && t.CustomerId.HasValue)
                .Select(t => t.CustomerId.Value)
                .Distinct()
                .Count();

            var customOrderCustomers = AllTransactions
                .Where(t => t.Type == TransactionType.CustomeOrder
                    && t.CustomerId.HasValue)
                .Select(t => t.CustomerId.Value)
                .Distinct()
                .Count();

            var repairementCustomers = AllTransactions
                .Where(t => t.Type == TransactionType.Repairement
                    && t.CustomerId.HasValue)
                .Select(t => t.CustomerId.Value)
                .Distinct()
                .Count();

            TotalCustomerCount = directSaleCustomers + onlineCustomers + customOrderCustomers + repairementCustomers;

            CustomerCountBySaleType.Clear();
            CustomerCountBySaleType.Add(new SaleTypeCountData
            {
                Name = "Direct Sale",
                Count = directSaleCustomers,
                Color = Color.FromArgb("#4ECDC4")
            });
            CustomerCountBySaleType.Add(new SaleTypeCountData
            {
                Name = "Online",
                Count = onlineCustomers,
                Color = Color.FromArgb("#45B7D1")
            });
            CustomerCountBySaleType.Add(new SaleTypeCountData
            {
                Name = "Custom Order",
                Count = customOrderCustomers,
                Color = Color.FromArgb("#96CEB4")
            });
            CustomerCountBySaleType.Add(new SaleTypeCountData
            {
                Name = "Repairement",
                Count = repairementCustomers,
                Color = Color.FromArgb("#FF6B8E")
            });

            OnPropertyChanged(nameof(TotalCustomerCount));
            OnPropertyChanged(nameof(CustomerCountBySaleType));
        }

        private void CalculateECommerceMetrics()
        {
            var onlineTransactions = AllTransactions
                .Where(t => t.Channel == SaleChannel.Online)
                .ToList();

            var onlineItems = onlineTransactions
                .SelectMany(t => t.Items ?? new List<TransactionItem>())
                .ToList();

            var goldRevenue = onlineItems
                .Where(i => IsGoldHallmark(i.Hallmark))
                .Sum(i =>
                {
                    var price = i.OfferPrice > 0 ? i.OfferPrice : i.UnitPrice;
                    return (price - i.UnitCost) * i.Quantity;
                });

            var silverRevenue = onlineItems
                .Where(i => i.Hallmark == HallmarkType.Sterling925)
                .Sum(i =>
                {
                    var price = i.OfferPrice > 0 ? i.OfferPrice : i.UnitPrice;
                    return (price - i.UnitCost) * i.Quantity;
                });

            var luxuryRevenue = onlineItems
                .Where(i => i.Hallmark == HallmarkType.LuxuryBrands)
                .Sum(i =>
                {
                    var price = i.OfferPrice > 0 ? i.OfferPrice : i.UnitPrice;
                    return (price - i.UnitCost) * i.Quantity;
                });

            ECommerceRevenue = goldRevenue + silverRevenue + luxuryRevenue;

            ECommerceRevenueByMaterial.Clear();
            if (ECommerceRevenue > 0)
            {
                if (goldRevenue > 0)
                    ECommerceRevenueByMaterial.Add(new SaleTypeData
                    {
                        Name = "Gold",
                        Value = goldRevenue,
                        Percentage = (goldRevenue / ECommerceRevenue) * 100,
                        Brush = new SolidColorBrush(Color.FromArgb("#FFD700"))
                    });
                if (silverRevenue > 0)
                    ECommerceRevenueByMaterial.Add(new SaleTypeData
                    {
                        Name = "Silver",
                        Value = silverRevenue,
                        Percentage = (silverRevenue / ECommerceRevenue) * 100,
                        Brush = new SolidColorBrush(Color.FromArgb("#C0C0C0"))
                    });
                if (luxuryRevenue > 0)
                    ECommerceRevenueByMaterial.Add(new SaleTypeData
                    {
                        Name = "Luxury",
                        Value = luxuryRevenue,
                        Percentage = (luxuryRevenue / ECommerceRevenue) * 100,
                        Brush = new SolidColorBrush(Color.FromArgb("#8B4513"))
                    });
            }

            OnPropertyChanged(nameof(ECommerceRevenue));
            OnPropertyChanged(nameof(ECommerceRevenueByMaterial));
        }
        #endregion

        #region Helper Classes
        public class SaleTypeData
        {
            public string Name { get; set; } = string.Empty;
            private float _value;
            public float Value
            {
                get => _value;
                set
                {
                    _value = value;
                    DisplayValue = value.ToString("#,##0");
                }
            }
            public string DisplayValue { get; private set; } = string.Empty;
            public float Percentage { get; set; }
            public Brush Brush { get; set; }
            public string FormattedValue => Value.ToString("#,##0");
            public string Label => Value.ToString("#,##0");

            public override string ToString() => DisplayValue;
        }

        public class SaleTypeCountData
        {
            public string Name { get; set; } = string.Empty;
            public int Count { get; set; }
            public Color Color { get; set; } = Colors.Gray;
        }
        #endregion


        public async Task LoadDataAsync()
        {
            _isLoading = true;
            OnPropertyChanged(nameof(IsLoading));

            ClearAll();

            var products = TestProducts.GetProducts();
            var customers = TestProducts.GetCustomers();
            TestTransactions.GenerateTestTransactions(products, customers);
            AllTransactions = new ObservableCollection<Transaction>(TestTransactions.Transactions);
            AllCustomers = new ObservableCollection<Customer>(customers);

            await LoadGoldPriceDataAsync();
            CalculateGoldWeightMetrics();
            CalculateRevenueMetrics();
            CalculateCustomerMetrics();
            CalculateCustomerCountBySaleType();
            CalculateECommerceMetrics();

            InitializeCommands();
            InitializePriceTimer();
            _isLoading = false;
            OnPropertyChanged(nameof(IsLoading));
        }

        private void ClearAll()
        {
            AllCustomers.Clear();
            AllTransactions.Clear();
            GoldWeightBySaleType.Clear();
            RevenueBySaleType.Clear();
            CustomerCountBySaleType.Clear();
            ECommerceRevenueByMaterial.Clear();
        }
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

      
        #endregion
    }
}
