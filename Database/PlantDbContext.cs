using Microsoft.EntityFrameworkCore;
using LemonMES.Models;

namespace LemonMES.Database
{
    public class PlantDbContext : DbContext
    {
        public PlantDbContext(DbContextOptions<PlantDbContext> options) : base(options) { }
        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }

        public DbSet<UnitStatus> UnitStatus { get; set; }


    }
}
