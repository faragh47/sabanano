//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Net;
//using System.Threading.Tasks;
//using Common;
//using Common.Exceptions;
//using Microsoft.AspNetCore.Builder;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.AspNetCore.Http;
//using Microsoft.Extensions.Hosting;
//using Microsoft.Extensions.Logging;
//using Microsoft.IdentityModel.Tokens;
//using Newtonsoft.Json;
//using WebFramework.Api;

//namespace WebFramework.Middlewares
//{
//    public static class CustomExceptionHandlerMiddlewareExtensions
//    {
//        public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder builder)
//        {
//            return builder.UseMiddleware<CustomExceptionHandlerMiddleware>();
//        }
//    }

//    public class CustomExceptionHandlerMiddleware
//    {
//        private readonly RequestDelegate _next;
//        private readonly IWebHostEnvironment _env;
//        private readonly ILogger<CustomExceptionHandlerMiddleware> _logger;

//        public CustomExceptionHandlerMiddleware(RequestDelegate next,
//            IWebHostEnvironment env,
//            ILogger<CustomExceptionHandlerMiddleware> logger)
//        {
//            _next = next;
//            _env = env;
//            _logger = logger;
//        }

//        public async Task Invoke(HttpContext context)
//        {
//            string message = null;
//            HttpStatusCode httpStatusCode = HttpStatusCode.InternalServerError;
//            ApiResultStatusCode apiStatusCode = ApiResultStatusCode.ServerError;

//            try
//            {
//                await _next(context);
//            }
//            catch (AppException exception)
//            {
//                LogError(exception);
//                _logger.LogError(exception, exception.Message);
//                httpStatusCode = exception.HttpStatusCode;
//                apiStatusCode = exception.ApiStatusCode;

//                if (_env.IsDevelopment())
//                {
//                    var dic = new Dictionary<string, string>
//                    {
//                        ["Exception"] = exception.Message,
//                        ["StackTrace"] = exception.StackTrace,
//                    };
//                    if (exception.InnerException != null)
//                    {
//                        dic.Add("InnerException.Exception", exception.InnerException.Message);
//                        dic.Add("InnerException.StackTrace", exception.InnerException.StackTrace);
//                    }
//                    if (exception.AdditionalData != null)
//                        dic.Add("AdditionalData", JsonConvert.SerializeObject(exception.AdditionalData));

//                    message = JsonConvert.SerializeObject(dic);
//                }
//                else
//                {
//                    message = exception.Message;
//                }
//                await WriteToResponseAsync();
//            }
//            catch (SecurityTokenExpiredException exception)
//            {
//                LogError(exception);
//                _logger.LogError(exception, exception.Message);
//                SetUnAuthorizeResponse(exception);
//                await WriteToResponseAsync();
//            }
//            catch (UnauthorizedAccessException exception)
//            {
//                _logger.LogError(exception, exception.Message);
//                SetUnAuthorizeResponse(exception);
//                await WriteToResponseAsync();
//            }
//            catch (Exception exception)
//            {
//                LogError(exception);
//                _logger.LogError(exception, exception.Message);

//                if (_env.IsDevelopment())
//                {
//                    var dic = new Dictionary<string, string>
//                    {
//                        ["Exception"] = exception.Message,
//                        ["StackTrace"] = exception.StackTrace,
//                    };
//                    message = JsonConvert.SerializeObject(dic);
//                }
//                await WriteToResponseAsync();
//            }

//            async Task WriteToResponseAsync()
//            {
//                if (context.Response.HasStarted)
//                    throw new InvalidOperationException("The response has already started, the http status code middleware will not be executed.");

//                var result = new ApiResult(false, apiStatusCode, message);
//                var json = JsonConvert.SerializeObject(result);

//                context.Response.StatusCode = (int)httpStatusCode;
//                context.Response.ContentType = "application/json";
//                await context.Response.WriteAsync(json);
//            }

//            void SetUnAuthorizeResponse(Exception exception)
//            {
//                httpStatusCode = HttpStatusCode.Unauthorized;
//                apiStatusCode = ApiResultStatusCode.UnAuthorized;

//                if (_env.IsDevelopment())
//                {
//                    var dic = new Dictionary<string, string>
//                    {
//                        ["Exception"] = exception.Message,
//                        ["StackTrace"] = exception.StackTrace
//                    };
//                    if (exception is SecurityTokenExpiredException tokenException)
//                        dic.Add("Expires", tokenException.Expires.ToString());

//                    message = JsonConvert.SerializeObject(dic);
//                }
//            }

