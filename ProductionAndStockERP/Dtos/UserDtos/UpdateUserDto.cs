using System.ComponentModel.DataAnnotations;

namespace ProductionAndStockERP.Dtos.UserDtos
{
    public class UpdateUserDto
    {
        [Required]
        public int UserId {  get; set; }

        [Required(ErrorMessage = "Kullanıcı adı zorunludur.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Kullanıcı adı 3 ile 50 karakter arasında olmalıdır.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "E-posta adresi zorunludur.")]
        [EmailAddress(ErrorMessage = "Geçersiz e-posta formatı.")]
        public string Email { get; set; }

        [Required]
        public bool? IsActive { get; set; }

        [Required]
        public string Role { get; set; }
    }
}
