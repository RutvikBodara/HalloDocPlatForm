using HalloDoc.DAL.DataModels;
using HalloDoc.DAL.ViewModals;
using HalloDoc.DAL.ViewModals.Access;
using HalloDoc.DAL.ViewModals.AdminDashBoardActions;
using HalloDoc.DAL.ViewModals.AdminDashBoardActions.NewRequestAction;
using HalloDoc.DAL.ViewModals.AdminDashBoardViewModels;
using HalloDoc.DAL.ViewModals.Partner;
using HalloDoc.DAL.ViewModals.Provider;
using HalloDoc.DAL.ViewModals.Records;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Asn1.X509;
using System;
namespace HalloDoc.BAL.Interface
{
    public interface IAdminPartialsRepo
    {
        #region Scheduling
        /// <summary>
        /// All other interface of Scheduling are in "IScheduleRepo"
        /// </summary>
        /// <param name="regionId"></param>
        /// <returns></returns>
        #endregion
        #region StatusWiseRequests
        public Task<DataCount> AdminDashBoardMain();
        public Task<NewRequestCombineViewModel> NewRequests(string? search, int? regid, int? reqType, int? PageNumber);
        public Task<PendingRequestCombine> PendingRequests(string? search, int? regid, int? reqType, int? PageNumber);
        public Task<ActiveRequestCombine> ActiveRequests(string? search, int? regid, int? reqType, int? PageNumber);
        public Task<ConcludeRequestCombine> ConcludeRequests(string? search, int? regid, int? reqType, int? PageNumber);
        public Task<ToCloseCombine> CloseRequests(string? search, int? regid, int? reqType, int? PageNumber);
        public Task<UnpaidCombine> UnPaidRequests(string? search, int? regid, int? reqType, int? PageNumber);
        #endregion
        #region ExcelFileRequestWise
        public Task<IEnumerable<NewRequestViewModel>> ExcelNewRequest();
        public Task<IEnumerable<PendingRequestViewModel>> ExcelPendingRequest();
        public Task<IEnumerable<ActiveRequestViewModel>> ExcelActiveRequest();
        public Task<IEnumerable<ConcludeRequestViewModel>> ExcelConcludeRequest();
        public Task<IEnumerable<ToCloseRequestViewModel>> ExcelCloseRequest();
        public Task<IEnumerable<UnPaidRequestViewModel>> ExcelUnPaidRequest();
        #endregion
        #region email   
        public Task<bool> sendEmail(string from, string to, string subject, string body, int? adminid, int? reqid, int? phyid, List<string>? fileList);
        public Task<bool> UrgentEmailSent(int reqid);
        public Task<bool> MarkUrgentEmailSent(int reqid);
        #endregion
        #region DashBoardActions
        public Task<IList<Physician>> GetPhysicianByRegion(int regionId);
        public Task AssignPhysician(int RegionSelect, int PhysicianSelect, string? AdminDescription, int reqid, int adminid, string actionName, int assignedBy);
        public Task CancelFormData(CancelRequestViewModel obj);
        public Task<bool> ClearCase(int reqid);
        public Task BlockRequest(int reqId, string BlockNotes, int adminId);
        public Task<ViewCasesViewModel> ViewNewCases(int requestid);
        public Task<bool> ViewNewCasespost(ViewCasesViewModel obj, int reqid);
        public Task<ViewNotesViewModel> ShowViewNotes(int requestid);
        public Task AddAdminNotes(string adminnots, int requestid, int adminId);
        public Task<bool> DeleteSeperateFile(string seperatefile);
        public Task<IEnumerable<PatientRequestWiseDocument>> View_Document(int id);
        public Task<bool> OrderAction(OrderActionViewModel obj, int reqid, string CreatedBy);
        public Task<Requestclient?> ClientData(int id);
        public Task<Healthprofessional?> GetBusinessData(int vendorid);
        public Task<IEnumerable<Healthprofessionaltype>> OrderCheckOutPage(int ReqId);
        public Task<IEnumerable<Healthprofessional>> GetBusinessByProfession(int ProfessionId);
        public Task<bool> EditUserData(string firstname, string lastname, string DateOfBirth, string callNumber, string Email, string reqid ,string PatientCountryCode);
        public Task<bool> CloseCasePermennt(string reqid);
        public Task<Encounterformdetail?> EncounterForm(int reqid, int? adminid);
        public Task<bool> EncounterFormSubmit(Encounterformdetail obj, string reqid, int adminid);
        public Task<int> sendDtyMessage(int adminid, string message);
        public Task<IEnumerable<Region>> loadRegion();
        public Task<bool> SendSMS(string Phone, int phyid);
        public Task<bool> IsDeleted(int reqId);
        #endregion
        #region ProviderAccount
        public Task<IEnumerable<Physician>?> LoadProviderLocationPartial(int adminid);
        public Task<ProviderInfoViewModel> ProviderPage(int? regionid, int? PageNumber);
        public Task<bool> StopNotify(int phyid, bool check);
        public Task<ProviderProfileViewModel> EditPhysicianProfile(int phyid);
        public Task<bool> ProviderAspFormSubmit(ProviderProfileViewModel obj, int adminid, int phyid, IFormFile? filePhoto, IFormFile? fileSign, string? submitfor);
        public Task<bool> DeleteProvider(int phyid, string adminid);
        public Task<bool> IsProviderDeleted(int phyid);
        public Task<bool> ProviderDocumentsUpload(IFormFile? file1, IFormFile? file2, IFormFile? file3, IFormFile? file4, IFormFile? file5, int phyid, int adminid);
        public Task<string> UploadProviderData(IFormFile? doc);
        public Task<string?> showProviderDocuments(int phyid, int filetype);
        public Task<bool> CreatePhysician(ProviderProfileViewModel obj, CreateProviderForm Filedata, int adminid);
        #endregion
        #region AdminAccount
        public Task<AdminProfileViewModel> AdminProfile(int adminid);
        public Task<bool> AdminAspFormSubmit(AdminProfileViewModel admin, int adminid);
        public Task<IEnumerable<Adminregion>> regionCheckbox(int adminid);
        public Task<bool> CreateAdminPost(AdminProfileViewModel obj);
        /// <summary>
        /// reset password for admin and provider
        /// </summary>
        /// <param name="password"></param>
        /// <param name="Id"></param>
        /// <param name="RequestType"></param>
        /// <returns></returns>
        public Task<bool> ResetPassword(string password, string Id, int RequestType);
        #endregion
        #region Access
        public Task<AcccountAccessViewModel> Accountaccess(int? PageNumber);
        public Task<IEnumerable<Menu>> getAccountRoleMenu(int? accountType);
        public Task<IEnumerable<AccountType>> CreateAccess();
        public Task<bool> CreateAccessRole(List<string> arrayPage, short accounttype, string rolename);
        public Task<bool> DeleteAccess(int roleid);
        public Task<Role?> EditAccessPage(int roleid);
        public Task<IEnumerable<Menu>> getRequestWiseAccess(int roleid);
        public Task<bool> EditAccess(string roleid, List<string> arrayPage, short accounttype, string rolename);
        #endregion
        #region userAccess
        public Task<UserAccessViewModel?> UserAccess(int? roletype, int adminid, int? PageNumber);
        #endregion
        #region Partners
        public Task<IEnumerable<Healthprofessionaltype>?> getProfession();
        public Task<IEnumerable<Healthprofessional>?> PartnerDataOnly(string? search, int? profId);
        public Task<PartnerInformationViewModel> PartnerData(string? search, int? profId, int? PageNumber);
        public Task<Healthprofessional?> VendorData(int vendorid);
        public Task<bool> UpdateBusiness(Healthprofessional hp, int? vendorId, int toggler);
        public Task<bool> DeleteBusiness(int vendorId);
        #endregion
        #region records
        public Task<SearchRecordsViewModel> SearchRecords(SearchFilterModel? SearchData, int? PageNumber);
        public Task<SearchFilterModel?> SearchRecordsIndex();
        public Task<bool> DeleteRecords(int reqid);
        public Task<IEnumerable<PatientRecords>> PatientRecords(SearchPatientRecords? obj);
        public Task<IEnumerable<PatientHistory>?> PatientHistoryIndex(int userid);
        public Task<EmailAndSMSCombineViewmodel> EmailLogRecords(SearchEmailLogsRecords? obj, int type, int? PageNumber);
        public Task<IEnumerable<Role>?> getRoles();
        public Task<BlockRecordsViewmodel> BlockHistoryIndex(SearchFilterModel? obj, int? PageNumber);
        public Task<bool> UnBlockPatient(int reqid, int blockid);
        public Task<bool> SentEmailWithFile(List<string> fileList, string email, string reqid);
        #endregion
    }
}
