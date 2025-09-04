﻿using MediatR;
using Microsoft.Extensions.Logging;

namespace MarcasAutos.Application.Common.Behaviors
{
    public sealed class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;
        public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger) => _logger = logger;

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
        {
            var name = typeof(TRequest).Name;
            _logger.LogInformation("Handling {RequestName} {@Request}", name, request);
            var response = await next();
            _logger.LogInformation("Handled {RequestName}", name);
            return response;
        }
    }
}
