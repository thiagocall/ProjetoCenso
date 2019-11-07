using Censo.API.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Censo.API.Data.Censo
{
    public class UserContext: IdentityDbContext<ApplicationUser, Role, int,
                            IdentityUserClaim<int>, UserRole, IdentityUserLogin<int>,
                            IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
         public UserContext(DbContextOptions<UserContext> options) : base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<UserRole>(userRole =>
                {
                    userRole.HasKey(ur => new {ur.UserId, ur.RoleId});

                    userRole.HasOne(ur =>  ur.Role)
                        .WithMany(r => r.UserRoles)
                        .HasForeignKey(ur => ur.RoleId)
                        .IsRequired();


                    userRole.HasOne(ur =>  ur.ApplicationUser)
                        .WithMany(r => r.UserRoles)
                        .HasForeignKey(ur => ur.UserId)
                        .IsRequired();
                }
            );

        }

    }
}