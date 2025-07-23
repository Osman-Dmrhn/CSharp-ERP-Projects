namespace ProductionAndStockERP.Dtos.ActivityLogDtos
{
    public class ActivityLogDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Action { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Status { get; set; }
        public string TargetEntity { get; set; }
    }
}
