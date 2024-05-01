using AdminHalloDoc.Controllers.Login;
using HalloDoc.BAL.Interface;
using HalloDoc.DAL.Data;
using HalloDoc.DAL.DataModels;
using HalloDoc.DAL.ViewModals;
using HalloDoc.DAL.ViewModals.Family;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using NuGet.Protocol.Plugins;
using System.Collections;
using System.Drawing.Text;
using System.IO.Compression;
using System.Linq;
using System.Reflection.Metadata;

namespace hellodocsrsmvc.Controllers
{

    //[AuthUser]

    [AdminAuth("patient")]
    public class PatientDashboardController : Controller
    {
        //private readonly HalloDocDBContext _db;
        private readonly IContactRepository _contactRepository;
        private readonly IPatientDashboardRepo _PatientDashboardRepo;
        private readonly IJwtAuthInterface _JwtAuth;

        public PatientDashboardController(HalloDocDBContext db, IContactRepository contactRepository, IPatientDashboardRepo PatientDashboardRepo, IJwtAuthInterface jwtAuthInterface)
        {
            //_db = db;
            _contactRepository = contactRepository;
            _PatientDashboardRepo = PatientDashboardRepo;
            _JwtAuth = jwtAuthInterface;
        }

        public async Task<IActionResult> PatientDashBoard(int id, int roleid)
        {
            //var Patient_asp_id = HttpContext.Session.GetString("ASPID");
            //if(Patient_asp_id == null)
            //{
            //    return RedirectToAction("LoginPage", "PatientSide");
            //}
            //var UserDetails = await _PatientDashboardRepo.User(patientID);

            //if (UserDetails != null)
            //{
            //    ViewBag.firstname = UserDetails.Firstname;
            //    ViewBag.lastname = UserDetails.Lastname;
            //}
            //else
            //{
            //    return RedirectToAction("LoginPage", "PatientSide");

            //}
            var token = Request.Cookies["Jwt"];
            if (token == null)
            {
                return View();
            }
            UserDataViewModel jwtData = _JwtAuth.AccessData(token);

            ViewBag.firstname = jwtData.FirstName;
            ViewBag.lastname = jwtData.LastName;
            return await Task.Run(() => View());
        }

        public async Task<IActionResult> PatientDashBoardTable()
        {
            var token = Request.Cookies["Jwt"];
            if (token == null)
            {
                return View();
            }
            UserDataViewModel jwtData = _JwtAuth.AccessData(token);

            var patUserId = await _PatientDashboardRepo.User(jwtData.UserId);

            if (patUserId == null)
            {
                return RedirectToAction("LoginPage", "PatientSide");
            }

            var patientData = await _PatientDashboardRepo.PatientRequestTable(patUserId);

            return PartialView("/Views/PatientDashBoard/_PatientRequestTablePartial.cshtml", patientData);
        }

        public async Task<IActionResult> PatientProfileEdit()
        {
            var token = Request.Cookies["Jwt"];
            if (token == null)
            {
                return RedirectToAction("LoginPage", "PatientSide");
            }
            UserDataViewModel jwtData = _JwtAuth.AccessData(token);

            var patUserId = await _PatientDashboardRepo.User(jwtData.UserId);
            if (patUserId == null)
            {
                return RedirectToAction("LoginPage", "PatientSide");
            }
            var useriddata = await _PatientDashboardRepo.PatientProfileData(patUserId, jwtData.UserId);

            return PartialView("/Views/PatientDashBoard/_ProfileEditPartial.cshtml", useriddata);
        }

        public async Task<IActionResult> PatientDocumentRequestWise(int id)
        {
            var useriddata = await _PatientDashboardRepo.CheckRequest(id);

            if (useriddata == null)
            {
                return View("Error");
            }

            var token = Request.Cookies["Jwt"];
            if (token == null)
            {
                return RedirectToAction("LoginPage", "PatientSide");
            }
            UserDataViewModel jwtData = _JwtAuth.AccessData(token);

            var patUserId = await _PatientDashboardRepo.User(jwtData.UserId);
            if (patUserId == null)
            {
                return RedirectToAction("LoginPage", "PatientSide");
            }
            //var docuserId = useriddata.Userid;
            //var usertabledata =_db.Users.Where(x => x.Userid == docuserId).FirstOrDefault();

            var Patientdocuments = await _PatientDashboardRepo.RequestWiseDocument(id);

            HttpContext.Session.SetString("reqid", id.ToString());

            ViewBag.firstname = patUserId.Firstname;

            return PartialView("/Views/PatientDashBoard/_PatientDocumentPartial.cshtml", Patientdocuments);
        }

