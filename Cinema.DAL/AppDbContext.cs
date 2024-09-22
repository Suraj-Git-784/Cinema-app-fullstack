using Cinema.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Cinema.DAL
{
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }
        // DbSet properties for your model classes
        public DbSet<City> Cities { get; set; }
        public DbSet<Theater> Theaters { get; set; }
        public DbSet<ShowTiming> ShowTimings { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public ClaimsIdentity? UserName { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<City>()
       .HasMany(c => c.Theaters)
       .WithOne(t => t.City)
       .HasForeignKey(t => t.CityId);

            modelBuilder.Entity<Theater>()
                .HasMany(t => t.ShowTimings)
                .WithOne(st => st.Theater)
                .HasForeignKey(st => st.TheaterId);

            modelBuilder.Entity<Movie>()
                .HasMany(m => m.ShowTimings)
                .WithOne(st => st.Movie)
                .HasForeignKey(st => st.MovieId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
