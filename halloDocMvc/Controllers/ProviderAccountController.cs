using AdminHalloDoc.Controllers.Login;
using HalloDoc.BAL.Interface;
using HalloDoc.BAL.Repository;
using HalloDoc.DAL.DataModels;
using HalloDoc.DAL.ViewModals;
using HalloDoc.DAL.ViewModals.AdminDashBoardActions;
using HalloDoc.DAL.ViewModals.AdminDashBoardActions.NewRequestAction;
using HalloDoc.DAL.ViewModals.AdminDashBoardViewModels;
using HalloDoc.DAL.ViewModals.ProviderAccount;
using HalloDoc.DAL.ViewModals.Schedule;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.VisualBasic;
using NuGet.Common;
using Rotativa.AspNetCore;
using System.Drawing;
using System.Transactions;

namespace hellodocsrsmvc.Controllers
{
    [AdminAuth("physician")]
    public class ProviderAccountController : Controller
    {
        private readonly IProviderRepo _providerRepo;
        private readonly IJwtAuthInterface _JwtAuth;
        private readonly IAdminPartialsRepo _IAdminPartialsRepo;
        private readonly IPatientDashboardRepo _IPatientDashboardRepo;
        private readonly IScheduleRepo _scheduleRepo;
        private readonly IContactRepository _ContactRepository;
        public ProviderAccountController(IProviderRepo providerRepo, IJwtAuthInterface jwtAuth, IAdminPartialsRepo adminPartialsRepo, IPatientDashboardRepo iPatientDashboardRepo, IScheduleRepo scheduleRepo, IContactRepository contactRepository)
        {
            _providerRepo = providerRepo;
            _JwtAuth = jwtAuth;
            _IAdminPartialsRepo = adminPartialsRepo;
            _IPatientDashboardRepo = iPatientDashboardRepo;
            _scheduleRepo = scheduleRepo;
            _ContactRepository = contactRepository;
        }
        enum ProviderRoles
        {
            MyProfile = 5,
            MySchedule = 9,
            SendOrder = 17,
            Dashboard = 21,
            Invoicing = 28
        }
        #region PhysicianActions
        public async Task<IActionResult> UpdateLocation(float latitude, float longtitude)
        {
            var token = Request.Cookies["Jwt"];
            if (token == null)
            {
                return RedirectToAction("LoginPage", "PatientSide");
            }
            UserDataViewModel jwtData = _JwtAuth.AccessData(token);
            if (jwtData.UserId == null)
            {
                return RedirectToAction("LoginPage", "PatientSide");
            }
            bool status = await _providerRepo.UpdateLocation(latitude, longtitude, int.Parse(jwtData.UserId));
            return Json(status);
        }

        [RoleAuth((int)ProviderRoles.Dashboard)]
        public async Task<IActionResult> ProviderDashBoardMain()
        {
            var token = Request.Cookies["Jwt"];
            if (token == null)
            {
                return RedirectToAction("LoginPage", "PatientSide");
            }
            UserDataViewModel jwtData = _JwtAuth.AccessData(token);
            if (jwtData.UserId == null)
            {
                return RedirectToAction("LoginPage", "PatientSide");
            }
            DataCount datacount = await _providerRepo.ProviderDashBoardMain(int.Parse(jwtData.UserId));
            return PartialView("/Views/ProviderAccount/ProviderDashBoard/_ProviderDashBoard.cshtml", datacount);
        }

        [RoleAuth((int)ProviderRoles.Dashboard)]
        public async Task<IActionResult> NewRequests(string? search, int? regid, int? reqType, int? PageNumber)
        {
            var token = Request.Cookies["Jwt"];
            if (token == null)
            {
                return RedirectToAction("LoginPage", "PatientSide");
            }
            UserDataViewModel jwtData = _JwtAuth.AccessData(token);
            if (jwtData.UserId == null)
            {
                return RedirectToAction("LoginPage", "PatientSide");
            }
            NewRequestCombineViewModel combineModel = await _providerRepo.NewRequests(search, regid, reqType, PageNumber, int.Parse(jwtData.UserId));
            return PartialView("/Views/ProviderAccount/ProviderDashBoard/_NewTablePartial.cshtml", combineModel);
        }

