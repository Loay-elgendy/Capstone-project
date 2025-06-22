using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Capstone_project.Models
{
		public class Login
		{
		   [Key]
		    public int Id { get; set; }

			[Required(ErrorMessage = "Please enter your ID")]
			[Display(Name = "ID")]
			public string DoctorId { get; set; }

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
