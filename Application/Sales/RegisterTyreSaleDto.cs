namespace Application.Sales
{
    public class RegisterTyreSaleDto
    {
        public string ClientId { get; set; }
        public int QuantitySold { get; set; }
        public double PricePerUnit { get; set; }
        public string UnitOfMeasure { get; set; }
        public DateTime SaleDate { get; set; }
        public string ProductionOrderId { get; set; }
        public string TargetMarket { get; set; }
    }
}