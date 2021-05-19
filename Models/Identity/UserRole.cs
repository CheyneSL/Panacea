using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Models
{
    //Sub class [UserRole] with [IdentityRole] and change the [GUID] to [int]
    public class UserRole : IdentityRole<int> 
    {
    }
}
