namespace Domain
{
    public class Sale
    {
        public Guid Id { get; set; }
        public DateTime SaleDate { get; set; }
        public int QuantitySold { get; set; }
        public double PricePerUnit { get; set; }
        public string UnitOfMeasure { get; set; }
        public string TargetMarket { get; set; }
        public Tyre Tyre { get; set; }
        public Client Client { get; set; }
        public Production Production { get; set; } // veza sa Production
    }
}