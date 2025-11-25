using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TaskFlow.Api.Data;
using TaskFlow.Api.Interfaces;
using TaskFlow.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// ----------------------------
// 🔥 1. BASE DE DATOS
// ----------------------------
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHttpContextAccessor();

// ----------------------------
// 🔥 2. INYECCIÓN DE DEPENDENCIAS
// ----------------------------
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<IColumnService, ColumnService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ----------------------------
// 🔥 3. CORS LISTO PARA LOCAL + VPS
// ----------------------------
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
            .SetIsOriginAllowed(origin =>
            {
                // Permite Angular local
                if (origin.StartsWith("http://localhost")) return true;
                // Permite cualquier dominio necesario en Docker/VPS
                if (origin.Contains("your-vps-ip")) return true;
                if (origin.Contains("your-domain.com")) return true;
                return false;
            });
    });
});

// ----------------------------
// 🔥 4. JWT CONFIG
// ----------------------------
var jwtSection = builder.Configuration.GetSection("Jwt");
var jwtKey = jwtSection["Key"] ?? throw new Exception("Jwt:Key not configured");
var jwtIssuer = jwtSection["Issuer"];
var jwtAudience = jwtSection["Audience"];

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// ----------------------------
// 🔥 5. SEED AUTOMÁTICO DEL USUARIO DEL DOCENTE GALINDO
// ----------------------------
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await SeedData.InitializeAsync(context); // este se ejecuta una vez

    // 🔥 Asegurar usuario obligatorio del profe
    if (!context.Users.Any(u => u.Email == "doc_js_galindo@fesc.edu.co"))
    {
        context.Users.Add(new TaskFlow.Api.Models.User
        {
            Email = "doc_js_galindo@fesc.edu.co",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("0123456789"),
            Role = "Admin",
            FullName = "Docente José Galindo",
            CreatedAt = DateTime.UtcNow
        });
        await context.SaveChangesAsync();
    }
}

// ----------------------------
// 🔥 6. MIDDLEWARE
// ----------------------------
app.UseCors("AllowFrontend");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
