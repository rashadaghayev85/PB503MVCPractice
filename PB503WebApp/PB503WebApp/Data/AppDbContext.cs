using Microsoft.EntityFrameworkCore;
using PB503WebApp.Models;

namespace PB503WebApp.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext>options):base(options) { }
        public DbSet<Slider> Sliders { get; set; }   

    }
    
}
