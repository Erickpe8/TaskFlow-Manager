using TaskFlow.Api.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace TaskFlow.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Permission> Permissions => Set<Permission>();
    public DbSet<UserRole> UserRoles => Set<UserRole>();
    public DbSet<RolePermission> RolePermissions => Set<RolePermission>();
    public DbSet<UserToken> UserTokens => Set<UserToken>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.HasIndex(u => u.Email).IsUnique();
            entity.Property(u => u.Email).IsRequired();
            entity.Property(u => u.PasswordHash).IsRequired();
            entity.Property(u => u.FullName).IsRequired();
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(r => r.Id);
            entity.Property(r => r.Name).IsRequired();
        });

        modelBuilder.Entity<Permission>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Name).IsRequired();
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(ur => new { ur.UserId, ur.RoleId });
            entity.HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId);
            entity.HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId);
        });

        modelBuilder.Entity<RolePermission>(entity =>
        {
            entity.HasKey(rp => new { rp.RoleId, rp.PermissionId });
            entity.HasOne(rp => rp.Role)
                .WithMany(r => r.RolePermissions)
                .HasForeignKey(rp => rp.RoleId);
            entity.HasOne(rp => rp.Permission)
                .WithMany(p => p.RolePermissions)
                .HasForeignKey(rp => rp.PermissionId);
        });

        modelBuilder.Entity<UserToken>(entity =>
        {
            entity.HasKey(ut => ut.Id);
            entity.Property(ut => ut.Token).IsRequired();
            entity.HasOne(ut => ut.User)
                .WithMany(u => u.Tokens)
                .HasForeignKey(ut => ut.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        SeedData(modelBuilder);
    }

    private void SeedData(ModelBuilder modelBuilder)
    {
        var teacherRoleId = 1;
        var studentRoleId = 2;

        modelBuilder.Entity<Role>().HasData(
            new Role { Id = teacherRoleId, Name = "Teacher" },
            new Role { Id = studentRoleId, Name = "Student" }
        );

        modelBuilder.Entity<Permission>().HasData(
            new Permission { Id = 1, Name = "users.read" },
            new Permission { Id = 2, Name = "users.create" },
            new Permission { Id = 3, Name = "users.update" },
            new Permission { Id = 4, Name = "users.delete" }
        );

        modelBuilder.Entity<RolePermission>().HasData(
            new RolePermission { RoleId = teacherRoleId, PermissionId = 1 },
            new RolePermission { RoleId = teacherRoleId, PermissionId = 2 },
            new RolePermission { RoleId = teacherRoleId, PermissionId = 3 },
            new RolePermission { RoleId = teacherRoleId, PermissionId = 4 }
        );

        modelBuilder.Entity<RolePermission>().HasData(
           new RolePermission { RoleId = studentRoleId, PermissionId = 1 }
       );

        var seededUser = new User
        {
            Id = 1,
            Email = "doc_js_galindo@fesc.edu.co",
            FullName = "Docente Galindo",
            IsActive = true
        };

        var passwordHasher = new PasswordHasher<User>();
        seededUser.PasswordHash = passwordHasher.HashPassword(seededUser, "0123456789");

        modelBuilder.Entity<User>().HasData(seededUser);

        modelBuilder.Entity<UserRole>().HasData(
            new UserRole { UserId = seededUser.Id, RoleId = teacherRoleId }
        );
    }
}
