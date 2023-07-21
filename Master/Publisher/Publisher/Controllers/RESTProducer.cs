using Microsoft.AspNetCore.Mvc;
using Publisher.Services;

namespace Publisher.Controllers
{
    public class RESTProducer : Controller
    {
        public readonly IDataProducerService dataProducerService;

        public RESTProducer(IDataProducerService dataProducerService)
        {
            this.dataProducerService = dataProducerService;
        }

        [HttpGet]
        public IActionResult Get()
        {

            var data = dataProducerService.GetJoysticData();

            return Ok(data);
        }
    }
}
