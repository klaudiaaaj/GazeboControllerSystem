using Contracts.Models;
using Microsoft.AspNetCore.Mvc;
using RabbitmqSubscriber.Services;

namespace RabbitmqSubscriber.Controllers
{
    public class RabbitMqClientController : Controller
    {
        private readonly IRabbitMqSubscriberService rabbitMqService;

        public RabbitMqClientController(IRabbitMqSubscriberService rabbitMqService)
        {
            this.rabbitMqService = rabbitMqService;
        }

        [HttpGet("Experiment Single")]
        public Task<Joystic> GetSingleObjectById()
        {
            var response = rabbitMqService.ExecuteAsyncSingle();

            return response;
        }
    }
}
