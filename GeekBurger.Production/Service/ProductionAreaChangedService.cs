using GeekBurger.Production.Configuration;
using GeekBurger.Production.Model;
using GeekBurger.Production.Service.Interfaces;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GeekBurger.Production.Service
{
    public class ProductionAreaChangedService : IProductionAreaChangedService
    {
        IConfiguration Configuration;

        public ProductionAreaChangedService(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public async Task SendMessagesAsync(ProductionModel production)
        {
            try
            {
                var config = Configuration
                    .GetSection("serviceBus")
                    .Get<ServiceBusConfiguration>();

                var json = JsonSerializer.Serialize(production);
                var message = new Message(Encoding.UTF8.GetBytes(json));

                var topicClient = new TopicClient(config.ConnectionString, config.TopicName);
                await topicClient.SendAsync(message);

                var closeTask = topicClient.CloseAsync();
                await closeTask;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
