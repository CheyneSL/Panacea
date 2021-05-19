using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Models
{
    public interface IBugRepository //CRUD
    {
        Bug GetBug(int id);
        IEnumerable<Bug> GetAllBugs();
        Bug Create(Bug bug);
        Bug Update(Bug bugChanges);
        Bug Delete(int id);
    }
}
