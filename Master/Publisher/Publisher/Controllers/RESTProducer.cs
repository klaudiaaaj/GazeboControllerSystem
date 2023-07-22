using Microsoft.AspNetCore.Mvc;
using Publisher.Services;

namespace Publisher.Controllers
{

    public class RESTProducer : Controller
    {
        public readonly IDataProducerService dataProducerService;
        public readonly ISqLiteRepo sqLiteRepo;

        public RESTProducer(IDataProducerService dataProducerService, ISqLiteRepo sqLiteRepo)
        {
            this.dataProducerService = dataProducerService;
            this.sqLiteRepo = sqLiteRepo;
        }

        [HttpGet("Rest")]
        public IActionResult Get()
        {


            var data = dataProducerService.GetJoysticData();

            return Ok(data);
        }

        [HttpGet("RetGetById/{id}")]
        public IActionResult GetById(string id)
        {
            var data = sqLiteRepo.GetJoysticById(int.Parse(id));

            return Ok(data);
        }

        [HttpGet("RestGetAll")]
        public IActionResult GetAll()
        {

           
            var test = sqLiteRepo.GetAllJoystics();
            return Ok(test);
        }
    }
}
