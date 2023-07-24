using Microsoft.AspNetCore.Mvc;
using Publisher.Services;

namespace Publisher.Controllers
{
    public class CommunicationController : Controller
    {
        public readonly IRabbitMqSender rabbitMqSender;
        public readonly IKaffkaSender kaffkaSender;
        public readonly IAzureServiceBusSender azureServiceBusSenderQueue;
        public readonly IAzureServiceBusSenderTopic azureServiceBusSenderTopic;
        public readonly IDataProducerService dataProducerService;
        public readonly ISqLiteRepo sqLiteRepo;

        public CommunicationController(IRabbitMqSender rabbitMqSender, IKaffkaSender kaffkaSender, IAzureServiceBusSender azureServiceBusSender, IDataProducerService dataProducerService, ISqLiteRepo sqLiteRepo, IAzureServiceBusSenderTopic azureServiceBusSenderTopic)
        {
            this.rabbitMqSender = rabbitMqSender;
            this.kaffkaSender = kaffkaSender;
            this.azureServiceBusSenderQueue = azureServiceBusSender;
            this.dataProducerService = dataProducerService;
            this.sqLiteRepo = sqLiteRepo;
            this.azureServiceBusSenderTopic = azureServiceBusSenderTopic;
        }

        [HttpGet("rabbitMq")]
        public Task SendDataByRabbitMq()
        {
            var data = dataProducerService.GetJoysticData();
            rabbitMqSender.Send(data);

            return Task.CompletedTask;
        }


        [HttpGet("kaffka")]
        public Task SendByKaffka()
        {
            var data = dataProducerService.GetJoysticData();
            kaffkaSender.Send(data);

            return Task.CompletedTask;
        }

        [HttpGet("azureServiceBusQueue")]
        public Task SendDataByAzureServiceBus()
        {
            var data = dataProducerService.GetJoysticData();
            azureServiceBusSenderQueue.Send(data);

            return Task.CompletedTask;
        }

        [HttpGet("azureServiceBusTopic")]
        public Task SendDataByAzureServiceBusTopic()
        {
            var data = dataProducerService.GetJoysticData();
            azureServiceBusSenderTopic.Send(data);

            return Task.CompletedTask;
        }

        [HttpGet("restProducer")]
        public Task SendBtRestToDatabase()
        {
            var data = dataProducerService.GetJoysticData();
            if (data.Count > 0)
            {
                sqLiteRepo.InsertAllJoystics(data);
            }
            return Task.CompletedTask;
        }
    }
}
