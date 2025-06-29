using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Capstone_project.Models
{
    public class statusmodel
    {
        [Key]
        public int Id { get; set; }
        // Personal Info
        [Required(ErrorMessage = "First Name is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Age is required")]
        public string Age { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        public string Gender { get; set; }

        public string Height { get; set; }
        public string Weight { get; set; }

        // Complaint
        public string Symptoms { get; set; }
        public string SymptomsStartDate { get; set; }
        public string SymptomsPattern { get; set; }

        // Severity
        public string SeverityLevel { get; set; }
        public string SeverityCondition { get; set; }

        // Medical History
        public string ChronicDiseases { get; set; }
        public string TriggerActivities { get; set; }
        public string PreviousMedications { get; set; }

        // Family History
        public string FamilyIssues { get; set; }

        // Current Medications
        public string CurrentMedications { get; set; }

        // Allergies
        public string Allergies { get; set; }

        // Uploads
        public string UploadedFilePaths { get; set; } // Save as comma-separated paths or JSON


        // Lifestyle
        public string SmokingOrDrinking { get; set; }
        public string SleepPattern { get; set; }
        public string ActivityLevel { get; set; }
    }
}