        [RoleAuth((int)ProviderRoles.Dashboard)]
        public async Task<IActionResult> PendingRequests(string? search, int? regid, int? reqType, int? PageNumber)
        {
            var token = Request.Cookies["Jwt"];
            if (token == null)
            {
                return RedirectToAction("LoginPage", "PatientSide");
            }
            UserDataViewModel jwtData = _JwtAuth.AccessData(token);
            if (jwtData.UserId == null)
            {
                return RedirectToAction("LoginPage", "PatientSide");
            }
            PendingRequestCombine Pendingquery = await _providerRepo.PendingRequests(search, regid, reqType, PageNumber, int.Parse(jwtData.UserId));
            return PartialView("/Views/ProviderAccount/ProviderDashBoard/_PendingTablePartial.cshtml", Pendingquery);
        }
        [RoleAuth((int)ProviderRoles.Dashboard)]
        public async Task<IActionResult> ActiveRequests(string? search, int? regid, int? reqType, int? PageNumber)
        {
            var token = Request.Cookies["Jwt"];
            if (token == null)
            {
                return RedirectToAction("LoginPage", "PatientSide");
            }
            UserDataViewModel jwtData = _JwtAuth.AccessData(token);
            if (jwtData.UserId == null)
            {
                return RedirectToAction("LoginPage", "PatientSide");
            }
            ActiveRequestCombine Activequery = await _providerRepo.ActiveRequests(search, regid, reqType, PageNumber, int.Parse(jwtData.UserId));
            return PartialView("/Views/ProviderAccount/ProviderDashBoard/_ActiveTablePartial.cshtml", Activequery);
        }
        [RoleAuth((int)ProviderRoles.Dashboard)]
        public async Task<IActionResult> ConcludeRequests(string? search, int? regid, int? reqType, int? PageNumber)
        {
            var token = Request.Cookies["Jwt"];
            if (token == null)
            {
                return RedirectToAction("LoginPage", "PatientSide");
            }
            UserDataViewModel jwtData = _JwtAuth.AccessData(token);
            if (jwtData.UserId == null)
            {
                return RedirectToAction("LoginPage", "PatientSide");
            }
            ConcludeRequestCombine Concludequery = await _providerRepo.ConcludeRequests(search, regid, reqType, PageNumber, int.Parse(jwtData.UserId));
            return PartialView("/Views/ProviderAccount/ProviderDashBoard/_ConcludeTablePartialView.cshtml", Concludequery);
        }

        public async Task<IActionResult> LoadRegion()
        {
            var region = await _IAdminPartialsRepo.loadRegion();
            return Json(new { region = region });
        }


        [RoleAuth((int)ProviderRoles.Dashboard)]
        public async Task<IActionResult> ViewNewCases(int requestid)
        {
            if (await _IAdminPartialsRepo.IsDeleted(requestid))
            {
                return RedirectToAction("Index", "Home");
            }
            ViewCasesViewModel patientDetails = await _IAdminPartialsRepo.ViewNewCases(requestid);
            if (patientDetails == null)
            {
                return RedirectToAction("ProviderDashBoardMain");
            }
            HttpContext.Session.SetString("reqid", requestid.ToString());
            return PartialView("/Views/ProviderAccount/ProviderDashBoard/_ViewNewCasesPartialView.cshtml", patientDetails);
        }

        [HttpPost]
        public async Task<IActionResult> ViewNewCasespost(ViewCasesViewModel obj)
        {
            var reqid = HttpContext.Session.GetString("reqid");
            if (reqid == null)
            {
                return RedirectToAction("ProviderDashBoardMain");
            }
            bool status = await _IAdminPartialsRepo.ViewNewCasespost(obj, int.Parse(reqid));
            return Json(new { status = status, reqid = reqid });
        }

