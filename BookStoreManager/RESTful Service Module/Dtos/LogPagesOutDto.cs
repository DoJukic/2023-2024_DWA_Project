using static SessionLoggerMiddleware.SessionLoggerMiddleware;

namespace RESTful_Service_Module.Dtos
{
    public class LogPagesOutDto
    {
        public List<Log>? Logs { get; set; }
        public int NumPages { get; set; }
    }
}
