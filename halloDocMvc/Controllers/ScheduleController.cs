using AdminHalloDoc.Controllers.Login;
using HalloDoc.BAL.Interface;
using HalloDoc.DAL.Data;
using HalloDoc.DAL.DataModels;
using HalloDoc.DAL.ViewModals;
using HalloDoc.DAL.ViewModals.Schedule;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.CodeAnalysis.Scripting.Hosting;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Converters;
using Org.BouncyCastle.Crypto.Operators;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace hellodocsrsmvc.Controllers
{
    [AdminAuth("admin")]
    [RoleAuth(2)]
    public class ScheduleController : Controller
    {
        //private readonly HalloDocDBContext _db;
        private readonly IAdminPartialsRepo _adminPartialsRepo;
        private readonly IScheduleRepo _scheduleRepo;
        private readonly IJwtAuthInterface _JwtAuth;
        public ScheduleController(HalloDocDBContext db, IAdminPartialsRepo adminPartialsRepo, IScheduleRepo scheduleRepo, IJwtAuthInterface jwtAuthInterface)
        {
            //_db = db;
            _adminPartialsRepo = adminPartialsRepo;
            _scheduleRepo = scheduleRepo;
            _JwtAuth = jwtAuthInterface;
        }
        public async Task<IActionResult> SchedulingIndexPage()
        {
            return await Task.Run(() => PartialView("/Views/Schedule/_EditScheduleMain.cshtml"));
        }
        public async Task<IActionResult> DayWiseSchedule()
        {
            return await Task.Run(() => PartialView("/Views/Schedule/_DayWiseScheduleTable.cshtml"));
        }
        public async Task<IActionResult> GetShiftData(DateOnly date, int? regid, DateOnly? enddate)
        {
            //add providers all with 
            ///model which have physician  -- also have details 2nd table with all data
            ScheduleViewModel details = await _scheduleRepo.DayWiseSchedule(date, regid, enddate);
            return Json(details);
        }
        public async Task<IActionResult> GetShifts()
        {
            IEnumerable<Shift>? shifts = await _scheduleRepo.GetShifts();
            return Json(shifts);
        }
        public async Task<IActionResult> CreateSchedule(ScheduleViewModel model, Shift shift, Shiftdetail shiftdetail)
        {
            var token = Request.Cookies["Jwt"];
            if (token == null)
            {
                return RedirectToAction("LoginPage", "AdminLogin");
            }
            UserDataViewModel jwtData = _JwtAuth.AccessData(token);
            if (jwtData.UserId == null)
            {
                return RedirectToAction("LoginPage", "AdminLogin");
            }

            bool ValidateShift = await _scheduleRepo.IsShiftValid(shift, shiftdetail, int.Parse(jwtData.UserId), model, 1, null);

            if (ValidateShift != true)
            {
                return Json("Not Valid");
            }
            else
            {
                bool status = await _scheduleRepo.CreateSchedule(shift, shiftdetail, int.Parse(jwtData.UserId), model, 1, null);
            return Json(status);
            }

        }
        public async Task<IActionResult> WeekWiseSchedule()
        {
            return await Task.Run(() => PartialView("/Views/Schedule/_WeekWiseScheduleTable.cshtml"));
        }
        public async Task<IActionResult> MonthWiseSchedule()
        {
            var data = await _scheduleRepo.MonthWiseScheduleStartData();
            ViewBag.startDate = data;
            return PartialView("/Views/Schedule/_MonthWiseSchedule.cshtml");
        }
        //public async Task<IActionResult> MonthWiseShiftData(int month, int?regid)
        //{
        //    var token = Request.Cookies["Jwt"];
        //    if (token == null)
        //    {
        //        return RedirectToAction("LoginPage", "AdminLogin");
        //    }
        //    UserDataViewModel jwtData = _JwtAuth.AccessData(token);
        //    if (jwtData.UserId == null)
        //    {
        //        return RedirectToAction("LoginPage", "AdminLogin");
        //    }
        //    IEnumerable<MonthShiftViewModel> data = await _scheduleRepo.MonthShiftData(month,regid,int.Parse(jwtData.UserId));
        //    return Json(data);
        //}
        public async Task<IActionResult> FetchScheduleData(int ShiftDetailsids)
        {
            var sd = await _scheduleRepo.FetchScheduleData(ShiftDetailsids);
            return Json(sd);
        }
        public async Task<IActionResult> EditShiftDetails(int shiftdetailsid, DateOnly date, TimeOnly start, TimeOnly end)
        {
            //validate One Shift
            bool IsShiftValidv =await _scheduleRepo.ValidateSingleShift(shiftdetailsid, date, start, end);

            if (!IsShiftValidv)
            {
                return Json("Not Valid");
            }
            else 
            { 
            bool status = await _scheduleRepo.EditShiftDetails(shiftdetailsid, date, start, end);
            return Json(status);
            }
        }
        public async Task<IActionResult> ChangeShiftStatus(int shiftdetailsid)
        {

            bool status = await _scheduleRepo.ChangeShiftStatus(shiftdetailsid);
            return Json(status);
        }
        public async Task<IActionResult> ProviderOnCallPartial(int? regid)
        {
            //off duty and on call physician
            var token = Request.Cookies["Jwt"];
            if (token == null)
            {
                return RedirectToAction("LoginPage", "AdminLogin");
            }
            UserDataViewModel jwtData = _JwtAuth.AccessData(token);
            if (jwtData.UserId == null)
            {
                return RedirectToAction("LoginPage", "AdminLogin");
            }
            ProviderOnCallViewModel model = await _scheduleRepo.ProviderOnCallPartial(int.Parse(jwtData.UserId), regid);
            return PartialView("/Views/Schedule/ProviderOnCall.cshtml", model);
        }
        public async Task<IActionResult> ShiftsForReviewPartial(int? PageNumber, int? regid)
        {
            var token = Request.Cookies["Jwt"];
            if (token == null)
            {
                return RedirectToAction("LoginPage", "AdminLogin");
            }
            UserDataViewModel jwtData = _JwtAuth.AccessData(token);
            if (jwtData.UserId == null)
            {
                return RedirectToAction("LoginPage", "AdminLogin");
            }

            PendingShiftCombine? model = await _scheduleRepo.PendingShifts(int.Parse(jwtData.UserId), PageNumber, regid);
            return PartialView("/Views/Schedule/ShiftForReview.cshtml", model);
        }
        public async Task<IActionResult> ApproveOrDeleteShift(List<int> idlist, string reqtype)
        {
            bool status = await _scheduleRepo.ApproveOrDeleteShift(idlist, reqtype);
            return Json(status);
        }
        public async Task<IActionResult> ShiftMonthsData(int month, int year, int? regid)
        {
            IEnumerable<ReqestedShiftDetailsviewmodel>? model = await _scheduleRepo.ShiftMonthsData(month, year, regid, null);
            return Json(model);
        }
    }
}
