namespace ExampleAspireBlazorApiProject.ApiService.Database;

public sealed class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<DrivingSchoolModel> DrivingSchools { get; set; }
    public DbSet<StudentModel> Students { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DrivingSchoolModel>()
            .HasMany(s => s.Students)
            .WithOne()
            .HasForeignKey(d => d.DrivingSchoolId);
    }
}