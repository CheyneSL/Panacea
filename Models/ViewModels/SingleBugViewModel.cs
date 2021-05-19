using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Models
{
    public class SingleBugViewModel
    {
        public IQueryable<Bug> Bugs { get; set; }
        public string Query { get; set; }
    }
}
