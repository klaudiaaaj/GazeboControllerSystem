using Microsoft.AspNetCore.Mvc;

namespace RESTClient.cs.Controllers
{
    public class ClientController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ClientController> _logger;

        public ClientController(IHttpClientFactory httpClientFactory, ILogger<ClientController> logger)
        {
            _httpClient = httpClientFactory.CreateClient();
            _logger = logger;
        }

        [HttpGet("restClient")]
        public async Task<IActionResult> GetData()
        {
            var response = await _httpClient.GetAsync("http://host.docker.internal:8080/Rest");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                return Ok(data);
            }
            else
            {
                _logger.LogError("Error", response.ToString());

                return StatusCode((int)response.StatusCode);
            }
        }
    }
}
