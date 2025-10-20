namespace Application.Sales
{
    public class SaleDto
    {
        public string Id { get; set; }
        public string ProductionOrderId { get; set; }
        public string TyreCode { get; set; }
        public double PricePerUnit { get; set; }
        public string TargetMarket { get; set; }
        public int QuantitySold { get; set; }
        public DateTime SaleDate { get; set; }
        public string ClientId { get; set; }
        public string ClientName { get; set; }
        public string TyreType { get; set; }
    }
}