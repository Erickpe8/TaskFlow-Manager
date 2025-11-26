using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Api.Domain;

namespace TaskFlow.Api.Data
{
    public static class SeedData
    {
        public static async Task InitializeAsync(AppDbContext context)
        {
            await context.Database.MigrateAsync();

            await SeedSecurityAsync(context);
            await SeedKanbanAsync(context);
        }

        private static async Task SeedSecurityAsync(AppDbContext context)
        {
            // 1. Roles
            if (!await context.Roles.AnyAsync())
            {
                var teacherRole = new Role { Name = "Teacher" };
                var studentRole = new Role { Name = "Student" };

                context.Roles.AddRange(teacherRole, studentRole);
                await context.SaveChangesAsync();

                // 2. Permisos
                var permissions = new List<Permission>
                {
                    new Permission { Name = "users.read" },
                    new Permission { Name = "users.create" },
                    new Permission { Name = "users.update" },
                    new Permission { Name = "users.delete" },
                    new Permission { Name = "tasks.read" },
                    new Permission { Name = "tasks.create" },
                    new Permission { Name = "tasks.update" },
                    new Permission { Name = "tasks.delete" },
                    new Permission { Name = "columns.read" },
                    new Permission { Name = "columns.create" },
                    new Permission { Name = "columns.update" },
                    new Permission { Name = "columns.delete" }
                };

                context.Permissions.AddRange(permissions);
                await context.SaveChangesAsync();

                // Asignar permisos a roles
                var teacherRoleId = teacherRole.Id;

                var teacherRolePermissions = permissions
                    .Select(p => new RolePermission
                    {
                        RoleId = teacherRoleId,
                        PermissionId = p.Id
                    });

                context.RolePermissions.AddRange(teacherRolePermissions);
                await context.SaveChangesAsync();

                // Usuario admin
                var seededUser = new User
                {
                    Email = "doc_js_galindo@fesc.edu.co",
                    FullName = "Docente Galindo",
                    IsActive = true
                };

                var passwordHasher = new PasswordHasher<User>();
                seededUser.PasswordHash =
                    passwordHasher.HashPassword(seededUser, "0123456789");

                context.Users.Add(seededUser);
                await context.SaveChangesAsync();

                // Relación UserRole
                context.UserRoles.Add(new UserRole
                {
                    UserId = seededUser.Id,
                    RoleId = teacherRoleId
                });

                await context.SaveChangesAsync();
            }
        }

        private static async Task SeedKanbanAsync(AppDbContext context)
        {
            if (!await context.Columns.AnyAsync())
            {
                var columns = new List<Column>
                {
                    new Column { Name = "Pendiente", Order = 1 },
                    new Column { Name = "En progreso", Order = 2 },
                    new Column { Name = "Completada", Order = 3 }
                };

                context.Columns.AddRange(columns);
                await context.SaveChangesAsync();

                var colPendiente = columns[0].Id;
                var colProgreso = columns[1].Id;

                var tasks = new List<TaskItem>
                {
                    new TaskItem {
                        Title = "Configurar estructura Angular",
                        Description = "Crear módulos, servicios y componentes base",
                        ColumnId = colPendiente,
                        Order = 1,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    },
                    new TaskItem {
                        Title = "Diseñar Layout del Kanban",
                        Description = "Crear columnas y tarjetas mock",
                        ColumnId = colPendiente,
                        Order = 2,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    },
                    new TaskItem {
                        Title = "Implementar backend CRUD",
                        Description = "Services, controllers y EF Core",
                        ColumnId = colProgreso,
                        Order = 1,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    }
                };

                context.TaskItems.AddRange(tasks);
                await context.SaveChangesAsync();
            }
        }
    }
}
