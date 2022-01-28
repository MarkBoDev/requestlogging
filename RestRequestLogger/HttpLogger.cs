[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("DynamicProxyGenAssembly2")]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("RequestLogging.Test")]
namespace RestRequestLogger
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;

    internal class HttpLogger
    {
        public HttpLogger(RestRequestLoggerOptions options)
        {
            Options = options;
        }

        public RestRequestLoggerOptions Options { get; }

        internal virtual async Task LogRequestAsync(HttpContext context)
        {
            throw new NotImplementedException();
        }

        internal virtual async Task LogResponseAsync(HttpContext context)
        {
            var fieldValues = new List<KeyValuePair<string, object>>();

            var fields = (Options?.Fields?.Any() ?? false) ? Options.Fields : RestRequestLoggerOptions.DefaultRequestFields;

            if (fields.Contains(RestRequestLoggerFields.Method.ToString()))
            {
                AddKeyValue(fieldValues, RestRequestLoggerFields.Method.ToString(), context.Request.Method);
            }

            if (fields.Contains(RestRequestLoggerFields.ContentType.ToString()))
            {
                AddKeyValue(fieldValues, RestRequestLoggerFields.Method.ToString(), context.Request.ContentType);
            }

            if (fields.Contains(RestRequestLoggerFields.Path.ToString()))
            {
                AddKeyValue(fieldValues, RestRequestLoggerFields.Path.ToString(), context.Request.Path);
                AddKeyValue(fieldValues, RestRequestLoggerFields.Path.ToString(), context.Request.PathBase);
            }

            if (fields.Contains(RestRequestLoggerFields.QueryString.ToString()))
            {
                AddKeyValue(fieldValues, RestRequestLoggerFields.QueryString.ToString(), context.Request.QueryString.ToString());
            }
        }

        private static void AddKeyValue(List<KeyValuePair<string, object>> list, string key, object value)
        {
            list.Add(new KeyValuePair<string, object>(key, value));
        }
    }
}
