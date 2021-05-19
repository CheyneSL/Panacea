using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Models
{
    public class ProjectModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProjectID { get; set; }

        public string UserID { get; set; }

        public DateTime CreatedOn { get; set; }

        [Required(ErrorMessage = "Name required")]
        [MaxLength(30, ErrorMessage = "Cannot exceed 30 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Version required")]
        [MaxLength(15, ErrorMessage = "Cannot exceed 15 characters")]
        public string Version { get; set; }

        [Required(ErrorMessage = "Description required")]
        [StringLength(255, ErrorMessage = "Cannot exceed 255 Characters")]
        public string Description { get; set; }
    }
}
