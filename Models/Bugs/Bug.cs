using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Models
{
    public class Bug
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BugID { get; set; }

        public string UserID { get; set; }

        [Required(ErrorMessage = "Project required")]
        [ForeignKey("ProjectID")]
        public int ProjectID { get; set; }

        [Required(ErrorMessage = "Project required")]
        public string ProjectName { get; set; }

        [Required(ErrorMessage = "Classification required")]
        public string Classification { get; set; }

        [Required(ErrorMessage = "Priority required")]
        public string Priority { get; set; }

        [Required(ErrorMessage = "Severity required")]
        public string Severity { get; set; }

        [MaxLength(100, ErrorMessage = "Cannot exceed 100 characters")]
        [Required(ErrorMessage = "Environment required")]
        public string Environment { get; set; }

        [MaxLength(50, ErrorMessage = "Cannot exceed 50 characters")]
        [Required(ErrorMessage = "Summary required, no more than 25 chars")]
        public string Title { get; set; }

        [MaxLength(1000, ErrorMessage = "Cannot exceed 1000 characters")]
        [Required(ErrorMessage = "Description required")]
        public string Description { get; set; }

        [MaxLength(1000, ErrorMessage = "Cannot exceed 1000 characters")]
        public string StepsToReproduce { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime LastModified { get; set; }

        public bool Open { get; set; }

        /*
#nullable enable
        public string? imagePath { get; set; }
#nullable disable
        */
    }
}
