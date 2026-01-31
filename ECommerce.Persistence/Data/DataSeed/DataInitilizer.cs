using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Entities.Orders;
using ECommerce.Domain.Entities.ProductModule;
using ECommerce.Persistence.Data.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Persistence.Data.DataSeed
{
    public class DataInitilizer : IDatainitilizer
    {
        private readonly StoreDbContext _dbContext;

        public DataInitilizer(StoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task InitializeAsync()
        {
            try
            {
                var HasProducts = await _dbContext.Products.AnyAsync();
                var HasBrands = await _dbContext.ProductBrands.AnyAsync();
                var HasTypes = await _dbContext.ProductTypes.AnyAsync();
                var HasDelivery = await _dbContext.Set<DeliveryMethod>().AnyAsync();

                if (HasProducts && HasBrands && HasTypes && HasDelivery) return;

                if (!HasBrands)
                    await SeedDataFromJsonAsync<ProductBrand, int>("brands.json", _dbContext.ProductBrands);
                if (!HasTypes)
                    await SeedDataFromJsonAsync<ProductType, int>("types.json", _dbContext.ProductTypes);

                _dbContext.SaveChanges();

                if (!HasProducts)
                    await SeedDataFromJsonAsync<Product, int>("products.json", _dbContext.Products);

                if (!HasDelivery)
                    await SeedDataFromJsonAsync<DeliveryMethod, int>("delivery.json", _dbContext.Set<DeliveryMethod>());

                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Data Seeding Failed : {ex}");
            }
        }

        private async Task SeedDataFromJsonAsync<T, TKey>(string fileName, DbSet<T> dbset) where T : BaseEntity<TKey>
        {
            //C: \Users\makys\OneDrive\Desktop\Route Assignments\API\ECommerce.Web.Solution\ECommerce.Persistence\Data\DataSeed\JSONFiles\
            var FilePath = @"..\ECommerce.Persistence\Data\DataSeed\JSONFiles\" + fileName;

            if (!File.Exists(FilePath)) throw new FileNotFoundException($"File {fileName} Not Found");

            try
            {
                //var Data = File.ReadAllText(FilePath);

                using var DataStream = File.OpenRead(FilePath);

                var Data = JsonSerializer.Deserialize<List<T>>(DataStream, new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true
                });

                if (Data is not null)
                {
                    await dbset.AddRangeAsync(Data);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed To Read Data From JSON : {ex}");
            }
        }
    }
}
