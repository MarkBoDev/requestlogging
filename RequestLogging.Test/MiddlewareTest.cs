namespace RequestLogging.Test
{
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

    public class MiddlewareTest
    {
        public MiddlewareTest()
        {
            NextMoq = new Mock<RequestDelegate>();
            NextMoq.Setup(n => n.Invoke(It.IsAny<HttpContext>())).Returns(Task.CompletedTask);

            DefaultOptions = new RestRequestLoggerOptions();
            HttpLoggerMoq = new Mock<HttpLogger>();
            ILoggerFactoryMoq = new Mock<ILoggerFactory>();
            ILoggerMoq = new Mock<ILogger>();
            ILoggerFactoryMoq.Setup(m => m.CreateLogger(It.IsAny<string>())).Returns(ILoggerMoq.Object);
            HttpContextMoq = new Mock<HttpContext>();
        }

        public RestRequestLoggerOptions DefaultOptions { get; private set; }

        public Mock<HttpContext> HttpContextMoq { get; private set; }
        public Mock<ILoggerFactory> ILoggerFactoryMoq { get; private set; }

        public Mock<ILogger> ILoggerMoq { get; private set; }

        public Mock<RequestDelegate> NextMoq { get; private set; }

        internal Mock<HttpLogger> HttpLoggerMoq { get; private set; }
        [Fact]
        public async Task LogRequestThenNextThenResponse()
        {
            // Arrange
            ILoggerMoq.Setup(l => l.IsEnabled(It.Is<LogLevel>(v => v == LogLevel.Debug))).Returns(true);
            var middleware = new RestRequestLoggerMiddleware(NextMoq.Object, DefaultOptions, HttpLoggerMoq.Object, ILoggerFactoryMoq.Object);

            var callOrder = new Queue<string>();
            var expectedCallOrder = new Queue<string>(
                new string[]
                {
                    nameof(HttpLogger.LogRequestAsync),
                    nameof(RequestDelegate.Invoke),
                    nameof(HttpLogger.LogResponseAsync)
                });

            HttpLoggerMoq.Setup(x => x.LogRequestAsync(It.IsAny<HttpContext>())).Callback(() => callOrder.Enqueue(nameof(HttpLogger.LogRequestAsync)));
            NextMoq.Setup(x => x.Invoke(It.IsAny<HttpContext>())).Callback(() => callOrder.Enqueue(nameof(RequestDelegate.Invoke)));
            HttpLoggerMoq.Setup(x => x.LogResponseAsync(It.IsAny<HttpContext>())).Callback(() => callOrder.Enqueue(nameof(HttpLogger.LogResponseAsync)));

            // Act
            await middleware.Invoke(HttpContextMoq.Object);

            // Assert
            Assert.Equal(expectedCallOrder, callOrder);
            NextMoq.Verify(n => n.Invoke(It.IsAny<HttpContext>()), Times.Once);
            HttpLoggerMoq.Verify(l => l.LogRequestAsync(It.IsAny<HttpContext>()), Times.Once);
            HttpLoggerMoq.Verify(l => l.LogResponseAsync(It.IsAny<HttpContext>()), Times.Once);
        }

        [Fact]
        public async Task SkipsLoggingIfDebugIsDisabled()
        {
            // Arrange
            ILoggerMoq.Setup(l => l.IsEnabled(It.Is<LogLevel>(v => v == LogLevel.Debug))).Returns(false);
            var middleware = new RestRequestLoggerMiddleware(NextMoq.Object, DefaultOptions, HttpLoggerMoq.Object, ILoggerFactoryMoq.Object);

            // Act
            await middleware.Invoke(HttpContextMoq.Object);

            // Assert
            NextMoq.Verify(n => n.Invoke(It.IsAny<HttpContext>()), Times.Once);
            HttpLoggerMoq.Verify(l => l.LogRequestAsync(It.IsAny<HttpContext>()), Times.Never);
            HttpLoggerMoq.Verify(l => l.LogResponseAsync(It.IsAny<HttpContext>()), Times.Never);
        }
    }
}
