using AdminHalloDoc.Controllers.Login;
using CommandLine;
using HalloDoc.BAL.Interface;
using HalloDoc.DAL.Data;
using HalloDoc.DAL.DataModels;
using HalloDoc.DAL.ViewModals;
using HalloDoc.DAL.ViewModals.Access;
using HalloDoc.DAL.ViewModals.AdminDashBoardActions;
using HalloDoc.DAL.ViewModals.AdminDashBoardActions.NewRequestAction;
using HalloDoc.DAL.ViewModals.AdminDashBoardViewModels;
using HalloDoc.DAL.ViewModals.Partner;
using HalloDoc.DAL.ViewModals.Provider;
using HalloDoc.DAL.ViewModals.Records;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Diagnostics.Runtime.DacInterface;
using OfficeOpenXml;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Reflection;
using System.Text;

namespace hellodocsrsmvc.Controllers
{
    //[adminauthentication]
    [AdminAuth("admin")]
    public class AdminPartialsController : Controller
    {
        enum adminRoles
        {
            Regions = 1,
            Scheduling = 2,
            History = 3,
            Accounts = 4,
            AdminDashboard = 6,
            Dashboard = 7,
            MyProfile = 10,
            Role,
            Provider,
            RequestData,
            SendOrder,
            Vendersinfo,
            Profession,
            EmailLogs = 18,
            HaloAdministrators,
            HaloUsers,
            CancelledHistory,
            ProviderLocation = 23,
            HaloEmployee,
            HaloWorkPlace,
            PatientRecords,
            BlockedHistory,
            Invoicing = 29,
            SMSLogs
        }
        //private readonly HalloDocDBContext _db;
        private readonly IAdminPartialsRepo _IAdminPartialsRepo;
        private readonly IPatientDashboardRepo _IPatientDashboardRepo;
        private readonly IContactRepository _ContactRepository;
        private readonly IJwtAuthInterface _JwtAuth;

        public AdminPartialsController(HalloDocDBContext db, IAdminPartialsRepo IAdminPartialsRepo, IPatientDashboardRepo iPatientDashboardRepo, IJwtAuthInterface jwtAuth, IContactRepository contactRepository)
        {
            //_db = db;
            _IAdminPartialsRepo = IAdminPartialsRepo;
            _IPatientDashboardRepo = iPatientDashboardRepo;
            _JwtAuth = jwtAuth;
            _ContactRepository = contactRepository;
        }

        #region StatusWiseRequests

