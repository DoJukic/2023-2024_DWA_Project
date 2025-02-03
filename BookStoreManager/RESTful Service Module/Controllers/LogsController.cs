using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RESTful_Service_Module.Dtos;
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

        [HttpPost("search")]
        [Authorize(Roles = "Administrator")]
        public ActionResult GetLogsPage(LogPagesInDto pageData)
        {
            List<Log> logs = SessionLoggerMiddleware.SessionLoggerMiddleware.GetLogsCopy();

            LogPagesOutDto dtoOut = new();

            try
            {
                dtoOut.NumPages = (int)Math.Ceiling((double)logs.Count / (double)pageData.LogCount);

                if (pageData.LogCount * pageData.Page >= logs.Count)
                    dtoOut.Logs = null;
                else if (logs.Count <= pageData.LogCount * pageData.Page + pageData.LogCount)
                    dtoOut.Logs = new(logs.GetRange(pageData.LogCount * pageData.Page, logs.Count - pageData.LogCount * pageData.Page));
                else
                    dtoOut.Logs = new(logs.GetRange(pageData.LogCount * pageData.Page, pageData.LogCount));
            }
            catch(Exception e){}

            return Ok(dtoOut);
        }

        [HttpGet("count")]
        [Authorize(Roles = "Administrator")]
        public ActionResult Count()
        {
            return Ok(SessionLoggerMiddleware.SessionLoggerMiddleware.GetLogsCopy().Count);
        }
    }
}
