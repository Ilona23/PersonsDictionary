using System.Net;
using Application;
using Domain.Exceptions;
using Serilog;

namespace Web.Middlewares
{
    public class ErrorLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                // Log the exception
                Log.Error(ex, ex.Message);

                var code = (int)HttpStatusCode.InternalServerError;
                var response = new FailedRequestResponse();

                if (ex is HttpException httpException)
                {
                    code = (int)httpException.Code;
                }
                else if (ex is BadRequestException badRequestException)
                {
                    code = (int)HttpStatusCode.BadRequest;

                    if (badRequestException.ShowMessage)
                    {
                        response.Message = ex.Message;
                        response.ErrorCode = code.ToString();
                    }
                }
                else if (ex is NotFoundException notFoundException)
                {
                    code = (int)HttpStatusCode.NotFound;

                    if (notFoundException.ShowMessage)
                    {
                        response.Message = ex.Message;
                        response.ErrorCode = code.ToString();
                    }
                }
                else if (ex is UnprocessableEntityException unprocessableEntityException)
                {
                    foreach (var item in unprocessableEntityException.ValidationResult.Errors)
                    {
                        if (!response.Details.ContainsKey(item.PropertyName))
                        {
                            response.Details.Add(item.PropertyName, item.ErrorMessage);
                        }
                    }

                    code = (int)HttpStatusCode.UnprocessableEntity;
                }
                else if (ex is ArgumentNullException argumentNullException && ex.Source == "MediatR")
                {
                    code = (int)HttpStatusCode.BadRequest;
                }

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = code;
                await context.Response.WriteAsync(response.ToString());
            }
        }
    }
}
