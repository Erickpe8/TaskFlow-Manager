using Microsoft.EntityFrameworkCore;
using TaskFlow.Api.Models;

namespace TaskFlow.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Column> Columns { get; set; } = null!;
        public DbSet<TaskItem> TaskItems { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // COLUMN
            modelBuilder.Entity<Column>(entity =>
            {
                entity.ToTable("Columns");
                entity.HasKey(c => c.Id);

                entity.Property(c => c.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(c => c.Order)
                    .IsRequired();
            });

            // TASKITEM
            modelBuilder.Entity<TaskItem>(entity =>
            {
                entity.ToTable("Tasks");
                entity.HasKey(t => t.Id);

                entity.Property(t => t.Title)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(t => t.Description)
                    .HasMaxLength(1000);

                entity.Property(t => t.Order)
                    .IsRequired();

                entity.Property(t => t.CreatedAt)
                    .IsRequired();

                entity.Property(t => t.UpdatedAt)
                    .IsRequired();

                entity.HasOne(t => t.Column)
                    .WithMany(c => c.Tasks)
                    .HasForeignKey(t => t.ColumnId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
