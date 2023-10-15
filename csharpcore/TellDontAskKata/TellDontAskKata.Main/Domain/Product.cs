using TellDontAskKata.Main.Helpers;

namespace TellDontAskKata.Main.Domain
{
    public class Product
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public Category Category { get; set; }
        public decimal UnitaryTax => DecimalHelper.RoundToPositiveInfinity((Price / 100m) * Category.TaxPercentage);
        public decimal UnitaryTaxedAmount => DecimalHelper.RoundToPositiveInfinity(Price + UnitaryTax);
    }
}