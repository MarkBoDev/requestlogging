using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
[assembly: InternalsVisibleTo("RequestLogging.Test")]
namespace RestRequestLogger
{
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
