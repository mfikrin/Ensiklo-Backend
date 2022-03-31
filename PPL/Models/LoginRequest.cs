using System.ComponentModel.DataAnnotations;
namespace PPL.Models
{
    public class LoginRequest
    {
        [Required(AllowEmptyStrings = false)]
        public string Email { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string Password { get; set; }
    }
}
