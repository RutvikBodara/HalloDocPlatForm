using HalloDoc.DAL.DataModels;
using HalloDoc.DAL.ViewModals.AdminDashBoardViewModels;
using HalloDoc.DAL.ViewModals.ProviderAccount;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
//using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.BAL.Interface
{
    /// <summary>
    /// Provider Account Unique Action Will Handle From This Repository (common Method will Directly Access from IAdminPartialsRepo)
    /// </summary>
    public interface IProviderRepo
    {
        #region ProviderRequests
        public Task<DataCount> ProviderDashBoardMain(int PhysicainId);
        public Task<NewRequestCombineViewModel> NewRequests(string? search, int? regid, int? reqType, int? PageNumber, int PhysicainId);
        public Task<PendingRequestCombine> PendingRequests(string? search, int? regid, int? reqType, int? PageNumber,int PhysicainId);
        public Task<ActiveRequestCombine> ActiveRequests(string? search, int? regid, int? reqType, int? PageNumber, int PhysicainId);
        public Task<ConcludeRequestCombine> ConcludeRequests(string? search, int? regid, int? reqType, int? PageNumber, int PhysicainId);
        #endregion
        #region ProviderActions

        public Task<IEnumerable<Region>> GetRegionOfPhysician(int PhysicianId);
        public Task AddPhysicianNotes(string PhysicianNotes, int requestid, int PhysicianId);
        public Task<bool> AcceptCaseByProvider(int RequestId);
        public Task<bool> TransferBackAdmin(int reqid, string TransferNotes);
        public Task<bool> FinalizeEnconuter(int reqid);
        public Task<bool> SaveEncounterPreferences(int reqid, string SelectType, int Phyid);
        public Task<bool> ConcludeCase(string PhysicianNotes, int reqid);
        public Task<bool> UpdateLocation(float latitude,float longtitude, int  physicianId);
        public Task<IEnumerable<Shift>?> GetShifts(int physicianId);
        public Task<Admin?> getAdminInfo(int physicianId);
        public Task<Physician?> getPhysicianInfo(int physicianId);
        #endregion
        #region invoicing
        Task<IEnumerable<WeeklyTimeSheetViewModel>?> FinalizeTimeSheetView(int PhysicianId,string DateScoped);
        Task<bool> SubmitWeeklyForm(WeeklyTimeSheetViewModel obj ,int physicianId);
        #endregion
    }
}
