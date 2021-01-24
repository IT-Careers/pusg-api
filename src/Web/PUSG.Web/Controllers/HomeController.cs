using Microsoft.AspNetCore.Mvc;

namespace PUSG.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        [Route("/")]
        public IActionResult Index()
        {
            return Ok();
        }
    }
}
