using Microsoft.AspNetCore.Mvc;
using MusteriMobilUygulamaAPI.Common;
using MusteriMobilUygulamaAPI.Models;

namespace MmuAPI
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        
        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Exception occurred: {Message}", exception.Message);
                 
                await LoggerX.LogIn(context, "Exception occurred: " + exception.Message + " --- " + exception.StackTrace);

                var problemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "Server Error"
                };
                cResponseModel<int> oResponseModel = new cResponseModel<int>();

                oResponseModel.errorModel.ErrorCode = -9999;
                oResponseModel.errorModel.ErrorMessage = "Exception occurred: " + exception.Message + " --- " + exception.StackTrace;
                
                context.Response.StatusCode = StatusCodes.Status200OK;

                await context.Response.WriteAsJsonAsync(oResponseModel);
            }
        }
    }
}
