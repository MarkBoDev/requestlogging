[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("DynamicProxyGenAssembly2")]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("RequestLogging.Test")]
namespace RestRequestLogger
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;

    internal class HttpLogger
    {
        public HttpLogger(RestRequestLoggerOptions options)
        {
            Options = options;
        }

        public RestRequestLoggerOptions Options { get; }

        public List<KeyValuePair<string, object>> RequestFields { get; set; }

        public List<KeyValuePair<string, object>> ResponseFields { get; set; }

        internal virtual async Task LogRequestAsync(HttpContext context, ILogger logger)
        {
            var fields = (Options?.Fields?.Any() ?? false) ? Options.Fields : RestRequestLoggerOptions.DefaultResponseFields;

            if (fields.Contains(RestRequestLoggerFields.ContentType.ToString()))
            {
                AddKeyValue(ResponseFields, RestRequestLoggerFields.ContentType.ToString(), context.Response.ContentType);
            }

            if (fields.Contains(RestRequestLoggerFields.ContentLength.ToString()))
            {
                AddKeyValue(ResponseFields, RestRequestLoggerFields.ContentLength.ToString(), context.Response.ContentLength);
            }

            if (fields.Contains(RestRequestLoggerFields.StatusCode.ToString()))
            {
                AddKeyValue(ResponseFields, RestRequestLoggerFields.StatusCode.ToString(), context.Response.StatusCode);
            }

            // TODO header

            // TODO body

            var fieldsToLog = ResponseFields.Union(RequestFields.Except(ResponseFields));

            logger.LogDebug("Response", fieldsToLog.ToArray());
        }

        internal virtual async Task LogResponseAsync(HttpContext context, ILogger logger)
        {
            var fields = (Options?.Fields?.Any() ?? false) ? Options.Fields : RestRequestLoggerOptions.DefaultRequestFields;

            if (fields.Contains(RestRequestLoggerFields.Method.ToString()))
            {
                AddKeyValue(RequestFields, RestRequestLoggerFields.Method.ToString(), context.Request.Method);
            }

            if (fields.Contains(RestRequestLoggerFields.ContentType.ToString()))
            {
                AddKeyValue(RequestFields, RestRequestLoggerFields.ContentType.ToString(), context.Request.ContentType);
            }

            if (fields.Contains(RestRequestLoggerFields.ContentLength.ToString()))
            {
                AddKeyValue(RequestFields, RestRequestLoggerFields.ContentLength.ToString(), context.Request.ContentLength);
            }

            if (fields.Contains(RestRequestLoggerFields.Path.ToString()))
            {
                AddKeyValue(RequestFields, RestRequestLoggerFields.Path.ToString(), context.Request.Path);
                AddKeyValue(RequestFields, "PathBase", context.Request.PathBase);
            }

            if (fields.Contains(RestRequestLoggerFields.QueryString.ToString()))
            {
                AddKeyValue(RequestFields, RestRequestLoggerFields.QueryString.ToString(), context.Request.QueryString.ToString());
            }

            // TODO header

            // TODO body

            logger.LogDebug("Request", RequestFields.ToArray());
        }

        private static void AddKeyValue(List<KeyValuePair<string, object>> list, string key, object value)
        {
            list.Add(new KeyValuePair<string, object>(key, value));
        }
    }
}
