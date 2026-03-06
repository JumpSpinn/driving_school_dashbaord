using ExampleAspireBlazorApiProject.Shared.Models;

namespace ExampleAspireBlazorApiProject.ApiService.Database;

public sealed class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<DrivingSchoolModel> DrivingSchools { get; set; }
    public DbSet<DriverModel> Drivers { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DrivingSchoolModel>()
            .HasMany(s => s.Drivers)
            .WithOne()
            .HasForeignKey(d => d.DrivingSchoolId);
    }
}