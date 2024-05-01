using HalloDoc.BAL.Interface;
using HalloDoc.DAL.Data;
using HalloDoc.DAL.DataModels;
using HalloDoc.DAL.ViewModals.AdminDashBoardViewModels;
using HalloDoc.DAL.ViewModals.ProviderAccount;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
using OfficeOpenXml.Drawing.Chart.ChartEx;
using Org.BouncyCastle.Asn1.Ocsp;
using Org.BouncyCastle.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.BAL.Repository
{
    /// <summary>
    /// Provider Account Unique Action Will Handle From This Repository (common Method will Directly Access from IAdminPartialsRepo)
    /// </summary>
    public class ProviderRepo : IProviderRepo
    {
        public readonly HalloDocDBContext _db;
        public ProviderRepo(HalloDocDBContext db)
        {
            _db = db;
        }
        #region ProviderRequests
        public async Task<bool> UpdateLocation(float latitude, float longtitude, int physicianId)
        {
            var physicianData = await _db.Physicians.FirstOrDefaultAsync(x => x.Physicianid == physicianId);
            if (physicianData != null)
            {
                physicianData.Lattitude = latitude;
                physicianData.Longtitude = longtitude;
                _db.Physicians.Update(physicianData);
                _db.SaveChanges();
                return true;
            }
            return false;
        }
        public async Task<DataCount> ProviderDashBoardMain(int PhysicainId)
        {
            var countNewRequests = (from req in _db.Requests where (req.Status == 21) && req.Isdeleted != true && req.Physicianid == PhysicainId select req).Count();
            var countPendingRequests = (from req in _db.Requests where req.Status == 4 && req.Isdeleted != true && req.Physicianid == PhysicainId select req).Count();
            var countActiveRequests = (from req in _db.Requests where (req.Status == 6 || req.Status == 2) && req.Physicianid == PhysicainId && req.Isdeleted != true select req).Count();
            var countConcludeRequests = (from req in _db.Requests where req.Status == 11 && req.Physicianid == PhysicainId && req.Isdeleted != true select req).Count();
            var datacount = new DataCount()
            {
                NewRequest = countNewRequests,
                PendingRequest = countPendingRequests,
                ActiveRequest = countActiveRequests,
                ConcludeRequest = countConcludeRequests,
            };
            return await Task.Run(() => datacount);
        }
        public async Task<NewRequestCombineViewModel> NewRequests(string? search, int? regid, int? reqType, int? PageNumber, int PhysicainId)
        {
            if (regid == -1)
            {
                regid = null;
            }
            IEnumerable<NewRequestViewModel> Newquery;

            Newquery = from reqClient in _db.Requestclients
                       join Request in _db.Requests on reqClient.Requestid equals Request.Requestid
                       where (Request.Status == 21) && Request.Physicianid == PhysicainId & (search == null || ((reqClient.Firstname + reqClient.Lastname).ToLower()).Contains(search.ToLower()) || ((Request.Firstname + Request.Lastname).ToLower()).Contains(search.ToLower()))
                       & Request.Isdeleted != true
                       & (regid == null || reqClient.Regionid == regid)
                       & (reqType == null || Request.Requesttypeid == reqType)
                       orderby Request.Createddate
                       select new NewRequestViewModel()
                       {
                           Firstname = reqClient.Firstname + reqClient.Lastname,
                           DateOfBirth = reqClient.Strmonth,
                           Requestor = Request.Firstname + Request.Lastname,
                           Requesterdate = Request.Createddate,
                           Phonenumber = reqClient.Phonenumber,
                           Region = reqClient.State,
                           Address = reqClient.Street + reqClient.City + reqClient.State + reqClient.Zipcode,
                           Notes = reqClient.Notes,
                           RequestType = Request.Requesttypeid,
                           MobileRequestor = Request.Phonenumber,
                           RequestId = Request.Requestid,
                           Email = reqClient.Email,
                       };
            int count = Newquery.Count();
            int page = 1;
            int maxPage = count / 10;
            if (count % 10 != 0)
            {
                maxPage += 1;
            }
            if (PageNumber != null)
            {
                if (PageNumber == 999999)
                {
                    if (page != maxPage)
                        PageNumber = count / 10 + 1;
                    else
                    {
                        PageNumber = 1;
                    }
                }

                page = (int)PageNumber;
            }
            var Paginateddata = Newquery.Skip((page - 1) * 10).Take(10);
            //avial regions


            var region = (from reg in _db.Regions
                          select new Region()
                          {
                              Regionid = reg.Regionid,
                              Name = reg.Name,
                          }).ToList();

            //avail physicians
            var phys = (from phy in _db.Physicians
                        select new Physician()
                        {
                            Physicianid = phy.Physicianid,
                            Firstname = phy.Firstname,
                        }).ToList();
            var castags = (from casttag in _db.Casetags
                           select casttag).ToList();
            var combineModel = new NewRequestCombineViewModel()
            {
                NewRequestViewModels = Paginateddata,
                Regions = region,
                physicians = phys,
                casetags = castags,
                //Search = search,
                PageNumber = page,
                maxPage = maxPage
            };

            return await Task.Run(() => combineModel);
        }
        public async Task<PendingRequestCombine> PendingRequests(string? search, int? regid, int? reqType, int? PageNumber, int PhysicainId)
        {
            if (regid == -1)
            {
                regid = null;
            }
            IEnumerable<PendingRequestViewModel> Pendingquery;

            Pendingquery = from reqClient in _db.Requestclients
                           join Request in _db.Requests on reqClient.Requestid equals Request.Requestid
                           join Phy in _db.Physicians on Request.Physicianid equals Phy.Physicianid into temp
                           from phy in temp.DefaultIfEmpty()
                           where Request.Status == 4 && Request.Physicianid == PhysicainId && (search == null || ((reqClient.Firstname + reqClient.Lastname).ToLower()).Contains(search.ToLower()) || ((Request.Firstname + Request.Lastname).ToLower()).Contains(search.ToLower()))
                            & Request.Isdeleted != true
                            & (regid == null || reqClient.Regionid == regid)
                            & (reqType == null || Request.Requesttypeid == reqType)
                           orderby Request.Createddate
                           //4 status is Forbid pending
                           select new PendingRequestViewModel()
                           {
                               Firstname = reqClient.Firstname + reqClient.Lastname,
                               DateOfBirth = reqClient.Strmonth,
                               Requestor = Request.Firstname + Request.Lastname,
                               PhysicianName = (phy != null) ? (phy.Firstname + " " + phy.Lastname) : null,

                               DateOfService = Request.Accepteddate,
                               Phonenumber = reqClient.Phonenumber,
                               Region = reqClient.State,
                               PhysicianId = (phy != null) ? (phy.Physicianid) : null,
                               Address = reqClient.Street + reqClient.City + reqClient.Street + reqClient.Zipcode,
                               //Notes = reqClient.Notes,
                               RequestType = Request.Requesttypeid,
                               MobileRequestor = Request.Phonenumber,
                               RequestId = Request.Requestid,
                               Email = reqClient.Email,
                           };

            //for each request find
            var datawithnotes = Pendingquery.ToList();
            foreach (var item in datawithnotes)
            {
                var transferdata = _db.Requeststatuslogs.Where(x => x.Requestid == item.RequestId).OrderByDescending(x => x.Requeststatuslogid).Take(1).FirstOrDefault();
                if (transferdata != null)
                {
                    if (transferdata.Transtophysicianid != null)
                    {
                        item.Notes = transferdata.Notes + "(" + transferdata.Createddate + ")";
                    }
                }

            }

            int count = datawithnotes.Count();
            int page = 1;
            int maxPage = count / 10;
            if (count % 10 != 0)
            {
                maxPage += 1;
            }
            if (PageNumber != null)
            {
                if (PageNumber == 999999)
                {
                    if (page != maxPage)
                        PageNumber = count / 10 + 1;
                    else
                    {
                        PageNumber = 1;
                    }
                }

                page = (int)PageNumber;
            }
            var Paginateddata = datawithnotes.Skip((page - 1) * 10).Take(10);

            var region = (from reg in _db.Regions
                          select new Region()
                          {
                              Regionid = reg.Regionid,
                              Name = reg.Name,
                          }).ToList();

            //avail physicians
            var phys = (from phy in _db.Physicians
                        select new Physician()
                        {
                            Physicianid = phy.Physicianid,
                            Firstname = phy.Firstname,
                        }).ToList();

            var model = new PendingRequestCombine()
            {
                PendingRequestViewModels = Paginateddata,
                physicians = phys,
                Regions = region,
                PageNumber = page,
                maxPage = maxPage

            };
            return await Task.Run(() => model);
        }
        public async Task<ActiveRequestCombine> ActiveRequests(string? search, int? regid, int? reqType, int? PageNumber, int PhysicainId)
        {
            if (regid == -1)
            {
                regid = null;
            }
            IEnumerable<ActiveRequestViewModel> Activequery;

            Activequery = from reqClient in _db.Requestclients
                          join Request in _db.Requests on reqClient.Requestid equals Request.Requestid
                          join Phy in _db.Physicians on Request.Physicianid equals Phy.Physicianid into temp
                          from phy in temp.DefaultIfEmpty()
                          where (Request.Status == 6 || Request.Status == 2) && Request.Physicianid == PhysicainId && (search == null || ((reqClient.Firstname + reqClient.Lastname).ToLower()).Contains(search.ToLower()) || ((Request.Firstname + Request.Lastname).ToLower()).Contains(search.ToLower()))
                            & Request.Isdeleted != true
                            & (regid == null || reqClient.Regionid == regid)
                            & (reqType == null || Request.Requesttypeid == reqType)
                          orderby Request.Createddate
                          //4 status is Forbid pending
                          select new ActiveRequestViewModel()
                          {
                              Firstname = reqClient.Firstname + reqClient.Lastname,
                              DateOfBirth = reqClient.Strmonth,
                              Requestor = Request.Firstname + Request.Lastname,
                              PhysicianName = (phy != null) ? (phy.Firstname + " " + phy.Lastname) : null,
                              DateOfService = Request.Accepteddate,
                              Phonenumber = reqClient.Phonenumber,
                              Address = reqClient.Street + reqClient.City + reqClient.Street + reqClient.Zipcode,
                              //Notes = reqClient.Notes,
                              RequestType = Request.Requesttypeid,
                              StatusType = Request.Status,
                              MobileRequestor = Request.Phonenumber,
                              RequestId = Request.Requestid,
                              IsFinalize = Request.Completedbyphysician

                          };

            //for each request find
            var datawithnotes = Activequery.ToList();
            foreach (var item in datawithnotes)
            {
                var transferdata = _db.Requeststatuslogs.Where(x => x.Requestid == item.RequestId).OrderByDescending(x => x.Requeststatuslogid).Take(1).FirstOrDefault();
                if (transferdata != null)
                {
                    if (transferdata.Transtophysicianid != null)
                    {
                        item.Notes = transferdata.Notes + "(" + transferdata.Createddate + ")";
                    }
                }

            }
            int count = datawithnotes.Count();
            int page = 1;
            int maxPage = count / 10;
            if (count % 10 != 0)
            {
                maxPage += 1;
            }
            if (PageNumber != null)
            {
                if (PageNumber == 999999)
                {
                    if (page != maxPage)
                        PageNumber = count / 10 + 1;
                    else
                    {
                        PageNumber = 1;
                    }
                }

                page = (int)PageNumber;
            }
            var Paginateddata = datawithnotes.Skip((page - 1) * 10).Take(10);

            ActiveRequestCombine model = new()
            {
                ActiveRequestViewModels = Paginateddata,
                PageNumber = page,
                maxPage = maxPage
            };
            return await Task.Run(() => model);
        }
        public async Task<ConcludeRequestCombine> ConcludeRequests(string? search, int? regid, int? reqType, int? PageNumber, int PhysicainId)
        {
            if (regid == -1)
            {
                regid = null;
            }
            IEnumerable<ConcludeRequestViewModel> Concludequery;

            Concludequery = from reqClient in _db.Requestclients
                            join Request in _db.Requests on reqClient.Requestid equals Request.Requestid
                            join Phy in _db.Physicians on Request.Physicianid equals Phy.Physicianid into temp
                            from phy in temp.DefaultIfEmpty()
                            where Request.Status == 11 && Request.Physicianid == PhysicainId && (search == null || ((reqClient.Firstname + reqClient.Lastname).ToLower()).Contains(search.ToLower()) || ((Request.Firstname + Request.Lastname).ToLower()).Contains(search.ToLower()))
                            & Request.Isdeleted != true
                            & (regid == null || reqClient.Regionid == regid)
                            & (reqType == null || Request.Requesttypeid == reqType)
                            orderby Request.Createddate
                            //4 status is Forbid pending
                            select new ConcludeRequestViewModel()
                            {
                                Firstname = reqClient.Firstname + reqClient.Lastname,
                                DateOfBirth = reqClient.Strmonth,
                                Requestor = Request.Firstname + Request.Lastname,
                                PhysicianName = (phy != null) ? (phy.Firstname + " " + phy.Lastname) : null,
                                DateOfService = Request.Accepteddate,
                                Phonenumber = reqClient.Phonenumber,
                                Address = reqClient.Street + reqClient.City + reqClient.Street + reqClient.Zipcode,
                                RequestType = Request.Requesttypeid,
                                MobileRequestor = Request.Phonenumber,
                                RequestId = Request.Requestid,
                                IsFinalize = Request.Completedbyphysician
                            };
            int count = Concludequery.Count();
            int page = 1;
            int maxPage = count / 10;
            if (count % 10 != 0)
            {
                maxPage += 1;
            }
            if (PageNumber != null)
            {
                if (PageNumber == 999999)
                {
                    if (page != maxPage)
                        PageNumber = count / 10 + 1;
                    else
                    {
                        PageNumber = 1;
                    }
                }

                page = (int)PageNumber;
            }
            var Paginateddata = Concludequery.Skip((page - 1) * 10).Take(10);

            ConcludeRequestCombine model = new()
            {
                concludeRequestViewModels = Paginateddata,
                PageNumber = page,
                maxPage = maxPage
            };
            return await Task.Run(() => model);
        }
        #endregion
        #region ProviderActions
        public async Task<IEnumerable<Region>> GetRegionOfPhysician(int PhysicianId)
        {
            IEnumerable<Region> regions = from x1 in _db.Physicianregions
                                          where x1.Physicianid == PhysicianId
                                          join x2 in _db.Regions on x1.Regionid equals x2.Regionid
                                          select x2;
            return await Task.Run(() => regions);
        }
        public async Task AddPhysicianNotes(string PhysicianNotes, int requestid, int PhysicianId)
        {

            int ReqNoteId = _db.Requestnotes.Count();
            //check is there a entry  in the requestNotes
            var checkReq = _db.Requestnotes.SingleOrDefault(x => x.Requestid == requestid);
            var requestNotesAdd = new Requestnote()
            {
                Requestnotesid = ReqNoteId + 1,
                Requestid = (int)requestid,
                Physiciannotes = PhysicianNotes,
                Createdby = PhysicianId.ToString(),
                Createddate = DateTime.Now,
            };

            //logs
            var requestStatusLogs = new Requeststatuslog()
            {
                Requestid = requestid,
                Physicianid = PhysicianId,
                Status = 1,
                Notes = PhysicianNotes,
                Createddate = DateTime.Now,

            };
            _db.Requeststatuslogs.Add(requestStatusLogs);
            await _db.SaveChangesAsync();

            if (checkReq == null)
            {

                _db.Requestnotes.Add(requestNotesAdd);
                await _db.SaveChangesAsync();
            }
            else
            {
                checkReq.Physiciannotes = PhysicianNotes;
                checkReq.Modifieddate = DateTime.Now;
                checkReq.Modifiedby = PhysicianId.ToString();
                _db.Requestnotes.Update(checkReq);
                await _db.SaveChangesAsync();
            }
        }
        public async Task<bool> AcceptCaseByProvider(int RequestId)
        {
            var RequestorData = await _db.Requests.FirstOrDefaultAsync(x => x.Requestid == RequestId);
            if (RequestorData != null)
            {
                RequestorData.Status = 4;
                RequestorData.Accepteddate = DateTime.Now;
                _db.Requests.Update(RequestorData);
                _db.SaveChanges();

                //logs
                var requestStatusLogs = new Requeststatuslog()
                {
                    Requestid = RequestId,
                    Physicianid = RequestorData.Physicianid,
                    Status = 4,
                    Notes = "Request Accepted ",
                    Createddate = DateTime.Now,
                };
                _db.Requeststatuslogs.Add(requestStatusLogs);
                await _db.SaveChangesAsync();
                return true;
            }
            return false;
        }
        public async Task<bool> TransferBackAdmin(int reqid, string TransferNotes)
        {
            var requestdata = await _db.Requests.FirstOrDefaultAsync(x => x.Requestid == reqid);
            if (requestdata != null)
            {
                requestdata.Status = 22;
                requestdata.Modifieddate = DateTime.Now;

                //logs
                var requestStatusLogs = new Requeststatuslog()
                {
                    Requestid = reqid,
                    Physicianid = requestdata.Physicianid,
                    Status = 22,
                    Transtoadmin = true,
                    Notes = TransferNotes,
                    Createddate = DateTime.Now,
                };
                _db.Requeststatuslogs.Add(requestStatusLogs);
                await _db.SaveChangesAsync();

                requestdata.Physicianid = null;
                _db.Requests.Update(requestdata);
                await _db.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<bool> FinalizeEnconuter(int reqid)
        {
            var RequestData = await _db.Requests.FirstOrDefaultAsync(x => x.Requestid == reqid);
            if (RequestData != null)
            {
                RequestData.Completedbyphysician = true;
                _db.Requests.Update(RequestData);
                _db.SaveChanges();
                return true;
            }
            return false;
        }
        public async Task<bool> SaveEncounterPreferences(int reqid, string SelectType, int Phyid)
        {
            var data = await _db.Requests.FirstOrDefaultAsync(x => x.Requestid == reqid);
            if (data == null)
            {
                return false;
            }
            if (SelectType == "Consult")
            {
                data.Status = 11;
                data.Modifieddate = DateTime.Now;
            }
            else
            {
                data.Status = 6;
                data.Modifieddate = DateTime.Now;
            }
            _db.Requests.Update(data);
            _db.SaveChanges();

            //logs

            var log = new Requeststatuslog()
            {
                Requestid = reqid,
                Status = data.Status,
                Createddate = DateTime.Now,
                Physicianid = Phyid,
                Notes = "Mode Of Service Changed"
            };
            _db.Requeststatuslogs.Add(log);
            _db.SaveChanges();
            return true;
        }
        public async Task<bool> ConcludeCase(string PhysicianNotes, int reqid)
        {
            var requestData = await _db.Requests.FirstOrDefaultAsync(x => x.Requestid == reqid);
            if (requestData != null)
            {
                requestData.Status = 8;
                requestData.Modifieddate = DateTime.Now;
                _db.Requests.Update(requestData);
                _db.SaveChanges();
                if (requestData.Physicianid != null)
                {
                    //add logs and request notes
                    await AddPhysicianNotes(PhysicianNotes, reqid, (int)requestData.Physicianid);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }
        public async Task<IEnumerable<Shift>?> GetShifts(int physicianId)
        {
            return await Task.Run(() => _db.Shifts.Where(x => x.Physicianid == physicianId));
        }
        public async Task<Admin?> getAdminInfo(int physicianId)
        {
            Physician? PhysicianData = await _db.Physicians.FirstOrDefaultAsync(x => x.Physicianid == physicianId);
            if (PhysicianData == null)
            {
                return null;
            }
            if (PhysicianData.Createdby == null)
            {
                return null;
            }

            var admindata = await _db.Admins.FirstOrDefaultAsync(x => x.Aspnetuserid == PhysicianData.Createdby);
            return admindata;
        }
        public async Task<Physician?> getPhysicianInfo(int physicianId)
        {
            Physician? PhysicianData = await _db.Physicians.FirstOrDefaultAsync(x => x.Physicianid == physicianId);
            return PhysicianData;
        }
        #endregion
        #region Invoicing
        public async Task<IEnumerable<WeeklyTimeSheetViewModel>?> FinalizeTimeSheetView(int PhysicianId, string DateScoped)
        {
            DateOnly StaticDate = DateOnly.Parse(DateScoped.Split("/")[0]);
            DateOnly StartDate = DateOnly.Parse(DateScoped.Split("/")[0]);
            DateOnly EndDate = DateOnly.Parse(DateScoped.Split("/")[1]);
            List<WeeklyTimeSheetViewModel> model = new List<WeeklyTimeSheetViewModel>();
            IEnumerable<Shift>? shiftData = from shift in _db.Shifts where shift.Physicianid == PhysicianId select shift;
            var lastDate = EndDate.AddDays(1);
            var tempData = shiftData.ToList();
            while (StartDate != lastDate)
            {
                WeeklyTimeSheetViewModel MultiShift = new();
                MultiShift.Date = StartDate;

                //foreach (var item in tempData)
                //{
                    var ShiftDetails = from shift in _db.Shiftdetails where shift.Shiftdate == StartDate && shift.Isdeleted != true  && shiftData.Any(x => x.Shiftid == shift.Shiftid ) select shift;
                   
                    if (ShiftDetails.Count() == 0)
                    {
                        MultiShift.OnCallHours = 0;
                    }
                    else
                    {
                        //WeeklyTimeSheetViewModel OneShift = new();
                        //OneShift.Date = StartDate;
                        DateTime today = DateTime.Today;
                        double differinHours = 0.0;
                        var TempShiftDetails = ShiftDetails.ToList();
                        foreach (var shift in TempShiftDetails)
                        {
                            DateTime first = today.AddHours(shift.Starttime.Hour).AddMinutes(shift.Starttime.Minute);
                            DateTime second = today.AddHours(shift.Endtime.Hour).AddMinutes(shift.Endtime.Minute);
                            TimeSpan data = second - first;
                            differinHours += data.TotalHours;
                            //duration += (shift.Endtime).Subtract(shift.Endtime);
                            //int TotalOnCallHour = shift.Starttime. shift.Endtime;
                        }
                        MultiShift.OnCallHours = differinHours;
                    }

                //add physicain added data to the value

                //weekly id
                WeeklyInvoice? IsAvail = await _db.WeeklyInvoices.FirstOrDefaultAsync(x => x.Physicianid == PhysicianId && x.StartDate == StaticDate);
                if(IsAvail != null)
                {
                    DailyInvoice? data =await _db.DailyInvoices.FirstOrDefaultAsync(x=>x.WeeklyInvoiceId == IsAvail.Id && x.Date == StartDate);

                    if (data != null)
                    {
                        MultiShift.TotalHours = (int)data.TotalHours;
                        MultiShift.NumberOfHouseCall = (int)data.CountHouseCall;
                        MultiShift.NumberOfPhoneConsult = (int)data.CountPhoneConsult;
                    }
                }
                model.Add(MultiShift);
                StartDate = StartDate.AddDays(1);
            }
            return model;
        }
        public async Task<bool> SubmitWeeklyForm(WeeklyTimeSheetViewModel obj, int physicianId)
        {
            WeeklyInvoice? IsAvail =await _db.WeeklyInvoices.FirstOrDefaultAsync(x => x.Physicianid == physicianId && x.StartDate == obj.Date);
            if (IsAvail != null)
            {
                //update
                int countdata = 0;
                IEnumerable<DailyInvoice> data = from daily in _db.DailyInvoices where daily.WeeklyInvoiceId == IsAvail.Id select daily;
                var tempdata = data.ToList();
                foreach (var item in tempdata)
                { 
                    //item.WeeklyInvoiceId = IsAvail.Id;
                    //item.Date = obj.Date;
                    item.TotalHours = obj.TotalHoursPost[countdata];
                    //need to change
                    item.IsHoliday = false;
                    item.CountHouseCall = obj.HouseCallPost[countdata];
                    item.CountPhoneConsult = obj.PhoneConsult[countdata];
                    countdata++;
                    _db.DailyInvoices.Update(item);
                    _db.SaveChanges();
                }
            }
            else
            {
                //add
                WeeklyInvoice weekInvoice = new ();
                weekInvoice.StartDate=obj.Date;
                weekInvoice.EndDate=obj.Date.AddDays(obj.TotalHoursPost.Count()-1);
                weekInvoice.Physicianid = physicianId;
                weekInvoice.CreatedDate=DateTime.Now;

                _db.WeeklyInvoices.Add(weekInvoice);
                _db.SaveChanges();

                //add in daily
                int count = 0;
                foreach(var item in obj.TotalHoursPost)
                {
                    DailyInvoice dayInvoice = new();
                    dayInvoice.WeeklyInvoiceId = weekInvoice.Id;
                    dayInvoice.Date = obj.Date;
                    dayInvoice.TotalHours = item;
                    //need to change
                    dayInvoice.IsHoliday = false;
                    dayInvoice.CountHouseCall = obj.HouseCallPost[count];
                    dayInvoice.CountPhoneConsult = obj.PhoneConsult[count];
                    count++;
                    obj.Date = obj.Date.AddDays(1);
                    _db.DailyInvoices.Add(dayInvoice);
                    _db.SaveChanges();
                }
            }
            return true;
        }
        #endregion
    }
}