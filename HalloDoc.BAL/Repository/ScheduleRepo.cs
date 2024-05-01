using HalloDoc.BAL.Interface;
using HalloDoc.DAL.Data;
using HalloDoc.DAL.DataModels;
using HalloDoc.DAL.ViewModals.Schedule;
using Microsoft.EntityFrameworkCore;

namespace HalloDoc.BAL.Repository
{
    /// <summary>
    /// Scheduling Of Provider for Admin And Provider Site 
    /// </summary>
    public class ScheduleRepo : IScheduleRepo
    {
        enum Days
        {
            sunday = 1,
            monday,
            Tuesday,
            Wednesday,
            Thursday,
            friday,
            saturday
        }
        #region Scheduling
        private readonly HalloDocDBContext _db;
        public ScheduleRepo(HalloDocDBContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Shift>?> GetShifts()
        {
            return await Task.Run(() => _db.Shifts);
        }

        public async Task<bool> IsShiftValid(Shift shift, Shiftdetail shiftdetail, int CreatedBy, ScheduleViewModel model, int toggler, int? phyid)
        {
            if (phyid != null)
            {
                shift.Physicianid = (int)phyid;
            }
            //check for data
            if (shift.Isrepeat == false)
            {
                var checkDetails = from x1 in _db.Shifts
                                   join sd in _db.Shiftdetails on x1.Shiftid equals sd.Shiftid into temp1
                                   from SD in temp1.DefaultIfEmpty()
                                   where x1.Physicianid == shift.Physicianid
                                   && SD.Isdeleted != true
                                   && SD.Shiftdate == shift.Startdate
                                   && ((shiftdetail.Starttime >= SD.Starttime && shiftdetail.Starttime <= SD.Endtime)
                                       || (shiftdetail.Endtime >= SD.Starttime && shiftdetail.Starttime <= SD.Endtime)
                                       || (SD.Starttime >= shiftdetail.Starttime && SD.Starttime <= shiftdetail.Endtime)
                                       || (SD.Endtime >= shiftdetail.Starttime && SD.Endtime <= shiftdetail.Endtime)
                                     )
                                   select x1;

                var count = checkDetails.Count();

                if (count == 0)
                {
                    return true;
                }
                return await Task.Run(()=>false);
            }
            else
            {
                //do check for every date upto  3 weeks
                int k = 7;
                var startdate = shift.Startdate;
                //multiple shifts can be possible
                for (var i = 1; i <= shift.Repeatupto; i++)
                {
                    //for the week
                    //than do repeats

                    var LastWeekdate = shift.Startdate.AddDays((k * (i)));
                    //var j = 0;
                    do
                    {

                        var weekdate = (int)startdate.DayOfWeek;
                        if (weekdate == 0 && model.sunday || weekdate == 1 && model.monday || weekdate == 2 && model.Tuesday || weekdate == 3 && model.Wednesday || weekdate == 4 && model.Thursday || weekdate == 5 && model.friday || weekdate == 6 && model.saturday)
                        {
                            var checkDetails = from x1 in _db.Shifts
                                               join sd in _db.Shiftdetails on x1.Shiftid equals sd.Shiftid into temp1
                                               from SD in temp1.DefaultIfEmpty()
                                               where x1.Physicianid == shift.Physicianid
                                               && SD.Isdeleted != true
                                               && SD.Shiftdate == startdate
                                               && ((shiftdetail.Starttime >= SD.Starttime && shiftdetail.Starttime <= SD.Endtime)
                                                   || (shiftdetail.Endtime >= SD.Starttime && shiftdetail.Starttime <= SD.Endtime)
                                                   || (SD.Starttime >= shiftdetail.Starttime && SD.Starttime <= shiftdetail.Endtime)
                                                   || (SD.Endtime >= shiftdetail.Starttime && SD.Endtime <= shiftdetail.Endtime)
                                                 )
                                               select x1;
                            var countShifts = checkDetails.Count();
                            if (countShifts != 0)
                            {
                                return false;
                            }
                            startdate = startdate.AddDays(1);
                        }
                        else
                        {
                            startdate = startdate.AddDays(1);
                            continue;
                        }
                    } while (startdate != LastWeekdate);
                }
                return true;
            }
        }

        public async Task<bool> CreateSchedule(Shift shift, Shiftdetail shiftdetail, int CreatedBy, ScheduleViewModel model, int toggler, int? phyid)
        {


            Shift Newshift = new()
            {
                //Physicianid = shift.Physicianid,
                Startdate = shift.Startdate,
                Isrepeat = shift.Isrepeat,
                Repeatupto = shift.Repeatupto,
                //Createdby = admin.Aspnetuserid,
                Createddate = DateTime.Now,
            };
            if (model.sunday || model.monday || model.Tuesday || model.Wednesday || model.Thursday || model.friday || model.saturday)
            {
                Newshift.Weekdays += (model.sunday) ? "sunday" : "";
                Newshift.Weekdays += (model.monday) ? "monday" : "";
                Newshift.Weekdays += (model.Tuesday) ? "Tuesday" : "";
                Newshift.Weekdays += (model.Wednesday) ? "Wednesday" : "";
                Newshift.Weekdays += (model.Thursday) ? "Thursday" : "";
                Newshift.Weekdays += (model.friday) ? "friday" : "";
                Newshift.Weekdays += (model.saturday) ? "saturday" : "";
            }

            //admin will create shift   
            if (toggler == 1)
            {
                var admin = await _db.Admins.FirstOrDefaultAsync(x => x.Adminid == CreatedBy);
                if (admin == null)
                {
                    return false;
                }
                if (admin.Aspnetuserid == null)
                {
                    return false;
                }
                Newshift.Physicianid = shift.Physicianid;
                Newshift.Createdby = admin.Aspnetuserid;
            }
            //provider requesrt shift
            if (toggler == 2)
            {
                var phydata = await _db.Physicians.FirstOrDefaultAsync(x => x.Physicianid == CreatedBy);
                if (phydata == null)
                {
                    return false;
                }
                if (phydata.Aspnetuserid == null)
                {
                    return false;
                }

                Newshift.Physicianid = CreatedBy;
                Newshift.Createdby = phydata.Aspnetuserid;
            }
            _db.Shifts.Add(Newshift);
            _db.SaveChanges();


            if (shift.Isrepeat)
            {
                //shift for toady "DoubtFULL"
                Shiftdetail TodayShiftDetail = new()
                {
                    Shiftid = Newshift.Shiftid,
                    Shiftdate = shift.Startdate,
                    Regionid = shiftdetail.Regionid,
                    Starttime = shiftdetail.Starttime,
                    Endtime = shiftdetail.Endtime,
                    Status = 0,
                    Isdeleted = false
                };
                _db.Shiftdetails.Add(TodayShiftDetail);
                _db.SaveChanges();
                if (shiftdetail.Regionid != null)
                {
                    Shiftdetailregion shiftdetailregion = new()
                    {
                        Shiftdetailid = TodayShiftDetail.Shiftdetailid,
                        Regionid = (int)shiftdetail.Regionid
                    };
                    _db.Shiftdetailregions.Add(shiftdetailregion);
                }
                _db.SaveChanges();

                //do check for every date upto  3 weeks
                int k = 7;
                var startdate = shift.Startdate;
                //multiple shifts can be possible
                for (var i = 1; i <= shift.Repeatupto; i++)
                {
                    //for the week
                    //than do repeats

                    var LastWeekdate = shift.Startdate.AddDays((k * (i)));
                    //var j = 0;
                    do
                    {
                        //var year= startdate.Year;
                        //var month= startdate.Month;
                        //var day = startdate.Day;
                        //DateTime dateValue = new DateTime(year,day,month);


                        //check for physican and his all shifts
                        //var physiciandata= _db.Shifts.Where(x=>x.Physicianid == shift.Physicianid);
                        //if (physiciandata!= null)
                        //{
                        //    //check in the shiftdetails
                        //    foreach (var item in physiciandata)
                        //    {
                        //        var lists = _db.Shiftdetails.Where(x => x.Shiftid == item.Shiftid );

                        //    }
                        //}

                        //get shift with data given on that day
                        //if match

                        var weekdate = (int)startdate.DayOfWeek;
                        if (weekdate == 0 && model.sunday || weekdate == 1 && model.monday || weekdate == 2 && model.Tuesday || weekdate == 3 && model.Wednesday || weekdate == 4 && model.Thursday || weekdate == 5 && model.friday || weekdate == 6 && model.saturday)
                        {
                            //check for overlap conditions

                            //var checkoverlap = _db.Shiftdetails.Any(x=>x.Shiftdate == startdate &&); 

                            Shiftdetail NewShiftDetail = new()
                            {
                                Shiftid = Newshift.Shiftid,
                                Shiftdate = startdate,
                                Regionid = shiftdetail.Regionid,
                                Starttime = shiftdetail.Starttime,
                                Endtime = shiftdetail.Endtime,
                                Status = 0,
                                Isdeleted = false
                            };
                            startdate = startdate.AddDays(1);
                            _db.Shiftdetails.Add(NewShiftDetail);
                            _db.SaveChanges();
                            if (shiftdetail.Regionid != null)
                            {
                                Shiftdetailregion shiftdetailregion = new()
                                {
                                    Shiftdetailid = NewShiftDetail.Shiftdetailid,
                                    Regionid = (int)shiftdetail.Regionid
                                };
                                _db.Shiftdetailregions.Add(shiftdetailregion);
                            }
                            _db.SaveChanges();
                        }
                        else
                        {
                            startdate = startdate.AddDays(1);
                            continue;
                        }
                    } while (startdate != LastWeekdate);
                }
            }
            else
            {
                //only one time shift
                Shiftdetail NewShiftDetail = new()
                {
                    Shiftid = Newshift.Shiftid,
                    Shiftdate = shift.Startdate,
                    Regionid = shiftdetail.Regionid,
                    Starttime = shiftdetail.Starttime,
                    Endtime = shiftdetail.Endtime,
                    Status = 0,
                    Isdeleted = false
                };
                _db.Shiftdetails.Add(NewShiftDetail);
                _db.SaveChanges();
                if (shiftdetail.Regionid != null)
                {
                    Shiftdetailregion shiftdetailregion = new()
                    {
                        Shiftdetailid = NewShiftDetail.Shiftdetailid,
                        Regionid = (int)shiftdetail.Regionid
                    };
                    _db.Shiftdetailregions.Add(shiftdetailregion);
                }
                _db.SaveChanges();
            }
            return true;
        }
        public async Task<ScheduleViewModel> DayWiseSchedule(DateOnly date, int? regid, DateOnly? enddate)
        {
            IQueryable<Shiftdetail> shiftdetails;
            if (enddate != null)
            {
                shiftdetails = _db.Shiftdetails.Where(x => x.Shiftdate >= date && x.Shiftdate <= enddate && x.Isdeleted != true).OrderBy(x => x.Shiftdate);
            }
            else
            {
                shiftdetails = _db.Shiftdetails.Where(x => x.Shiftdate == date && x.Isdeleted != true).OrderBy(x => x.Shiftdate);
            }
            ScheduleViewModel model = new()
            {
                Physician = _db.Physicians.Where(x => x.Regionid == regid || regid == null).OrderBy(x => x.Physicianid),
                Shiftdetail = shiftdetails
            };
            return await Task.Run(() => model);
        }
        public async Task<Shiftdetail?> FetchScheduleData(int ShiftDetailsids)
        {
            Shiftdetail? shiftdetaillist = _db.Shiftdetails.SingleOrDefault(x => x.Shiftdetailid == ShiftDetailsids && x.Isdeleted != true);
            return await Task.Run(() => shiftdetaillist);
        }
        public async Task<bool> ValidateSingleShift(int shiftdetailsid, DateOnly date, TimeOnly start, TimeOnly end)
        {
            var shiftdetails = _db.Shiftdetails.SingleOrDefault(x => x.Shiftdetailid == shiftdetailsid);
            if (shiftdetails == null)
            {
                return false;
            }
            //shift data
            var shiftData = _db.Shifts.FirstOrDefault(x => x.Shiftid == shiftdetails.Shiftid);
            if (shiftData == null)
            {
                return false;
            }
            //check for overlap
            var checkDetails = from x1 in _db.Shifts
                               join sd in _db.Shiftdetails on x1.Shiftid equals sd.Shiftid into temp1
                               from SD in temp1.DefaultIfEmpty()
                               where x1.Physicianid == shiftData.Physicianid
                               && SD.Shiftdate == date
                               && SD.Isdeleted != true
                               && ((start >= SD.Starttime && start <= SD.Endtime)
                                   || (end >= SD.Starttime && end <= SD.Endtime)
                                   || (SD.Starttime >= start && SD.Starttime <= end)
                                   || (SD.Endtime >= start && SD.Endtime <= end)
                                 )
                               select x1;
            var count = checkDetails.Count();
            if (count == 1)
            {
                return true;
            }
            return await Task.Run(() => false);
        }
        public async Task<bool> EditShiftDetails(int shiftdetailsid, DateOnly date, TimeOnly start, TimeOnly end)
        {
            var shiftdetails = _db.Shiftdetails.FirstOrDefault(x => x.Shiftdetailid == shiftdetailsid);
            if (shiftdetails != null)
            {
                shiftdetails.Shiftdate = date;
                shiftdetails.Starttime = start;
                shiftdetails.Endtime = end;
                _db.Shiftdetails.Update(shiftdetails);
                await _db.SaveChangesAsync();
                return true;
            }
            return false;
        }
        public async Task<bool> ChangeShiftStatus(int shiftdetailsid)
        {
            var data = _db.Shiftdetails.FirstOrDefault(x => x.Shiftdetailid == shiftdetailsid && x.Isdeleted != true);
            if (data != null)
            {
                data.Status = 1;
                _db.Shiftdetails.Update(data);
                await _db.SaveChangesAsync();
                return true;
            }
            return false;
        }
        public async Task<ProviderOnCallViewModel> ProviderOnCallPartial(int adminid, int? regid)
        {
            //provider on call right now
            if (regid == -1)
            {
                regid = null;
            }
            var date = DateTime.Now;
            var admindata = await _db.Admins.FirstOrDefaultAsync(x => x.Adminid == adminid);

            var curYear = DateTime.Now.ToString("yyyy");
            var curMonth = DateTime.Now.ToString("MM");
            var curDay = DateTime.Now.ToString("dd");
            //var Curtime = DateTime.Now.ToString("mm:HH:ss");
            var curHour = DateTime.Now.ToString("HH");
            var curmin = DateTime.Now.ToString("mm");

            var curtime = new TimeOnly(int.Parse(curHour), int.Parse(curmin), 0);
            //Provider of Duty
            IEnumerable<Physician> phydata = _db.Physicians.Where(x => x.Createdby == admindata.Aspnetuserid);

            // in shifts
            var query = (from phy in _db.Physicians
                         join shifts in _db.Shifts on phy.Physicianid equals shifts.Physicianid into temp
                         from x2 in temp.DefaultIfEmpty()
                         join Shiftdetail in _db.Shiftdetails on x2.Shiftid equals Shiftdetail.Shiftid into temp2
                         from x3 in temp2.DefaultIfEmpty()
                         where phy.Createdby == admindata.Aspnetuserid && x3.Shiftdate.Year == int.Parse(curYear)
                         && x3.Shiftdate.Month == int.Parse(curMonth) && x3.Shiftdate.Day == int.Parse(curDay)
                         && x3.Starttime <= curtime && x3.Endtime >= curtime
                         && x3.Isdeleted != true
                         && x3.Status == 1
                         && (regid == null || regid == phy.Regionid)
                         && (phy.Regionid == regid || regid == null)
                         //&& (x3.Starttime.Hour == int.Parse(curHour)) ? x3.Starttime.Minute <= int.Parse(curMin) : (x3.Starttime.Hour < int.Parse(curHour))
                         //&& (x3.Endtime.Hour == int.Parse(curHour)) ? x3.Endtime.Minute >= int.Parse(curMin) : (x3.Endtime.Hour > int.Parse(curHour))
                         select phy);

            var offdutyphysician = _db.Physicians.Except(query).Where(X => X.Regionid == regid || regid == null);


            //var offdutyphyCheck = _db.Physicians.Any(x => x.Status == 6 && ((x.Regionid == regid) || (regid == null)));
            //offdutyphy = _db.Physicians.Where(x => x.Status == 6);
            //IEnumerable<Physician> offdutyphy;

            var model = new ProviderOnCallViewModel();
            model.ProviderOffDuty = offdutyphysician;
            model.ProviderOnCall = query;
            return model;
        }
        public async Task<PendingShiftCombine?> PendingShifts(int adminid, int? PageNumber, int? regid)
        {
            IEnumerable<ReqestedShiftDetailsviewmodel>? query = null;

            if (regid == -1)
            {
                regid = null;
            }
            var admindata = _db.Admins.FirstOrDefault(x => x.Adminid == adminid);
            if (admindata != null)
            {
                if (admindata.Aspnetuserid != null)
                {
                    var phy = _db.Physicians.Where(x => x.Createdby == admindata.Aspnetuserid);
                    var curmonth = int.Parse(DateTime.Now.ToString("MM"));
                    //fetch month
                    query = from ph in _db.Physicians
                            join Shift in _db.Shifts on ph.Physicianid equals Shift.Physicianid into temp
                            from shift in temp.DefaultIfEmpty()
                            join sd in _db.Shiftdetails on shift.Shiftid equals sd.Shiftid into temp2
                            from x3 in temp2.DefaultIfEmpty()
                            join Reg in _db.Regions on x3.Regionid equals Reg.Regionid into temp3
                            from x4 in temp3.DefaultIfEmpty()
                            where ph.Createdby == admindata.Aspnetuserid &&
                            x3.Status == 0 && (regid == null || ph.Regionid == regid)
                            && x3.Isdeleted != true
                            select new ReqestedShiftDetailsviewmodel()
                            {
                                ShiftdetailId = x3.Shiftdetailid,
                                Physicianname = ph.Firstname + "," + ph.Lastname,
                                ShiftDate = x3.Shiftdate,
                                Start = x3.Starttime,
                                End = x3.Endtime,
                                Region = x4.Name
                            };
                }
            }
            PendingShiftCombine model = new();
            if (query == null)
            {

                model.PageNumber = 0;
                model.maxPage = 0;
                return model;
            }

            int count = query.Count();
            int page = 1;
            int maxPage = count / 5;
            if (count % 5 != 0)
            {
                maxPage += 1;
            }
            if (PageNumber != null)
            {
                if (PageNumber == 999999)
                {
                    if (page != maxPage)
                    {
                        PageNumber = count / 5 + 1;
                    }
                    else
                    {
                        PageNumber = 1;
                    }
                }

                page = (int)PageNumber;
            }
            var Paginateddata = query.Skip((page - 1) * 5).Take(5);
            if (count == 0)
            {
                page = 0;
            }
            model.ReqestedShiftDetailsviewmodel = Paginateddata;
            model.maxPage = maxPage;
            model.PageNumber = page;
            model.RegionData = _db.Regions;
            return await Task.Run(() => model);
        }
        public async Task<bool> ApproveOrDeleteShift(List<int> idlist, string reqtype)
        {
            var i = 0;
            while (i != idlist.Count)
            {
                var sdData = _db.Shiftdetails.FirstOrDefault(x => x.Shiftdetailid == idlist[i]);

                if (sdData == null)
                {
                    return false;
                }
                else
                {
                    if (reqtype == "ApproveShift")
                        sdData.Status = 1;
                    else if (reqtype == "DeleteShift")
                        sdData.Isdeleted = true;
                    _db.Shiftdetails.Update(sdData);
                    await _db.SaveChangesAsync();
                }
                i++;
            }
            return true;
        }
        public async Task<int> MonthWiseScheduleStartData()
        {
            var date = DateTime.Now;
            var firstdate = new DateTime(date.Year, date.Month, 1);
            int dayofweek = (int)firstdate.DayOfWeek;
            return await Task.Run(() => dayofweek);
        }
        //public async Task<IEnumerable<MonthShiftViewModel>?> MonthShiftData(int month , int?regid, int adminid)
        //{
        //    IEnumerable<MonthShiftViewModel>? query=null;

        //    if (regid == -1)
        //    {
        //        regid = null;
        //    }
        //    return query;
        //}
        public async Task<IEnumerable<ReqestedShiftDetailsviewmodel>?> ShiftMonthsData(int month, int year, int? regid, int? PhysicianId)
        {
            IEnumerable<ReqestedShiftDetailsviewmodel> model = from sd in _db.Shiftdetails
                                                               join x1 in _db.Shifts on sd.Shiftid equals x1.Shiftid into temp1
                                                               from X1 in temp1.DefaultIfEmpty()
                                                               join x2 in _db.Physicians on X1.Physicianid equals x2.Physicianid into temp2
                                                               from X2 in temp2.DefaultIfEmpty()
                                                               join x3 in _db.Regions on sd.Regionid equals x3.Regionid into temp3
                                                               from X3 in temp3.DefaultIfEmpty()
                                                               where sd.Shiftdate.Month == month && sd.Shiftdate.Year == year
                                                               && (regid == null || sd.Regionid == regid)
                                                               && sd.Isdeleted != true
                                                               && (PhysicianId == null || X1.Physicianid == PhysicianId)
                                                               select new ReqestedShiftDetailsviewmodel()
                                                               {
                                                                   phyid = X2.Physicianid,
                                                                   Physicianname = X2.Firstname + "%" + X2.Lastname,
                                                                   Region = X3.Name,
                                                                   RegionId = sd.Regionid,
                                                                   ShiftDate = sd.Shiftdate,
                                                                   ShiftdetailId = sd.Shiftdetailid,
                                                                   Start = sd.Starttime,
                                                                   End = sd.Endtime,
                                                                   status = sd.Status,
                                                               };


            return await Task.Run(() => model);
        }
        #endregion
    }

}
