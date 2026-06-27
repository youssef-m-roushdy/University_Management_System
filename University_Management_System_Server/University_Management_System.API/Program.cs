using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using University_Management_System.Application.Contracts;
using University_Management_System.MiddelWares;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.RateLimiting;
using System.Security.Cryptography;
using System.Threading.RateLimiting;
using University_Management_System.Domain.Entities.Identity;
using University_Management_System.Application.Mapping;
using System.Text.Json.Serialization;
using University_Management_System.Infrastructure.Presistence;
using University_Management_System.Domain.Contracts;
using University_Management_System.Infrastructure.Presistence.Repositories;
using University_Management_System.Infrastructure.Presistence.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using University_Management_System.Shared.Settings;
using University_Management_System.Infrastructure.Presistence.Data;
using FluentValidation;
using University_Management_System.Infrastructure.Presentation.Filters;

var builder = WebApplication.CreateBuilder(args);

// ────────────────────────────────────────────────────────────────────────
// 1. CONFIGURATION & SERVICES
// ────────────────────────────────────────────────────────────────────────

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(
                "http://localhost:3000", 
                "https://localhost:3000",
                "http://localhost:5173",  // Vite default
                "https://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// Controllers
// ✅ ValidationFilter runs before every action — short-circuits with 400
// whenever a FluentValidation validator is registered for an incoming DTO
// and that DTO fails validation.
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidationFilter>();
})
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ────────────────────────────────────────────────────────────────────────
// 2. DATABASE
// ────────────────────────────────────────────────────────────────────────
builder.Services.AddDbContext<UniversityDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// ────────────────────────────────────────────────────────────────────────
// 3. JWT AUTHENTICATION
// ────────────────────────────────────────────────────────────────────────
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>()!;

var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey));

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = key,
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();

// ────────────────────────────────────────────────────────────────────────
// 4. IDENTITY
// ────────────────────────────────────────────────────────────────────────
builder.Services.AddIdentityCore<User>(options =>
{
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireDigit = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    options.Password.RequiredLength = 8;
    options.User.RequireUniqueEmail = true;
})
.AddRoles<Role>()
.AddEntityFrameworkStores<UniversityDbContext>()
.AddDefaultTokenProviders()
.AddSignInManager<SignInManager<User>>();

// ────────────────────────────────────────────────────────────────────────
// 5. RATE LIMITING
// ────────────────────────────────────────────────────────────────────────
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("PolicyLimitRate", opt =>
    {
        opt.PermitLimit = 3;
        opt.Window = TimeSpan.FromMinutes(1);
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        opt.QueueLimit = 2;
    });
});

// ────────────────────────────────────────────────────────────────────────
// 6. DEPENDENCY INJECTION
// ────────────────────────────────────────────────────────────────────────

// Auth Services
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

// Email Services
builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddScoped<IEmailService, EmailService>();

// Cloudinary
builder.Services.Configure<CloudinarySettings>(
    builder.Configuration.GetSection("CloudinarySettings"));
builder.Services.AddScoped<ICloudinaryService, CloudinaryService>();

// ────────────────────────────────────────────────────────────────────────
// STORAGE SERVICES
// ────────────────────────────────────────────────────────────────────────
builder.Services.Configure<R2Settings>(
    builder.Configuration.GetSection("R2Settings"));
builder.Services.AddScoped<IR2StorageService, R2StorageService>();

// MediatR
builder.Services.AddMediatR(cfg => 
    cfg.RegisterServicesFromAssembly(typeof(University_Management_System.Application.AssemblyReference).Assembly));

// AutoMapper - Scan the Application assembly where your mapping profiles live
builder.Services.AddAutoMapper(
    cfg => { },
    typeof(University_Management_System.Application.AssemblyReference).Assembly);

// ✅ FluentValidation - register every AbstractValidator<T> found in the
// Application assembly (LoginDtoValidator, etc.). ValidationFilter above
// resolves these from DI per-request to actually run the checks.
builder.Services.AddValidatorsFromAssembly(
    typeof(University_Management_System.Application.AssemblyReference).Assembly);

// Repositories & Unit of Work
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Services
builder.Services.AddScoped<IServiceManager, ServiceManager>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IGpaCalculationService, GpaCalculationService>();

// Data Seeding
builder.Services.AddScoped<IDataSeeding, DataSeeding>();

// Http Context Accessor
builder.Services.AddHttpContextAccessor();

// ────────────────────────────────────────────────────────────────────────
// 7. SWAGGER AUTH CONFIG
// ────────────────────────────────────────────────────────────────────────
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "University Management System API", 
        Version = "v1",
        Description = "API for University Management System",
        Contact = new OpenApiContact
        {
            Name = "AYA Academy",
            Email = "support@ayaacademy.com"
        }
    });
    
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid JWT token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });

    // ✅ Add XML comments for better Swagger docs (optional)
    // var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    // var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    // option.IncludeXmlComments(xmlPath);
});

// ────────────────────────────────────────────────────────────────────────
// 8. BUILD APP
// ────────────────────────────────────────────────────────────────────────
var app = builder.Build();

// ────────────────────────────────────────────────────────────────────────
// 9. MIDDLEWARE PIPELINE
// ────────────────────────────────────────────────────────────────────────

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowFrontend");
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRateLimiter();

// ✅ Custom middleware (order matters)
app.UseMiddleware<GlobalExceptionHandlingMiddelWare>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// ────────────────────────────────────────────────────────────────────────
// 10. DATA SEEDING
// ────────────────────────────────────────────────────────────────────────
using (var scope = app.Services.CreateScope())
{
    var dataSeeder = scope.ServiceProvider.GetRequiredService<IDataSeeding>();
    await dataSeeder.SeedDataInfoAsync();
    await dataSeeder.SeedIdentityDataAsync();
}

// ────────────────────────────────────────────────────────────────────────
// 11. RUN
// ────────────────────────────────────────────────────────────────────────
app.Run();