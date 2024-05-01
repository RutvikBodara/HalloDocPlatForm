using AdminHalloDoc.Controllers.Login;
using HalloDoc.BAL.Interface;
using HalloDoc.DAL.Data;
using HalloDoc.DAL.DataModels;
using HalloDoc.DAL.ViewModals;
using Microsoft.AspNetCore.Mvc;

namespace hellodocsrsmvc.Controllers
{
    [AdminAuth("physician")]
    public class ProviderSiteController : Controller
    {
        //[adminauthentication]

        private readonly HalloDocDBContext _db;
        private readonly IJwtAuthInterface _JwtAuth;

        public ProviderSiteController(HalloDocDBContext db, IJwtAuthInterface jwtAuth)
        {
            _db = db;
            _JwtAuth = jwtAuth;
        }
        //[adminauthentication]
        [RoleAuth(21)]
        public IActionResult ProviderDataLoader(string id, int roleid)
        {
            var token = Request.Cookies["Jwt"];
            if (token == null)
            {
                return View();
            }
            UserDataViewModel jwtData = _JwtAuth.AccessData(token);
            var physiciandata = _db.Physicians.FirstOrDefault(x => x.Physicianid == int.Parse(id));
            if (physiciandata == null)
            {
                ViewBag.firstname = jwtData.FirstName;
                ViewBag.lastname = jwtData.LastName;
            }
            else
            {
                ViewBag.firstname = physiciandata.Firstname;
                ViewBag.lastname = physiciandata.Lastname;
            }
            IEnumerable<Rolemenu> model = _db.Rolemenus.Where(x => x.Roleid == roleid).ToList();
            return View(model);
        }
        public IActionResult LoadProviderDashBoardPartial()
        {
            return PartialView("/Views/ProviderAccount/ProviderDashBoard/_ProviderDashBoard.cshtml");
        }
    }
}