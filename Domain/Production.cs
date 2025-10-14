namespace Domain
{
    public class Production
    {
        public Guid Id { get; set; }
        public int Shift { get; set; }
        public int QuantityProduced { get; set; }
        public DateTime ProductionDate { get; set; }
        public Tyre Tyre { get; set; }
        public ProductionOperator Operator { get; set; }
        //public QualitySupervisor Supervisor { get; set; }
        public Machine Machine { get; set; }
        public ICollection<Sale> Sales { get; set; } = [];
    }
}