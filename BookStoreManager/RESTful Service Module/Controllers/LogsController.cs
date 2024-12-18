using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static SessionLoggerMiddleware.SessionLoggerMiddleware;

namespace RESTful_Service_Module.Controllers
{
    [Route("api/logs")]
    [ApiController]
    public class LogsController : ControllerBase
    {
        [HttpPost("get")]
        [Authorize(Roles = "Administrator")]
        public ActionResult GetLogs(int? amount)
        {
            List<Log> logs = SessionLoggerMiddleware.SessionLoggerMiddleware.GetLogsCopy();

            if (amount == null || amount >= logs.Count)
                return Ok(logs);

            List<Log> logSend = new((int)amount);

            for (int i = logs.Count - (int)amount; i < logs.Count; i++)
            {
                logSend.Add(logs[i]);
            }

            return Ok(logSend);
        }
    }
}
