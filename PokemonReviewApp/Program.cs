using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PokemonReviewApp;
using PokemonReviewApp.ValidationRules;
using PokemonReviewApp.Data;
using PokemonReviewApp.Dtos.OwnerDtos;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Repository;
using PokemonReviewApp.Services.CategoryServices;
using PokemonReviewApp.Services.CountryServices;
using PokemonReviewApp.Services.OwnerServices;
using PokemonReviewApp.Services.PokemonServices;
using PokemonReviewApp.Services.ReviewerServices;
using PokemonReviewApp.Services.ReviewServices;
using System;
using System.Text.Json.Serialization;
using PokemonReviewApp.Dtos.PokemonDtos;
using Serilog;
using System.Text.Json.Serialization;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();


// Serilog Konfigürasyonu
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug() // Varsayýlan log seviyesi
    .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning) // Microsoft loglarý için seviye
    .MinimumLevel.Override("PokemonReviewApp.Controllers", Serilog.Events.LogEventLevel.Debug) // Controller log seviyesi
    .MinimumLevel.Override("PokemonReviewApp.Services", Serilog.Events.LogEventLevel.Warning) // Servis log seviyesi
    .MinimumLevel.Override("PokemonReviewApp.Repository", Serilog.Events.LogEventLevel.Information) // Repository log seviyesi
    .WriteTo.Console() // Konsola yazma
    .WriteTo.File(
            path: "logs/log-.txt",           // Dosya yolu ve adý
            rollingInterval: RollingInterval.Day, // Günlük dosya oluþturma
            outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
            retainedFileCountLimit: 7) // Günlük dosyasýna yazma
    .CreateLogger();

builder.Logging.ClearProviders(); // Varsayýlan saðlayýcýlarý temizle
builder.Logging.AddSerilog(Log.Logger); // Serilog'u ekle


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(setup => setup.SwaggerDoc("v1", new OpenApiInfo()
{
    Description = "This is a simple implementation of a Minimal Api in Asp.Net 8 Core",
    Title = "Pokemon Review Api",
    Version = "v1",
    Contact = new OpenApiContact()
    {
        Name = "Kardelen Mutlu",
        Url = new Uri("https://www.linkedin.com/in/kardelenmutlu/")
    }
}));

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped<IPokemonRepository, PokemonRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICountryRepository, CountryRepository>();
builder.Services.AddScoped<IOwnerRepository, OwnerRepository>();
builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
builder.Services.AddScoped<IReviewerRepository, ReviewerRepository>();

builder.Services.AddScoped<IPokemonOwnerRepository, PokemonOwnerRepository>();


builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICountryService, CountryService>();
builder.Services.AddScoped<IOwnerService, OwnerService>();
builder.Services.AddScoped<IPokemonService, PokemonService>();
builder.Services.AddScoped<IReviewerService, ReviewerService>();
builder.Services.AddScoped<IReviewService, ReviewService>();

builder.Services.AddScoped<IValidator<OwnerCreateDto>, OwnerCreateValidatior>();
builder.Services.AddScoped<IValidator<PokemonCreateDto>, PokemonCreateValidator>();
builder.Services.AddScoped<IValidator<CreateOwnerWithPokemonDto>, CreateOwnerWithPokemonValidator>();

builder.Services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddTransient<Seed>();
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();

if (args.Length == 1 && args[0].ToLower() == "seeddata")
    SeedData(app);

void SeedData(IHost app)
{
    var scopedFactory = app.Services.GetService<IServiceScopeFactory>();

    using (var scope = scopedFactory.CreateScope())
    {
        var service = scope.ServiceProvider.GetService<Seed>();
        service.SeedDataContext();
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
/*app.MapGet("/authors/{id}", async ([FromServices] Author entity, int id) =>
{
    return Results.Ok();
});*/



app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
