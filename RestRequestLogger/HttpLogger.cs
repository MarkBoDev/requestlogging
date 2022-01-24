[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("DynamicProxyGenAssembly2")]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("RequestLogging.Test")]
namespace RestRequestLogger
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;

    internal class HttpLogger
    {
        internal virtual async Task LogRequestAsync(HttpContext context)
        {
            throw new NotImplementedException();
        }

        internal virtual async Task LogResponseAsync(HttpContext context)
        {
            throw new NotImplementedException();
        }
    }
}
