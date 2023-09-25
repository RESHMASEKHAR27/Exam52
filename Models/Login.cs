using System.ComponentModel.DataAnnotations;

namespace Exam52.Models
{
    public class Login
    {
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }

}