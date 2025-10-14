namespace Domain
{
    public class BusinessUnitLeader : User
    {
        public ICollection<Report> Reports { get; set; }
    }
}