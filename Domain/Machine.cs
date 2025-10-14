namespace Domain
{
    public class Machine
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public ICollection<Production> Productions { get; set; }
    }
}