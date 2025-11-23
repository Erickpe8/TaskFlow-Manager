using Microsoft.EntityFrameworkCore;
using TaskFlow.Api.Data;
using TaskFlow.Api.Interfaces;
using TaskFlow.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// DbContext (ajusta el provider y la connection string según tu BD)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
// o UseNpgsql / UseSqlite según lo que uses.

builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<IColumnService, ColumnService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Más adelante aquí irá auth, pero por ahora lo dejamos limpio
// app.UseAuthentication();
// app.UseAuthorization();

app.MapControllers();

app.Run();
