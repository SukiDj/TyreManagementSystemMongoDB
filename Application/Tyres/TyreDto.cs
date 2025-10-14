using Domain;

namespace Application.Tyres
{
    public class TyreDto
    {
        public Guid Code { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Size { get; set; }
        public string Brand { get; set; }
        public double Price { get; set; }
        public DateTime ProductionDate { get; set; }
        public ICollection<Production> Productions { get; set; } = [];
        public ICollection<Sale> Sales { get; set; } = [];
    }
}