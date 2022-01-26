namespace RestRequestLogger
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.AspNetCore.Http;

    internal class RequestLogEntry
    {
        public List<KeyValuePair<string, object>> FieldValues { get; set; }

        public static RequestLogEntry Create(HttpRequest request, RestRequestLoggerOptions options)
        {
            var entry = new RequestLogEntry();

            var fields = (options?.Fields?.Any() ?? false) ? options.Fields : RestRequestLoggerOptions.DefaultRequestFields;

            if (fields.Contains(RestRequestLoggerFields.Method.ToString()))
            {
                entry.FieldValues.Add(new KeyValuePair<string, object>(RestRequestLoggerFields.Method.ToString(), request.Method));
            }

            if (fields.Contains(RestRequestLoggerFields.ContentType.ToString()))
            {
                entry.FieldValues.Add(new KeyValuePair<string, object>(RestRequestLoggerFields.Method.ToString(), request.ContentType));
            }

            if (fields.Contains(RestRequestLoggerFields.Path.ToString()))
            {
                entry.FieldValues.Add(new KeyValuePair<string, object>(RestRequestLoggerFields.Path.ToString(), request.Path));
                entry.FieldValues.Add(new KeyValuePair<string, object>(RestRequestLoggerFields.Path.ToString(), request.PathBase));
            }

            if (fields.Contains(RestRequestLoggerFields.QueryString.ToString()))
            {
                entry.FieldValues.Add(new KeyValuePair<string, object>(RestRequestLoggerFields.QueryString.ToString(), request.QueryString.ToString()));
            }

            return entry;
        }
    }
}
