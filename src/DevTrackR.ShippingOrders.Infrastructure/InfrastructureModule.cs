using DevTrackR.ShippingOrders.Core.Repositories;
using DevTrackR.ShippingOrders.Infrastructure.Persistence;
using DevTrackR.ShippingOrders.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace DevTrackR.ShippingOrders.Infrastructure
{
    public static class InfrastructureModule
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services
                .AddMongo()
                .AddRepositories();
            return services;
        }

        public static IServiceCollection AddMongo(this IServiceCollection services)
        {
            services.AddSingleton<MongoDbOptions>(sp =>
            {
                var configuration = sp.GetService<IConfiguration>();
                var options = new MongoDbOptions();

                configuration.GetSection("Mongo").Bind(options);

                BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));

                return options;
            });

            services.AddSingleton<IMongoClient>(sp => {
                var configuration = sp.GetService<IConfiguration>();
                var options = sp.GetService<MongoDbOptions>();

                var client = new MongoClient(options.ConnectionString);
                var db = client.GetDatabase(options.DataBase);

                var dbSeed = new DbSeed(db);
                dbSeed.Populate();

                return client;
            });

            services.AddTransient(sp =>
            {

                var options = sp.GetService<MongoDbOptions>();
                var mongoClient = sp.GetService<IMongoClient>();

                var db = mongoClient.GetDatabase(options.DataBase);

                return db;
            });

            return services;
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IShippingOrderRepository, ShippingOrderRepository>();
            services.AddScoped<IShippingServiceRepository, ShippingServiceRepository>();

            return services;
        }

    }
}