        [RoleAuth((int)ProviderRoles.Dashboard)]
        public async Task<IActionResult> ShowViewNotes(int requestid)
        {
            if (await _IAdminPartialsRepo.IsDeleted(requestid))
            {
                return RedirectToAction("Index", "Home");
            }
            ViewNotesViewModel model = await _IAdminPartialsRepo.ShowViewNotes(requestid);
            if (model == null)
            {
                return RedirectToAction("ProviderDashBoardMain");
            }
            HttpContext.Session.SetString("requestid", requestid.ToString());
            return PartialView("/Views/ProviderAccount/ProviderDashBoard/_ViewNotesDashBoardPartial.cshtml", model);
        }
        [HttpPost]
        public async Task<IActionResult> AddProviderNotes(string PhysicianNotes)
        {
            //update request
            int requestid = int.Parse(HttpContext.Session.GetString("requestid"));
            if (requestid.Equals(null))
            {
                return RedirectToAction("LoginPage", "PatientSide");
            }
            var token = Request.Cookies["Jwt"];
            if (token == null)
            {
                return RedirectToAction("LoginPage", "PatientSide");
            }
            UserDataViewModel jwtData = _JwtAuth.AccessData(token);
            if (jwtData.UserId == null)
            {
                return RedirectToAction("LoginPage", "PatientSide");
            }

            await _providerRepo.AddPhysicianNotes(PhysicianNotes, requestid, int.Parse(jwtData.UserId));
            return Json(new { requestid = requestid });
        }
        [RoleAuth((int)ProviderRoles.Dashboard)]
        public async Task<IActionResult> AcceptCaseByProvider(int RequestId)
        {
            bool status = await _providerRepo.AcceptCaseByProvider(RequestId);
            return Json(status);
        }

        [RoleAuth((int)ProviderRoles.Dashboard)]
        public async Task<IActionResult> SendAgreement(int reqid, string Email)
        {
            if (await _IAdminPartialsRepo.UrgentEmailSent(reqid))
            {
                return Json(new { responseText = "Already Sent!" });
            }

            var link = $"https://localhost:44313" + Url.Action("PatientAgreement", "Agreement", new { reqid = await Encode(reqid.ToString()) });
            var from = "hallodocpms@gmail.com";
            var to = Email;
            var subject = "Agreement for services";
            var body = $"Hi,<br /><br />Please click on the following link to Register on HelloDoc:<br /><br/><a href={link}>{link}<a>";
            //send email and pop up in the patient account

            var token = Request.Cookies["Jwt"];
            if (token == null)
            {
                return RedirectToAction("LoginPage", "PatientSide");
            }
            UserDataViewModel jwtData = _JwtAuth.AccessData(token);
            if (jwtData.UserId == null)
            {
                return RedirectToAction("LoginPage", "PatientSide");
            }
            bool send = await _IAdminPartialsRepo.sendEmail(from, to, subject, body, null, reqid, null, null);
            bool status = false;
            if (send)
            {
                status = await _IAdminPartialsRepo.MarkUrgentEmailSent(reqid);
            }
            return Json(send);
        }
        public async Task<string> Encode(string encodeMe)
        {
            byte[] encoded = System.Text.Encoding.UTF8.GetBytes(encodeMe);
            return await Task.Run(() => Convert.ToBase64String(encoded));
        }

        [RoleAuth((int)ProviderRoles.Dashboard)]
        public async Task<IActionResult> View_Document(int id)
        {
            if (await _IAdminPartialsRepo.IsDeleted(id))
            {
                return RedirectToAction("Index", "Home");
            }
            IEnumerable<PatientRequestWiseDocument> RequestWiseDoc = await _IAdminPartialsRepo.View_Document(id);

            var clientData = await _IAdminPartialsRepo.ClientData(id);

            if (clientData == null)
            {
                return RedirectToAction("ProviderDashBoardMain");
            }
            ViewBag.requestId = id;
            ViewBag.emailSend = clientData.Email;
            HttpContext.Session.SetString("requestid", id.ToString());
            ViewBag.ClientName = clientData;

            return PartialView("/Views/ProviderAccount/ProviderDashBoard/_View_Uploads.cshtml", RequestWiseDoc);
        }

        [RoleAuth((int)ProviderRoles.Dashboard)]
        public async Task<IActionResult> Upload_View_Document(IFormFile file)
        {
            string reqid;
            if (HttpContext.Session.GetString("requestid") != null)
            {
                reqid = HttpContext.Session.GetString("requestid");
                if (await _IAdminPartialsRepo.IsDeleted(int.Parse(reqid)))
                {
                    return RedirectToAction("Index", "Home");
                }
                await _IPatientDashboardRepo.PatientDocumentRequestWise(file, reqid);
            }
            else
            {
                return RedirectToAction("ProviderDashBoardMain");
            }
            return Json(reqid);
        }
        public async Task<IActionResult> DeleteSeperateFile(string filename)
        {
            //mark is deleted as true and remove the name 
            var reqid = HttpContext.Session.GetString("requestid");
            if (reqid != null)
                if (await _IAdminPartialsRepo.IsDeleted(int.Parse(reqid)))
                {
                    return RedirectToAction("Index", "Home");
                }
            bool deleteStatus = await _IAdminPartialsRepo.DeleteSeperateFile(filename);
            if (!deleteStatus)
            {
                return Json(false);
            }
            return Json(reqid);
        }
        [RoleAuth((int)ProviderRoles.Dashboard)]
        public async Task<IActionResult> SentEmailWithFile(List<string> fileList, string email)
        {
            var reqid = HttpContext.Session.GetString("requestid");
            if (reqid == null)
            {
                return RedirectToAction("ProviderDashBoardMain");
            }
            bool status = await _IAdminPartialsRepo.SentEmailWithFile(fileList, email, reqid);
            return Json(true);
        }
        public async Task<IActionResult> TransferBackAdmin(int reqid, string TransferNotes)
        {
            bool status = await _providerRepo.TransferBackAdmin(reqid, TransferNotes);
            return Json(status);
        }

