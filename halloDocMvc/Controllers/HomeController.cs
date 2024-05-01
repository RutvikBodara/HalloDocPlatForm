using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using hellodocsrsmvc.Models;
using HalloDoc.DAL.ViewModals;
using HalloDoc.BAL.Interface;

namespace hellodocsrsmvc.Controllers;

public class HomeController : Controller
{
    private readonly IJwtAuthInterface _JwtAuth;
    private readonly IContactRepository _contactRepository;
    public HomeController(IJwtAuthInterface jwtAuth, IContactRepository contactRepository)
    {
        _JwtAuth = jwtAuth;
        _contactRepository = contactRepository;
    }

    public IActionResult Index()
    {
        //var token = Request.Cookies["Jwt"];
        //if (token != null)
        //{
        //    UserDataViewModel jwtData = _JwtAuth.AccessData(token);
        //    if (jwtData.UserId != null)
        //    {
        //        var method = "";
        //        var controller = "";
        //        if (jwtData.Role == "admin")
        //        {
        //            method = "AdminDashboard";
        //            controller = "AdminSite";

        //            var admindata = _contactRepository.Admin(jwtData.UserId);
        //            if (admindata == null)
        //            {
        //                return View();
        //            }
        //            return RedirectToAction(method, controller, new { id = admindata.Adminid, roleid = jwtData.roleid });
        //        }
        //        else if (jwtData.Role == "physician")
        //        {
        //            controller = "ProviderSite";
        //            method = "ProviderDataLoader";
        //        }
        //        else if (jwtData.Role == "patient")
        //        {
        //            controller = "PatientDashBoard";
        //            method = "PatientDashboard";
        //        }
        //        return RedirectToAction(method, controller, new { id = jwtData.UserId, roleid = jwtData.roleid });
        //    }
        //}
        return View();
    }
}

