using GeekBurger.Production.Model;
using System.Threading.Tasks;

namespace GeekBurger.Production.Service.Interfaces
{
    public interface IProductionAreaChangedService
    {
        Task SendMessagesAsync(ProductionModel production);
    }
}
