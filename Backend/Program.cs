using Microsoft.EntityFrameworkCore;
using TaskFlow.Api.Data;

var builder = WebApplication.CreateBuilder(args);

// ----------------------------------------------------------
// CORS (Angular-friendly)
// ----------------------------------------------------------
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
});

// ----------------------------------------------------------
// Controllers
// ----------------------------------------------------------
builder.Services.AddControllers();

// ----------------------------------------------------------
// OpenAPI nativo (solo JSON)
// /openapi/v1.json
// ----------------------------------------------------------
builder.Services.AddOpenApi();

// ----------------------------------------------------------
// EF Core (se activará cuando agreguemos AppDbContext)
// ----------------------------------------------------------
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// ----------------------------------------------------------
// Build app
// ----------------------------------------------------------
var app = builder.Build();

// ----------------------------------------------------------
// Middlewares
// ----------------------------------------------------------
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();   // <-- funciona en .NET moderno
}

app.UseCors("AllowAll");

// ----------------------------------------------------------
// Controllers routing
// ----------------------------------------------------------
app.MapControllers();

// ----------------------------------------------------------
// Arrancar el backend
// ----------------------------------------------------------
app.Run();
