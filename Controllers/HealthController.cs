using Microsoft.AspNetCore.Mvc;

namespace CricHeroesAnalytics.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get() => Ok(new { status = "Healthy" });

        [HttpGet("ready")]
        public IActionResult Ready() => Ok(new { status = "Ready" });
    }
}
