using DbDemoProject.Model;
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



    }
}
