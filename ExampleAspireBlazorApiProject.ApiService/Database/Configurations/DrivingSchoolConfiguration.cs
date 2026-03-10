namespace ExampleAspireBlazorApiProject.ApiService.Database.Configurations;

public class DrivingSchoolConfiguration : IEntityTypeConfiguration<DrivingSchoolModel>
{
    public void Configure(EntityTypeBuilder<DrivingSchoolModel> builder)
    {
        builder
            .HasMany(d => d.Students)
            .WithOne()
            .HasForeignKey(s => s.DrivingSchoolId);
    }
}