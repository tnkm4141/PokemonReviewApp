using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokemonReviewApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PokemonReviewApp.Models;

namespace PokemonReviewIntegrationTest
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
       
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Var olan DbContext'i kaldır
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<DataContext>));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                // InMemory Database ekle
                services.AddDbContext<DataContext>(options =>
                {
                    options.UseInMemoryDatabase("TestDatabase");
                });

                // Test sırasında veritabanını sıfırla
                var serviceProvider = services.BuildServiceProvider();

                using (var scope = serviceProvider.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<DataContext>();

                    // Veritabanını sıfırla
                    db.Database.EnsureDeleted();
                    db.Database.EnsureCreated();

                    // Opsiyonel: Test verilerini yükle
                    try
                    {
                        SeedTestData(db);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"An error occurred seeding the database with test data. {ex.Message}");
                    }
                }
            });
        }

        private void SeedTestData(DataContext context)
        {
            
            // Örnek test verisi ekle
            context.Categories.Add(new PokemonReviewApp.Models.Category { Id = 1, Name = "Test Category" });

            var newPokemon = new Pokemon
            {
                Id = 1,
                Name = "PicaKar",
                BirthDate = new DateTime(2020, 1, 1),
                PokemonCategories = new List<PokemonCategory>
                {
                    new PokemonCategory { CategoryId = 1 }
                }
            };

            context.Pokemon.Add(newPokemon);
            context.SaveChangesAsync();

            context.SaveChanges();
        }

    }
}
