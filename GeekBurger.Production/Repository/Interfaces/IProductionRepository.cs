using System.Collections.Generic;
using System.Threading.Tasks;

namespace GeekBurger.Production.Repository.Interfaces
{
    public interface IProductionRepository
    {
        Task<List<Contract.Production>> Get();
        Task Create(Contract.Production production);
        Task Create(List<Contract.Production> productions);
        Task<int> GetNewProductionId();
        Task<Contract.Production> GetById(int productionId);
        Task ChangeProductionAreaStatus(int productionId, bool status);
    }
}
