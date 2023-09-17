using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.WebUtilities;
using Domain.Exceptions;
using Application;
using Serilog;
using System.Net;

namespace Web.Middlewares
{
    public static class ErrorHandlingMiddlewareExtensions
    {
        public static void UseErrorHandlingMiddleware(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    bool fillDefaultResponse = true;
                    bool writeResponseBody = true;

                    int code = (int)HttpStatusCode.InternalServerError;

                    FailedRequestResponse response = new FailedRequestResponse();

                    IExceptionHandlerPathFeature? exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                    Exception exception = exceptionHandlerPathFeature.Error;

                    Log.Error(exception, exception.Message);

                    if (exception is HttpException httpException)
                    {
                        code = (int)httpException.Code;

                        if (code < 300)
                        {
                            writeResponseBody = false;
                        }
                    }
                    else if (exception is BadRequestException badRequestException)
                    {
                        code = (int)HttpStatusCode.BadRequest;

                        if (badRequestException.ShowMessage)
                        {
                            response.Message = exception.Message;
                            response.ErrorCode = code.ToString();

                            fillDefaultResponse = false;
                        }
                    }
                    else if (exception is NotFoundException notFoundException)
                    {
                        code = (int)HttpStatusCode.NotFound;

                        if (notFoundException.ShowMessage)
                        {
                            response.Message = exception.Message;
                            response.ErrorCode = code.ToString();

                            fillDefaultResponse = false;
                        }
                    }
                    else if (exception is UnprocessableEntityException unprocessableEntityException)
                    {
                        foreach (FluentValidation.Results.ValidationFailure? item in unprocessableEntityException.ValidationResult.Errors)
                        {
                            if (!response.Details.ContainsKey(item.PropertyName))
                            {
                                response.Details.Add(item.PropertyName, item.ErrorMessage);
                            }
                        }

                        code = (int)HttpStatusCode.UnprocessableEntity;
                    }
                    else if (exception is ArgumentNullException && exception.Source == "MediatR")
                    {
                        code = (int)HttpStatusCode.BadRequest;
                    }

                    if (fillDefaultResponse)
                    {
                        response.ErrorCode = code.ToString();
                        response.Message = ReasonPhrases.GetReasonPhrase(code);
                    }

                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = code;

                    if (writeResponseBody)
                    {
                        await context.Response.WriteAsync(response.ToString());
                    }
                });
            });
        }
    }
}
