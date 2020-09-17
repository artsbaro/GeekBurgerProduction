using GeekBurger.Production.Configuration;
using GeekBurger.Production.Model;
using GeekBurger.Production.Repository.Interfaces;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GeekBurger.Production.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IMongoCollection<NewOrderModel> _orders;

        public OrderRepository(INewOrderDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _orders = database.GetCollection<NewOrderModel>(settings.OrdersCollectionName);
        }

        public async Task Create(NewOrderModel order)
        {
            try
            {
                await _orders.InsertOneAsync(order);
            }
            catch { }
        }

        public async Task Create(List<NewOrderModel> NewOrders)
        {
            await Task.Run(() => _orders.InsertMany(NewOrders));
        }

        public async Task<NewOrderModel> GetById(int orderId)
        {
            var options = new FindOptions<NewOrderModel, NewOrderModel>
            {
                Limit = 1
            };

            var oldNewOrder = (await _orders.FindAsync(x => x.OrderId == orderId, options)).FirstOrDefault();

            return oldNewOrder;
        }

        public async Task<List<NewOrderModel>> Get()
        {
            try
            {
                var options = new FindOptions<NewOrderModel, NewOrderModel>
                {
                    Sort = Builders<NewOrderModel>.Sort.Descending(o => o.OrderId)
                };

                var oldNewOrder = await _orders.FindAsync(FilterDefinition<NewOrderModel>.Empty, options);
                return oldNewOrder.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
