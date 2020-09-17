using GeekBurger.Production.Configuration;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ServiceBus.Fluent;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;

namespace GeekBurger.Production.Service
{
    public static class ServiceBusNamespaceExtension
    {
        private const string SubscriptionName = "Sub_ProductionAreaChanged";

        public static IServiceBusNamespace GetServiceBusNamespace(this IConfiguration configuration)
        {
            var config = configuration
                .GetSection("serviceBus")
                .Get<ServiceBusConfiguration>();

            var credentials = SdkContext.AzureCredentialsFactory
                .FromServicePrincipal(config.ClientId,
                config.ClientSecret,
                config.TenantId,
                AzureEnvironment.AzureGlobalCloud);

            var serviceBusManager = ServiceBusManager
                .Authenticate(credentials, config.SubscriptionId);

            return serviceBusManager
                .Namespaces
                .GetByResourceGroup(config.ResourceGroup, config.NamespaceName);
        }

        public static void CreateTopic(this IConfiguration configuration)
        {
            configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

            var config = configuration
                .GetSection("serviceBus")
                .Get<ServiceBusConfiguration>();

            var serviceBusNamespace = configuration.GetServiceBusNamespace();

            if (!serviceBusNamespace.Topics.List().Any(t => t.Name
            .Equals(config.TopicName, StringComparison.InvariantCultureIgnoreCase)))
            {
                serviceBusNamespace.Topics
                .Define(config.TopicName)
                .WithSizeInMB(1024)
                .Create();
            }

            var topic = serviceBusNamespace.Topics.GetByName(config.TopicName);

            if (topic.Subscriptions.List().Any(subscription => subscription.Name.Equals(SubscriptionName, StringComparison.InvariantCultureIgnoreCase)))
            {
                topic.Subscriptions.DeleteByName(SubscriptionName);
            }

            topic.Subscriptions.Define(SubscriptionName).Create();
        }
    }
}