        [RoleAuth((int)ProviderRoles.SendOrder)]
        public async Task<IActionResult> OrderCheckOutPage(int ReqId)
        {
            if (await _IAdminPartialsRepo.IsDeleted(ReqId))
            {
                return RedirectToAction("Index", "Home");
            }
            IEnumerable<Healthprofessionaltype> allProfession = await _IAdminPartialsRepo.OrderCheckOutPage(ReqId);

            HttpContext.Session.SetString("requestid", ReqId.ToString());
            var model = new OrderActionViewModel()
            {
                Profession = allProfession,
            };
            return await Task.Run(() => PartialView("/Views/ProviderAccount/ProviderDashBoard/ActionOrders.cshtml", model));
        }
        [RoleAuth((int)ProviderRoles.SendOrder)]
        public async Task<IActionResult> GetBusinessByProfession(int ProfessionId)
        {
            IEnumerable<Healthprofessional> BusinessData = await _IAdminPartialsRepo.GetBusinessByProfession(ProfessionId);
            return await Task.Run(() => Json(new { BusinessData = BusinessData }));
        }
        [RoleAuth((int)ProviderRoles.SendOrder)]
        public async Task<IActionResult> GetBusinessData(int VendorId)
        {
            var BusinessData = await _IAdminPartialsRepo.GetBusinessData(VendorId);
            return Json(BusinessData);
        }
        [RoleAuth((int)ProviderRoles.SendOrder)]
        public async Task<IActionResult> OrderAction(OrderActionViewModel obj)
        {
            var reqid = int.Parse(HttpContext.Session.GetString("requestid") ?? string.Empty);
            if (await _IAdminPartialsRepo.IsDeleted(reqid))
            {
                return RedirectToAction("Index", "Home");
            }
            if (reqid.Equals(Empty))
            {
                Console.Write("error");
            }
            var token = Request.Cookies["Jwt"];
            if (token == null)
            {
                return RedirectToAction("LoginPage", "PatientSide");
            }
            UserDataViewModel jwtData = _JwtAuth.AccessData(token);
            bool result = await _IAdminPartialsRepo.OrderAction(obj, reqid, jwtData.FirstName + " " + jwtData.LastName);
            return Json(result);
        }
        [RoleAuth((int)ProviderRoles.Dashboard)]
        public async Task<IActionResult> SaveEncounterPreferences(int reqid, string SelectType)
        {
            var token = Request.Cookies["Jwt"];
            if (token == null)
            {
                return RedirectToAction("LoginPage", "PatientSide");
            }
            UserDataViewModel jwtData = _JwtAuth.AccessData(token);

            if (jwtData.UserId == null)
            {
                return RedirectToAction("LoginPage", "PatientSide");
            }
            bool status = await _providerRepo.SaveEncounterPreferences(reqid, SelectType, int.Parse(jwtData.UserId));
            return Json(status);
        }


