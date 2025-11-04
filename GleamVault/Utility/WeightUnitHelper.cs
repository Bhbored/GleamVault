using Shared.Models.Enums;

namespace GleamVault.Utility
{
    public static class WeightUnitHelper
    {
        public static string ToString(WeightUnit weightUnit)
        {
            return weightUnit switch
            {
                WeightUnit.Grams => "Grams",
                WeightUnit.Carats => "Carats",
                WeightUnit.Ounces => "Ounces",
                WeightUnit.Pennyweight => "Pennyweight",
                WeightUnit.Kilograms => "Kilograms",
                _ => "Grams"
            };
        }
    }
}
