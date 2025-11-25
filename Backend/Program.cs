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

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "TaskFlow.Api API",
        Version = "v1"
    });

    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Ingrese el token en el formato: Bearer {token}",
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


    options.AddSecurityDefinition("Bearer", securityScheme);
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            securityScheme,
            Array.Empty<string>()
        }
    });
});

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

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? builder.Configuration["ConnectionStrings:DefaultConnection"]
    ?? throw new InvalidOperationException("DefaultConnection is not configured.");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(connectionString));

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<IColumnService, ColumnService>();

var jwtSection = builder.Configuration.GetSection("Jwt");
var jwtKey = jwtSection.GetValue<string>("Key")
    ?? throw new InvalidOperationException("Jwt:Key is not configured.");
var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
        ValidIssuer = jwtSection.GetValue<string>("Issuer"),
        ValidAudience = jwtSection.GetValue<string>("Audience"),
        IssuerSigningKey = signingKey,
        ClockSkew = TimeSpan.Zero
    };

    options.Events = new JwtBearerEvents
    {
        OnTokenValidated = async context =>
        {
            var tokenService = context.HttpContext.RequestServices.GetRequiredService<ITokenService>();
            var jwt = context.SecurityToken as JsonWebToken;
            var rawToken = jwt?.EncodedToken
                       ?? context.Request.Headers["Authorization"]
                               .ToString()
                               .Replace("Bearer ", "", StringComparison.OrdinalIgnoreCase)
                               .Trim();
            var subjectId = context.Principal?.FindFirstValue(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub)
           ?? context.Principal?.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(subjectId) || !int.TryParse(subjectId, out var userId))
            {
                context.Fail("Token sin identificador de usuario.");
                return;
            }

            if (string.IsNullOrWhiteSpace(rawToken))
            {
                context.Fail("Token no válido.");
                return;
            }

            var isActive = await tokenService.IsTokenActiveAsync(rawToken, userId, jwt?.ValidTo ?? DateTime.UtcNow);
            if (!isActive)
            {
                context.Fail("El token está revocado o expiró.");
            }
        }
    };
});

builder.Services.AddAuthorization(options =>
{
    // USERS
    options.AddPolicy("Users.Read", policy =>
        policy.RequireClaim("permissions", "users.read"));
    options.AddPolicy("Users.Create", policy =>
        policy.RequireClaim("permissions", "users.create"));
    options.AddPolicy("Users.Update", policy =>
        policy.RequireClaim("permissions", "users.update"));
    options.AddPolicy("Users.Delete", policy =>
        policy.RequireClaim("permissions", "users.delete"));

    // TASKS
    options.AddPolicy("Tasks.Read", policy =>
        policy.RequireClaim("permissions", "tasks.read"));
    options.AddPolicy("Tasks.Create", policy =>
        policy.RequireClaim("permissions", "tasks.create"));
    options.AddPolicy("Tasks.Update", policy =>
        policy.RequireClaim("permissions", "tasks.update"));
    options.AddPolicy("Tasks.Delete", policy =>
        policy.RequireClaim("permissions", "tasks.delete"));

    // COLUMNS
    options.AddPolicy("Columns.Read", policy =>
        policy.RequireClaim("permissions", "columns.read"));
    options.AddPolicy("Columns.Create", policy =>
        policy.RequireClaim("permissions", "columns.create"));
    options.AddPolicy("Columns.Update", policy =>
        policy.RequireClaim("permissions", "columns.update"));
    options.AddPolicy("Columns.Delete", policy =>
        policy.RequireClaim("permissions", "columns.delete"));
});

var app = builder.Build();
app.UseCors("AllowFrontend");
if (app.Environment.IsDevelopment())
{

    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        db.Database.Migrate();
        await SeedData.InitializeAsync(db);
    }

    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
