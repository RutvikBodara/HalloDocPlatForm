using HalloDoc.BAL.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.JsonWebTokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace AdminHalloDoc.Controllers.Login
{
    [AttributeUsage(AttributeTargets.All)]
    public class AdminAuth : ActionFilterAttribute, IAuthorizationFilter
    {
        private readonly string _role;
        public AdminAuth(string role)
        {
            _role = role;

        }
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            filterContext.HttpContext.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
            filterContext.HttpContext.Response.Headers["Expires"] = "-1";
            filterContext.HttpContext.Response.Headers["Pragma"] = "no-cache";
            base.OnResultExecuting(filterContext);
        }
        public void OnAuthorization(AuthorizationFilterContext filterContext)
        {

            var jwtservice = filterContext.HttpContext.RequestServices.GetService<IJwtAuthInterface>();

            if (jwtservice == null)
            {
                filterContext.Result = new RedirectResult("~/AdminLogin");
                return;
            }

            var request = filterContext.HttpContext.Request;
            var token = request.Cookies["Jwt"];

            if (token == null || !jwtservice.ValidateToken(token, out JwtSecurityToken jwtSecurityTokenHandler))
            {
                filterContext.Result = new RedirectResult("~/Home/Index");
                return;
            }
            var roles = jwtSecurityTokenHandler.Claims.FirstOrDefault(claiim => claiim.Type == ClaimTypes.Role);

            if (roles == null)
            {
                filterContext.Result = new RedirectResult("~/Home/Index");
                return;
            }
            //if ((roles.Value == "admin") && (_role == "patient"))
            //{
            //    filterContext.Result = new RedirectResult("~/AdminLogin/LoginPage");
            //    return;
            //}

            //if ((roles.Value == "patient") && (_role == "admin" ))
            //{
            //    filterContext.Result = new RedirectResult("~/PatientSide/LoginPage");
            //    return;
            //}
            if (string.IsNullOrWhiteSpace(_role) || roles.Value != _role)
            {
                filterContext.Result = new RedirectResult("~/PatientSide/AccessDenied");
            }
        }
    }
}