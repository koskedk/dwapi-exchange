using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Serilog;

namespace Dwapi.Exchange.Core.Application.Common.Behaviors
{
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken,RequestHandlerDelegate<TResponse> next)
        {
            Log.Information($"Request [{typeof(TRequest).Name}] Starting ...");
            Stopwatch stopwatch = Stopwatch.StartNew();
            var response = await next();
            stopwatch.Stop();
            Log.Information($"Request [{typeof(TRequest).Name}] Completed [{stopwatch.ElapsedMilliseconds} ms]");
            return response;
        }
    }
}
