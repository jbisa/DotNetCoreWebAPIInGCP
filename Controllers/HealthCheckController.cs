using Microsoft.AspNetCore.Mvc;

namespace DotNetCoreWebAPIInGCP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthCheckController : ControllerBase
    {
        // GET: api/HealthCheck
        [HttpGet]
        public ActionResult Get()
        {
            return Ok("Service is healthy!");
        }
    }
}
