
using ECommerce.Domain.Contracts;
using ECommerce.Persistence.Data.DataSeed;
using ECommerce.Persistence.Data.DbContexts;
using ECommerce.Persistence.Repositories;
using ECommerce.Service;
using ECommerce.Service.Abstraction;
using ECommerce.Service.MappingProfiles;
using ECommerce.Web.Extensions;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace ECommerce.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            builder.Services.AddDbContext<StoreDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddScoped<IDatainitilizer, DataInitilizer>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddAutoMapper(X => X.AddProfile<ProductProfile>());
            builder.Services.AddScoped<IProductService, ProductService>();

            builder.Services.AddSingleton<IConnectionMultiplexer>(options =>
            {
                return ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("RedisConnection")!);
            });

            builder.Services.AddScoped<IBasketRepository, BasketRepository>();
            builder.Services.AddAutoMapper(X => X.AddProfile<BasketProfile>());
            builder.Services.AddScoped<IBasketService, BasketService>();

            var app = builder.Build();


            #region DataSeed

            await app.MigrateDbAsync();
            await app.SeedDbAsync();

            #endregion


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
