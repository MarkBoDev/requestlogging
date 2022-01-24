using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using RestRequestLogger;
using Xunit;

namespace RequestLogging.Test
{
    public class MiddlewareTest
    {
        public MiddlewareTest()
        {
            NextMoq = new Mock<RequestDelegate>(MockBehavior.Strict);
            NextMoq.Setup(n => n.Invoke(It.IsAny<HttpContext>())).Returns(Task.CompletedTask);

            DefaultOptions = new RestRequestLoggerOptions();
            HttpLoggerMoq = new Mock<HttpLogger>();
            ILoggerFactoryMoq = new Mock<ILoggerFactory>();
            ILoggerMoq = new Mock<ILogger>();
            ILoggerFactoryMoq.Setup(m => m.CreateLogger(It.IsAny<string>())).Returns(ILoggerMoq.Object);
        }

        [Fact]
        public async Task SkipsLoggingIfDebugIsDisabled()
        {
            // Arrange
            ILoggerMoq.Setup(l => l.IsEnabled(It.Is<LogLevel>(v => v == LogLevel.Debug))).Returns(false);
            var middleware = new RestRequestLoggerMiddleware(NextMoq.Object, DefaultOptions, HttpLoggerMoq.Object, ILoggerFactoryMoq.Object);

            // Act
            await middleware.Invoke(null);

            // Assert
            NextMoq.Verify(n => n.Invoke(It.IsAny<HttpContext>()), Times.Once);
            HttpLoggerMoq.Verify(l => l.LogRequestAsync(It.IsAny<HttpContext>()), Times.Never);
            HttpLoggerMoq.Verify(l => l.LogResponseAsync(It.IsAny<HttpContext>()), Times.Never);
        }

        public Mock<RequestDelegate> NextMoq { get; private set; }
        public RestRequestLoggerOptions DefaultOptions { get; private set; }
        internal Mock<HttpLogger> HttpLoggerMoq { get; private set; }
        public Mock<ILoggerFactory> ILoggerFactoryMoq { get; private set; }
        public Mock<ILogger> ILoggerMoq { get; private set; }
    }
}
