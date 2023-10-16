using Microsoft.AspNetCore.Http;
using System.Net;
using System.Threading.Tasks;
using System;
using System.Text.Json;

namespace Timelogger.Api.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            int statusCode = (int)HttpStatusCode.InternalServerError;
            string responseMessage = "Something went wrong with your request";
            context.Response.ContentType = "application/json";

            switch ( exception)
            {
                case ArgumentNullException aex:
                    statusCode = (int)HttpStatusCode.BadRequest;
                    responseMessage = aex.Message;
                    break;
                case ArgumentException ax:
                    statusCode = (int)HttpStatusCode.BadRequest;
                    responseMessage =ax.Message;
                    break;
                case InvalidOperationException iex:
                    statusCode = (int) HttpStatusCode.Locked;
                    responseMessage = iex.Message;
                    break;
            }

            context.Response.StatusCode = statusCode;
            await context.Response.WriteAsync(responseMessage);
        }
    }
}