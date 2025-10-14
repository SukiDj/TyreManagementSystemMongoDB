namespace Domain
{
    public class Client
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Contact { get; set; }
        public string Address { get; set; }
        public ICollection<Sale> Sales { get; set; }
    }
}