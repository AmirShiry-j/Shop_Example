//using Microsoft.AspNetCore.Builder;
//using Microsoft.AspNetCore.Http;
//using Microsoft.Extensions.Logging;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Threading.Tasks;

//namespace Shop_Example.Web
//{
//    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
//    public class CustomExceptionHandlerMiddleware
//    {
//        private readonly RequestDelegate _next;
//        private readonly ILogger<CustomExceptionHandlerMiddleware> _logger;
//        public CustomExceptionHandlerMiddleware(RequestDelegate next, ILogger<CustomExceptionHandlerMiddleware> logger)
//        {
//            _next = next;
//            _logger = logger;
//        }

//        public async Task Invoke(HttpContext httpContext)
//        {
//            try
//            {
//                await _next(httpContext);

//            }
//            catch (Exception error)
//            {
//                _logger.LogError(error.ToString());

//                if (error.InnerException != null)
//                {
//                    _logger.LogError($"Inner Exception of Error(Message) = {error.Message}: " + error.InnerException.ToString());
//                }


//            }
//        }
//    }

//    // Extension method used to add the middleware to the HTTP request pipeline.
//    public static class CustomExceptionHandlerMiddlewareExtensions
//    {
//        public static IApplicationBuilder UseCustomExceptionHandlerMiddleware(this IApplicationBuilder builder)
//        {
//            return builder.UseMiddleware<CustomExceptionHandlerMiddleware>();
//        }
//    }
//}
