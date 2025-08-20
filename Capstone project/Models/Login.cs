using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Capstone_project.Models
{
    public class Login
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }

        [Required(ErrorMessage = "Please select your role")]
        [Display(Name = "Role")]
        public string Role { get; set; }  // Doctor, Patient, Admin

        [Required(ErrorMessage = "Please enter your email")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter your password")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }
    }
}
