using Microsoft.AspNetCore.Mvc;

namespace RESTClient.cs.Controllers
{
    public class ClientController : Controller
    {
        private readonly HttpClient _httpClient;

        public ClientController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            // Wywołanie API mikroserwisu producenta
            var response = await _httpClient.GetAsync("https://adres-producenta/api/sample");

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                return Ok(data);
            }
            else
            {
                return StatusCode((int)response.StatusCode);
            }
        }
    }
}
