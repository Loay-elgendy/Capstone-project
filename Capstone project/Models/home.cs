using System.ComponentModel.DataAnnotations;

namespace Capstone_project.Models
{
    public class home
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Display(Name = "Price")]
        public string price { get; set; }
        [Display(Name = "Date")]
        public string Date { get; set; }
        [Display(Name = "Specialty")]
        public string Specialty { get; set; }

    }
}
