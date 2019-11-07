using Microsoft.AspNetCore.Identity;

namespace Censo.API.Model
{
    public class UserRole   : IdentityUserRole<int>
    {
        public ApplicationUser ApplicationUser { get; set; }
        public Role Role { get; set; }
    }
}