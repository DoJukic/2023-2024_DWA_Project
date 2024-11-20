using FIS_API.Security;
using Microsoft.Net.Http.Headers;

namespace SessionLoggerMiddleware
{
    // https://m.youtube.com/watch?v=cu4CUJAcJ-4

    // To access asp net stuff stuff like RequestDelegate see here: https://learn.microsoft.com/en-us/answers/questions/1283273/how-to-access-microsoft-aspnetcore-http-httpcontex
    // In short, go to project config and make sure it says 'Project Sdk="Microsoft.NET.Sdk.Web"' instead of 'Project Sdk="Microsoft.NET.Sdk"'
    // Then go to the propertyGroup and make sure you add '<OutputType>Library</OutputType>'

    public class SessionLoggerMiddleware
    {
        private static string contextItemUsernameKey = nameof(SessionLoggerMiddlewareBetweenAuth) + "_USER";
        public class Log
        {
            public String? User { get; set; }
            public String LogData { get; set; }
            public DateTimeOffset LogTime { get; set; }
            
            public Log(string? user, string logData)
            {
                this.User = user;
                this.LogData = logData;
                LogTime = DateTimeOffset.UtcNow;
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

        public class SessionLoggerMiddlewareBetweenAuth : IMiddleware
        {
            public Task InvokeAsync(HttpContext context, RequestDelegate next)
            {
                return Task.Run(async () =>
                {
                    String? user = JwtTokenProvider.ReadIdentifierFromToken(context.User) ?? "[NO_AUTH]";

                    String logData = "User entered '" + context.Request.Path + "'.";

                    await next(context); // let the other stuff do their part and we'll continue below once they're all wrapped up

                    logData += $" Responding with code '{context.Response.StatusCode}'.";

                    var usernameMaybe = context.Items[contextItemUsernameKey];
                    if (usernameMaybe is string userStr)
                        user = userStr;

                    lock (logs_LOCKME)
                    {
                        logs_LOCKME.Add(new(user, logData));
                    }
                });
            }
        }
    }

    public static class SessionLoggerMiddlewareExtensions
    {
        public static IServiceCollection AddTransientSessionLoggerMidAuthPls(this IServiceCollection services)
            => services.AddTransient<SessionLoggerMiddleware.SessionLoggerMiddlewareBetweenAuth>();
        public static IApplicationBuilder UseSessionLoggerMidAuthPls(this IApplicationBuilder builder)
            => builder.UseMiddleware<SessionLoggerMiddleware.SessionLoggerMiddlewareBetweenAuth>();
    }
}
