using GeekBurger.Order.Contracts;
using GeekBurger.Production.Model;
using GeekBurger.Production.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeekBurger.Production.Service.Interfaces
{
    public class OrderService : IOrderService
    {

        IOrderRepository _database;

        public OrderService(IOrderRepository database)
        {
            _database = database;
        }

        public async Task Create(NewOrder order)
        {
            try
            {
                var obj = Map(order);
                await _database.Create(obj);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task Create(List<NewOrder> NewOrders)
        {
            var orders = Map(NewOrders);

            await Task.Run(() => _database.Create(orders.ToList()));
        }

        public async Task<NewOrder> GetById(int orderId)
        {
            return await _database.GetById(orderId);
        }

        public async Task<List<NewOrder>> Get()
        {
            var result = await _database.Get();

            return Map(result).ToList();
        }

        #region Map
        public NewOrderModel Map(NewOrder obj)
        {
            return new NewOrderModel
            {
                OrderId = obj.OrderId,
                ProductionIds = obj.ProductionIds,
                Products = obj.Products,
                StoreName = obj.StoreName,
                Total = obj.Total
            };
        }

        public IEnumerable<NewOrderModel> Map(IEnumerable<NewOrder> obj)
        {
            return obj.Select(x => Map(x));
        }

        public NewOrder Map(NewOrderModel obj)
        {
            return new NewOrder
            {
                OrderId = obj.OrderId,
                ProductionIds = obj.ProductionIds,
                Products = obj.Products,
                StoreName = obj.StoreName,
                Total = obj.Total
            };
        }

        public IEnumerable<NewOrder> Map(IEnumerable<NewOrderModel> obj)
        {
            return obj.Select(x => Map(x));
        }


        #endregion

    }
}
