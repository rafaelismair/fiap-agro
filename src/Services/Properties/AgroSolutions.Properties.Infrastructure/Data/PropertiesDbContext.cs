using AgroSolutions.Properties.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AgroSolutions.Properties.Infrastructure.Data;

public class PropertiesDbContext : DbContext
{
    public DbSet<Property> Properties => Set<Property>();
    public DbSet<Talhao> Talhoes => Set<Talhao>();

    public PropertiesDbContext(DbContextOptions<PropertiesDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Property>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Name).HasMaxLength(200).IsRequired();
            e.Property(x => x.Location).HasMaxLength(500);
            e.HasMany(x => x.Talhoes).WithOne().HasForeignKey(t => t.PropertyId);
        });

        modelBuilder.Entity<Talhao>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Name).HasMaxLength(200).IsRequired();
            e.Property(x => x.Culture).HasMaxLength(100).IsRequired();
            e.Property(x => x.Status).HasMaxLength(50).HasDefaultValue("Normal");
        });
    }
}
