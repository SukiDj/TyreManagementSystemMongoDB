namespace Domain
{
    public class ActionLog
    {
        public Guid Id { get; set; }
        public string ActionName { get; set; }
        public string UserId { get; set; }
        public DateTime Timestamp { get; set; }
        public string Details { get; set; }
    }
}