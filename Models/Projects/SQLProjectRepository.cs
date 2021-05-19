using Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Models
{
    public class SQLProjectRepository : IProjectRepository
    {
        //Context (Session)
        private readonly ApplicationDbContext context;

        public SQLProjectRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        //CREATE
        ProjectModel IProjectRepository.Create(ProjectModel project)
        {
            context.Projects.Add(project);
            context.SaveChanges();
            return project;
        }

        //RED ONE
        ProjectModel IProjectRepository.GetProject(int id)
        {
            return context.Projects.Find(id);
        }

        //READ ALL
        IEnumerable<ProjectModel> IProjectRepository.GetAllProjects()
        {
            return context.Projects;
        }

        //UPDATE
        ProjectModel IProjectRepository.Update(ProjectModel projectChanges)
        {
            var project = context.Projects.Attach(projectChanges);
            project.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return projectChanges;
        }

        //DELETE
        ProjectModel IProjectRepository.Delete(int id)
        {
            ProjectModel project = context.Projects.Find(id);
            if (project != null)
            {
                context.Projects.Remove(project);
                context.SaveChanges();
            }
            return project;
        }
    }
}
