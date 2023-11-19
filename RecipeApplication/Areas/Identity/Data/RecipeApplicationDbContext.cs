using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecipeApplication.Areas.Identity.Data;

namespace RecipeApplication.Areas.Identity.Data;

public class RecipeApplicationDbContext : IdentityDbContext<RecipeApplicationUser>
{
    public RecipeApplicationDbContext(DbContextOptions<RecipeApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);

        builder.ApplyConfiguration(new ApplicationUserEntityConfiguration());
    }

    public DbSet<RecipeApplication.Areas.Identity.Data.RecipeApplicationUser> user { get; set; }
}

public class ApplicationUserEntityConfiguration : IEntityTypeConfiguration<RecipeApplicationUser>
{
    public void Configure(EntityTypeBuilder<RecipeApplicationUser> builder)
    {
        builder.Property(u => u.SpoonacularUsername);
        builder.Property(u => u.SpoonacularPassword);
        builder.Property(u => u.SpoonacularHash);
    }
}