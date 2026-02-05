using Microsoft.EntityFrameworkCore;
using Selu383.SP26.Api.Entities;

namespace Selu383.SP26.Api.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    public DbSet<Location> Locations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Seed data - at least 3 locations required
        modelBuilder.Entity<Location>().HasData(
            new Location
            {
                Id = 1,
                Name = "Caffeinated Lions Downtown",
                Address = "123 Main Street, Hammond, LA 70401",
                TableCount = 15
            },
            new Location
            {
                Id = 2,
                Name = "Caffeinated Lions Uptown",
                Address = "456 Oak Avenue, Hammond, LA 70403",
                TableCount = 20
            },
            new Location
            {
                Id = 3,
                Name = "Caffeinated Lions Lakeside",
                Address = "789 Lake Drive, Mandeville, LA 70448",
                TableCount = 12
            }
        );
    }
}