        [RoleAuth((int)ProviderRoles.Dashboard)]
        public async Task<IActionResult> EncounterForm(int reqid)
        {
            var token = Request.Cookies["Jwt"];
            if (token == null)
            {
                return RedirectToAction("LoginPage", "PatientSide");
            }
            UserDataViewModel jwtData = _JwtAuth.AccessData(token);
            if (jwtData.UserId == null)
            {
                return RedirectToAction("LoginPage", "PatientSide");
            }
            HttpContext.Session.SetString("reqid", reqid.ToString());
            Encounterformdetail model = await _IAdminPartialsRepo.EncounterForm(reqid, int.Parse(jwtData.UserId));
            return PartialView("/Views/AdminPartials/ActionMenu/EncounterForm.cshtml", model);
        }
        [RoleAuth((int)ProviderRoles.Dashboard)]
        public async Task<IActionResult> EncounterFormSubmit(Encounterformdetail obj)
        {
            var reqid = HttpContext.Session.GetString("reqid");
            if (reqid == null)
            {
                return Json(false);
            }
            //if (!ModelState.IsValid)
            //{
            //    return Json(false);
            //}
            var token = Request.Cookies["Jwt"];
            if (token == null)
            {
                return RedirectToAction("LoginPage", "PatientSide");
            }
            UserDataViewModel jwtData = _JwtAuth.AccessData(token);
            if (jwtData.UserId == null)
            {
                return RedirectToAction("LoginPage", "PatientSide");
            }
            bool status = false;

            status = await _IAdminPartialsRepo.EncounterFormSubmit(obj, reqid, int.Parse(jwtData.UserId));

            return Json(new { status = status, reqid = reqid });
        }
        [RoleAuth((int)ProviderRoles.Dashboard)]
        public async Task<IActionResult> FinalizeEnconuter()
        {
            var reqid = HttpContext.Session.GetString("reqid");
            if (reqid == null)
            {
                return Json(false);
            }
            bool status = await _providerRepo.FinalizeEnconuter(int.Parse(reqid));
            return Json(status);
        }
        [RoleAuth((int)ProviderRoles.Dashboard)]
        public async Task<IActionResult> ConcludeRequest(int reqid)
        {
            var token = Request.Cookies["Jwt"];
            if (token == null)
            {
                return RedirectToAction("LoginPage", "PatientSide");
            }
            UserDataViewModel jwtData = _JwtAuth.AccessData(token);
            if (jwtData.UserId == null)
            {
                return RedirectToAction("LoginPage", "PatientSide");
            }
            bool status = await _providerRepo.SaveEncounterPreferences(reqid, "Consult", int.Parse(jwtData.UserId));
            return Json(status);
        }

        [RoleAuth((int)ProviderRoles.Dashboard)]
        public async Task<IActionResult> ConcludeCareIndex(int RequestId)
        {
            if (await _IAdminPartialsRepo.IsDeleted(RequestId))
            {
                return RedirectToAction("Index", "Home");
            }
            HttpContext.Session.SetString("requestid", RequestId.ToString());
            //documents 
            IEnumerable<PatientRequestWiseDocument> RequestWiseDoc = await _IAdminPartialsRepo.View_Document(RequestId);

            Requestclient? clientData = await _IAdminPartialsRepo.ClientData(RequestId);
            ViewBag.requestId = RequestId;
            HttpContext.Session.SetString("requestid", RequestId.ToString());
            ViewBag.clientName = clientData;
            return PartialView("/Views/ProviderAccount/ProviderDashBoard/_ConcludeCareIndex.cshtml", RequestWiseDoc);
        }
        [RoleAuth((int)ProviderRoles.Dashboard)]
        public async Task<IActionResult> ConcludeCase(string PhysicianNotes)
        {
            var reqid = HttpContext.Session.GetString("requestid");
            if (reqid == null)
            {
                return RedirectToAction("ProviderDashBoardMain");
            }
            bool status = await _providerRepo.ConcludeCase(PhysicianNotes, int.Parse(reqid));
            return Json(status);
        }
        public async Task<IActionResult> MySchedule()
        {
            return await Task.Run(() => PartialView("/Views/ProviderAccount/ProviderMySchedule/_EditScheduleMain.cshtml"));
        }

        public async Task<IActionResult> LoadPhysicianRegion()
        {
            var token = Request.Cookies["Jwt"];
            if (token == null)
            {
                return RedirectToAction("LoginPage", "PatientSide");
            }
            UserDataViewModel jwtData = _JwtAuth.AccessData(token);
            if (jwtData.UserId == null)
            {
                return RedirectToAction("LoginPage", "PatientSide");
            }

            var data =await _providerRepo.GetRegionOfPhysician(int.Parse(jwtData.UserId));
            return Json(data);
        }
        [RoleAuth((int)ProviderRoles.MySchedule)]
        public async Task<IActionResult> MonthWiseSchedule()
        {
            var data = await _scheduleRepo.MonthWiseScheduleStartData();
            ViewBag.startDate = data;
            return PartialView("/Views/Schedule/_MonthWiseSchedule.cshtml");
        }

