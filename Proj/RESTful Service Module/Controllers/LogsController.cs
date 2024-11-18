using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RESTful_Service_Module.Controllers
{
    [Route("api/logs")]
    [ApiController]
    public class LogsController : ControllerBase
    {
        [HttpGet("get")]
        public ActionResult GetLogs()
        {
            return Ok(SessionLoggerMiddleware.SessionLoggerMiddleware.GetLogsCopy());
        }
    }
}
