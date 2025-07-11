using System.ComponentModel.DataAnnotations;

namespace ProductionAndStockERP.Dtos.UserDtos
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "Email Alanı Zorunludur")]
        [EmailAddress(ErrorMessage = "Geçersiz e-posta formatı.")]
        public string Email { get; set; }

        [Required(ErrorMessage ="Şifre Alanı Zorunludur")]
        public string PasswordHash { get; set; }
    }
}
