namespace ProductionAndStockERP.Dtos.UserDtos
{
    public class UpdateUserPasswordDto
    {
        public int UserId { get; set; }
        public string oldpass { get; set; }
        public string newpass { get; set; }
    }
}
