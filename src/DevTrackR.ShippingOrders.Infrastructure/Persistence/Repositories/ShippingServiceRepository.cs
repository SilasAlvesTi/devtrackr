using DevTrackR.ShippingOrders.Core.Entities;
using DevTrackR.ShippingOrders.Core.Repositories;
using MongoDB.Driver;

namespace DevTrackR.ShippingOrders.Infrastructure.Persistence.Repositories
{
    public class ShippingServiceRepository : IShippingServiceRepository
    {
        private readonly IMongoCollection<ShippingService> _colletion;
        public ShippingServiceRepository(IMongoDatabase database)
        {
            _colletion = database.GetCollection<ShippingService>("shipping-services"); 
        }

        public async Task<List<ShippingService>> GetAllAsync()
        {
            return await _colletion.Find(c => true).ToListAsync();
        }
    }
}
