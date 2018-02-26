using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ToDoList.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ToDoDbInitialiser: CreateDatabaseIfNotExists<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext db)
        {
            var rstore = new RoleStore<IdentityRole>(db);
            var rmanager = new RoleManager<IdentityRole>(rstore);
            rmanager.Create(new IdentityRole() { Name = "test" });

            var store = new UserStore<ApplicationUser>(db);

            var manager = new UserManager<ApplicationUser>(store);

            ApplicationUser test = new ApplicationUser();
            test.Email = "test@tt.tt";
            test.UserName = "test";

            manager.Create(test, "test");
            manager.AddToRole(test.Id, "test");

            base.Seed(db);
        }
        
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        //static ApplicationDbContext()
        //{
        //    Database.SetInitializer<ApplicationDbContext>(new ToDoContextInitialiser());
        //}
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public DbSet<Project> Projects { get; set; }
        public DbSet<JobTask> JobTasks { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        //public System.Data.Entity.DbSet<ToDoList.Models.ApplicationUser> ApplicationUsers { get; set; }
    }


}