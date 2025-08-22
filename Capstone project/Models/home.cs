using System.ComponentModel.DataAnnotations;

namespace Capstone_project.Models
{
    public class home
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }

    }
}
