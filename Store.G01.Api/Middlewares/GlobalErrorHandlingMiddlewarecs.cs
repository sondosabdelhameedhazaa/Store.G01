using Domain.Exceptions;
using Shared.errorModels;


namespace Store.G01.Api.Middlewares
{
    public class GlobalErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalErrorHandlingMiddleware> _logger;

        public GlobalErrorHandlingMiddleware(RequestDelegate next, ILogger<GlobalErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }


        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
                if (context.Response.StatusCode == StatusCodes.Status404NotFound)
                {
                    await HandingNotFoundEndPointAsync(context);

                }

            }
            catch (Exception ex)
            {
                // log exception
                _logger.LogError(ex, ex.Message);
                await HandlingErrorAsync(context, ex);
            }

        }

        private static async Task HandlingErrorAsync(HttpContext context, Exception ex)
        {
            // 1 set Status code for response
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            // 2 set content type for response
            context.Response.ContentType = "application/json";
            // 3 response object (body)
            var response = new ErrorDetails()
            {
                StatusCode = StatusCodes.Status500InternalServerError,
                ErrorMessage = ex.Message
            };
            response.StatusCode = ex switch
            {
                NotFoundException => StatusCodes.Status404NotFound,
                BadRequestException => StatusCodes.Status400BadRequest,
                UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
                ValidationExceptions => HandleValidationExceptionAsync((ValidationExceptions)ex,response),
                _ => StatusCodes.Status500InternalServerError,
            };

            context.Response.StatusCode = response.StatusCode;
            // 4 Return Response
            await context.Response.WriteAsJsonAsync(response);
        }

        private static async Task HandingNotFoundEndPointAsync(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            var response = new ErrorDetails()
            {
                StatusCode = 404,
                ErrorMessage = $" End Point {context.Request.Path} is Not Found "
            };
            await context.Response.WriteAsJsonAsync(response);
        }




        private static int HandleValidationExceptionAsync(ValidationExceptions ex, ErrorDetails response)
        {
            response.Errors = ex.Errors;
            return StatusCodes.Status400BadRequest;
        }



    }
}