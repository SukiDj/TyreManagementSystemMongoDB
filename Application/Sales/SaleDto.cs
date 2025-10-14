namespace Application.Sales
{
    public class SaleDto
    {
        public Guid Id { get; set; }
        public Guid ProductionOrderId { get; set; }
        public Guid TyreCode { get; set; }
        public double PricePerUnit { get; set; }
        public string TargetMarket { get; set; }
        public int QuantitySold { get; set; }
        public DateTime SaleDate { get; set; }
        public Guid ClientId { get; set; }
    }
}