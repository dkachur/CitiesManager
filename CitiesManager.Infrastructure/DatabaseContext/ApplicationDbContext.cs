using Microsoft.EntityFrameworkCore;
using CitiesManager.Core.Domain.Entities;
using System.Text.Json;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using CitiesManager.Infrastructure.Identity;

namespace CitiesManager.Infrastructure.DatabaseContext
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public virtual DbSet<City> Cities { get; set; }

        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions options): base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            // Add cities seed data 
            string citiesJson = File.ReadAllText("cities.json");
            List<City>? cities = JsonSerializer.Deserialize<List<City>>(citiesJson);

            if (cities is not null)
                modelBuilder.Entity<City>().HasData(cities);

            modelBuilder.Entity<City>().Property(c => c.Name)
                .HasColumnType("nvarchar(40)");
        }
    }
}
