using GleamVault.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace GleamVault.Services
{
    //anwayrooo
    public class GoldPriceService : IGoldPriceService
    {
        private readonly HttpClient _httpClient;
        private decimal _previousPrice = 0m;
        private DateTime _lastUpdateTime = DateTime.MinValue;

        public GoldPriceService()
        {
            _httpClient = new HttpClient();
            _httpClient.Timeout = TimeSpan.FromSeconds(10);
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0");
        }

        public async Task<GoldPriceData> GetGoldPriceAsync()
        {
            try
            {
                var priceData = await GetPriceFromExchangeRateAPIAsync();
                if (priceData != null && priceData.Price > 1000m && priceData.Price < 10000m)
                {
                    return priceData;
                }
            }
            catch
            {
            }

            try
            {
                var priceData = await GetPriceFromGoldPriceAPIAsync();
                if (priceData != null && priceData.Price > 1000m && priceData.Price < 10000m)
                {
                    return priceData;
                }
            }
            catch
            {
            }

            try
            {
                var priceData = await GetPriceFromMetalPriceAPIAsync();
                if (priceData != null && priceData.Price > 1000m && priceData.Price < 10000m)
                {
                    return priceData;
                }
            }
            catch
            {
            }

            if (_previousPrice > 0)
            {
                return CreatePriceData(_previousPrice);
            }

            return new GoldPriceData
            {
                Price = 4090m,
                Currency = "USD",
                Timestamp = DateTime.Now,
                ChangePercent = 0m,
                Open = 4090m,
                High = 4090m,
                Low = 4090m,
                Close = 4090m
            };
        }

        private async Task<GoldPriceData> GetPriceFromMetalPriceAPIAsync()
        {
            try
            {
                var url = "https://api.metals.live/v1/spot/gold";
                var response = await _httpClient.GetStringAsync(url);
                var jsonDoc = JsonDocument.Parse(response);
                var root = jsonDoc.RootElement;

                if (root.TryGetProperty("price", out var priceElement))
                {
                    var price = priceElement.GetDecimal();
                    return CreatePriceData(price);
                }
            }
            catch
            {
            }
            return null;
        }

        private async Task<GoldPriceData> GetPriceFromExchangeRateAPIAsync()
        {
            try
            {
                var url = "https://api.exchangerate-api.com/v4/latest/XAU";
                var response = await _httpClient.GetStringAsync(url);
                var jsonDoc = JsonDocument.Parse(response);
                var root = jsonDoc.RootElement;

                decimal price = 0m;
                if (root.TryGetProperty("rates", out var rates))
                {
                    if (rates.TryGetProperty("USD", out var usdRate))
                    {
                        price = usdRate.GetDecimal();
                    }
                }

                if (price > 0)
                {
                    return CreatePriceData(price);
                }
            }
            catch
            {
            }
            return null;
        }

        private async Task<GoldPriceData> GetPriceFromGoldPriceAPIAsync()
        {
            try
            {
                var url = "https://www.goldapi.io/api/XAU/USD";
                var response = await _httpClient.GetStringAsync(url);
                var jsonDoc = JsonDocument.Parse(response);
                var root = jsonDoc.RootElement;

                if (root.TryGetProperty("price", out var priceElement))
                {
                    var price = priceElement.GetDecimal();
                    if (price > 0)
                    {
                        return CreatePriceData(price);
                    }
                }
            }
            catch
            {
            }
            return null;
        }

        private GoldPriceData CreatePriceData(decimal price)
        {
            var now = DateTime.Now;
            decimal changePercent = 0m;

            if (_previousPrice > 0 && (now - _lastUpdateTime).TotalSeconds < 60)
            {
                changePercent = ((price - _previousPrice) / _previousPrice) * 100m;
            }

            _previousPrice = price;
            _lastUpdateTime = now;

            var open = _previousPrice > 0 ? _previousPrice : price * 0.9995m;
            var high = price * 1.002m;
            var low = price * 0.998m;

            return new GoldPriceData
            {
                Price = price,
                Currency = "USD",
                Timestamp = now,
                ChangePercent = changePercent,
                Open = open,
                High = high,
                Low = low,
                Close = price
            };
        }

        public async Task<List<GoldPriceHistoryPoint>> GetGoldPriceHistoryAsync(string timeframe = "1D", int limit = 100)
        {
            try
            {
                var currentPriceData = await GetGoldPriceAsync();
                var basePrice = currentPriceData.Price;
                var history = new List<GoldPriceHistoryPoint>();
                var random = new Random();
                var now = DateTime.Now;

                bool isLessThanDay = timeframe switch
                {
                    "1m" or "5m" or "15m" or "30m" or "1H" or "4H" => true,
                    _ => false
                };

                DateTime startTime;
                int intervals;

                if (isLessThanDay)
                {
                    var startOfDay = now.Date;

                    int totalIntervals = timeframe switch
                    {
                        "1m" => (int)(now - startOfDay).TotalMinutes,
                        "5m" => (int)(now - startOfDay).TotalMinutes / 5,
                        "15m" => (int)(now - startOfDay).TotalMinutes / 15,
                        "30m" => (int)(now - startOfDay).TotalMinutes / 30,
                        "1H" => (int)(now - startOfDay).TotalHours,
                        "4H" => (int)(now - startOfDay).TotalHours / 4,
                        _ => limit
                    };

                    if (totalIntervals > limit)
                    {
                        intervals = limit;
                        startTime = timeframe switch
                        {
                            "1m" => now.AddMinutes(-limit),
                            "5m" => now.AddMinutes(-limit * 5),
                            "15m" => now.AddMinutes(-limit * 15),
                            "30m" => now.AddMinutes(-limit * 30),
                            "1H" => now.AddHours(-limit),
                            "4H" => now.AddHours(-limit * 4),
                            _ => now.AddDays(-limit)
                        };
                    }
                    else
                    {
                        intervals = totalIntervals;
                        startTime = startOfDay;
                    }
                }
                else
                {
                    startTime = timeframe switch
                    {
                        "1D" => now.AddDays(-limit),
                        "1W" => now.AddDays(-limit * 7),
                        "1M" => now.AddDays(-limit * 30),
                        _ => now.AddDays(-limit)
                    };

                    intervals = limit;
                }

                intervals = Math.Max(1, intervals);

                var price = basePrice;
                for (int i = 0; i <= intervals; i++)
                {
                    DateTime date = timeframe switch
                    {
                        "1m" => startTime.AddMinutes(i),
                        "5m" => startTime.AddMinutes(i * 5),
                        "15m" => startTime.AddMinutes(i * 15),
                        "30m" => startTime.AddMinutes(i * 30),
                        "1H" => startTime.AddHours(i),
                        "4H" => startTime.AddHours(i * 4),
                        "1D" => startTime.AddDays(i),
                        "1W" => startTime.AddDays(i * 7),
                        "1M" => startTime.AddDays(i * 30),
                        _ => startTime.AddDays(i)
                    };

                    if (i == intervals)
                    {
                        date = now;
                    }

                    var trend = (decimal)(random.NextDouble() * 0.02 - 0.01);
                    var volatility = (decimal)(random.NextDouble() * 0.015);

                    var open = price;
                    var change = price * trend;
                    price = price + change;
                    var high = price * (1 + volatility);
                    var low = price * (1 - volatility);
                    var close = price + (decimal)(random.NextDouble() * 0.01 - 0.005) * price;
                    var volume = (decimal)(random.NextDouble() * 1000000 + 500000);

                    history.Add(new GoldPriceHistoryPoint
                    {
                        Date = date,
                        Price = close,
                        Open = open,
                        High = high,
                        Low = low,
                        Close = close,
                        Volume = volume
                    });
                }

                if (history.Count > 0)
                {
                    var lastPoint = history[^1];
                    lastPoint.Date = now;
                    lastPoint.Close = basePrice;
                    lastPoint.Price = basePrice;
                    lastPoint.Open = basePrice * 0.9995m;
                    lastPoint.High = basePrice * 1.002m;
                    lastPoint.Low = basePrice * 0.998m;
                }

                return history;
            }
            catch
            {
                var fallbackHistory = new List<GoldPriceHistoryPoint>();
                var basePrice = 4000m;
                var random = new Random();
                var now = DateTime.Now;

                for (int i = 0; i <= limit; i++)
                {
                    DateTime date = now.AddDays(-(limit - i));
                    if (i == limit)
                    {
                        date = now;
                    }
                    var variation = (decimal)(random.NextDouble() * 0.02 - 0.01);
                    var price = basePrice * (1 + variation);

                    fallbackHistory.Add(new GoldPriceHistoryPoint
                    {
                        Date = date,
                        Price = price,
                        Open = price * 0.999m,
                        High = price * 1.002m,
                        Low = price * 0.998m,
                        Close = price,
                        Volume = (decimal)(random.NextDouble() * 1000000 + 500000)
                    });
                }

                if (fallbackHistory.Count > 0)
                {
                    var lastPoint = fallbackHistory[^1];
                    lastPoint.Date = now;
                    lastPoint.Close = basePrice;
                    lastPoint.Price = basePrice;
                }

                return fallbackHistory;
            }
        }
    }
}

