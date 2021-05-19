using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Models
{
    public class SQLBugRepository : IBugRepository
    {
        //Context (Session)
        private readonly ApplicationDbContext context;

        public SQLBugRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        //Create
        public Bug Create(Bug bug)
        {
            context.Bugs.Add(bug);
            context.SaveChanges();
            return bug;
        }

        //READ ONE
        public Bug GetBug(int id)
        {
            return context.Bugs.Find(id);
        }

        //READ ALL
        public IEnumerable<Bug> GetAllBugs()
        {
            return context.Bugs;
        }

        //UPDATE
        public Bug Update(Bug bugChanges)
        {
            var bug = context.Bugs.Attach(bugChanges);
            bug.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return bugChanges;
        }

        //DELETE
        public Bug Delete(int id)
        {
            Bug bug = context.Bugs.Find(id);
            if (bug != null)
            {
                context.Bugs.Remove(bug);
                context.SaveChanges();
            }
            return bug;
        }
    }
}