//            void LogError(Exception exception)
//            {
//                string path = "errors.log";
//                var dic = new Dictionary<string, string>
//                {
//                    ["Exception"] = exception.Message,
//                    ["StackTrace"] = exception.StackTrace,
//                };
//                if (exception.InnerException != null)
//                {
//                    dic.Add("InnerException.Exception", exception.InnerException.Message);
//                    dic.Add("InnerException.StackTrace", exception.InnerException.StackTrace);
//                }
//                //if (exception.AdditionalData != null)
//                //    dic.Add("AdditionalData", JsonConvert.SerializeObject(exception.AdditionalData));

//                message = JsonConvert.SerializeObject(dic);

//                // This text is added only once to the file.
//                if (!File.Exists(path))
//                {
//                    // Create a file to write to.
//                    using StreamWriter sw = File.CreateText(path);
//                    sw.WriteLine(message);
//                }
//                else
//                {
//                    using StreamWriter sw = File.AppendText(path);
//                    sw.WriteLine(message);
//                }


//            }
//        }
//    }
//}

using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common;
using System.Net;
using Common.Exceptions;
using DataTransferObjects.SharedModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using CleanArchitecture.Application.Common.Exceptions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace WebFramework.Middlewares
{
    public static class CustomExceptionHandlerMiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CustomExceptionHandlerMiddleware>();
        }
    }

    public class CustomExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<CustomExceptionHandlerMiddleware> _logger;

        public CustomExceptionHandlerMiddleware(RequestDelegate next,
            IWebHostEnvironment env,
            ILogger<CustomExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _env = env;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            string message = null;
            HttpStatusCode httpStatusCode = HttpStatusCode.InternalServerError;
            ApiResultStatusCode apiStatusCode = ApiResultStatusCode.ServerError;

            try
            {
                await _next(context);
            }
            catch (AppException exception)
            {
                _logger.LogError(exception, exception.Message);
                httpStatusCode = HttpStatusCode.BadRequest;
                apiStatusCode = exception.ApiStatusCode;


                //if (_env.IsDevelopment())
                //{
                //    var dic = new Dictionary<string, string>
                //    {
                //        ["Exception"] = exception.Message,
                //        ["StackTrace"] = exception.StackTrace,
                //    };
                //    if (exception.InnerException != null)
                //    {
                //        dic.Add("InnerException.Exception", exception.InnerException.Message);
                //        dic.Add("InnerException.StackTrace", exception.InnerException.StackTrace);
                //    }
                //    if (exception.AdditionalData != null)
                //        dic.Add("AdditionalData", JsonConvert.SerializeObject(exception.AdditionalData));

                //    message = JsonConvert.SerializeObject(dic);
                //}
                //else
                //{
                    message = exception.Message;
                //}
                await WriteToResponseAsync(true);
            }
         
            catch (SecurityTokenExpiredException exception)
            {
                _logger.LogError(exception, exception.Message);
                SetUnAuthorizeResponse(exception);
                await WriteToResponseAsync(false);
            }
            catch (UnauthorizedAccessException exception)
            {
                _logger.LogError(exception, exception.Message);
                SetUnAuthorizeResponse(exception);
                await WriteToResponseAsync(false);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, exception.Message);

                if (_env.IsDevelopment())
                {
                    var dic = new Dictionary<string, string>
                    {
                        ["Exception"] = exception.Message,
                        ["StackTrace"] = exception.StackTrace,
                    };
                    message = JsonConvert.SerializeObject(dic);
                }
                httpStatusCode = HttpStatusCode.InternalServerError;
                await WriteToResponseAsync(false);
            }

            async Task WriteToResponseAsync(bool isSuccess)
            {
                if (context.Response.HasStarted)
                    throw new InvalidOperationException("The response has already started, the http status code middleware will not be executed.");

                var result = new ApiResult<object>(false, apiStatusCode, null,message);
                var json = JsonConvert.SerializeObject(result).ToString();

                if (isSuccess)
                    context.Response.StatusCode = 200;
                else
                    context.Response.StatusCode = (int)apiStatusCode;
                context.Response.ContentType = "application/json";
                //await context.Response.WriteAsync(json);
            }

            void SetUnAuthorizeResponse(Exception exception)
            {
                httpStatusCode = HttpStatusCode.Unauthorized;
                apiStatusCode = ApiResultStatusCode.UnAuthorized;

                if (_env.IsDevelopment())
                {
                    var dic = new Dictionary<string, string>
                    {
                        ["Exception"] = exception.Message,
                        ["StackTrace"] = exception.StackTrace
                    };
                    if (exception is SecurityTokenExpiredException tokenException)
                        dic.Add("Expires", tokenException.Expires.ToString());

                    message = JsonConvert.SerializeObject(dic);
                }
            }
        }
    }
}
