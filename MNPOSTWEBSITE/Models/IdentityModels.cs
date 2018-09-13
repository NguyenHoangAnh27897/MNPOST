using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace MNPOSTWEBSITE.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public bool? IsActive { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public int? IDRole { get; set; }
        public string ResetPasswordCode { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection")
        {
            Database.SetInitializer<ApplicationDbContext>(null);
        }
    }
}