        [HttpPost]
        public async Task<IActionResult> PatientDocumentRequestWise(IFormFile file)
        {

            var reqid = HttpContext.Session.GetString("reqid");
            if (reqid == null)
            {
                return RedirectToAction("LoginPage", "PatientSide");
            }
            await _PatientDashboardRepo.PatientDocumentRequestWise(file, reqid);

            var token = Request.Cookies["Jwt"];
            if (token == null)
            {
                return RedirectToAction("LoginPage", "PatientSide");
            }
            UserDataViewModel jwtData = _JwtAuth.AccessData(token);

            return RedirectToAction("PatientDashBoard", "PatientDashboard", new { patientID = jwtData.UserId });

        }

        public async Task<IActionResult> DownLoadAll(int id)
        {
            var zipName = $"Docs-{DateTime.Now.ToString("yyyy_MM_dd-HH_mm_ss")}.zip";

            using (MemoryStream ms = new MemoryStream())
            {
                using (var zipFile = new ZipArchive(ms, ZipArchiveMode.Create, true))
                {
                    IEnumerable<Requestwisefile>? files = await _PatientDashboardRepo.getRequestwisefiles(id);
                    if (files == null)
                    {
                        //handle here
                        return Ok(null);
                    }
                    //var files = _db.Requestwisefiles.Where(u => u.Requestid == id).ToList();
                    foreach (var file in files)
                    {
                        var path = Path.Combine(Environment.CurrentDirectory, "wwwroot", "uploads", file.Filename);

                        var addfile = zipFile.CreateEntry(file.Filename);

                        using (var str = new FileStream(path, FileMode.Open, FileAccess.Read))
                        using (var zipStr = addfile.Open())
                        {
                            await str.CopyToAsync(zipStr);
                        }
                    }
                }
                return File(ms.ToArray(), "application/zip", zipName);
            }
        }
        //public async Task<IActionResult> DownloadFile(String filename) { 
        //    var path =Path.Combine(Environment.CurrentDirectory,"wwwroot" , "uploads" , filename);

        //    if (!System.IO.File.Exists(path))
        //    {
        //        return NotFound();
        //    }
        //    return await Task.Run(() => PhysicalFile(path, "application/jpg", filename));
        //}

        [HttpPost]
        public async Task<IActionResult> EditProfile(PatientProfileEdit obj, string aspid, int userid)
        {
            if (ModelState.IsValid)
            {
                await _PatientDashboardRepo.EditProfile(obj, aspid, userid);
            }
            TempData["success"] = "Profile Edited";
            return RedirectToAction("PatientDashBoard", "PatientDashboard", new { patientID = aspid });
        }

        public async Task<IActionResult> logout()
        {
            //HttpContext.Session.Clear();
            Response.Cookies.Delete("Jwt");
            return await Task.Run(() => RedirectToAction("LoginPage", "PatientSide"));
        }

        public async Task<IActionResult> MeRequest()
        {
            var token = Request.Cookies["Jwt"];
            if (token == null)
            {
                return RedirectToAction("LoginPage", "PatientSide");
            }
            UserDataViewModel jwtData = _JwtAuth.AccessData(token);

            var meRequest = await _PatientDashboardRepo.MeRequestGet(jwtData.UserId);

            return PartialView("/Views/PatientDashBoard/_MePartial.cshtml", meRequest);
        }
        
        public async Task<IActionResult> LoadRegion()
        {
            IEnumerable<Region> regions = await _PatientDashboardRepo.LoadRegion();
            return Json(regions);
        }
       
        public async Task<IActionResult> MeRequestSubmit(CreatePatientRequestviewmodel obj, IFormFile? uploadFile)
        {
            bool stat = false;
            if (ModelState.IsValid)
            {
                //convert in bool
                //if (uploadFile != null)
                    await _contactRepository.MeRequest(obj, uploadFile);
                stat = true;
                return Json(stat);
            }
            return Json(stat);
        }
       
        public IActionResult SomeoneRequest()
        {
            return PartialView("/Views/PatientDashBoard/_SomeOneElsePartial.cshtml");
        }

        public IActionResult SomeoneRequestSubmit(FriendRequest obj)
        {
            bool stat = false;
            if (ModelState.IsValid)
            {
                var token = Request.Cookies["Jwt"];
                if (token == null)
                {
                    return RedirectToAction("LoginPage", "PatientSide");
                }
                UserDataViewModel jwtData = _JwtAuth.AccessData(token);
                //convert in bool
                _contactRepository.SomeOneRequest(obj, jwtData.UserId);
                stat = true;
                return Json(stat);
            }

            return Json(stat);
        }

    }
}
