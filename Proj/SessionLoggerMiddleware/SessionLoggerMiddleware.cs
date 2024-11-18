using FIS_API.Security;

namespace SessionLoggerMiddleware
{
    public class Program
    {
        public static void Main(string[] args)
        {
            return;
        }
    }

    // To access asp net stuff stuff like RequestDelegate see here: https://learn.microsoft.com/en-us/answers/questions/1283273/how-to-access-microsoft-aspnetcore-http-httpcontex
    // In short, go to project config and make sure it says 'Project Sdk="Microsoft.NET.Sdk.Web"' instead of 'Project Sdk="Microsoft.NET.Sdk"'
    public class SessionLoggerMiddleware : IMiddleware
    {
        public class Log
        {
            public readonly String? user;
            public readonly String logData;
            public readonly DateTimeOffset logTime = DateTimeOffset.UtcNow;

            public Log(string? user, string logData)
            {
                this.user = user;
                this.logData = logData;
            }
        }
        private static List<Log> logs_LOCKME = new();

        public static List<Log> GetLogsCopy()
        {
            lock(logs_LOCKME)
            {
                return new(logs_LOCKME);
            }
        }

        public Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            return Task.Run(async () =>
            {
                String? user = JwtTokenProvider.ReadIdentifierFromToken(context.User) ?? "[UNKNOWN]";

                String logData = "User entered '" + context.Request.Path + "'.";

                await next(context); // let the other stuff do their part and we'll continue below once they're all wrapped up

                logData += $" Responding with code '{context.Response.StatusCode}'.";

                lock (logs_LOCKME)
                {
                    logs_LOCKME.Add(new(user, logData));
                }
            });
        }
    }

    public static class SessionLoggerMiddlewareExtensions
    {
        public static IServiceCollection AddTransientSessionLoggerPls(this IServiceCollection services)
            => services.AddTransient<SessionLoggerMiddleware>();
        public static IApplicationBuilder UseSessionLoggerPls(this IApplicationBuilder builder)
            => builder.UseMiddleware<SessionLoggerMiddleware>();
    }
}
