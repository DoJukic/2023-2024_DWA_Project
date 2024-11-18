namespace RESTful_Service_Module.Systems
{
    public static class SessionLogger
    {
        public class Log
        {
            public readonly String user = "";
            public readonly String logData = "";
            public readonly DateTimeOffset logTime = DateTimeOffset.UtcNow;

            public Log(string user, string logData)
            {
                this.user = user;
                this.logData = logData;
            }
        }

        private static List<Log> logs = new();

        public static void AddLog(Log log)
        {
            logs.Add(log);
        }

        public static List<Log> GetLogs()
        {
            return new(logs);
        }
    }
}
