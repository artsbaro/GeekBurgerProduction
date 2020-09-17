using System.Collections.Generic;
using System.Threading.Tasks;
using GeekBurger.Production.Model;
using GeekBurger.Production.Repository.Interfaces;
using GeekBurger.Production.Service.Interfaces;

namespace GeekBurger.Production.Service
{
    public class ProductionService : IProductionService
    {
        private readonly IProductionRepository _productionRepository;
        private readonly IProductionAreaChangedService  _productionAreaChangedService;

        public ProductionService(IProductionRepository productionRepository, 
            IProductionAreaChangedService productionAreaChangedService)
        {
            _productionRepository = productionRepository;
            _productionAreaChangedService = productionAreaChangedService;
        }

        public async Task<Contract.Production> ChangeProductionAreaStatus(int productionId, bool status)
        {
            var production = await GetById(productionId);

            if (production?.On == status)
                return production;

            await _productionRepository.ChangeProductionAreaStatus(productionId, status);

            var productionModel = new ProductionModel
            {
                ProductionId = production.ProductionId,
                Restrictions = production.Restrictions,
                On = status
            };

            await _productionAreaChangedService.SendMessagesAsync(productionModel);
            production.On = status;
            return production;
        }

        public async Task Create(Contract.Production production)
        {
           await _productionRepository.Create(production);
        }

        public async Task Create(List<Contract.Production> productions)
        {
            await _productionRepository.Create(productions);
        }

        public async Task<List<Contract.Production>> Get()
        {
            return await _productionRepository.Get();
        }

        public async Task<Contract.Production> GetById(int productionId)
        {
            return await _productionRepository.GetById(productionId);
        }

        public async Task<int> GetNewProductionId()
        {
            return await _productionRepository.GetNewProductionId();
        }
    }
}
