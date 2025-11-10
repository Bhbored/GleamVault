using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GleamVault.Services.Interfaces
{
    public interface IGoldPriceService
    {
        Task<GoldPriceData> GetGoldPriceAsync();
        Task<List<GoldPriceHistoryPoint>> GetGoldPriceHistoryAsync(string timeframe = "1D", int limit = 100);
    }

    public enum Timeframe
    {
        OneMinute,
        FiveMinutes,
        FifteenMinutes,
        ThirtyMinutes,
        OneHour,
        FourHours,
        OneDay,
        OneWeek,
        OneMonth
    }

    public class GoldPriceData
    {
        public decimal Price { get; set; }
        public string Currency { get; set; } = "USD";
        public DateTime Timestamp { get; set; } = DateTime.Now;
        public decimal ChangePercent { get; set; }
        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Close { get; set; }
    }

    public class GoldPriceHistoryPoint
    {
        public DateTime Date { get; set; }
        public decimal Price { get; set; }
        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Close { get; set; }
        public decimal Volume { get; set; }
    }
}

