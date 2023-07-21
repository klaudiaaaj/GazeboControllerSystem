﻿using Microsoft.AspNetCore.Mvc;
using Publisher.Services;

namespace Publisher.Controllers
{
    public class CommunicationController : Controller
    {
        public readonly IRabbitMqSender rabbitMqSender;
        public readonly IKaffkaSender kaffkaSender;
        public readonly IAzureServiceBusSender azureServiceBusSender;
        public readonly IDataProducerService dataProducerService;   

        public CommunicationController(IRabbitMqSender rabbitMqSender, IKaffkaSender kaffkaSender, IAzureServiceBusSender azureServiceBusSender, IDataProducerService dataProducerService)
        {
            this.rabbitMqSender = rabbitMqSender;
            this.kaffkaSender = kaffkaSender;
            this.azureServiceBusSender = azureServiceBusSender;
            this.dataProducerService = dataProducerService;
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

        [HttpGet("azureServiceBus")]
        public Task SendDataByAzureServiceBus()
        {
            var data = dataProducerService.GetJoysticData();
            azureServiceBusSender.Send(data);

            return Task.CompletedTask;
        }   
    }
}
