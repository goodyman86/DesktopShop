using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebApplication1.Filters
{
    public class AdminAuthFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var session = filterContext.HttpContext.Session;
            if (session == null || session["AdminId"] == null)
            {
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary(new { controller = "Auth", action = "Login" }));
                return;
            }

            base.OnActionExecuting(filterContext);
        }
    }
}
