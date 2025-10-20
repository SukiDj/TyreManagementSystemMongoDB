namespace Application.Productions
{
    public class ProductionDto
    {
        public string Id { get; set; }
        public string TyreCode { get; set; }
        public int Shift { get; set; }
        public int QuantityProduced { get; set; }
        public string MachineNumber { get; set; }
        public DateTime ProductionDate { get; set; }
        public string OperatorId { get; set; }
        public string MachineName { get; set; }
        public string OperatorName { get; set; }
        public string TyreType { get; set; }
    }
}
