using DevTrackR.ShippingOrders.Core.Entities;
using DevTrackR.ShippingOrders.Core.Repositories;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevTrackR.ShippingOrders.Infrastructure.Persistence.Repositories
{
    internal class ShippingOrderRepository : IShippingOrderRepository
    {
        private readonly IMongoCollection<ShippingOrder> _collecion;

        public ShippingOrderRepository(IMongoDatabase database)
        {
            _collecion = database.GetCollection<ShippingOrder>("shipping-orders");
        }

        public async Task AddAsync(ShippingOrder shippingOrder)
        {
            await _collecion.InsertOneAsync(shippingOrder);
        }

        public async Task<ShippingOrder> GetByCodeAsync(string code)
        {
            return await _collecion.Find(c => c.TrackingCode == code).SingleOrDefaultAsync();
        }
    }
}
