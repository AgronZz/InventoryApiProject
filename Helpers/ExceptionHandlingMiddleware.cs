namespace InventoryApiProject.Helpers
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;   //Represents next middleware.
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)  //Receives request context, contains: request,response,headers,user info
        {
            try
            {
                await _next(context);  //Pass request to next middleware
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred");  //Saves error in logs

                context.Response.ContentType = "application/json";  //response type - return JSON.
                context.Response.StatusCode = 500; //HTTP 500 meand Internal Server Error

                await context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(new
                {
                    Message = "Something went wrong",
                    Error = ex.Message
                }));
            }
        }
    }
}
