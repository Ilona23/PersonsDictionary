using Application;
using Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.WebUtilities;
using Serilog;
using System.Net;

namespace Web.Middlewares
{
    public static class ErrorHandlingMiddlewareExtension
    {
        public static void UseErrorHandlingMiddleware(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    var fillDefaultResponse = true;
                    var writeReponseBody = true;

                    var code = (int)HttpStatusCode.InternalServerError;

                    var response = new FailedRequestResponse();

                    var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                    var exception = exceptionHandlerPathFeature.Error;

                    Log.Error(exception, exception.Message);

                    if (exception is HttpException)
                    {
                        var httpException = exception as HttpException;

                        code = (int)httpException.Code;

                        if (code < 300)
                        {
                            writeReponseBody = false;
                        }
                    }
                    else if (exception is BadRequestException)
                    {
                        var badRequestException = exception as BadRequestException;

                        code = (int)HttpStatusCode.BadRequest;

                        if (badRequestException.ShowMessage)
                        {
                            response.Message = exception.Message;
                            response.ErrorCode = code.ToString();

                            fillDefaultResponse = false;
                        }
                    }
                    else if (exception is NotFoundException)
                    {
                        var notFoundException = exception as NotFoundException;

                        code = (int)HttpStatusCode.NotFound;

                        if (notFoundException.ShowMessage)
                        {
                            response.Message = exception.Message;
                            response.ErrorCode = code.ToString();

                            fillDefaultResponse = false;
                        }
                    }
                    else if (exception is UnprocessableEntityException)
                    {
                        var unprocessableEntityException = exception as UnprocessableEntityException;

                        foreach (var item in unprocessableEntityException.ValidationResult.Errors)
                        {
                            if (response.Details.ContainsKey(item.PropertyName))
                            {
                                continue;
                            }

                            response.Details.Add(item.PropertyName, item.ErrorMessage);
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

                    if (writeReponseBody)
                    {
                        await context.Response.WriteAsync(response.ToString());
                    }
                });
            });
        }
    }
}
