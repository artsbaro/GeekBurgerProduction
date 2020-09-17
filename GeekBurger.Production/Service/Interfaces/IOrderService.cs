using GeekBurger.Order.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GeekBurger.Production.Service.Interfaces
{
    public interface IOrderService
    {
        Task<List<NewOrder>> Get();
        Task Create(NewOrder order);
        Task Create(List<NewOrder> orders);
        Task<NewOrder> GetById(int orderId);
    }
}
