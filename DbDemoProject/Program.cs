using DbDemoProject.Data;
using DbDemoProject.Helper;
using DbDemoProject.Model;
using DbDemoProject.Repository.Account;
using DbDemoProject.Service;
using DbDemoProject.Service.Account;
using DbDemoProject.Service.Admin;
using DbDemoProject.Service.Appointment;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


// ======================================================
// DATABASE
// ======================================================

builder.Services.AddDbContext<DbContextHelper>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("AppDB")
    );
});


// ======================================================
// CORS
// ======================================================

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy
            .WithOrigins(
                "http://localhost:3000",
                "https://localhost:3000"
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});


// ======================================================
// JWT CONFIGURATION
// ======================================================

var jwtKey = builder.Configuration["Jwt:Key"];

var jwtIssuer = builder.Configuration["Jwt:Issuer"];

var jwtAudience = builder.Configuration["Jwt:Audience"];


if (string.IsNullOrWhiteSpace(jwtKey))
{
    throw new Exception(
        "Jwt:Key is missing in appsettings.json"
    );
}


// ======================================================
// AUTHENTICATION
// ======================================================

builder.Services
    .AddAuthentication(
        JwtBearerDefaults.AuthenticationScheme
    )
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = true,

                ValidateAudience = true,

                ValidateLifetime = true,

                ValidateIssuerSigningKey = true,


                ValidIssuer = jwtIssuer,

                ValidAudience = jwtAudience,

                
                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtKey)
                    ),
                RoleClaimType = ClaimTypes.Role,
            };

    });


// ======================================================
// AUTHORIZATION
// ======================================================

builder.Services.AddAuthorization();


// ======================================================
// APPLICATION SERVICES
// ======================================================

// Account Service
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<INavigationMenuService, NavigationMenuService>();
builder.Services.AddScoped<INavigationMenuRepository, NavigationMenuRepository>();
builder.Services.AddScoped<IDashbordItems, DashboardItemService>();
builder.Services.AddScoped<IMemberService, MemberService>();
builder.Services.AddScoped<IAppointmentService, AppointmentService>();
builder.Services.AddScoped<ILetterService, LetterService>();
builder.Services.AddScoped<DbContextHelper>();


// Password Hasher
builder.Services.AddScoped<
    IPasswordHasher<Users>,
    PasswordHasher<Users>
>();
builder.Services.AddScoped<
    IRoleService,
   RoleService
>();
builder.Services.AddScoped<
    IRoleRepository,
   RoleRepository
>();



// JWT Helper
builder.Services.AddScoped<JWTHelper>();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddProblemDetails();


// ======================================================
// CONTROLLERS
// ======================================================

builder.Services.AddControllers();


// ======================================================
// API EXPLORER
// ======================================================

builder.Services.AddEndpointsApiExplorer();


// ======================================================
// SWAGGER
// ======================================================

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File(
        "Logs/log-.txt",
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 7)
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddSwaggerGen(options =>
{
    // -----------------------------------------------
    // Bearer Authentication
    // -----------------------------------------------

    options.AddSecurityDefinition(
        "Bearer",
        new OpenApiSecurityScheme
        {
            Name = "Authorization",

            Type = SecuritySchemeType.Http,

            Scheme = "Bearer",

            BearerFormat = "JWT",

            In = ParameterLocation.Header,

            Description =
                "Enter JWT token like: Bearer {your token}"
        });


    // -----------------------------------------------
    // Security Requirement
    // -----------------------------------------------

    options.AddSecurityRequirement(
        new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference =
                        new OpenApiReference
                        {
                            Type =
                                ReferenceType.SecurityScheme,

                            Id = "Bearer"
                        }
                },

                Array.Empty<string>()
            }
        });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI();
}

app.UseExceptionHandler();

app.UseHttpsRedirection();

app.UseCors("AllowReactApp");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();