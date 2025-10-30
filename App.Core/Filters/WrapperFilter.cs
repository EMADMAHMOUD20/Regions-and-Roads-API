using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace App.Core.Filters
{

    public class WrapperFilter : IAsyncResultFilter
    {
        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            if (context.Result is ObjectResult objectResult)
            {
                context.Result = new ObjectResult(new
                {
                    Success = true,
                    Data = objectResult.Value
                })
                {
                    StatusCode = objectResult.StatusCode
                };
            }
            await next();
        }
    }
}
