using System.ComponentModel.DataAnnotations;

namespace Capstone_project.Models
{
    public class Select
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string DoctorName { get; set; }

        [Required]
        public string DoctorId { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        public string Day { get; set; }

        [Required]
        public string Time { get; set; }
    }
}
