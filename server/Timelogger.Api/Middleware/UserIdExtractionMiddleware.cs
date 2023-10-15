using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System;
using Timelogger.Core.Interfaces;

namespace Timelogger.Api.Middleware
{
    public class UserIdExtractionMiddleware
    {
        private readonly RequestDelegate _next; 


        public UserIdExtractionMiddleware(RequestDelegate next)
        {
            _next = next;
             
        }

        public async Task InvokeAsync(HttpContext httpContext,   IIdentityService identityService)
        {

            identityService.CurrentUserId = 1;
            await _next(httpContext);

        }
    }
}