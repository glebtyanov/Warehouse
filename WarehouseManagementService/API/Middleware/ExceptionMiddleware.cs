using BLL.Exceptions;
using System.Net;

namespace API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception exception)
            {
                var response = context.Response;

                response.ContentType = "text/plain";

                var (httpStatusCode, message) = GetResponse(exception);

                response.StatusCode = (int)httpStatusCode;

                await response.WriteAsync(message);
            }
        }

        public (HttpStatusCode httpStatusCode, string message) GetResponse(Exception exception)
        {
            HttpStatusCode httpStatusCode;

            switch (exception)
            {
                case NotFoundException:
                    httpStatusCode = HttpStatusCode.NotFound;
                    break;

                case AlreadyExistsException:
                    httpStatusCode = HttpStatusCode.Conflict;
                    break;

                case NotValidException:
                    httpStatusCode = HttpStatusCode.UnprocessableEntity;
                    break;

                default:
                    httpStatusCode = HttpStatusCode.InternalServerError;
                    break;
            }

            return (httpStatusCode, exception.Message);
        }

    }
}
