using Microsoft.EntityFrameworkCore;
using ApplicationService.Models;

namespace ApplicationService;

public class ApplicationDataContext : DbContext
{
    public ApplicationDataContext(DbContextOptions<ApplicationDataContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseSerialColumns();
    }

    public DbSet<WorkType> WorkTypes { get; set; }
    public DbSet<ApplicationStatus> Statuses { get; set; }
    public DbSet<Application> Applications { get; set; }
}