        [RoleAuth((int)ProviderRoles.MySchedule)]
        public async Task<IActionResult> GetShifts()
        {
            var token = Request.Cookies["Jwt"];
            if (token == null)
            {
                return RedirectToAction("LoginPage", "PatientSide");
            }
            UserDataViewModel jwtData = _JwtAuth.AccessData(token);
            if (jwtData.UserId == null)
            {
                return RedirectToAction("LoginPage", "PatientSide");
            }

            IEnumerable<Shift>? model = await _providerRepo.GetShifts(int.Parse(jwtData.UserId));
            return Json(model);
        }

        //[RoleAuth((int)ProviderRoles.MySchedule)]
        //public async Task<IActionResult> MonthWiseShiftData(int month, int? regid)
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
        //    IEnumerable<MonthShiftViewModel> data = await _scheduleRepo.MonthShiftData(month, regid, int.Parse(jwtData.UserId));
        //    return Json(data);
        //}
        [RoleAuth((int)ProviderRoles.MySchedule)]
        public async Task<IActionResult> ShiftMonthsData(int month, int year, int? regid)
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
            IEnumerable<ReqestedShiftDetailsviewmodel>? model = await _scheduleRepo.ShiftMonthsData(month, year, regid, int.Parse(jwtData.UserId));
            return Json(model);
        }

        [RoleAuth((int)ProviderRoles.MySchedule)]
        public async Task<IActionResult> FetchScheduleData(int ShiftDetailsids)
        {
            var sd = await _scheduleRepo.FetchScheduleData(ShiftDetailsids);
            return Json(sd);
        }

        [RoleAuth((int)ProviderRoles.MySchedule)]
        public async Task<IActionResult> ChangeShiftStatus(int shiftdetailsid)
        {
            bool status = await _scheduleRepo.ChangeShiftStatus(shiftdetailsid);
            return Json(status);
        }
        [RoleAuth((int)ProviderRoles.MySchedule)]
        public async Task<IActionResult> EditShiftDetails(int shiftdetailsid, DateOnly date, TimeOnly start, TimeOnly end)
        {
            bool IsShiftValidv = await _scheduleRepo.ValidateSingleShift(shiftdetailsid, date, start, end);

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

        [RoleAuth((int)ProviderRoles.MySchedule)]
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

            bool ValidateShift = await _scheduleRepo.IsShiftValid(shift, shiftdetail, int.Parse(jwtData.UserId), model, 2, int.Parse(jwtData.UserId));

            if (ValidateShift != true)
            {
                return Json("Not Valid");
            }
            else
            {
                bool status = await _scheduleRepo.CreateSchedule(shift, shiftdetail, int.Parse(jwtData.UserId), model, 2, int.Parse(jwtData.UserId));
                return Json(status);
            }
        }

