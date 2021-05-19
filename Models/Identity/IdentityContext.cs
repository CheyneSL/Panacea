using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Models
{
    /*Custom [IdentityDbContext] where [TUser = User], [TRole = UserRole], [TKey = int]*/
    public class IdentityContext : IdentityDbContext<User, UserRole, int>
    {
        public IdentityContext(DbContextOptions<IdentityContext> options) : base(options)
        {

        }
    }
}
