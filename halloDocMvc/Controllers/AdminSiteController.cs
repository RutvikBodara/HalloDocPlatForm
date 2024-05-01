using AdminHalloDoc.Controllers.Login;
using HalloDoc.BAL.Interface;
using HalloDoc.DAL.Data;
using HalloDoc.DAL.DataModels;
using HalloDoc.DAL.ViewModals;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Diagnostics.Tracing.Parsers.AspNet;

namespace hellodocsrsmvc.Controllers
{

    //[adminauthentication]
    [AdminAuth("admin")]
    public class AdminSiteController : Controller
    {
        //private readonly HalloDocDBContext _db;
        private readonly IJwtAuthInterface _JwtAuth;
        private readonly IContactRepository _contactRepository;

        public AdminSiteController(HalloDocDBContext db, IJwtAuthInterface jwtAuth, IContactRepository contactRepository)
        {
            _JwtAuth = jwtAuth;
            _contactRepository = contactRepository;
        }
        //[adminauthentication]
        [RoleAuth(1)]
        public async Task<IActionResult> AdminDashBoard()
        {
            var token = Request.Cookies["Jwt"];
            if (token == null)
            {
                return View();
            }
            UserDataViewModel jwtData = _JwtAuth.AccessData(token);
            if (jwtData == null)
            {
                return RedirectToAction("LoginPage", "PatientSide");
            }
            if (jwtData.UserId == null)
            {
                return RedirectToAction("LoginPage", "PatientSide");
            }
            var admindata = _contactRepository.Admin(jwtData.UserId);
            if (admindata == null)
            {
                ViewBag.firstname = jwtData.FirstName;
                ViewBag.lastname = jwtData.LastName;
            }
            else
            {
                ViewBag.firstname = admindata.Firstname;
                ViewBag.lastname = admindata.Lastname;
            }
            IEnumerable<Rolemenu>? model = await _contactRepository.getRoleMenu(int.Parse(jwtData.roleid));
            return View(model);
        }
        public IActionResult LoadAdminDashBoardPartial()
        {
            return PartialView("/Views/AdminPartials/_AdminDashBoard.cshtml");
        }
        public IActionResult LoadProviderLocationPartial()
        {
            return PartialView("/Views/AdminPartials/_LoadProviderLocationPartial.cshtml");
        }

    }
}