        [RoleAuth((int)ProviderRoles.MyProfile)]
        public async Task<IActionResult> EditPhysicianProfile()
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
            /////nedd to change here also
            ///
            ProviderProfileViewModel model = await _IAdminPartialsRepo.EditPhysicianProfile(int.Parse(jwtData.UserId));
            return PartialView("/Views/Provider/_ProviderProfileEdit.cshtml", model);
        }
        [RoleAuth((int)ProviderRoles.MyProfile)]
        public async Task<IActionResult> SendProfileEditRequest(string Message)
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
            Admin? admindata = await _providerRepo.getAdminInfo(int.Parse(jwtData.UserId));
            Physician? physicianData = await _providerRepo.getPhysicianInfo(int.Parse(jwtData.UserId));
            if (admindata != null && physicianData != null)
            {
                string from = physicianData.Email;
                string to = admindata.Email;
                string subject = "For Profile Details Edit";
                string body = Message;
                bool status = await _IAdminPartialsRepo.sendEmail(from, to, subject, body, null, null, int.Parse(jwtData.UserId), null);
                return Json(status);
            }
            return Json(false);
        }
        [RoleAuth((int)ProviderRoles.MyProfile)]
        public async Task<IActionResult> showProviderDocuments(int fileType)
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


            string? name = await _IAdminPartialsRepo.showProviderDocuments(int.Parse(jwtData.UserId), fileType);
            return Json(name);
        }
        [RoleAuth((int)ProviderRoles.Dashboard)]
        public async Task<IActionResult> SendLink(string email, string mobilenumber, string firstname, string lastname)
        {
            var link = $"https://localhost:44313" + Url.Action("PatientRequestCreate", "PatientRequests");
            var from = "hallodocpms@gmail.com";
            var to = email;
            var subject = "Reguarding Submit Request on HalloDoc";
            var body = $"Hi {firstname}{lastname}, <br /><br />Please click on the following link to create request on HelloDoc:<br /><br/><a href={link}>{link}<a>";
            var token = Request.Cookies["Jwt"];
            if (token == null)
            {
                return RedirectToAction("LoginPage", "PatientSide");
            }
            UserDataViewModel jwtData = _JwtAuth.AccessData(token);
            if (jwtData.UserId == null)
            {
                return RedirectToAction("LoginPage", "PatientSide");
            }
            bool send = await _IAdminPartialsRepo.sendEmail(from, to, subject, body, int.Parse(jwtData.UserId), null, null, null);
            return Json(true);
        }

        [RoleAuth((int)ProviderRoles.Dashboard)]
        public async Task<IActionResult> ProviderPatientRequest()
        {
            var data = await _IAdminPartialsRepo.loadRegion();
            ViewBag.region = data;
            return PartialView("/Views/AdminPartials/_AdminPatientRequest.cshtml");
        }
        [RoleAuth((int)ProviderRoles.Dashboard)]
        //use for provider request for patient
        public async Task<IActionResult> AdminPatientRequestPost(CreatePatientRequestviewmodel obj)
        {
            var token = Request.Cookies["Jwt"];
            if (token == null)
            {
                return RedirectToAction("LoginPage", "PatientSide");
            }
            UserDataViewModel jwtData = _JwtAuth.AccessData(token);
            if (jwtData.UserId == null)
            {
                return RedirectToAction("LoginPage", "PatientSide");
            }
            bool status = await _ContactRepository.ADDPatientRequestData(obj, jwtData.UserId, 2);
            return Json(status);
        }
        [RoleAuth((int)ProviderRoles.Dashboard)]
        public async Task<IActionResult> DownloadEncounterReport(int requestId)
        {
            Encounterformdetail data = await _IAdminPartialsRepo.EncounterForm(requestId, null);
            return new ViewAsPdf("ExportEncounterPdf", data)
            {
                FileName = "FinalReport.pdf"
            };
        }

        [RoleAuth((int)ProviderRoles.MyProfile)]
        public async Task<IActionResult> ResetProviderPass(string password, string phyId)
        {
            bool status = await _IAdminPartialsRepo.ResetPassword(password, phyId, 2);
            return Json(status);
        }
        #endregion
        #region invoice
        [RoleAuth((int)ProviderRoles.Invoicing)]
        public async Task<IActionResult> LoadInvoiceIndex()
        {
            return await Task.Run(() =>PartialView("/Views/ProviderAccount/Invoicing/_TimeSheetIndex.cshtml") );
        }
        [RoleAuth((int)ProviderRoles.Invoicing)]
        public async Task<IActionResult> FinalizeTimeSheetView(string DateScoped)
        {
            var token = Request.Cookies["Jwt"];
            if (token == null)
            {
                return RedirectToAction("LoginPage", "PatientSide");
            }
            UserDataViewModel jwtData = _JwtAuth.AccessData(token);
            if (jwtData.UserId == null)
            {
                return RedirectToAction("LoginPage", "PatientSide");
            }
            IEnumerable<WeeklyTimeSheetViewModel>? model =await _providerRepo.FinalizeTimeSheetView(int.Parse(jwtData.UserId),DateScoped);
            return await Task.Run(() => PartialView("/Views/ProviderAccount/Invoicing/_WeeklyTimeSheet.cshtml" , model));
        }
        public async Task<IActionResult> SubmitWeeklyForm(WeeklyTimeSheetViewModel obj)
        {
            var token = Request.Cookies["Jwt"];
            if (token == null)
            {
                return RedirectToAction("LoginPage", "PatientSide");
            }
            UserDataViewModel jwtData = _JwtAuth.AccessData(token);
            if (jwtData.UserId == null)
            {
                return RedirectToAction("LoginPage", "PatientSide");
            }
            bool status = await _providerRepo.SubmitWeeklyForm(obj, int.Parse(jwtData.UserId));
            return Json(true);
        }
        #endregion
    }
}


