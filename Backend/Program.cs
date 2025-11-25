using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using System.Text;
using TaskFlow.Api.Data;
using TaskFlow.Api.Interfaces;
using TaskFlow.Api.Services;

var builder = WebApplication.CreateBuilder(args);

var env = builder.Environment;

// -------------------------
// 1. CONTROLLERS
// -------------------------
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
        options.JsonSerializerOptions.WriteIndented = false;
    });

// -------------------------
// 2. SWAGGER (PROD-READY)
// -------------------------
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "TaskFlow API",
        Version = "v1",
        Description = "API del sistema TaskFlow Manager (.NET + Angular)"
    });

    // JWT
    var jwtScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Bearer {token}",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
    };

    options.AddSecurityDefinition("Bearer", jwtScheme);
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { jwtScheme, Array.Empty<string>() }
    });
});

// -------------------------
// 3. CORS (PROD + DOCKER + FRONTEND)
// -------------------------
builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendPolicy", policy =>
    {
        policy
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
            .SetIsOriginAllowed(origin =>
            {
                // Desarrollo Angular
                if (origin.StartsWith("http://localhost:4200")) return true;

                // Docker local
                if (origin.StartsWith("http://host.docker.internal")) return true;

                // Dominio en el VPS
                if (origin.Contains("tudominio.com")) return true;

                // IP pública del VPS
                if (origin.StartsWith("http://TU_IP_PUBLICA")) return true;

                return false;
            });
    });
});

// -------------------------
// 4. BASE DE DATOS
// -------------------------
var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection")
    ?? builder.Configuration["ConnectionStrings:DefaultConnection"]
    ?? throw new Exception("No hay cadena de conexión.");

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite(connectionString);

    // Mejor performance en PRODUCCIÓN
    if (env.IsProduction())
        options.EnableSensitiveDataLogging(false);
});

// -------------------------
// 5. INYECCIÓN DE SERVICIOS
// -------------------------
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<IColumnService, ColumnService>();

// -------------------------
// 6. JWT ESTABLE PARA PRODUCCIÓN
// -------------------------
var jwtSection = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSection["Key"]!);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = env.IsProduction();
        options.SaveToken = true;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            ValidIssuer = jwtSection["Issuer"],
            ValidAudience = jwtSection["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(key),

            // PRODUCCIÓN: un poco de tolerancia por hora del VPS
            ClockSkew = TimeSpan.FromSeconds(10)
        };
    });

// -------------------------
// 7. AUTORIZACIÓN (DEJAR LISTO PARA ESCALAR)
// -------------------------
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("columns.read", policy => policy.RequireAssertion(_ => true));
    options.AddPolicy("columns.create", policy => policy.RequireAssertion(_ => true));
    options.AddPolicy("columns.update", policy => policy.RequireAssertion(_ => true));
    options.AddPolicy("columns.delete", policy => policy.RequireAssertion(_ => true));

    options.AddPolicy("tasks.read", policy => policy.RequireAssertion(_ => true));
    options.AddPolicy("tasks.create", policy => policy.RequireAssertion(_ => true));
    options.AddPolicy("tasks.update", policy => policy.RequireAssertion(_ => true));
    options.AddPolicy("tasks.delete", policy => policy.RequireAssertion(_ => true));

    options.AddPolicy("users.read", policy => policy.RequireAssertion(_ => true));
    options.AddPolicy("users.create", policy => policy.RequireAssertion(_ => true));
    options.AddPolicy("users.update", policy => policy.RequireAssertion(_ => true));
    options.AddPolicy("users.delete", policy => policy.RequireAssertion(_ => true));
});


// -------------------------
// 8. BUILD APP
// -------------------------
var app = builder.Build();

// -------------------------
// 9. Swagger configurado por variable de entorno
// -------------------------
var swaggerEnabled = builder.Configuration.GetValue<bool>("Swagger:Enabled");

if (swaggerEnabled || env.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// -------------------------
// 10. CORS
// -------------------------
app.UseCors("FrontendPolicy");

// -------------------------
// 11. MIGRACIONES EN PRODUCCIÓN (OPCIONAL Y SEGURO)
// -------------------------
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    if (builder.Configuration.GetValue<bool>("Database:AutoMigrate"))
    {
        db.Database.Migrate();
        await SeedData.InitializeAsync(db);
    }
}

// -------------------------
// 12. HTTPS + AUTH
// -------------------------

app.UseAuthentication();
app.UseAuthorization();

// -------------------------
// 13. ENDPOINTS
// -------------------------
app.MapControllers();

app.Run();
