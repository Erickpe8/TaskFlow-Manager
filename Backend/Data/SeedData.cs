using TaskFlow.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace TaskFlow.Api.Data
{
    public static class SeedData
    {
        public static async Task InitializeAsync(AppDbContext context)
        {
            await context.Database.MigrateAsync();

            if (await context.Columns.AnyAsync())
                return;

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
            var colDone = columns[2].Id;

            var tasks = new List<TaskItem>
            {
                new TaskItem {
                    Title = "Configurar estructura Angular",
                    Description = "Crear módulos, servicios y componentes base",
                    ColumnId = colPendiente,
                    Order = 1
                },
                new TaskItem {
                    Title = "Diseñar Layout del Kanban",
                    Description = "Crear columnas y tarjetas mock",
                    ColumnId = colPendiente,
                    Order = 2
                },
                new TaskItem {
                    Title = "Implementar backend CRUD",
                    Description = "Services, controllers y EF Core",
                    ColumnId = colProgreso,
                    Order = 1
                }
            };

            if (!context.Users.Any())
            {
                var passwordHash = BCrypt.Net.BCrypt.HashPassword("0123456789");

                context.Users.Add(new User
                {
                    Email = "doc_js_galindo@fesc.edu.co",
                    FullName = "Docente",
                    Role = "Teacher",
                    PasswordHash = passwordHash
                });

                await context.SaveChangesAsync();
            }

            context.TaskItems.AddRange(tasks);
            await context.SaveChangesAsync();
        }
    }
}
