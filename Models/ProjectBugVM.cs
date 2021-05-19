using System.Collections.Generic;

namespace Project.Models
{
    public class ProjectBugVM
    {
        public IEnumerable<ProjectModel> Projects { get; set; }
        public Bug Bug { get; set; }
        public Image image { get; set; }
    }
}
