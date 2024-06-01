using System.ComponentModel.DataAnnotations;

namespace CRUD.Models.Authentication
{
    public class RegisterUser
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
