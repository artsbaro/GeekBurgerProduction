using Microsoft.AspNetCore.Mvc;
using GeekBurger.Production.Service.Interfaces;
using System.Collections.Generic;
using GeekBurger.Production.Model;
using System.Threading.Tasks;
using System.Linq;

namespace GeekBurger.Production.Controllers
{
    [Produces("application/json")]
    [ApiController]
    public class ProductionController : ControllerBase
    {
        private readonly IProductionService _service;
        private readonly IServiceBusService _serviceBusService;

        public ProductionController(
              IProductionService service
            , IServiceBusService serviceBusService)
        {
            _service = service;
            _serviceBusService = serviceBusService;
        }

        [HttpGet("api/production/areas")]
        [ProducesResponseType(typeof(IEnumerable<ProductionModel>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAreas()
        {
            try
            {
                //await _service.Create(_list);

                var productions = await _service.Get();

                IEnumerable<ProductionModel> produtionModels = productions.Select(p => new ProductionModel
                {
                    ProductionId = p.ProductionId,
                    Restrictions = p.Restrictions,
                    On = p.On
                });

                return Ok(produtionModels);
            }
            catch { return StatusCode(500); }
        }

        [HttpPut("{productionId}/turnOff")]
        public async Task<IActionResult> TurnOffProductionArea([FromRoute] int productionId)
        {
            try
            {
                var result = await _service.ChangeProductionAreaStatus(productionId, false);

                var productionModel = new ProductionModel
                {
                    ProductionId = result.ProductionId,
                    Restrictions = result.Restrictions,
                    On = result.On
                };

                return Ok(productionModel);
            }
            catch
            {
                return StatusCode(500);
            }
        }

        [HttpPut("{productionId}/turnOn")]
        public async Task<IActionResult> TurnOnProductionArea([FromRoute] int productionId)
        {
            try
            {
                var result = await _service.ChangeProductionAreaStatus(productionId, true);

                var productionModel = new ProductionModel
                {
                    ProductionId = result.ProductionId,
                    Restrictions = result.Restrictions,
                    On = result.On
                };

                return Ok(productionModel);
            }
            catch
            {
                return StatusCode(500);
            }
        }

        [HttpGet("listaTopicos")]
        public async Task<IActionResult> ListaTopicos()
        {
            try
            {
                var result = await _serviceBusService.ListarTopicos();

                List<dynamic> lista = new List<dynamic>();

                foreach (var topic in result.Topics.List())
                {
                    foreach (var sub in topic.Subscriptions.List())
                    {
                        lista.Add(new { TopicName = topic.Name, SubscriptionName = sub.Name });
                    }
                }

                return Ok(lista);
            }
            catch
            {
                return StatusCode(500);
            }
        }

    }
}
