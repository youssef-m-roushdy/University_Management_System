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

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
}); ;
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<UniversityDbContext>(options =>
   options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

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

builder.Services.AddIdentityCore<User>(options =>
{
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireDigit = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    options.User.RequireUniqueEmail = true;
})
.AddRoles<Role>()
.AddEntityFrameworkStores<UniversityDbContext>()
.AddDefaultTokenProviders()
.AddSignInManager<SignInManager<User>>();

builder.Services.AddScoped<IDataSeeding, DataSeeding>();

builder.Services.AddRateLimiter(options =>
{
    options.AddPolicy("PolicyLimitRate", httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.Connection.RemoteIpAddress!.ToString(),
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 3,
                Window = TimeSpan.FromMinutes(1),
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                QueueLimit = 2
            }));
});

// JWT + Auth services
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
// Use "emailSettings" (lowercase) to match your appsettings.json key exactly
builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("emailSettings"));

builder.Services.AddScoped<IEmailService, EmailService>();


// MediatR for CQRS
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(University_Management_System.Application.AssemblyReference).Assembly));

// AutoMapper
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<MappingProfile>();
});

// Unit of Work & Repositories
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Service Manager (Auth + Roles)
builder.Services.AddScoped<IServiceManager, ServiceManager>();
builder.Services.AddScoped<IUserService, UserService>();

// Configure Cloudinary Settings
builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));

// Infrastructure Services
builder.Services.AddScoped<ICloudinaryService, CloudinaryService>();

// GPA Calculation Service
builder.Services.AddScoped<IGpaCalculationService, GpaCalculationService>();

builder.Services.AddHttpContextAccessor();


#region Auth In Swagger

builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
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
                                 Type=ReferenceType.SecurityScheme,
                                  Id="Bearer"
                           }
                    },
                        new string[]{}
                }
      });
});

#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowFrontend");
app.UseMiddleware<GlobalExceptionHandlingMiddelWare>();

var scope = app.Services.CreateScope();
var dataSeeder = scope.ServiceProvider.GetRequiredService<IDataSeeding>();
await dataSeeder.SeedDataInfoAsync();
await dataSeeder.SeedIdentityDataAsync();
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.MapControllers();
app.UseStaticFiles();
app.Run();