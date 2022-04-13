using System;
using System.ComponentModel.DataAnnotations;
namespace PPL.Models
{
    public class RegisterRequest
    {
        [Required(AllowEmptyStrings = false)]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Username { get; set; }
        
        [Required(AllowEmptyStrings = false)]
        public string Password { get; set; }    
    }
}
