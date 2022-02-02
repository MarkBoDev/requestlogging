namespace RestRequestLogger
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;

    internal class RestRequestLoggerMiddleware
    {
        internal const string DefaultLoggerSource = "Request.Logger";

        internal RestRequestLoggerMiddleware(RequestDelegate next, RestRequestLoggerOptions options, HttpLogger restRequestLogger, ILoggerFactory logFactory)
        {
            Next = next ?? throw new ArgumentNullException(nameof(next));
            Options = options ?? throw new ArgumentNullException(nameof(options));
            RestRequestLogger = restRequestLogger;
            Logger = logFactory.CreateLogger(Options.LoggerSource ?? DefaultLoggerSource);
        }

        internal ILogger Logger { get; }
        internal RestRequestLoggerOptions Options { get; }
        internal HttpLogger RestRequestLogger { get; }
        private RequestDelegate Next { get; }

        public Task Invoke(HttpContext context) => !Logger.IsEnabled(LogLevel.Debug) ? Next(context) : InvokeInternalAsync(context);

        private async Task InvokeInternalAsync(HttpContext context)
        {
            await RestRequestLogger.LogRequestAsync(context, Logger);

            await Next(context);

            await RestRequestLogger.LogResponseAsync(context, Logger);
        }
    }
}
