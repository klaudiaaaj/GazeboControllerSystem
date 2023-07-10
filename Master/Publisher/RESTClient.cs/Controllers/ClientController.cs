using Microsoft.AspNetCore.Mvc;

namespace RESTClient.cs.Controllers
{
    public class ClientController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
