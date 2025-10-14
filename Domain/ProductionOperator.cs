namespace Domain
{
    public class ProductionOperator : User
    {
        public ICollection<Production> Productions { get; set; }
    }
}