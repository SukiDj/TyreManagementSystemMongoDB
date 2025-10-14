using Domain;

namespace Application.Machines
{
    public class MachineDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public ICollection<Production> Productions { get; set; } = [];
    }
}