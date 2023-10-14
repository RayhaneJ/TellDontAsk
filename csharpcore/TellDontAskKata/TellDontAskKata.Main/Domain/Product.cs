namespace TellDontAskKata.Main.Domain
{
    public class Product
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public Category Category { get; set; }
        public decimal UnitaryTax => decimal.Round((Price / 100m) * Category.TaxPercentage);
        public decimal UnitaryTaxedAmount => decimal.Round(Price + UnitaryTax);
    }
}