using DbDemoProject.Model;
using DbDemoProject.Model.Data.Model;
using Microsoft.EntityFrameworkCore;

namespace DbDemoProject.Data
{
    public class DbContextHelper : DbContext
    {
        public DbContextHelper(DbContextOptions<DbContextHelper> options) : base(options)
        {
            
        }

        public DbSet<Users> Users { get; set; }
        public DbSet<UserRoles> Roles { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<NavigationMenu> NavigationMenu { get; set; }
        public DbSet<Dashboard> DashboardItems { get; set; }
        public DbSet<MemberModel> Member { get; set; }
        public DbSet<AppointmentModel> Appointment { get; set; }
        public DbSet<TransactionModel> Transaction { get; set; }
    }
}
