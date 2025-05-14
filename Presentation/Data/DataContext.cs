using Microsoft.EntityFrameworkCore;
using Presentation.Models;

namespace Presentation.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options)
        : base(options) { }
    public DbSet<EventModel> Events { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<EventModel>()
            .HasKey(e => e.Id);
        modelBuilder.Entity<EventModel>()
            .Property(e => e.Price).HasColumnType("decimal(18,2)");
    }

}
