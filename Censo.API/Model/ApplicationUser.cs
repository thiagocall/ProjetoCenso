using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Censo.API.Model
{
    public class ApplicationUser: IdentityUser<int>
    {
        public List<UserRole> UserRoles { get; set; }
        
    }
}