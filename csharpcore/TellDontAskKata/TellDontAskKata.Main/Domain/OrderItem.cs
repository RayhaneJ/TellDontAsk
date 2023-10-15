using TellDontAskKata.Main.Helpers;

namespace TellDontAskKata.Main.Domain
{
    public class OrderItem
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public decimal TaxedAmount => DecimalHelper.RoundToPositiveInfinity(Product.UnitaryTaxedAmount * Quantity);
        public decimal Tax => DecimalHelper.RoundToPositiveInfinity(Product.UnitaryTax * Quantity);
    }
}
