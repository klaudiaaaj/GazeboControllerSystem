using Contracts.Models;
using Microsoft.AspNetCore.Mvc;
using RabbitmqSubscriber.Services;

namespace RabbitmqSubscriber.Controllers
{
    [ApiController]
    [Route("api/rabbitMq")]
    public class RabbitMqClientController : Controller
    {
        private readonly IRabbitMqSubscriberService rabbitMqService;

        public RabbitMqClientController(IRabbitMqSubscriberService rabbitMqService)
        {
            this.rabbitMqService = rabbitMqService;
        }

        [HttpGet("single")]
        public IActionResult GetSingleObjectById()
        {
            var response = rabbitMqService.ExecuteAsyncSingle();
            return Ok(response);
        }
    }
}
