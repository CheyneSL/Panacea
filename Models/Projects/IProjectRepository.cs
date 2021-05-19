using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Models
{
    public interface IProjectRepository //CRUD | Projects
    {
        ProjectModel GetProject(int id);
        IEnumerable<ProjectModel> GetAllProjects();
        ProjectModel Create(ProjectModel project);
        ProjectModel Update(ProjectModel project);
        ProjectModel Delete(int id);
    }
}
