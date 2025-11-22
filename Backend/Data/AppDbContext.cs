using Microsoft.EntityFrameworkCore;

namespace TaskFlow.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    // Aquí agregas tus tablas:
    // public DbSet<TaskModel> Tasks { get; set; }
}
