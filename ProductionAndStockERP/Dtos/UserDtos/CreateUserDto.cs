using System.ComponentModel.DataAnnotations;

namespace ProductionAndStockERP.Dtos.UserDtos
{
    public class CreateUserDto
    {
        [Required(ErrorMessage = "Kullanıcı adı zorunludur.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Kullanıcı adı 3 ile 50 karakter arasında olmalıdır.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "E-posta adresi zorunludur.")]
        [EmailAddress(ErrorMessage = "Geçersiz e-posta formatı.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Parola zorunludur.")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Parola en az 8 karakter olmalıdır.")]
        public string PasswordHash { get; set; }

        public string Role { get; set; }
    }
}
