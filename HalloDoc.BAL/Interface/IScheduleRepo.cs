using HalloDoc.DAL.DataModels;
using HalloDoc.DAL.ViewModals.Schedule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.BAL.Interface
{
    /// <summary>
    /// Scheduling Of Provider for Admin And Provider Site 
    /// </summary>
    public interface IScheduleRepo
    {
        #region Scheduling
        public Task<IEnumerable<Shift>?> GetShifts();
        public Task<bool> IsShiftValid(Shift shift, Shiftdetail shiftdetail, int CreatedBy, ScheduleViewModel model, int toggler, int? phyid);
        public Task<bool> CreateSchedule(Shift shift, Shiftdetail shiftdetail, int CreatedBy, ScheduleViewModel model, int toggler, int? phyid);
        public Task<ScheduleViewModel> DayWiseSchedule(DateOnly date, int? regid, DateOnly? enddate);
        public Task<Shiftdetail?> FetchScheduleData(int ShiftDetailsids);
        public Task<bool> ValidateSingleShift(int shiftdetailsid, DateOnly date, TimeOnly start, TimeOnly end);
        public Task<bool> EditShiftDetails(int shiftdetailsid, DateOnly date, TimeOnly start, TimeOnly end);
        public Task<bool> ChangeShiftStatus(int shiftdetailsid);
        public Task<ProviderOnCallViewModel> ProviderOnCallPartial(int adminid, int? regid);
        public Task<PendingShiftCombine?> PendingShifts(int adminid, int? PageNumber, int? regid);
        public Task<bool> ApproveOrDeleteShift(List<int> idlist, string reqtype);
        public Task<int> MonthWiseScheduleStartData();
        //public Task<IEnumerable<MonthShiftViewModel>> MonthShiftData(int month, int? regid, int adminid);
        public Task<IEnumerable<ReqestedShiftDetailsviewmodel>?> ShiftMonthsData(int month, int year, int? regid, int? PhysicianId);
        #endregion
    }
}
