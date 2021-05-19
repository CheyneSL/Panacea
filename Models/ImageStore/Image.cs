using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Models
{
    public class Image
    {
        [Key]
        public int ImageId { get; set; }

        [DataType(DataType.Upload)]
        [DisplayName("Upload File")]
        public IFormFile ImageFile { get; set; }

        public int UserID { get; set; }

        public int ProjectID { get; set; }

        public int BugID { get; set; }
    }
}
