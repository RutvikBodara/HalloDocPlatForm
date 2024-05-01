//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Filters;

//namespace hellodocsrsmvc.Controllers
//{
//    public class AuthUser : ActionFilterAttribute, IAuthorizationFilter
//    {

//        public void OnAuthorization(AuthorizationFilterContext filterContext)
//        {
//            if (filterContext.HttpContext.Session.GetString("ASPID") == null)
//            {
//                filterContext.Result = new RedirectResult("/PatientSide/LoginPage");
//            }
//        }
//        public override void OnResultExecuting(ResultExecutingContext filterContext)
//        {
//            filterContext.HttpContext.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
//            filterContext.HttpContext.Response.Headers["Expires"] = "-1";
//            filterContext.HttpContext.Response.Headers["Pragma"] = "no-cache";
//            base.OnResultExecuting(filterContext);
//        }
//    }
//}
