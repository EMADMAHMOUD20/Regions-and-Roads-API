using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace App.Core.Filters
{
    public class GlobalLoggerFilter:IActionFilter
    {
        private readonly ILogger<GlobalLoggerFilter> _logger;

        public GlobalLoggerFilter(ILogger<GlobalLoggerFilter> logger)
        {
            _logger = logger;
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogInformation(
            "✅ Finished {ActionName}",
            context.ActionDescriptor.DisplayName);
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInformation("➡️ Starting {ActionName} with arguments {@Arguments}",
            context.ActionDescriptor.DisplayName,
            context.ActionArguments);
        }
    }
}
