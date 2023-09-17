using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Web.Middlewares
{
    public class ValidationActionFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState.ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToList()
                );

                var response = new BadRequestResponse("One or more validation errors occurred.");

                context.Result = new BadRequestObjectResult(new
                {
                    Errors = errors,
                    Error = response.Message
                });

                return;
            }

            await next();
        }
    }

    public class BadRequestResponse : IRequest<IActionResult>
    {
        public string Message { get; }

        public BadRequestResponse(string message)
        {
            Message = message;
        }
    }
}
