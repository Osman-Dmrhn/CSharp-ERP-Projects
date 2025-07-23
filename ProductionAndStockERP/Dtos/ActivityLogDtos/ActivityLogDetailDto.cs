namespace ProductionAndStockERP.Dtos.ActivityLogDtos
{
    public class ActivityLogDetailDto:ActivityLogDto
    {
        public int UserId { get; set; }
        public string TargetEntityId { get; set; }
        public string Changes { get; set; }
    }
}
