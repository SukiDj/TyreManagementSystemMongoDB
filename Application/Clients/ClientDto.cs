using Domain;

namespace Application.Clients
{
    public class ClientDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Contact { get; set; }
        public string Address { get; set; }
        public ICollection<Sale> Sales { get; set; } = [];
    }
}