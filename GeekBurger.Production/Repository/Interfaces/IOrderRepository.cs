using GeekBurger.Production.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GeekBurger.Production.Repository.Interfaces
{
    public interface IOrderRepository
    {
        Task Create(NewOrderModel order);

        Task Create(List<NewOrderModel> NewOrders);

        Task<NewOrderModel> GetById(int orderId);

        Task<List<NewOrderModel>> Get();
    }
}
