namespace ProductionAndStockERP.Models
{
    public class ActivityLogs
    {
        public int LogId { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }

        public string Action { get; set; }

        public DateTime CreatedAt { get; set; }

        //Detaylandırma Ekleme
        public string Status { get; set; }
        public string TargetEntity { get; set; }
        public string TargetEntityId { get; set; }
        public string Changes { get; set; }
    }
}
