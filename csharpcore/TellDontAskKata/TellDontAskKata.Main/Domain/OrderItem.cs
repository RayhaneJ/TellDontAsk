namespace TellDontAskKata.Main.Domain
{
    public class OrderItem
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public decimal TaxedAmount => decimal.Round(Product.UnitaryTaxedAmount * Quantity);
        public decimal Tax => decimal.Round(Product.UnitaryTax * Quantity);
    }
}