        [RoleAuth((int)adminRoles.Dashboard)]
        [HttpGet]
        public async Task<IActionResult> AdminDashBoardMain()
        {
            try
            {
                DataCount datacount = await _IAdminPartialsRepo.AdminDashBoardMain();
                return PartialView("/Views/AdminPartials/_AdminDashBoard.cshtml", datacount);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        [RoleAuth((int)adminRoles.Dashboard)]
        public async Task<IActionResult> NewRequests(string? search, int? regid, int? reqType, int? PageNumber)
        {
            HttpContext.Session.SetString("CurrentRequest", "1");
            NewRequestCombineViewModel combineModel = await _IAdminPartialsRepo.NewRequests(search, regid, reqType, PageNumber);
            return PartialView("/Views/AdminPartials/_NewTablePartial.cshtml", combineModel);
        }

        [RoleAuth((int)adminRoles.Dashboard)]
        public async Task<IActionResult> PendingRequests(string? search, int? regid, int? reqType, int? PageNumber)
        {
            try
            {
                HttpContext.Session.SetString("CurrentRequest", "2");
                PendingRequestCombine Pendingquery = await _IAdminPartialsRepo.PendingRequests(search, regid, reqType, PageNumber);
                return PartialView("/Views/AdminPartials/_PendingTablePartial.cshtml", Pendingquery);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        [RoleAuth((int)adminRoles.Dashboard)]
        public async Task<IActionResult> ActiveRequests(string? search, int? regid, int? reqType, int? PageNumber)
        {
            try
            {
                HttpContext.Session.SetString("CurrentRequest", "3");
                ActiveRequestCombine Activequery = await _IAdminPartialsRepo.ActiveRequests(search, regid, reqType, PageNumber);
                return PartialView("/Views/AdminPartials/_ActiveTablePartial.cshtml", Activequery);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        [RoleAuth((int)adminRoles.Dashboard)]
        public async Task<IActionResult> ConcludeRequests(string? search, int? regid, int? reqType, int? PageNumber)
        {
            try
            {

                HttpContext.Session.SetString("CurrentRequest", "4");
                ConcludeRequestCombine Concludequery = await _IAdminPartialsRepo.ConcludeRequests(search, regid, reqType, PageNumber);
                return PartialView("/Views/AdminPartials/_ConcludeTablePartialView.cshtml", Concludequery);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        [RoleAuth((int)adminRoles.Dashboard)]
        public async Task<IActionResult> CloseRequests(string? search, int? regid, int? reqType, int? PageNumber)
        {
            try
            {
                HttpContext.Session.SetString("CurrentRequest", "5");
                ToCloseCombine ToClosequery = await _IAdminPartialsRepo.CloseRequests(search, regid, reqType, PageNumber);
                return PartialView("/Views/AdminPartials/_ToCloseTablePartialView.cshtml", ToClosequery);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        [RoleAuth((int)adminRoles.Dashboard)]
        public async Task<IActionResult> UnPaidRequests(string? search, int? regid, int? reqType, int? PageNumber)
        {
            try
            {
                HttpContext.Session.SetString("CurrentRequest", "6");
                UnpaidCombine Unpaidquery = await _IAdminPartialsRepo.UnPaidRequests(search, regid, reqType, PageNumber);
                return PartialView("/Views/AdminPartials/_UnPaidTablePartialView.cshtml", Unpaidquery);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }
        #endregion

        #region DashBoardActions

        public async Task<IActionResult> GetPhysicianByRegion(int RegionId)
        {
            try
            {
                IList<Physician> physicianName = await _IAdminPartialsRepo.GetPhysicianByRegion(RegionId);

                Console.Write(physicianName);
                return Json(new { physicianName = physicianName });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
            //return List of the physicians 
        }

        [RoleAuth((int)adminRoles.ProviderLocation)]
        public async Task<IActionResult> LoadProviderLocationPartial()
        {
            try
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
                IEnumerable<Physician>? phy = await _IAdminPartialsRepo.LoadProviderLocationPartial(int.Parse(jwtData.UserId));
                return await Task.Run(() => PartialView("/Views/AdminPartials/_LoadProviderLocationPartial.cshtml", phy));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        [RoleAuth((int)adminRoles.Dashboard)]
        public async Task<IActionResult> ViewNewCases(int requestid)
        {
            try
            {
                if (await _IAdminPartialsRepo.IsDeleted(requestid))
                {
                    return RedirectToAction("Index", "Home");
                }
                ViewCasesViewModel patientDetails = await _IAdminPartialsRepo.ViewNewCases(requestid);
                if (patientDetails == null)
                {
                    return RedirectToAction("AdminDashBoardMain");
                }
                HttpContext.Session.SetString("reqid", requestid.ToString());
                return PartialView("/Views/AdminPartials/_ViewNewCasesPartialView.cshtml", patientDetails);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> ViewNewCasespost(ViewCasesViewModel obj)
        {
            try
            {
                var reqid = HttpContext.Session.GetString("reqid");
                if (reqid == null)
                {
                    return RedirectToAction("AdminDashBoardMain");
                }
                bool status = await _IAdminPartialsRepo.ViewNewCasespost(obj, int.Parse(reqid));
                return Json(new { status = status, reqid = reqid });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        [RoleAuth((int)adminRoles.Dashboard)]
        public async Task<IActionResult> ShowViewNotes(int requestid)
        {
            try
            {
                if (await _IAdminPartialsRepo.IsDeleted(requestid))
                {
                    return RedirectToAction("Index", "Home");
                }

                ViewNotesViewModel model = await _IAdminPartialsRepo.ShowViewNotes(requestid);
                if (model == null)
                {
                    return RedirectToAction("AdminDashBoardMain");
                }
                HttpContext.Session.SetString("requestid", requestid.ToString());
                return PartialView("/Views/AdminPartials/_ViewNotesDashBoardPartial.cshtml", model);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddAdminNotes(string adminnots)
        {
            try
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
                var adminId = _ContactRepository.Admin(jwtData.UserId);
                //var adminId = _db.Admins.SingleOrDefault(x => x.Aspnetuserid == jwtData.UserId);
                if (adminId == null)
                {
                    return RedirectToAction("LoginPage", "PatientSide");
                }

                await _IAdminPartialsRepo.AddAdminNotes(adminnots, requestid, adminId.Adminid);
                return Json(new { requestid = requestid });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> CancelFormData(string AdditionalNotes, string reason, int reqid)
        {
            try
            {
                if (await _IAdminPartialsRepo.IsDeleted(reqid))
                {
                    return RedirectToAction("Index", "Home");
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
                var adminId = _ContactRepository.Admin(jwtData.UserId);
                //var adminId = _db.Admins.SingleOrDefault(x => x.Aspnetuserid == jwtData.UserId);

                if (adminId == null)
                {
                    return RedirectToAction("LoginPage", "PatientSide");
                }

                var model = new CancelRequestViewModel()
                {
                    AdditionalNotes = AdditionalNotes,
                    CancelReason = reason,
                    Requestid = reqid,
                    AdminId = adminId.Adminid
                };

                await _IAdminPartialsRepo.CancelFormData(model);
                bool status = true;
                return Json(new { status = status });

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        [HttpPost]
        [RoleAuth((int)adminRoles.Dashboard)]
        public async Task<IActionResult> SendAgreement(int reqid, string Email)
        {
            try
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        [RoleAuth((int)adminRoles.Dashboard)]
        public async Task<IActionResult> ClearCase(int reqid)
        {
            try
            {
                if (await _IAdminPartialsRepo.IsDeleted(reqid))
                {
                    return RedirectToAction("Index", "Home");
                }
                bool status = await _IAdminPartialsRepo.ClearCase(reqid);
                return Json(status);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        [RoleAuth((int)adminRoles.Dashboard)]
        public async Task<IActionResult> AssignPhysician(int RegionSelect, int PhysicianSelect, string AdminDescription, int reqid, string actionName)
        {
            try
            {
                if (await _IAdminPartialsRepo.IsDeleted(reqid))
                {
                    return RedirectToAction("Index", "Home");
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
                var adminId = _ContactRepository.Admin(jwtData.UserId);
                //var adminId = _db.Admins.SingleOrDefault(x => x.Aspnetuserid == jwtData.UserId);
                if (adminId == null)
                {
                    return RedirectToAction("LoginPage", "PatientSide");
                }
                //convert into bool
                await _IAdminPartialsRepo.AssignPhysician(RegionSelect, PhysicianSelect, AdminDescription, reqid, adminId.Adminid, actionName, 1);
                return Json("success");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        [RoleAuth((int)adminRoles.Dashboard)]
        public async Task<IActionResult> BlockRequest(int reqid, string BlockNotes)
        {
            try
            {
                if (await _IAdminPartialsRepo.IsDeleted(reqid))
                {
                    return RedirectToAction("Index", "Home");
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
                var adminId = _ContactRepository.Admin(jwtData.UserId);

                if (adminId == null)
                {
                    return RedirectToAction("LoginPage", "PatientSide");
                }

                await _IAdminPartialsRepo.BlockRequest(reqid, BlockNotes, adminId.Adminid);
                return Json("success");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        [RoleAuth((int)adminRoles.Dashboard)]
        public async Task<IActionResult> View_Document(int id)
        {
            try
            {
                if (await _IAdminPartialsRepo.IsDeleted(id))
                {
                    return RedirectToAction("Index", "Home");
                }
                IEnumerable<PatientRequestWiseDocument> RequestWiseDoc = await _IAdminPartialsRepo.View_Document(id);

                Requestclient? clientData = await _ContactRepository.RequestclientsData(id);
                //var clientData =await _db.Requestclients.SingleOrDefaultAsync(x=>x.Requestid == id);
                if (clientData == null)
                {
                    return RedirectToAction("AdminDashBoardMain");
                }
                ViewBag.requestId = id;
                ViewBag.emailSend = clientData.Email;
                HttpContext.Session.SetString("requestid", id.ToString());
                ViewBag.ClientName = clientData;

                return PartialView("/Views/AdminPartials/_View_Uploads.cshtml", RequestWiseDoc);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        [RoleAuth((int)adminRoles.Dashboard)]
        public async Task<IActionResult> Upload_View_Document(IFormFile file)
        {
            try
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
                    return RedirectToAction("AdminDashBoardMain");
                }
                return Json(reqid);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        public async Task<IActionResult> DeleteSeperateFile(string filename)
        {
            try
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        [RoleAuth((int)adminRoles.SendOrder)]
        public async Task<IActionResult> OrderCheckOutPage(int ReqId)
        {
            try
            {
                if (await _IAdminPartialsRepo.IsDeleted(ReqId))
                {
                    return RedirectToAction("Index", "Home");
                }

                IEnumerable<Healthprofessionaltype>? allProfession = await _IAdminPartialsRepo.getProfession();

                //IEnumerable<Healthprofessionaltype> allProfession = _db.Healthprofessionaltypes.Select(s => new Healthprofessionaltype
                //{
                //    Healthprofessionalid = s.Healthprofessionalid,
                //    Professionname = s.Professionname,
                //}) ;
                HttpContext.Session.SetString("requestid", ReqId.ToString());
                var model = new OrderActionViewModel()
                {
                    Profession = allProfession,
                };
                return await Task.Run(() => PartialView("/Views/AdminPartials/ActionMenu/ActionOrders.cshtml", model));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }
        [RoleAuth((int)adminRoles.SendOrder)]
        public async Task<IActionResult> GetBusinessByProfession(int ProfessionId)
        {
            try
            {
                IEnumerable<Healthprofessional>? BusinessData = await _IAdminPartialsRepo.PartnerDataOnly(null, ProfessionId);
                //return list first to append
                //IEnumerable<Healthprofessional> BusinessData = _db.Healthprofessionals.Where(x=>x.Profession ==  ProfessionId).Select(x=> new Healthprofessional
                //{
                //    Vendorid = x.Vendorid,
                //    Vendorname = x.Vendorname,
                //}); 
                return await Task.Run(() => Json(new { BusinessData = BusinessData }));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        [RoleAuth((int)adminRoles.SendOrder)]
        public async Task<IActionResult> GetBusinessData(int VendorId)
        {
            try
            {
                var BusinessData = await _IAdminPartialsRepo.GetBusinessData(VendorId);
                //var BusinessData = _db.Healthprofessionals.SingleOrDefault(x =>x.Vendorid ==VendorId);
                return Json(BusinessData);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        [RoleAuth((int)adminRoles.SendOrder)]
        public async Task<IActionResult> OrderAction(OrderActionViewModel obj)
        {
            try
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

                if (jwtData.UserId == null)
                {
                    return RedirectToAction("LoginPage", "PatientSide");
                }
                var adminId = _ContactRepository.Admin(jwtData.UserId);
                //var adminId = _db.Admins.SingleOrDefault(x => x.Aspnetuserid ==jwtData.UserId);

                if (adminId == null)
                {
                    return RedirectToAction("LoginPage", "AdminSite");
                }
                bool result = await _IAdminPartialsRepo.OrderAction(obj, reqid, adminId.Firstname + " " + adminId.Lastname);
                return Json(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        public async Task<IActionResult> LoadRegion()
        {
            try
            {
                IEnumerable<Region> region = await _IAdminPartialsRepo.loadRegion();
                return Json(new { region = region });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        [RoleAuth((int)adminRoles.Dashboard)]
        public async Task<IActionResult> CloseCase(int reqid)
        {
            try
            {
                if (await _IAdminPartialsRepo.IsDeleted(reqid))
                {
                    return RedirectToAction("Index", "Home");
                }
                HttpContext.Session.SetString("requestid", reqid.ToString());
                //documents 
                IEnumerable<PatientRequestWiseDocument> RequestWiseDoc = await _IAdminPartialsRepo.View_Document(reqid);
                var clientData = await _ContactRepository.RequestclientsData(reqid);
                ViewBag.requestId = reqid;
                HttpContext.Session.SetString("requestid", reqid.ToString());
                ViewBag.clientName = clientData;
                //profile
                return PartialView("/Views/AdminPartials/ActionMenu/_CloseCase.cshtml", RequestWiseDoc);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        [RoleAuth((int)adminRoles.Dashboard)]
        public async Task<IActionResult> EditUserData(string firstname, string lastname, string DateOfBirth, string callNumber, string Email, string PatientCountryCode)
        {
            try
            {
                var reqid = HttpContext.Session.GetString("requestid");
                if (reqid == null)
                {
                    //define
                    return RedirectToAction("");
                }
                bool status = await _IAdminPartialsRepo.EditUserData(firstname, lastname, DateOfBirth, callNumber, Email, reqid, PatientCountryCode);

                return Json(reqid);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        [RoleAuth((int)adminRoles.Dashboard)]
        public async Task<IActionResult> CloseCasePermennt()
        {
            try
            {
                var reqid = HttpContext.Session.GetString("requestid");
                if (reqid == null)
                {
                    //define
                    return RedirectToAction("");
                }
                bool status = await _IAdminPartialsRepo.CloseCasePermennt(reqid);
                return Json(status);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        public async Task<IActionResult> LoadStatus()
        {
            try
            {
                return await Task.Run(async () => Json(await _ContactRepository.loadStatus()));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        public async Task<IActionResult> LoadRole()
        {
            try
            {
                IEnumerable<Role>? role = await _IAdminPartialsRepo.getRoles();
                return await Task.Run(() => Json(role));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        [RoleAuth((int)adminRoles.Dashboard)]
        public async Task<IActionResult> EncounterForm(int reqid)
        {
            try
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }
        public async Task<IActionResult> EncounterFormSubmit(Encounterformdetail obj)
        {
            try
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
                var reqid = HttpContext.Session.GetString("reqid");
                bool status = false;

                status = await _IAdminPartialsRepo.EncounterFormSubmit(obj, reqid, int.Parse(jwtData.UserId));

                return Json(new { status = status, reqid = reqid });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        [RoleAuth((int)adminRoles.Dashboard)]
        public async Task<IActionResult> AdminPatientRequest()
        {
            try
            {
                var data = await _IAdminPartialsRepo.loadRegion();
                ViewBag.region = data;
                return PartialView("_AdminPatientRequest");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        //[RoleAuth((int)adminRoles.PatientRecords)]
        public async Task<IActionResult> AdminPatientRequestPost(CreatePatientRequestviewmodel obj)
        {
            try
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
                bool status = await _ContactRepository.ADDPatientRequestData(obj, jwtData.UserId, 1);

                return Json(status);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }
        #endregion

        #region ExcelFileRequestWise

        [RoleAuth((int)adminRoles.RequestData)]
        public async Task<ActionResult> ExportAllData()
        {
            try
            {
                var RequestType = HttpContext.Session.GetString("CurrentRequest");
                if (RequestType == null)
                {
                    return RedirectToAction("AdminDashBoardMain");
                }
                int TypeOfRequest = int.Parse(RequestType);
                byte[] excelData = null;
                switch (TypeOfRequest)
                {
                    case 1:
                        IEnumerable<NewRequestViewModel> model = await _IAdminPartialsRepo.ExcelNewRequest();
                        excelData = ExportToExcel(model.ToList());
                        break;
                    case 2:
                        IEnumerable<PendingRequestViewModel> model2 = await _IAdminPartialsRepo.ExcelPendingRequest();
                        excelData = ExportToExcel(model2.ToList());
                        break;
                    case 3:
                        IEnumerable<ActiveRequestViewModel> model3 = await _IAdminPartialsRepo.ExcelActiveRequest();
                        excelData = ExportToExcel(model3.ToList());
                        break;
                    case 4:
                        IEnumerable<ConcludeRequestViewModel> model4 = await _IAdminPartialsRepo.ExcelConcludeRequest();
                        excelData = ExportToExcel(model4.ToList());
                        break;
                    case 5:
                        IEnumerable<ToCloseRequestViewModel> model5 = await _IAdminPartialsRepo.ExcelCloseRequest();
                        excelData = ExportToExcel(model5.ToList());
                        break;
                    case 6:
                        IEnumerable<UnPaidRequestViewModel> model6 = await _IAdminPartialsRepo.ExcelUnPaidRequest();
                        excelData = ExportToExcel(model6.ToList());
                        break;
                }

                return File(excelData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Requests.xlsx");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }


        }

        [RoleAuth((int)adminRoles.RequestData)]
        public byte[] ExportToExcel<T>(List<T> data)
        {
            using (var excelPackage = new ExcelPackage())
            {
                var worksheet = excelPackage.Workbook.Worksheets.Add("Requests");
                PropertyInfo[] headerNames = typeof(T).GetProperties();
                for (int i = 0; i < headerNames.Length; i++)
                {
                    worksheet.Cells[1, i + 1].Value = headerNames[i].Name;
                }

                // Add data to the worksheet
                for (int i = 0; i < data.Count; i++)
                {
                    var item = data[i];
                    for (int j = 0; j < headerNames.Length; j++)
                    {
                        //not clear
                        object value = headerNames[j].GetValue(item);
                        worksheet.Cells[i + 2, j + 1].Value = value;
                    }
                }
                return excelPackage.GetAsByteArray();
            }
        }

        //[RoleAuth((int)adminRoles.RequestData)]
        public async Task<IActionResult> SentEmailWithFile(List<string> fileList, string email)
        {
            try
            {
                var reqid = HttpContext.Session.GetString("requestid");

                bool status = await _IAdminPartialsRepo.SentEmailWithFile(fileList, email, reqid);
                return Json(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        [RoleAuth((int)adminRoles.Dashboard)]
        public async Task<IActionResult> DTYRequest(string message)
        {
            try
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
                int status = await _IAdminPartialsRepo.sendDtyMessage(int.Parse(jwtData.UserId), message);
                return Json(status);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }
        #endregion

        #region email   

        //[RoleAuth((int)adminRoles.RequestData)]
        public async Task<IActionResult> SendLink(string email, string mobilenumber, string firstname, string lastname)
        {
            try
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
                return Json(send);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        #endregion

        #region AdminAccount

        [RoleAuth((int)adminRoles.MyProfile)]
        public async Task<IActionResult> AdminProfile(int? curadminid)
        {
            try
            {
                var token = Request.Cookies["Jwt"];
                if (token == null)
                {
                    return RedirectToAction("LoginPage", "PatientSide");
                }
                UserDataViewModel jwtData = _JwtAuth.AccessData(token);
                var adminid = jwtData.UserId;
                if (curadminid != null)
                {
                    //HttpContext.Session.SetString("AdminID", curadminid.ToString());
                    adminid = curadminid.ToString();
                }
                else
                {
                    var adminData = _ContactRepository.Admin(adminid);
                    adminid = adminData.Adminid.ToString();
                    ViewBag.CurrentProfile = "Self";
                }
                if (adminid == null)
                {
                    return RedirectToAction("LoginPage", "PatientSide");
                }
                HttpContext.Session.SetString("CurrentAdmin", adminid);
                AdminProfileViewModel model = await _IAdminPartialsRepo.AdminProfile(int.Parse(adminid));

                return PartialView("/Views/AdminPartials/AdminProfile/_AdminProfile.cshtml", model);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        public async Task<IActionResult> AdminAspFormSubmit(AdminProfileViewModel obj, int? custadminid)
        {
            try
            {
                //var token = Request.Cookies["Jwt"];
                //if (token == null)
                //{
                //    return RedirectToAction("LoginPage", "PatientSide");
                //}
                //UserDataViewModel jwtData = _JwtAuth.AccessData(token);
                //if (jwtData.UserId == null)
                //{
                //    return RedirectToAction("LoginPage", "PatientSide");
                //}
                string AdminId = HttpContext.Session.GetString("CurrentAdmin");
                int adminid = int.Parse(AdminId);

                //var admin = HttpContext.Session.GetString("AdminID");
                //if (admin != "" && admin != null) { 
                //    adminid = int.Parse(admin);
                //    HttpContext.Session.SetString("AdminID", "");
                //};
                bool status = false;
                status = await _IAdminPartialsRepo.AdminAspFormSubmit(obj, adminid);
                return Json(status);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        public async Task<IActionResult> regionCheckbox()
        {
            try
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
                IEnumerable<Adminregion> region = await _IAdminPartialsRepo.regionCheckbox(int.Parse(jwtData.UserId));
                return Json(region);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        #endregion

        /// <summary>
        /// encode string for email and password
        /// </summary>
        /// <param name="encodeMe"></param>
        /// <returns></returns>
        #region encode
        public async Task<string> Encode(string encodeMe)
        {
            byte[] encoded = System.Text.Encoding.UTF8.GetBytes(encodeMe);
            return await Task.Run(() => Convert.ToBase64String(encoded));
        }
        #endregion

        #region ProviderAccount

        [RoleAuth((int)adminRoles.Provider)]
        public async Task<IActionResult> ProviderPage(int? regionid, int? PageNumber)
        {
            try
            {
                ProviderInfoViewModel Physiciandata = await _IAdminPartialsRepo.ProviderPage(regionid, PageNumber);
                return PartialView("/Views/Provider/_ProviderInformation.cshtml", Physiciandata);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        [RoleAuth((int)adminRoles.Provider)]
        public async Task<IActionResult> ContactProvider(string options, string bodyData, string email, string phone, int phyid, string name)
        {
            try
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
                var adminId = _ContactRepository.Admin(jwtData.UserId);
                if (adminId == null)
                {
                    return RedirectToAction("LoginPage", "PatientSide");
                }
                bool status = false;
                if (options == "Email" || options == "Both")
                {
                    var from = "hallodocpms@gmail.com";
                    var to = email;
                    var subject = $"Notification from {jwtData.Username}";
                    var body = $"Hi {name}, <br /><br />{bodyData}";

                    bool send = await _IAdminPartialsRepo.sendEmail(from, to, subject, body, null, null, phyid, null);
                    if (!send)
                    {
                        return Json(send);
                    }
                }
                if (options == "Sms" || options == "Both")
                {
                    //logs entry files
                    status = await _IAdminPartialsRepo.SendSMS(phone, phyid);
                }
                return Json(status);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        [RoleAuth((int)adminRoles.Provider)]
        public async Task<IActionResult> StopNotify(int phyid, bool check)
        {
            try
            {
                bool status = await _IAdminPartialsRepo.StopNotify(phyid, check);
                return Json(status);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        [RoleAuth((int)adminRoles.Provider)]
        public async Task<IActionResult> EditPhysicianProfile(int phyid)
        {
            try
            {
                if (await _IAdminPartialsRepo.IsProviderDeleted(phyid))
                {
                    return RedirectToAction("Index", "Home");
                }
                HttpContext.Session.SetString("phyid", phyid.ToString());
                ProviderProfileViewModel model = await _IAdminPartialsRepo.EditPhysicianProfile(phyid);
                return PartialView("/Views/Provider/_ProviderProfileEdit.cshtml", model);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        [RoleAuth((int)adminRoles.Provider)]
        public async Task<IActionResult> ProviderAspFormSubmit(ProviderProfileViewModel FormData, IFormFile? fileSign, IFormFile? filePhoto, string? submitfor)
        {
            try
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
                var phyid = HttpContext.Session.GetString("phyid");
                if (phyid == null)
                {
                    return RedirectToAction("AdminDashBoardMain");
                }
                bool status = await _IAdminPartialsRepo.ProviderAspFormSubmit(FormData, int.Parse(jwtData.UserId), int.Parse(phyid), filePhoto, fileSign, submitfor);
                return Json(status);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        [RoleAuth((int)adminRoles.Provider)]
        public async Task<IActionResult> DeleteProvider(int phyid)
        {
            try
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
                bool status = await _IAdminPartialsRepo.DeleteProvider(phyid, jwtData.UserId);
                return Json(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        [RoleAuth((int)adminRoles.Provider)]
        public async Task<IActionResult> ProviderDocumentsUpload(IFormFile? file1, IFormFile? file2, IFormFile? file3, IFormFile? file4, IFormFile? file5)
        {
            try
            {
                var phyid = HttpContext.Session.GetString("phyid");
                if (phyid == null)
                {
                    return RedirectToAction("AdminDashBoardMain");
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
                bool status = await _IAdminPartialsRepo.ProviderDocumentsUpload(file1, file2, file3, file4, file5, int.Parse(phyid), int.Parse(jwtData.UserId));
                return Json(new { status = status, phyid = int.Parse(phyid) });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        [RoleAuth((int)adminRoles.Provider)]
        public async Task<IActionResult> showProviderDocuments(int phyid, int fileType)
        {
            try
            {
                string? name = await _IAdminPartialsRepo.showProviderDocuments(phyid, fileType);
                return Json(name);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        [RoleAuth((int)adminRoles.Provider)]
        public async Task<IActionResult> ResetProviderPass(string password, string phyId)
        {
            try
            {
                bool status = await _IAdminPartialsRepo.ResetPassword(password, phyId, 2);
                return Json(status);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }
        #endregion

        #region Access
        [RoleAuth((int)adminRoles.Role)]
        public async Task<IActionResult> Accountaccess(int? PageNumber)
        {
            try
            {
                AcccountAccessViewModel model = await _IAdminPartialsRepo.Accountaccess(PageNumber);
                return PartialView("/Views/Access/_AccountAccess.cshtml", model);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }
        [RoleAuth((int)adminRoles.Role)]
        public async Task<IActionResult> AccountaccessFetch()
        {
            try
            {
                AcccountAccessViewModel model = await _IAdminPartialsRepo.Accountaccess(null);
                return Json(model);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }
        [RoleAuth((int)adminRoles.Role)]
        public async Task<IActionResult> CreateAccess()
        {
            try
            {
                IEnumerable<AccountType> model = await _IAdminPartialsRepo.CreateAccess();
                return PartialView("/Views/Access/_CreateAccess.cshtml", model);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }
        [RoleAuth((int)adminRoles.Role)]
        public async Task<IActionResult> getAccountRoleMenu(int? accountType)
        {
            try
            {
                IEnumerable<Menu> model = await _IAdminPartialsRepo.getAccountRoleMenu(accountType);
                return Json(model);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }
        [RoleAuth((int)adminRoles.Role)]
        public async Task<IActionResult> CreateAccessRole(List<string> arrayPage, short accounttype, string rolename)
        {
            try
            {
                //IFormCollection formdata,
                //string rolename = formdata["Rolename"];
                //string accounttype = formdata["accounttype"];
                //List<string> array = JsonConvert.DeserializeObject<List<string>>(formdata["arrayPage"]);
                bool status = await _IAdminPartialsRepo.CreateAccessRole(arrayPage, accounttype, rolename);
                return Json(status);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }
        [RoleAuth((int)adminRoles.Role)]
        public async Task<IActionResult> DeleteAccess(int roleid)
        {
            try
            {
                bool status = await _IAdminPartialsRepo.DeleteAccess(roleid);
                return Json(status);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }
        [RoleAuth((int)adminRoles.Role)]
        public async Task<IActionResult> EditAccess(int roleid)
        {
            try
            {
                //rolename
                //accounttype
                Role? model = await _IAdminPartialsRepo.EditAccessPage(roleid);
                if (model == null)
                {
                    return Json(false);
                }
                IEnumerable<Menu> checkbox = await _IAdminPartialsRepo.getAccountRoleMenu(null);
                IEnumerable<AccountType> accounttype = await _IAdminPartialsRepo.CreateAccess();
                Rolemenucombine combinemodel = new()
                {
                    Role = model,
                    menu = checkbox,
                    accounttype = accounttype
                };
                HttpContext.Session.SetString("roleid", roleid.ToString());
                return PartialView("/Views/Access/_EditAccess.cshtml", combinemodel);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }
        [RoleAuth((int)adminRoles.Role)]
        public async Task<IActionResult> getRequestWiseAccess(int roleid)
        {
            try
            {
                var data = await _ContactRepository.getRole(roleid);
                if (data == null)
                {
                    return Json("error");
                }
                IEnumerable<Menu> menu = await _IAdminPartialsRepo.getRequestWiseAccess(roleid);
                IEnumerable<Menu> menumain = await _IAdminPartialsRepo.getAccountRoleMenu(data.Accounttype);
                return Json(new { menumain = menumain, menu = menu });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }
        //public async Task<IActionResult> EditAccessRolemenu(int roleid)
        //{
        //    Rolemenu model = await _IAdminPartialsRepo.EditAccessRolemenu(roleid);
        //    return Json(model);
        //}
        public async Task<IActionResult> EditAccessSubmit(List<string> arrayPage, short accounttype, string rolename)
        {
            try
            {
                int flag = 3;
                List<string> temporaryList = arrayPage.ToList();
                var roleid = HttpContext.Session.GetString("roleid");
                bool status = await _IAdminPartialsRepo.EditAccess(roleid, arrayPage, accounttype, rolename);
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
                if (status == true)
                {
                    flag = 2;
                }
                if (accounttype == 2 && roleid == jwtData.roleid)
                {
                    flag = 1;
                    string roles = "";
                    foreach (var role in temporaryList)
                    {
                        roles += role + ",";
                    }
                    Response.Cookies.Append("RoleMenu", roles);
                }

                return Json(flag);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }
        #endregion

        #region AdminAccount

        [RoleAuth((int)adminRoles.Accounts)]
        public async Task<IActionResult> CreateAdmin()
        {
            try
            {
                ViewBag.Regions = await _IAdminPartialsRepo.loadRegion();
                return PartialView("/Views/Access/_CreateAdmin.cshtml");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        [RoleAuth((int)adminRoles.Accounts)]
        public async Task<IActionResult> CreateAdminPost(AdminProfileViewModel obj)
        {
            try
            {
                var token = Request.Cookies["Jwt"];
                if (token == null)
                {
                    return RedirectToAction("LoginPage", "PatientSide");
                }
                UserDataViewModel jwtData = _JwtAuth.AccessData(token);
                bool status = await _IAdminPartialsRepo.CreateAdminPost(obj);
                return Json(status);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        [RoleAuth((int)adminRoles.MyProfile)]
        public async Task<IActionResult> ResetAdminPass(string password, int From)
        {
            try
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
                bool status = await _IAdminPartialsRepo.ResetPassword(password, jwtData.UserId, 1);
                return Json(status);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        #endregion

        #region userAccess

        [RoleAuth((int)adminRoles.Accounts)]
        public async Task<IActionResult> CreateProvider()
        {
            try
            {
                ViewBag.Regions = await _IAdminPartialsRepo.loadRegion();
                return PartialView("/Views/Provider/_CreateProvider.cshtml");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        [RoleAuth((int)adminRoles.Accounts)]
        public async Task<IActionResult> CreatePhysician(ProviderProfileViewModel obj, CreateProviderForm Filedata)
        {
            try
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
                bool status = await _IAdminPartialsRepo.CreatePhysician(obj, Filedata, int.Parse(jwtData.UserId));
                return Json(status);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        [RoleAuth((int)adminRoles.Accounts)]
        public async Task<IActionResult> UserAccess(int? roletype, int? PageNumber)
        {
            try
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
                UserAccessViewModel? model = await _IAdminPartialsRepo.UserAccess(roletype, int.Parse(jwtData.UserId), PageNumber);
                return PartialView("/Views/Access/_UserAccess.cshtml", model);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        #endregion

        #region Partners
        [RoleAuth((int)adminRoles.Vendersinfo)]
        public async Task<IActionResult> PartnerIndex(string? search, int? profId)
        {
            try
            {
                //get professions 
                var profession = await _IAdminPartialsRepo.getProfession();
                ViewBag.profession = profession;
                return PartialView("/Views/Partners/VendorIndex.cshtml");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        [RoleAuth((int)adminRoles.Vendersinfo)]
        public async Task<IActionResult> PartnerTablePartial(string? search, int? profId, int? PageNumber)
        {
            try
            {
                PartnerInformationViewModel model = await _IAdminPartialsRepo.PartnerData(search, profId, PageNumber);
                return PartialView("/Views/Partners/PartnerTablePartials.cshtml", model);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        [RoleAuth((int)adminRoles.Vendersinfo)]
        public async Task<IActionResult> EditVendor(int vendorid)
        {
            try
            {
                var profession = await _IAdminPartialsRepo.getProfession();
                ViewBag.profession = profession;
                //use to make difference between edit and create vendor
                if (vendorid != 0)
                {
                    HttpContext.Session.SetString("vendorid", vendorid.ToString());
                    Healthprofessional? vendordata = await _IAdminPartialsRepo.VendorData(vendorid);
                    return PartialView("/Views/Partners/EditPartner.cshtml", vendordata);
                }
                else
                {
                    HttpContext.Session.SetString("vendorid", "-1");
                }
                return PartialView("/Views/Partners/EditPartner.cshtml");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        [RoleAuth((int)adminRoles.Vendersinfo)]
        public async Task<IActionResult> UpdateBusiness(Healthprofessional hp)
        {
            try
            {
                bool status = false;
                //for edit
                int toggler = 1;
                var vendorid = HttpContext.Session.GetString("vendorid");
                if (vendorid == null)
                {
                    return Json(status);
                }
                if (HttpContext.Session.GetString("vendorid") == "-1")
                {
                    //for create 
                    toggler = 2;
                }
                status = await _IAdminPartialsRepo.UpdateBusiness(hp, int.Parse(vendorid), toggler);
                if (!status)
                {
                    return Json(false);
                }
                return Json(toggler);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        [RoleAuth((int)adminRoles.Vendersinfo)]
        public async Task<IActionResult> DeleteBusiness(int vendorid)
        {
            try
            {
                bool status = await _IAdminPartialsRepo.DeleteBusiness(vendorid);
                return Json(status);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        #endregion

        #region Records

        [RoleAuth((int)adminRoles.PatientRecords)]
        public async Task<IActionResult> SearchRecordsIndex()
        {
            try
            {
                SearchFilterModel? model = await _IAdminPartialsRepo.SearchRecordsIndex();
                return await Task.Run(() => PartialView("/Views/Records/SearchRecordsIndex.cshtml", model));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        [RoleAuth((int)adminRoles.PatientRecords)]
        public async Task<IActionResult> SearchRecords(SearchFilterModel? SearchData, int? PageNumber)
        {
            try
            {
                SearchRecordsViewModel model = await _IAdminPartialsRepo.SearchRecords(SearchData, PageNumber);
                return PartialView("/Views/Records/_SearchRecords.cshtml", model);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        [RoleAuth((int)adminRoles.PatientRecords)]
        public async Task<IActionResult> PatientRecordsIndex()
        {
            try
            {
                return await Task.Run(() => PartialView("/Views/Records/PatientRecords/PatientRecordsIndex.cshtml"));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        [RoleAuth((int)adminRoles.PatientRecords)]
        public async Task<IActionResult> DeleteRecords(int reqid)
        {
            try
            {
                //delete
                bool status = await _IAdminPartialsRepo.DeleteRecords(reqid);
                return Json(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        [RoleAuth((int)adminRoles.PatientRecords)]
        public async Task<IActionResult> PatientRecords(SearchPatientRecords? obj)
        {
            try
            {
                IEnumerable<PatientRecords> model = await _IAdminPartialsRepo.PatientRecords(obj);
                return PartialView("/Views/Records/PatientRecords/_PatientRecords.cshtml", model);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        [RoleAuth((int)adminRoles.History)]
        public async Task<IActionResult> PatientHistoryIndex(int userid)
        {
            try
            {
                IEnumerable<PatientHistory>? model = await _IAdminPartialsRepo.PatientHistoryIndex(userid);
                return PartialView("/Views/Records/PatientRecords/PatientHistoryIndex.cshtml", model);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        [RoleAuth((int)adminRoles.EmailLogs)]
        public async Task<IActionResult> EmailLogsIndex(int type)
        {
            try
            {
                var profession = await _IAdminPartialsRepo.getRoles();
                SearchEmailLogsRecords model = new();
                if (type == 1)
                {
                    ViewBag.CurrentLog = "Email Logs";
                }
                else
                {
                    ViewBag.CurrentLog = "SMS Logs";
                    //handle other case when needed
                }
                model.RoleData = profession;
                return await Task.Run(() => PartialView("/Views/Records/Logs/EmailLogsIndex.cshtml", model));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }


        [RoleAuth((int)adminRoles.EmailLogs)]
        public async Task<IActionResult> EmailLogRecords(SearchEmailLogsRecords? obj, int type, int? PageNumber)
        {
            try
            {
                //for sms and email both this method will call
                if (type == 1)
                {
                    ViewBag.Currenttype = 1;
                }
                else
                {
                    ViewBag.Currenttype = 2;
                    //handle other case when needed
                }
                EmailAndSMSCombineViewmodel model = await _IAdminPartialsRepo.EmailLogRecords(obj, type, PageNumber);
                return await Task.Run(() => PartialView("/Views/Records/Logs/EmailLogRecords.cshtml", model));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        [RoleAuth((int)adminRoles.BlockedHistory)]
        public async Task<IActionResult> BlockHistoryIndex()
        {
            try
            {
                //for sms and email both this method will call
                return await Task.Run(() => PartialView("/Views/Records/BlockCases/BlockHistoryIndex.cshtml"));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        [RoleAuth((int)adminRoles.BlockedHistory)]
        public async Task<IActionResult> BlockHistoryRecords(SearchFilterModel? obj, int? PageNumber)
        {
            try
            {
                BlockRecordsViewmodel model = await _IAdminPartialsRepo.BlockHistoryIndex(obj, PageNumber);
                return PartialView("/Views/Records/BlockCases/BlockHistoryRecords.cshtml", model);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        [RoleAuth((int)adminRoles.BlockedHistory)]
        public async Task<IActionResult> UnBlockPatient(int reqid, int blockid)
        {
            try
            {
                bool status = await _IAdminPartialsRepo.UnBlockPatient(reqid, blockid);
                return Json(status);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        #endregion
    }
}
