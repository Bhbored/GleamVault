using Shared.Models.Enums;

namespace GleamVault.Utility
{
    public static class HallmarkTypeHelper
    {
        public static string ToString(HallmarkType hallmarkType)
        {
            return hallmarkType switch
            {
                HallmarkType.Gold9K => "9K Gold",
                HallmarkType.Gold10K => "10K Gold",
                HallmarkType.Gold14K => "14K Gold",
                HallmarkType.Gold18K => "18K Gold",
                HallmarkType.Gold21K => "21K Gold",
                HallmarkType.Sterling925 => "Sterling 925",
                HallmarkType.LuxuryBrands => "Luxury Brands",
                _ => "Luxury Brands"
            };
        }
    }
}
