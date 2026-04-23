using System;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;

namespace DesktopShop.API.Middleware
{
    public class ExceptionHandlingFilter : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            HttpStatusCode statusCode;
            string message;

            if (context.Exception is InvalidOperationException)
            {
                statusCode = HttpStatusCode.BadRequest;
                message = context.Exception.Message;
            }
            else
            {
                statusCode = HttpStatusCode.InternalServerError;
                message = "Đã xảy ra lỗi hệ thống.";
            }

            context.Response = context.Request.CreateResponse(statusCode, new { error = message });
        }
    }
}
