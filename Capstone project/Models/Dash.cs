using System.ComponentModel.DataAnnotations;

namespace Capstone_project.Models
{
    public class Dash
    {
        [Key]
        public int Id { get; set; }
        public List<SignUp> Patients { get; set; } = new List<SignUp>();
        public List<Select> Reservations { get; set; } = new List<Select>();
    }
}
