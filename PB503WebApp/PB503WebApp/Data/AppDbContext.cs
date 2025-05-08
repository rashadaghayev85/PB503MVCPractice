using Microsoft.EntityFrameworkCore;
using PB503WebApp.Models;

namespace PB503WebApp.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext>options):base(options) { }
        public DbSet<Slider> Sliders { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=dpg-d0eif1h5pdvs73ao3skg-a.oregon-postgres.render.com;Port=5432;Database=cozastoredb;Username=cozastoredb_user;Password=ilhLmOxy2vEpjvl8LDjlQyQHR7myYXOx;");
        }
    }
    
}
