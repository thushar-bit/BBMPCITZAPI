using BBMPCITZAPI.Database;
using BBMPCITZAPI.Services.Interfaces;
using BBMPCITZAPI.Services;
using BBMPCITZAPI.Models;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<EKYCSettings>(builder.Configuration.GetSection("EKYCSettings"));
builder.Services.Configure<BBMPSMSSETTINGS>(builder.Configuration.GetSection("BBMPSMSSETTINGS"));
builder.Services.Configure<PropertyDetails>(builder.Configuration.GetSection("PropertyDetails"));
builder.Services.AddScoped<DatabaseService>();
builder.Services.AddScoped<IBBMPBookModuleService, BBMPBookModuleService>();
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
//app.UseHttpsRedirection();

app.UseAuthorization();

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
