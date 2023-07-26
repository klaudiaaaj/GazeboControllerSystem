using Microsoft.AspNetCore.Mvc;
using Publisher.Services;

namespace Publisher.Controllers
{

    public class RESTProducer : Controller
    {
        public readonly IDataProducerService dataProducerService;
        public readonly ISqLiteRepo sqLiteRepo;
        private readonly ILogger<RESTProducer> _logger;

        public RESTProducer(IDataProducerService dataProducerService, ISqLiteRepo sqLiteRepo, ILogger<RESTProducer> logger)
        {
            this.dataProducerService = dataProducerService;
            this.sqLiteRepo = sqLiteRepo;
            _logger = logger;
        }

        [HttpGet("RetGetById/{id}")]
        public IActionResult GetById(int id)
        {
            try
            {
                var test = sqLiteRepo.GetJoysticById(id);
                _logger.LogInformation("Data: ", test.ToString());
                return Ok(test);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);

                throw ex;
            }
        }

        [HttpGet("RestGetAll")]
        public IActionResult GetAll()
        {
            try
            {
                var test = sqLiteRepo.GetAllJoystics();
                _logger.LogInformation("Data: ", test.Count);

                return Ok(test);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
        }
    }
}
