using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeekBurger.Production.Repository.Interfaces;
using GeekBurger.Production.Configuration;

namespace GeekBurger.Production.Repository
{
    public class ProductionRepository : IProductionRepository
    {
        private readonly IMongoCollection<Contract.Production> _productions;
        //private readonly IProductionAreaChangedService _messageService;

        public ProductionRepository(IProductionDatabaseSettings settings
            //, IProductionAreaChangedService messageService
            )
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _productions = database.GetCollection<Contract.Production>(settings.ProductionsCollectionName);
            //            _messageService = messageService;
        }

        public async Task Create(Contract.Production production)
        {
            try
            {
                production.ProductionId = await GetNewProductionId();
                await _productions.InsertOneAsync(production);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task Create(List<Contract.Production> productions)
        {
            await Task.Run(() => _productions.InsertMany(productions));
        }

        public async Task<int> GetNewProductionId()
        {
            var options = new FindOptions<Contract.Production, Contract.Production>
            {
                Limit = 1,
                Sort = Builders<Contract.Production>.Sort.Descending(o => o.ProductionId)
            };

            var oldProduction = (await _productions.FindAsync(FilterDefinition<Contract.Production>.Empty, options)).FirstOrDefault();

            if (oldProduction == null)
                return 1;

            return oldProduction.ProductionId + 1;
        }

        public async Task<Contract.Production> GetById(int productionId)
        {
            var options = new FindOptions<Contract.Production, Contract.Production>
            {
                Limit = 1,
                Sort = Builders<Contract.Production>.Sort.Descending(o => o.ProductionId)
            };

            var oldProduction = (await _productions.FindAsync(x => x.ProductionId == productionId, options)).FirstOrDefault();

            return oldProduction;
        }

        public async Task<List<Contract.Production>> Get()
        {
            var options = new FindOptions<Contract.Production, Contract.Production>
            {
                Sort = Builders<Contract.Production>.Sort.Descending(o => o.ProductionId)
            };

            var oldProduction = await _productions.FindAsync(FilterDefinition<Contract.Production>.Empty, options);
            return oldProduction.ToList();
        }

        public async Task ChangeProductionAreaStatus(int productionId, bool status)
        {
            var filter = Builders<Contract.Production>.Filter.Eq("ProductionId", productionId);
            var update = Builders<Contract.Production>.Update.Set("On", status);
            await _productions.UpdateOneAsync(filter, update);
        }
    }
}
