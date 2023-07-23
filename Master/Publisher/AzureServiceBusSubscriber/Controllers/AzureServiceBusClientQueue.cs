using AzureServiceBusSubscriber.Services;
using Microsoft.AspNetCore.Mvc;

namespace AzureServiceBusSubscriber
{

    [ApiController]
    [Route("api/asbClient")]
    public class AzureServiceBusClientClass: Controller
    {
        private readonly IAzureServiceBusSubscriberService azureServiceBusSubscriberService;

        public AzureServiceBusClientClass(IAzureServiceBusSubscriberService azureServiceBusSubscriberService)
        {
            this.azureServiceBusSubscriberService = azureServiceBusSubscriberService;
        }
    }
}
