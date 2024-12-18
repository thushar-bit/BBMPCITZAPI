using BBMPCITZAPI.Database;
using BBMPCITZAPI.Services.Interfaces;
using BBMPCITZAPI.Services;
using BBMPCITZAPI.Models;
using Serilog;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using static BBMPCITZAPI.Controllers.AuthController;
using Microsoft.OpenApi.Models;
//using StackExchange.Redis;
using System.Configuration;
using NUPMS_BA;
//using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAuthentication(options =>
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
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});
// Add services to the container.
//var redisConnectionString = builder.Configuration.GetValue<string>("Redis");
//builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConnectionString));

builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "BBMP CITIZEN", Version = "v1" });

    // Configure JWT authentication in Swagger
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter your valid token in the text input below.\r\n\r\nExample: \"eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\"",
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            new string[] {}
        }
    });
});
builder.Services.Configure<EKYCSettings>(builder.Configuration.GetSection("EKYCSettings"));
builder.Services.Configure<BBMPSMSSETTINGS>(builder.Configuration.GetSection("BBMPSMSSETTINGS"));
builder.Services.Configure<PropertyDetails>(builder.Configuration.GetSection("PropertyDetails"));
builder.Services.Configure<ESignSettings>(builder.Configuration.GetSection("E-sign"));
builder.Services.Configure<KaveriSettings>(builder.Configuration.GetSection("KaveriSettings"));
builder.Services.Configure<BescomSettings>(builder.Configuration.GetSection("BescomSettings"));
builder.Services.AddScoped<DatabaseService>();
builder.Services.AddSingleton<TokenService>();
//builder.Services.AddScoped<ICacheService, CacheService>();
builder.Services.AddScoped<IBBMPBookModuleService, BBMPBookModuleService>();
builder.Services.AddScoped<IObjectionService, ObjectionService>();
builder.Services.AddScoped<ISearchService, SearchService>();
builder.Services.AddScoped<INameMatchingService, NameMatchingService>();
builder.Services.AddScoped<IErrorLogService, ErrorLoggingService>();
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration) // Read configuration from appsettings.json
    .Enrich.FromLogContext()
    .WriteTo.Console() // Optional: Logs to console
    .WriteTo.File("E:\\ObjectionLogs\\log-.txt", rollingInterval: RollingInterval.Day) // Logs to a file
    .CreateLogger();

// Replace default logging with Serilog
builder.Host.UseSerilog();
var reactUrl = builder.Configuration.GetValue<string>("ReactURL");
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        builder => builder
            .WithOrigins(reactUrl!) // Adjust this with your React app URL
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowReactApp");
app.UseAuthentication();
app.UseAuthorization();

//app.UseHttpsRedirection();


app.MapControllers();

try
{
    app.Run();
}
catch (Exception ex)
{
    var logger = app.Services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occurred while starting the application.");
    throw;
}
