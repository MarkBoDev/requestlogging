namespace RestRequestLogger
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class RestRequestLoggerOptions
    {
        public static IEnumerable<string> DefaultRequestFields => DefaultFields.Where(f => f != RestRequestLoggerFields.StatusCode.ToString());
        public static IEnumerable<string> DefaultResponseFields => DefaultFields;
        public List<string> Fields { get; set; }
        public string LoggerSource { get; set; }
        internal static IEnumerable<string> DefaultFields => Enum.GetValues(typeof(RestRequestLoggerFields)).Cast<string>();
    }
}
