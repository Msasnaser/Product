using Microsoft.EntityFrameworkCore;
using Product.Model;

namespace Product.Data
{
    public class ApplicationDbContext :DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
   
        public DbSet<Products> products { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Products>().HasIndex(p => p.Name).IsUnique();


        }
    }
}
