namespace ProductionAndStockERP.Models
{
    public class ActivityLogs
    {
        public int LogId { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }

        public string Action { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
