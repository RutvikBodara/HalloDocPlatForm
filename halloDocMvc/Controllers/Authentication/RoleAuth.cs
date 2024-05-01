using HalloDoc.BAL.Interface;
using HalloDoc.DAL.Data;
using HalloDoc.DAL.ViewModals;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.JsonWebTokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace AdminHalloDoc.Controllers.Login
{
    /// <summary>
    /// Role Base Authentication For Admin And Provider
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public class RoleAuth : ActionFilterAttribute, IAuthorizationFilter
    {
        private readonly int _menuid;
        public RoleAuth(int menuid)
        {
            _menuid = menuid;
        }

        //clear Cache
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            filterContext.HttpContext.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
            filterContext.HttpContext.Response.Headers["Expires"] = "-1";
            //for giving directions
            filterContext.HttpContext.Response.Headers["Pragma"] = "no-cache";
            base.OnResultExecuting(filterContext);
        }

        public void OnAuthorization(AuthorizationFilterContext filterContext)
        {
            var jwtservice = filterContext.HttpContext.RequestServices.GetService<IJwtAuthInterface>();

            if (jwtservice == null)
            {
                filterContext.Result = new RedirectResult("~/PatientSide");
                return;
            }

            var request = filterContext.HttpContext.Request;
            var token = request.Cookies["Jwt"];
            var rolemenu = request.Cookies["RoleMenu"];

            if (token == null || rolemenu == null || !jwtservice.ValidateToken(token, out JwtSecurityToken jwtSecurityTokenHandler))
            {
                filterContext.Result = new RedirectResult("~/Home/Index");
                return;
            }

            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            var roleid = jwt.Claims.First(y => y.Type == "Roleid").Value;

            var RoleArray = rolemenu.Split(',');
            bool flag = false;

            foreach (var role in RoleArray)
            {
                if (role == "")
                {
                    break;
                }
                if (_menuid == int.Parse(role))
                {
                    flag = true;
                }
            }

            if (!flag)
            {
                filterContext.Result = new RedirectResult("~/PatientSide/AccessDenied");
            }
        }
    }
}