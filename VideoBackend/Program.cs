using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        Console.WriteLine("✅ CORS Policy Applied!"); // Debugging Log
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

// Do we need a singleton?
builder.Services.AddSingleton<AzureStorageService>();

// or a Scoped?
// builder.Services.AddScoped<AzureStorageService>();

// Add Swagger (available only in Development)
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
}

var app = builder.Build();


app.UseRouting();

app.UseCors("AllowAll");

app.UseAuthorization();
app.MapControllers();

// Enable Swagger in development mode only
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();
