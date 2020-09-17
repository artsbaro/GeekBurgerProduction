using System.Collections.Generic;
using System.Threading.Tasks;

namespace GeekBurger.Production.Service.Interfaces
{
    public interface IProductionService
    {
        Task<List<Contract.Production>> Get();
        Task Create(Contract.Production production);
        Task Create(List<Contract.Production> productions);
        Task<int> GetNewProductionId();
        Task<Contract.Production> GetById(int productionId);
        Task<Contract.Production> ChangeProductionAreaStatus(int productionId, bool status);
    }
}
