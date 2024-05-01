using HalloDoc.BAL.Interface;
using HalloDoc.DAL.Data;
using HalloDoc.DAL.DataModels;
using HalloDoc.DAL.ViewModals;
using HalloDoc.DAL.ViewModals.AdminDashBoardActions;
using HalloDoc.DAL.ViewModals.AdminDashBoardActions.NewRequestAction;
using HalloDoc.DAL.ViewModals.AdminDashBoardViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Net.Mail;
using System.Net;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using System.Data;
using HalloDoc.DAL.ViewModals.Provider;
using HalloDoc.DAL.ViewModals.Access;
using HalloDoc.DAL.ViewModals.Records;
using HalloDoc.DAL.ViewModals.Partner;
using MimeKit;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.IdentityModel.Abstractions;
using Org.BouncyCastle.X509;

namespace HalloDoc.BAL.Repository
{
    public class AdminPartialsRepo : IAdminPartialsRepo
    {
        private readonly HalloDocDBContext _db;
        public AdminPartialsRepo(HalloDocDBContext db)
        {
            _db = db;
        }
        #region ExcelFileRequestWise
        public async Task<IEnumerable<NewRequestViewModel>> ExcelNewRequest()
        {
            IEnumerable<NewRequestViewModel> Newquery;

            Newquery = from reqClient in _db.Requestclients
                       join Request in _db.Requests on reqClient.Requestid equals Request.Requestid
                       where (Request.Status == 1 || Request.Status == 21 || Request.Status == 22)
                       & Request.Isdeleted != true
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
                           //Notes = reqClient.Notes,
                           RequestType = Request.Requesttypeid,
                           MobileRequestor = Request.Phonenumber,
                           RequestId = Request.Requestid,
                           Email = reqClient.Email,
                           status = Request.Status

                       };
            return await Task.Run(() => Newquery);

        }
        public async Task<IEnumerable<PendingRequestViewModel>> ExcelPendingRequest()
        {
            IEnumerable<PendingRequestViewModel> Pendingquery;

            Pendingquery = from reqClient in _db.Requestclients
                           join Request in _db.Requests on reqClient.Requestid equals Request.Requestid
                           join Phy in _db.Physicians on Request.Physicianid equals Phy.Physicianid into temp
                           from phy in temp.DefaultIfEmpty()
                           where (Request.Status == 4)
                            & Request.Isdeleted != true
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
            return await Task.Run(() => Pendingquery);
        }
        public async Task<IEnumerable<ActiveRequestViewModel>> ExcelActiveRequest()
        {
            IEnumerable<ActiveRequestViewModel> Activequery;

            Activequery = from reqClient in _db.Requestclients
                          join Request in _db.Requests on reqClient.Requestid equals Request.Requestid
                          join Phy in _db.Physicians on Request.Physicianid equals Phy.Physicianid into temp
                          from phy in temp.DefaultIfEmpty()
                          where (Request.Status == 6 || Request.Status == 2)
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
                              MobileRequestor = Request.Phonenumber,
                              RequestId = Request.Requestid,
                              Email = reqClient.Email,
                          };

            return await Task.Run(() => Activequery);
        }
        public async Task<IEnumerable<ConcludeRequestViewModel>> ExcelConcludeRequest()
        {
            IEnumerable<ConcludeRequestViewModel> Concludequery;

            Concludequery = from reqClient in _db.Requestclients
                            join Request in _db.Requests on reqClient.Requestid equals Request.Requestid
                            join Phy in _db.Physicians on Request.Physicianid equals Phy.Physicianid into temp
                            from phy in temp.DefaultIfEmpty()
                            where (Request.Status == 11 || Request.Status == 12)
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
                                Email = reqClient.Email
                            };
            return await Task.Run(() => Concludequery);

        }
        public async Task<IEnumerable<ToCloseRequestViewModel>> ExcelCloseRequest()
        {
            IEnumerable<ToCloseRequestViewModel> ToClosequery;

            ToClosequery = from reqClient in _db.Requestclients
                           join Request in _db.Requests on reqClient.Requestid equals Request.Requestid
                           join Phy in _db.Physicians on Request.Physicianid equals Phy.Physicianid into temp
                           from phy in temp.DefaultIfEmpty()
                           where (Request.Status == 8)
                            & Request.Isdeleted != true
                           orderby Request.Createddate
                           //4 status is Forbid pending
                           select new ToCloseRequestViewModel()
                           {
                               Firstname = reqClient.Firstname + reqClient.Lastname,
                               DateOfBirth = reqClient.Strmonth,
                               Region = reqClient.State,
                               PhysicianName = (phy != null) ? (phy.Firstname + " " + phy.Lastname) : null,
                               //Notes = reqClient.Notes,
                               DateOfService = Request.Modifieddate,
                               Address = reqClient.Street + reqClient.City + reqClient.Street + reqClient.Zipcode,
                               RequestType = Request.Requesttypeid,
                               MobileRequestor = Request.Phonenumber,
                               RequestId = Request.Requestid,
                               Email = reqClient.Email,
                           };
            return await Task.Run(() => ToClosequery);

        }
        public async Task<IEnumerable<UnPaidRequestViewModel>> ExcelUnPaidRequest()
        {
            IEnumerable<UnPaidRequestViewModel> Unpaidquery;
            Unpaidquery = from reqClient in _db.Requestclients
                          join Request in _db.Requests on reqClient.Requestid equals Request.Requestid
                          join Phy in _db.Physicians on Request.Physicianid equals Phy.Physicianid into temp
                          from phy in temp.DefaultIfEmpty()
                          where Request.Status == 9
                            & Request.Isdeleted != true
                          orderby Request.Createddate
                          //4 status is Forbid pending
                          select new UnPaidRequestViewModel()
                          {
                              Firstname = reqClient.Firstname + reqClient.Lastname,
                              PhysicianName = (phy != null) ? (phy.Firstname + " " + phy.Lastname) : null,
                              DateOfService = Request.Modifieddate,
                              Address = reqClient.Street + reqClient.City + reqClient.Street + reqClient.Zipcode,
                              Phonenumber = reqClient.Phonenumber,
                              RequestType = Request.Requesttypeid,
                              MobileRequestor = Request.Phonenumber,
                              RequestId = Request.Requestid,
                              Email = reqClient.Email
                          };
            return await Task.Run(() => Unpaidquery);
        }
        #endregion
        #region StatusWiseRequests
        public async Task<DataCount> AdminDashBoardMain()
        {
            //count data

            var countNewRequests = (from req in _db.Requests where (req.Status == 1 || req.Status == 21 || req.Status == 22) && req.Isdeleted != true select req).Count();
            var countPendingRequests = (from req in _db.Requests where req.Status == 4 && req.Isdeleted != true select req).Count();
            var countActiveRequests = (from req in _db.Requests where (req.Status == 6 || req.Status == 2) && req.Isdeleted != true select req).Count();
            var countConcludeRequests = (from req in _db.Requests where req.Status == 11 && req.Isdeleted != true select req).Count();
            var countToCloseRequests = (from req in _db.Requests where (req.Status == 8) && req.Isdeleted != true select req).Count();
            var countUnpaidRequests = (from req in _db.Requests where req.Status == 9 && req.Isdeleted != true select req).Count();
            var datacount = new DataCount()
            {
                NewRequest = countNewRequests,
                PendingRequest = countPendingRequests,
                ActiveRequest = countActiveRequests,
                ConcludeRequest = countConcludeRequests,
                ToCloseRequest = countToCloseRequests,
                UnPaidRequest = countUnpaidRequests,
            };
            return await Task.Run(() => datacount);
        }
        public async Task<NewRequestCombineViewModel> NewRequests(string? search, int? regid, int? reqType, int? PageNumber)
        {
            if (regid == -1)
            {
                regid = null;
            }
            IEnumerable<NewRequestViewModel> Newquery;

            Newquery = from reqClient in _db.Requestclients
                       join Request in _db.Requests on reqClient.Requestid equals Request.Requestid
                       where (Request.Status == 1 || Request.Status == 21 || Request.Status == 22) & (search == null || ((reqClient.Firstname + reqClient.Lastname).ToLower()).Contains(search.ToLower()) || ((Request.Firstname + Request.Lastname).ToLower()).Contains(search.ToLower()))
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
                           //Notes = reqClient.Notes,
                           RequestType = Request.Requesttypeid,
                           MobileRequestor = Request.Phonenumber,
                           RequestId = Request.Requestid,
                           Email = reqClient.Email,
                           status = Request.Status

                       };
            var datawithnotes = Newquery.ToList();
            foreach (var item in datawithnotes)
            {
                var transferdata = _db.Requeststatuslogs.Where(x => x.Requestid == item.RequestId).OrderByDescending(x => x.Requeststatuslogid).Take(1).FirstOrDefault();
                if (transferdata != null)
                {
                    if (transferdata.Transtophysicianid != null || transferdata.Transtoadmin == true)
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
        public async Task<PendingRequestCombine> PendingRequests(string? search, int? regid, int? reqType, int? PageNumber)
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
                           where (Request.Status == 4 && (search == null || ((reqClient.Firstname + reqClient.Lastname).ToLower()).Contains(search.ToLower()) || ((Request.Firstname + Request.Lastname).ToLower()).Contains(search.ToLower())))
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
                    if (transferdata.Transtophysicianid != null || transferdata.Transtoadmin == true)
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
        public async Task<ActiveRequestCombine> ActiveRequests(string? search, int? regid, int? reqType, int? PageNumber)
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
                          where (Request.Status == 6 || Request.Status == 2) & (search == null || ((reqClient.Firstname + reqClient.Lastname).ToLower()).Contains(search.ToLower()) || ((Request.Firstname + Request.Lastname).ToLower()).Contains(search.ToLower()))
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
                              Notes = reqClient.Notes,
                              RequestType = Request.Requesttypeid,
                              MobileRequestor = Request.Phonenumber,
                              RequestId = Request.Requestid,
                              Email = reqClient.Email
                          };

            //for each request find
            var datawithnotes = Activequery.ToList();
            //foreach (var item in datawithnotes)
            //{
            //    var transferdata = _db.Requeststatuslogs.Where(x => x.Requestid == item.RequestId).OrderByDescending(x => x.Requeststatuslogid).Take(1).FirstOrDefault();
            //    if (transferdata != null)
            //    {
            //        if (transferdata.Transtophysicianid != null || transferdata.Transtoadmin == true)
            //        {
            //            item.Notes = transferdata.Notes + "(" + transferdata.Createddate + ")";
            //        }
            //    }

            //}

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
        public async Task<ConcludeRequestCombine> ConcludeRequests(string? search, int? regid, int? reqType, int? PageNumber)
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
                            where (Request.Status == 11 || Request.Status == 12) && (search == null || ((reqClient.Firstname + reqClient.Lastname).ToLower()).Contains(search.ToLower()) || ((Request.Firstname + Request.Lastname).ToLower()).Contains(search.ToLower()))
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
                                Email = reqClient.Email,
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
        public async Task<ToCloseCombine> CloseRequests(string? search, int? regid, int? reqType, int? PageNumber)
        {
            if (regid == -1)
            {
                regid = null;
            }
            IEnumerable<ToCloseRequestViewModel> ToClosequery;

            ToClosequery = from reqClient in _db.Requestclients
                           join Request in _db.Requests on reqClient.Requestid equals Request.Requestid
                           join Phy in _db.Physicians on Request.Physicianid equals Phy.Physicianid into temp
                           from phy in temp.DefaultIfEmpty()
                           join Regions in _db.Regions on reqClient.Regionid equals Regions.Regionid into temp2
                           from Reg in temp2.DefaultIfEmpty()
                           where (Request.Status == 8) & (search == null || ((reqClient.Firstname + reqClient.Lastname).ToLower()).Contains(search.ToLower()) || ((Request.Firstname + Request.Lastname).ToLower()).Contains(search.ToLower()))
                            & Request.Isdeleted != true
                            & (regid == null || reqClient.Regionid == regid)
                            & (reqType == null || Request.Requesttypeid == reqType)
                           orderby Request.Createddate
                           //4 status is Forbid pending
                           select new ToCloseRequestViewModel()
                           {
                               Firstname = reqClient.Firstname + reqClient.Lastname,
                               DateOfBirth = reqClient.Strmonth,
                               Region = Reg.Name,
                               PhysicianName = (phy != null) ? (phy.Firstname + " " + phy.Lastname) : null,
                               //Notes = reqClient.Notes,
                               DateOfService = Request.Modifieddate,
                               Address = reqClient.Street + reqClient.City + reqClient.Street + reqClient.Zipcode,
                               RequestType = Request.Requesttypeid,
                               MobileRequestor = Request.Phonenumber,
                               RequestId = Request.Requestid,
                               Email = reqClient.Email,
                           };

            var datawithnotes = ToClosequery.ToList();
            foreach (var item in datawithnotes)
            {
                var transferdata = _db.Requeststatuslogs.Where(x => x.Requestid == item.RequestId).OrderByDescending(x => x.Requeststatuslogid).Take(1).FirstOrDefault();
                if (transferdata != null)
                {
                    if (transferdata.Transtophysicianid != null || transferdata.Transtoadmin == true)
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
            ToCloseCombine model = new()
            {
                ToCloseRequestViewModels = Paginateddata,
                PageNumber = page,
                maxPage = maxPage,
            };
            return await Task.Run(() => model);
        }
        public async Task<UnpaidCombine> UnPaidRequests(string? search, int? regid, int? reqType, int? PageNumber)
        {
            if (regid == -1)
            {
                regid = null;
            }
            IEnumerable<UnPaidRequestViewModel> Unpaidquery;
            Unpaidquery = from reqClient in _db.Requestclients
                          join Request in _db.Requests on reqClient.Requestid equals Request.Requestid
                          join Phy in _db.Physicians on Request.Physicianid equals Phy.Physicianid into temp
                          from phy in temp.DefaultIfEmpty()
                          where Request.Status == 9 & (search == null || ((reqClient.Firstname + reqClient.Lastname).ToLower()).Contains(search.ToLower()) || ((Request.Firstname + Request.Lastname).ToLower()).Contains(search.ToLower()))
                            & Request.Isdeleted != true
                            & (regid == null || reqClient.Regionid == regid)
                            & (reqType == null || Request.Requesttypeid == reqType)
                          orderby Request.Createddate
                          //4 status is Forbid pending
                          select new UnPaidRequestViewModel()
                          {
                              Firstname = reqClient.Firstname + reqClient.Lastname,
                              PhysicianName = (phy != null) ? (phy.Firstname + " " + phy.Lastname) : null,
                              DateOfService = Request.Modifieddate,
                              Address = reqClient.Street + reqClient.City + reqClient.Street + reqClient.Zipcode,
                              Phonenumber = reqClient.Phonenumber,
                              RequestType = Request.Requesttypeid,
                              MobileRequestor = Request.Phonenumber,
                              RequestId = Request.Requestid,
                              Email = reqClient.Email
                          };
            int count = Unpaidquery.Count();
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
            var Paginateddata = Unpaidquery.Skip((page - 1) * 10).Take(10);
            UnpaidCombine model = new()
            {
                UnPaidRequestViewModels = Paginateddata,
                PageNumber = page,
                maxPage = maxPage,
            };
            return await Task.Run(() => model);
        }
        #endregion
        #region email   
        public async Task<bool> sendEmail(string from, string to, string subject, string body, int? adminid, int? reqid, int? phyid, List<string>? fileList)
        {

            ServicePointManager.ServerCertificateValidationCallback =
            (sender, certificate, chain, sslPolicyErrors) => true;

            MailMessage message = new MailMessage();
            //message.From = new MailAddress("tatva.dotnet.rutvikkumarbodara@outlook.com");
            message.From = new MailAddress(from);
            message.Subject = subject;
            message.To.Add(new MailAddress(to));
            message.Body = body;
            message.IsBodyHtml = true;
            Emaillog logs = new();
            //using (var smtpClient = new SmtpClient("smtp.office365.com"))
            using (var smtpClient = new SmtpClient("sandbox.smtp.mailtrap.io"))
            {
                try
                {
                    smtpClient.Port = 587;
                    //smtpClient.Credentials = new NetworkCredential("tatva.dotnet.rutvikkumarbodara@outlook.com", "Rutvik10@#97");
                    //mailtrap credentials
                    smtpClient.Credentials = new NetworkCredential("5ddcdcba9543c7", "d44ecbea64732c");
                    smtpClient.EnableSsl = true;
                    smtpClient.Timeout = (60 * 5 * 1000);
                    System.Net.Mail.Attachment attachment;
                    if (fileList != null)
                        foreach (var item in fileList)
                        {
                            var filepath = Path.Combine(Environment.CurrentDirectory, "wwwroot", "uploads", item);
                            attachment = new System.Net.Mail.Attachment(filepath);
                            message.Attachments.Add(attachment);
                        }

                    smtpClient.Send(message);

                    logs = new()
                    {
                        //not set any template right away may be in future
                        Emailtemplate = "1",
                        Subjectname = subject,
                        Emailid = to,
                        Createdate = DateTime.Now,
                        //Adminid = adminid,
                        Isemailsent = true,
                        Sentdate = DateTime.Now,
                    };

                    if (reqid != null)
                    {
                        var data = _db.Requests.FirstOrDefault(x => x.Requestid == reqid);
                        if (data != null)
                        {
                            logs.Confirmationnumber = data.Confirmationnumber;
                            if (fileList != null)
                            {
                                foreach (var item in fileList)
                                {
                                    var filepath = Path.Combine(Environment.CurrentDirectory, "wwwroot", "uploads", item);
                                    logs.Filepath += filepath + ",";
                                }
                            }
                            //specify patient roleid
                        }
                        logs.Requestid = reqid;
                    }
                    if (phyid != null)
                    {
                        var data = _db.Physicians.FirstOrDefault(x => x.Physicianid == phyid);
                        if (data != null)
                        {
                            logs.Roleid = data.Roleid;
                        }
                        logs.Physicianid = phyid;
                    }
                    //in future admin will added when provider created
                }
                catch (Exception e)
                {
                    logs.Isemailsent = false;
                    logs.Senttries += 1;
                    Console.WriteLine(e);
                }

                _db.Emaillogs.Add(logs);
                _db.SaveChanges();
                //add entry in email logs

                return await Task.Run(() => true);
            }

        }
        public async Task<bool> UrgentEmailSent(int reqid)
        {

            var data = await _db.Requests.FirstOrDefaultAsync(x => x.Requestid == reqid);

            if (data == null)
            {
                return false;
            }

            if (data.Isurgentemailsent)
            {
                return true;
            }
            return false;

        }
        public async Task<bool> MarkUrgentEmailSent(int reqid)
        {
            var data = await _db.Requests.FirstOrDefaultAsync(x => x.Requestid == reqid);

            if (data == null)
            {
                return false;
            }

            data.Isurgentemailsent = true;
            _db.Requests.Update(data);
            _db.SaveChanges();
            return true;
        }
        public async Task<bool> SentEmailWithFile(List<string> fileList, string email, string reqid)
        {
            //send email from here 

            string from = "hallodocpms@gmail.com";
            string to = email;
            string subject = "requested Documents from HalloDoc";
            string body = "Hey User, get your files below,";
            int requestid = int.Parse(reqid);
            bool status = await sendEmail(from, to, subject, body, null, requestid, null, fileList);
            return status;
        }

        #endregion
        #region DashBoardActions
        public async Task<IList<Physician>> GetPhysicianByRegion(int regionId)
        {
            var phys = (from phy in _db.Physicians
                        where phy.Regionid == regionId
                        select new Physician()
                        {
                            Physicianid = phy.Physicianid,
                            Firstname = phy.Firstname,
                        }).ToList();

            return await Task.Run(() => phys);
        }
        public async Task AssignPhysician(int RegionSelect, int PhysicianSelect, string? CreatorDescription, int reqid, int CreatorId, string actionName, int assignedBy)
        {
            //update request
            var RequestTableData = await _db.Requests.Where(x => x.Requestid == reqid).FirstOrDefaultAsync();
            //put in pendig state for as of now 
            if (RequestTableData != null)
            {
                RequestTableData.Status = 21;
                RequestTableData.Physicianid = PhysicianSelect;
                RequestTableData.Modifieddate = DateTime.Now;
                _db.Requests.Update(RequestTableData);
                _db.SaveChanges();
            }
            //change admin notes
            int ReqNoteId = _db.Requestnotes.Count();

            //check is there a entry  in the requestNotes
            var checkReq = _db.Requestnotes.SingleOrDefault(x => x.Requestid == reqid);

            var requestNotesAdd = new Requestnote()
            {
                Requestnotesid = ReqNoteId + 1,
                Requestid = reqid,
                //Adminnotes = AdminDescription,
                Createddate = DateTime.Now,
            };
            if (assignedBy == 2)
            {
                requestNotesAdd.Physiciannotes = CreatorDescription;
                var phydata = _db.Physicians.FirstOrDefault(x => x.Physicianid == CreatorId);
                if (phydata == null)
                {
                    return;
                }
                if (phydata.Aspnetuserid == null)
                {
                    return;
                }
                requestNotesAdd.Createdby = phydata.Aspnetuserid;
            }
            else
            {
                requestNotesAdd.Adminnotes = CreatorDescription;
                requestNotesAdd.Createdby = CreatorId.ToString();
            }

            if (checkReq == null)
            {
                _db.Requestnotes.Add(requestNotesAdd);
                _db.SaveChanges();
            }
            else
            {

                //physician
                if (assignedBy == 2)
                {
                    checkReq.Physiciannotes = CreatorDescription;
                    var phydata = _db.Physicians.FirstOrDefault(x => x.Physicianid == CreatorId);
                    if (phydata == null)
                    {
                        return;
                    }
                    checkReq.Modifiedby = phydata.Aspnetuserid;
                }
                else
                {
                    checkReq.Adminnotes = CreatorDescription;
                    //aspid for admin
                    checkReq.Modifiedby = CreatorId.ToString();
                }
                checkReq.Modifieddate = DateTime.Now;
                _db.Requestnotes.Update(checkReq);
                _db.SaveChanges();
            }
            Requeststatuslog requestStatusLogs;

            if (actionName == "TransferPhysician")
            {
                //logs update assign
                requestStatusLogs = new Requeststatuslog()
                {
                    Requestid = reqid,
                    //Adminid = adminId,
                    Status = 21,
                    Notes = CreatorDescription,
                    Transtophysicianid = PhysicianSelect,
                    Createddate = DateTime.Now,
                };
            }
            else
            {
                //logs update transfer
                requestStatusLogs = new Requeststatuslog()
                {
                    Requestid = reqid,
                    Status = 21,
                    Notes = CreatorDescription,
                    Physicianid = PhysicianSelect,
                    Createddate = DateTime.Now,
                };
            }
            if (assignedBy == 2)
            {
                requestStatusLogs.Physicianid = CreatorId;
            }
            else
            {
                requestStatusLogs.Adminid = CreatorId;
            }
            _db.Requeststatuslogs.Add(requestStatusLogs);
            _db.SaveChanges();
        }
        public async Task<ViewCasesViewModel> ViewNewCases(int requestid)
        {
            ViewCasesViewModel patientDetails = new ViewCasesViewModel();
            try
            {

                var userDetails = await _db.Requestclients.FirstOrDefaultAsync(x => x.Requestid == requestid);
                var requestdata = await _db.Requests.FirstOrDefaultAsync(x => x.Requestid == requestid);
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

                if (userDetails != null && requestdata != null)
                {
                    patientDetails = new ViewCasesViewModel()
                    {
                        Notes = userDetails.Notes,
                        FirstName = userDetails.Firstname,
                        LastName = (userDetails.Lastname != null) ? userDetails.Lastname : "",
                        Date = userDetails.Strmonth,
                        PhoneNumber = userDetails.Phonenumber,
                        Email = userDetails.Email,
                        Region = userDetails.State,
                        Business = userDetails.Street + " " + userDetails.Street + " " + userDetails.Zipcode,
                        RequestId = requestid,
                        ConfirmationNumber = requestdata.Confirmationnumber,
                        Regions = region,
                        physicians = phys,
                        casetags = castags,
                    };
                }
                return await Task.Run(() => patientDetails);
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return await Task.Run(() => patientDetails);
        }
        public async Task<ViewNotesViewModel> ShowViewNotes(int requestid)
        {

            ViewNotesViewModel model = new ViewNotesViewModel();

            try
            {
                var ReqNotes = _db.Requestnotes.Where(x => x.Requestid == requestid).FirstOrDefault();
                var statusCheck = _db.Requests.Where(x => x.Requestid == requestid).SingleOrDefault();

                var CaseTags = _db.Casetags.FirstOrDefault(x =>statusCheck.Casetag == null || x.Casetagid == int.Parse(statusCheck.Casetag));
                var reqStatusLog = from reqstat in _db.Requeststatuslogs
                                   where reqstat.Requestid == requestid
                                   select new ReqStatusLogViewModel
                                   {
                                       Status = reqstat.Status,
                                       PhysicianId = reqstat.Physicianid,
                                       AdminId = reqstat.Adminid,
                                       TransPhyId = reqstat.Transtophysicianid,
                                       Notes = reqstat.Notes,
                                       IsTransferBackToAdmin = reqstat.Transtoadmin,
                                       CreatedDate = reqstat.Createddate,
                                   };
                if (statusCheck != null && reqStatusLog != null)
                {

                    model = new ViewNotesViewModel
                    {
                        RequestId = requestid,
                        PhysicianNotes = ReqNotes?.Physiciannotes,
                        AdminNotes = ReqNotes?.Adminnotes,
                        ReqstatusLog = (reqStatusLog == null) ? null : reqStatusLog,
                        status = statusCheck.Status,
                        casetag = CaseTags.Name
                    };
                    return await Task.Run(() => model);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return model;
        }
        public async Task AddAdminNotes(string adminnots, int requestid, int adminId)
        {

            int ReqNoteId = _db.Requestnotes.Count();
            //check is there a entry  in the requestNotes
            var checkReq = _db.Requestnotes.SingleOrDefault(x => x.Requestid == requestid);
            var requestNotesAdd = new Requestnote()
            {
                Requestnotesid = ReqNoteId + 1,
                Requestid = (int)requestid,
                Adminnotes = adminnots,
                Createdby = "Admin",
                Createddate = DateTime.Now,
            };

            //logs
            var requestStatusLogs = new Requeststatuslog()
            {
                Requestid = requestid,
                Adminid = adminId,
                Status = 1,
                Notes = adminnots,
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
                checkReq.Adminnotes = adminnots;
                checkReq.Modifieddate = DateTime.Now;
                checkReq.Modifiedby = "admin";
                _db.Requestnotes.Update(checkReq);
                await _db.SaveChangesAsync();
            }
        }
        public async Task<bool> ViewNewCasespost(ViewCasesViewModel obj, int reqid)
        {

            var userDetails = _db.Requestclients.Where(x => x.Requestid == reqid).FirstOrDefault();
            if (userDetails != null)
            {
                {
                    //userDetails.Notes = obj.Notes;
                    //userDetails.Firstname = obj.FirstName;
                    //userDetails.Lastname = obj.LastName;
                    //userDetails.Strmonth = obj.Date;
                    userDetails.Phonenumber = obj.PatientCountryCode + obj.PhoneNumber;
                    userDetails.Email = obj.Email;
                    //userDetails.State = obj.Region;
                };
                _db.Requestclients.Update(userDetails);
                await _db.SaveChangesAsync();
                return true;
            }
            return false;
        }
        public async Task<IEnumerable<PatientRequestWiseDocument>> View_Document(int id)
        {

            var clientData = await _db.Requestclients.SingleOrDefaultAsync(x => x.Requestid == id);

            var RequestWiseDoc = from reqfiles in _db.Requestwisefiles
                                 join req in _db.Requests on reqfiles.Requestid equals req.Requestid into temp
                                 from Req in temp.DefaultIfEmpty()
                                 join reqclient in _db.Requestclients on Req.Requestid equals reqclient.Requestid into temp2
                                 from ReqClient in temp2.DefaultIfEmpty()
                                 where reqfiles.Requestid == id & reqfiles.Isdeleted != true
                                 select new PatientRequestWiseDocument
                                 {
                                     document = reqfiles.Filename,
                                     uploaddate = reqfiles.Createddate,
                                     Email = ReqClient.Email
                                 };

            return await Task.Run(() => RequestWiseDoc);

        }
        public async Task CancelFormData(CancelRequestViewModel obj)
        {
            //change in request table
            var requestdata = await _db.Requests.Where(x => x.Requestid == obj.Requestid).FirstOrDefaultAsync();

            if (requestdata != null)
            {
                requestdata.Status = 8;
                //update the data case tag for what reason he cancel
                requestdata.Casetag = obj.CancelReason;
                _db.Requests.Update(requestdata);
                await _db.SaveChangesAsync();
            }
            //add entry in logs 
            var requestStatusLog = new Requeststatuslog()
            {
                Requestid = obj.Requestid,
                Status = 8,
                Adminid = obj.AdminId,
                Notes = obj.CancelReason + " " + obj.AdditionalNotes,
                Createddate = DateTime.Now,
            };

            _db.Requeststatuslogs.Add(requestStatusLog);
            await _db.SaveChangesAsync();

            // request closed Table

            var closedReq = new Requestclosed()
            {
                Requestid = obj.Requestid,
                Requeststatuslogid = requestStatusLog.Requeststatuslogid,
            };
            _db.Requestcloseds.Add(closedReq);
            await _db.SaveChangesAsync(true);

        }
        public async Task<bool> ClearCase(int reqid)
        {
            //soft delete
            var requestData = await _db.Requests.FirstOrDefaultAsync(x => x.Requestid == reqid);

            if (requestData != null)
            {
                requestData.Status = 3;
                requestData.Modifieddate = DateTime.Now;
                _db.Requests.Update(requestData);
                _db.SaveChanges();
                return true;
            }
            return false;
        }
        public async Task BlockRequest(int reqid, string BlockNotes, int adminId)
        {
            var reqdata = await _db.Requests.SingleOrDefaultAsync(x => x.Requestid == reqid);

            var logs = new Requeststatuslog()
            {
                Requestid = reqid,
                Status = 16,
                Createddate = DateTime.Now,
                Adminid = adminId,
                Notes = BlockNotes
            };
            _db.Requeststatuslogs.Add(logs);
            await _db.SaveChangesAsync();

            if (reqdata != null)
            {
                //reqdata.Requestid = reqid;
                reqdata.Status = 16;
                _db.Requests.Update(reqdata);
                await _db.SaveChangesAsync();
                Blockrequest blockrequest = new Blockrequest()
                {
                    Phonenumber = reqdata.Phonenumber,
                    Email = reqdata.Email,
                    Reason = BlockNotes,
                    Isactive = true,
                    ReqId = reqid,
                    Createddate = DateTime.Now
                };
                _db.Blockrequests.Add(blockrequest);
                _db.SaveChanges();
            }
        }
        public async Task<bool> DeleteSeperateFile(string seperatefile)
        {
            var requestFile = await _db.Requestwisefiles.SingleOrDefaultAsync(x => x.Filename == seperatefile);
            if (requestFile != null)
            {
                requestFile.Isdeleted = true;
                _db.Requestwisefiles.Update(requestFile);
                await _db.SaveChangesAsync();
                return true;
            }
            return false;
        }
        public async Task<bool> OrderAction(OrderActionViewModel obj, int reqid, string CreatedBy)
        {
            Orderdetail details = new();

            if (obj != null)
            {
                details.Requestid = reqid;
                details.Vendorid = obj.BusinessName;
                details.Businesscontact = obj.BusinessContact;
                details.Email = obj.BusinessEmail;
                details.Faxnumber = obj.Faxnumber;
                details.Prescription = obj.Pres;
                details.Noofrefill = obj.refill;
                details.Createddate = DateTime.Now;
                details.Createdby = CreatedBy;
                _db.Orderdetails.Add(details);
                await _db.SaveChangesAsync();
                return await Task.Run(() => true);
            }
            return await Task.Run(() => false);
        }
        public async Task<bool> IsDeleted(int reqId)
        {
            return await Task.Run(() => _db.Requests.Any(x => x.Requestid == reqId && x.Isdeleted == true));
        }
        public async Task<Requestclient?> ClientData(int id)
        {
            return await _db.Requestclients.SingleOrDefaultAsync(x => x.Requestid == id);
        }
        public async Task<bool> EditUserData(string firstname, string lastname, string DateOfBirth, string callNumber, string Email, string reqid, string PatientCountryCode)
        {
            var data = await _db.Requestclients.FirstOrDefaultAsync(x => x.Requestid == int.Parse(reqid));

            if (data != null)
            {
                data.Firstname = firstname;
                data.Lastname = lastname;
                data.Strmonth = DateOfBirth;
                data.Email = Email;
                data.Phonenumber = PatientCountryCode + callNumber;
                _db.Requestclients.Update(data);
                _db.SaveChanges();
                return true;
            }
            return false;
        }
        public async Task<bool> CloseCasePermennt(string reqid)
        {
            var data = await _db.Requests.SingleOrDefaultAsync(x => x.Requestid == int.Parse(reqid));
            if (data != null)
            {
                data.Status = 9;
                _db.Requests.Update(data);
                _db.SaveChanges();
                return true;
            }
            return false;
        }
        public async Task<bool> EncounterFormSubmit(Encounterformdetail obj, string reqid, int adminid)
        {
            var modelddata = await _db.Encounterformdetails.FirstOrDefaultAsync(x => x.ReqId == int.Parse(reqid));
            var clientData = await _db.Requestclients.FirstOrDefaultAsync(x => x.Requestid == int.Parse(reqid));

            if (clientData != null && obj.Reqestclient != null)
            {
                clientData.Firstname = obj.Reqestclient.Firstname;
                clientData.Lastname = obj.Reqestclient.Lastname;
                clientData.Location = obj.Reqestclient.Location;
                clientData.Strmonth = obj.Reqestclient.Strmonth;
                clientData.Phonenumber = obj.Reqestclient.Phonenumber;
                clientData.Email = obj.Reqestclient.Email;
                _db.Requestclients.Update(clientData);
                _db.SaveChanges();
            }

            var tempdata = modelddata;
            if (modelddata != null)
            {
                modelddata.Injuryhistory = obj.Injuryhistory;
                modelddata.Medicalhistory = obj.Medicalhistory;
                modelddata.Medications = obj.Medications;
                modelddata.Allergies = obj.Allergies;
                modelddata.Temp = obj.Temp;
                modelddata.Hr = obj.Hr;
                modelddata.Rr = obj.Rr;
                modelddata.BpS = obj.BpS;
                modelddata.BpD = obj.BpD;
                modelddata.O2 = obj.O2;
                modelddata.Pain = obj.Pain;
                modelddata.Heent = obj.Heent;
                modelddata.Cv = obj.Cv;
                modelddata.Chest = obj.Chest;
                modelddata.Abd = obj.Abd;
                modelddata.Extr = obj.Extr;
                modelddata.Skin = obj.Skin;
                modelddata.Neuro = obj.Neuro;
                modelddata.Other = obj.Other;
                modelddata.Disgnosis = obj.Disgnosis;
                modelddata.Treatmentplan = obj.Treatmentplan;
                modelddata.Medicationdispensed = obj.Medicationdispensed;
                modelddata.Procedure = obj.Procedure;
                modelddata.Followup = obj.Followup;
                _db.Encounterformdetails.Update(modelddata);
                await _db.SaveChangesAsync();
                return true;
            }
            return false;
        }
        public async Task<Encounterformdetail?> EncounterForm(int reqid, int? adminid)
        {
            bool check = _db.Encounterformdetails.Any(x => x.ReqId == reqid);
            var count = _db.Encounterformdetails.Count();
            if (!check)
            {
                var requestTable = await _db.Requests.FirstOrDefaultAsync(x => x.Requestid == reqid);
                var reqclienttable = await _db.Requestclients.FirstOrDefaultAsync(x => x.Requestid == reqid);
                Encounterformdetail model = new Encounterformdetail()
                {
                    Encounterformdetailsid = count + 1,
                    Reqestclientid = (reqclienttable != null) ? reqclienttable.Requestclientid : null,
                    Adminid = adminid,
                    Physicianid = (requestTable != null) ? requestTable.Physicianid : null,
                    ReqId = reqid,
                };
                _db.Encounterformdetails.Add(model);
                await _db.SaveChangesAsync();
            }

            var requestmodel = (from obj in _db.Encounterformdetails
                                join x2 in _db.Requestclients
                                on obj.ReqId equals x2.Requestid
                                where obj.ReqId == reqid
                                select new Encounterformdetail
                                {
                                    ReqId = obj.ReqId,
                                    Reqestclient = x2,
                                    Injuryhistory = obj.Injuryhistory,
                                    Medicalhistory = obj.Medicalhistory,
                                    Medications = obj.Medications,
                                    Allergies = obj.Allergies,
                                    Temp = obj.Temp,
                                    Hr = obj.Hr,
                                    Rr = obj.Rr,
                                    BpS = obj.BpS,
                                    BpD = obj.BpD,
                                    O2 = obj.O2,
                                    Pain = obj.Pain,
                                    Heent = obj.Heent,
                                    Cv = obj.Cv,
                                    Chest = obj.Chest,
                                    Abd = obj.Abd,
                                    Extr = obj.Extr,
                                    Skin = obj.Skin,
                                    Neuro = obj.Neuro,
                                    Other = obj.Other,
                                    Disgnosis = obj.Disgnosis,
                                    Treatmentplan = obj.Treatmentplan,
                                    Medicationdispensed = obj.Medicationdispensed,
                                    Procedure = obj.Procedure,
                                    Followup = obj.Followup,

                                }).FirstOrDefault();
            return requestmodel;

        }
        public async Task<int> sendDtyMessage(int adminid, string message)
        {
            //send email to those physicians whose notifications are not stopped or not have any schedule for the day
            var date = DateTime.Now;
            var admindata = await _db.Admins.FirstOrDefaultAsync(x => x.Adminid == adminid);
            var curYear = DateTime.Now.ToString("yyyy");
            var curMonth = DateTime.Now.ToString("MM");
            var curDay = DateTime.Now.ToString("dd");

            var curdate = new DateOnly(int.Parse(curYear), int.Parse(curMonth), int.Parse(curDay));

            //physician with approved shift
            var schedulePhy = (from phy in _db.Physicians
                               join x1 in _db.Physiciannotifications on phy.Physicianid equals x1.Physicianid into temp1
                               from X1 in temp1.DefaultIfEmpty()
                               join x2 in _db.Shifts on phy.Physicianid equals x2.Physicianid into temp2
                               from X2 in temp2.DefaultIfEmpty()
                               join x3 in _db.Shiftdetails on X2.Shiftid equals x3.Shiftid into temp3
                               from X3 in temp3.DefaultIfEmpty()
                               where X1.Isnotificationstopped != true
                               && phy.Createdby == admindata.Aspnetuserid
                               && X3.Shiftdate == curdate
                               && X3.Status == 1
                               group phy by new { phy.Email, phy.Physicianid, phy.Firstname, phy.Lastname } into g
                               select new Physician()
                               {
                                   Physicianid = g.Key.Physicianid,
                               });
            int MailBounce = 0;
            ////FOR STOP NOTIFICATION CASE
            //if (schedulePhy.Count() == 0)
            //{
            //    return 999;
            //}
            //list of the scheduled phy

            //get all physicians which not in the 
            var phydata = from phy in _db.Physicians
                          join Notify in _db.Physiciannotifications on phy.PhyNotificationid equals Notify.Id
                          where Notify.Isnotificationstopped != true
                          select phy;

            var totalPhysicians = _db.Physicians.Count();
            //alll notification is stopped
            if (phydata.Count() == 0)
            {
                return 999;
            }

            //int[] list = null;
            int[] list = { };
            if (schedulePhy.Count() != 0)
            {

                list = new int[schedulePhy.Count()];
                var i = 0;
                foreach (var item in schedulePhy)
                {
                    list[i] = (item.Physicianid);
                    i++;
                }
            }
            var PHYlIST = phydata.ToList();
            foreach (var item in PHYlIST)
            {
                if (list != null)
                {
                    bool exist = Array.Exists(list, x => x == item.Physicianid);
                    if (exist)
                    {
                        //doctors are on call so message won't  be send
                        continue;
                    }
                }
                //send email
                var from = "hallodocpms@gmail.com";
                var to = item.Email;
                var subject = "URGENT! We are short on coverage and needs additional support On Call to respond to Requests";
                var body = message;
                bool status = await sendEmail(from, to, subject, body, adminid, null, item.Physicianid, null);
                if (!status)
                {
                    MailBounce++;
                }

            }
            return MailBounce;
        }
        public async Task<IEnumerable<Region>> loadRegion()
        {
            return await Task.Run(() => _db.Regions);
        }
        public async Task<bool> ResetPassword(string password, string Id, int RequestType)
        {
            //admin
            if (RequestType == 1)
            {
                var aspdata = await _db.Aspnetusers.FirstOrDefaultAsync(x => x.Id == Id.ToString());
                if (aspdata != null)
                {
                    var HASHPASS = BCrypt.Net.BCrypt.HashPassword(password);
                    aspdata.Passwordhash = HASHPASS;
                    _db.Aspnetusers.Update(aspdata);
                    _db.SaveChanges();
                    return true;
                }
                return false;
            }
            //physicians
            else
            {

                var aspdata = await _db.Aspnetusers.FirstOrDefaultAsync(x => x.Id == Id);
                if (aspdata != null)
                {
                    var HASHPASS = BCrypt.Net.BCrypt.HashPassword(password);
                    aspdata.Passwordhash = HASHPASS;
                    _db.Aspnetusers.Update(aspdata);
                    _db.SaveChanges();
                    return true;
                }
                return false;
            }

        }

        #endregion
        #region ProviderAccount
        public async Task<bool> SendSMS(string Phone, int phyid)
        {
            var phydata = await _db.Physicians.FirstOrDefaultAsync(x => x.Physicianid == phyid);
            if (phydata != null)
            {
                var smsLogs = new Smslog();
                smsLogs.Physicianid = phyid;
                smsLogs.Roleid = phydata.Roleid;
                smsLogs.Smstemplate = "1";
                smsLogs.Mobilenumber = Phone;
                smsLogs.Createdate = DateTime.Now;
                smsLogs.Senttries += 1;
                smsLogs.Sentdate = DateTime.Now;
                smsLogs.Issmssent = true;
                smsLogs.Physicianid = phyid;
                _db.Smslogs.Add(smsLogs);
                _db.SaveChanges();
                return true;
            }
            return false;
        }
        public async Task<ProviderInfoViewModel> ProviderPage(int? regionid, int? PageNumber)
        {
            //var curYear = DateTime.Now.ToString("yyyy");
            //var curMonth = DateTime.Now.ToString("MM");
            //var curDay = DateTime.Now.ToString("dd");
            ////var Curtime = DateTime.Now.ToString("mm:HH:ss");
            //var curHour = DateTime.Now.ToString("HH");
            //var curmin = DateTime.Now.ToString("mm");

            DateOnly curDate = DateOnly.FromDateTime(DateTime.Now);
            TimeOnly CurTime = TimeOnly.FromDateTime(DateTime.Now);
            //var curtime = new TimeOnly(int.Parse(curHour), int.Parse(curmin), 0);

            var CheckShiftExit = from x1 in _db.Shifts
                                 join x2 in _db.Shiftdetails on x1.Shiftid equals x2.Shiftid
                                 where x2.Shiftdate == curDate && x2.Starttime <= CurTime && CurTime <= x2.Endtime 
                                 && x2.Isdeleted != true 
                                 select x1;

            var Physiciandata = (from x1 in _db.Physicians
                                 where ((x1.Isdeleted != true) && (regionid == null || x1.Regionid == regionid))
                                 join x2 in _db.Regions on x1.Regionid equals x2.Regionid into temp
                                 from reg in temp.DefaultIfEmpty()
                                 join x3 in _db.Statuses on x1.Status equals x3.StatusId into temp2
                                 from states in temp2.DefaultIfEmpty()
                                 join x4 in _db.Roles on x1.Roleid equals x4.Roleid into temp3
                                 from roles in temp3.DefaultIfEmpty()
                                 join x5 in _db.Physiciannotifications on x1.Physicianid equals x5.Physicianid into temp4
                                 from notification in temp4.DefaultIfEmpty()
                                 //join shifts in _db.Shifts on x1.Physicianid equals shifts.Physicianid into temp6
                                 //from x6 in temp6.DefaultIfEmpty()
                                 //join Shiftdetail in _db.Shiftdetails on x6.Shiftid equals Shiftdetail.Shiftid into temp7
                                 //from x7 in temp7.DefaultIfEmpty()
                                 //where (x6 == null || (x7 == null || ((x7.Shiftdate == curDate) && x7.Starttime <= CurTime && x7.Endtime >= CurTime)))
                                 ////&& x7.Starttime <= CurTime && x7.Endtime >= CurTime))
                                 orderby x1.Physicianid
                                 //group x1 by new {x1.Physicianid, x1.Firstname , x1.Lastname} into G

                                 select new SubProviderViewModel
                                 {
                                     PhysicianId=x1.Physicianid,
                                     FirstName =x1.Firstname,
                                     LastName=x1.Lastname,
                                     Email=x1.Email,
                                     IsNotificationStopped=notification.Isnotificationstopped,
                                     Mobile=x1.Mobile,
                                     StatusName=states.Statusname,
                                     RoleName=roles.Name,
                                     ProviderOnCall=(CheckShiftExit.FirstOrDefault(x=>x.Physicianid == x1.Physicianid) != null)?"On Call":"Off Duty"


                                     //physician = x1,
                                     //status = states,
                                     //role = roles,
                                     //Physiciannotification = notification,
                                     //ProviderOnCall = (x7 != null) ? "On Call" : "Off Duty"
                                     //physician.Firstname = x1.Firstname,
                                     //physician.Mobile = x1.Mobile,
                                     //physician.Email = x1.Email,
                                     //physician.Physicianid = x1.Physicianid,
                                     //physician.Aspnetuserid = x1.Aspnetuserid,
                                     //physician.Lastname = x1.Lastname,
                                     //physician.StatusNavigation = states,
                                     //Role = roles,
                                     //PhyNotification = notification
                                 });


            int count = Physiciandata.Count();
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
                        PageNumber = count / 5 + 1;
                    else
                    {
                        PageNumber = 1;
                    }
                }
                page = (int)PageNumber;
            }
            var Paginateddata = Physiciandata.Skip((page - 1) * 5).Take(5);

            ProviderInfoViewModel model = new();
            model.physicians = Paginateddata;
            model.maxPage = maxPage;
            model.PageNumber = page;
            return await Task.Run(() => model);
        }
        public async Task<bool> StopNotify(int phyid, bool check)
        {
            var phynotify = await _db.Physiciannotifications.FirstOrDefaultAsync(x => x.Physicianid == phyid);
            int count = _db.Physicianlocations.Count();
            if (phynotify == null)
            {
                Physiciannotification notify = new();

                notify.Physicianid = phyid;
                notify.Isnotificationstopped = check;
                _db.Physiciannotifications.Add(notify);
                _db.SaveChanges();
                return true;
            }
            else
            {
                phynotify.Isnotificationstopped = check;
                _db.Physiciannotifications.Update(phynotify);
                _db.SaveChanges(true);
                return true;
            }
        }
        public async Task<ProviderProfileViewModel> EditPhysicianProfile(int phyid)
        {
            var model = await (from req in _db.Physicians
                               join Roles in _db.Roles on req.Roleid equals Roles.Roleid into temp
                               from roles in temp.DefaultIfEmpty()
                               join Status in _db.Statuses on req.Status equals Status.StatusId into temp2
                               from status in temp2.DefaultIfEmpty()
                               join aspnet in _db.Aspnetusers on req.Aspnetuserid equals aspnet.Id into temp3
                               from asp in temp3.DefaultIfEmpty()
                               join Regions in _db.Regions on req.Regionid equals Regions.Regionid into temp4
                               from regions in temp4.DefaultIfEmpty()
                                   //join doc in _db.ProviderDocuments on req.Physicianid equals doc.PhysicianId into temp5   
                                   //from docs in temp5.DefaultIfEmpty()
                               where req.Physicianid == phyid && req.Isdeleted != true
                               select new Physician()
                               {
                                   Address1 = req.Address1,
                                   Address2 = req.Address2,
                                   Firstname = req.Firstname,
                                   Lastname = req.Lastname,
                                   Email = req.Email,
                                   Mobile = req.Mobile,
                                   City = req.City,
                                   Region = regions,
                                   Regionid = req.Regionid,
                                   Zip = req.Zip,
                                   Altphone = req.Altphone,
                                   Role = roles,
                                   StatusNavigation = status,
                                   Aspnetuser = asp,
                                   Medicallicense = req.Medicallicense,
                                   Npinumber = req.Npinumber,
                                   Syncemailaddress = req.Syncemailaddress,
                                   Businessname = req.Businessname,
                                   Businesswebsite = req.Businesswebsite,
                                   Adminnotes = req.Adminnotes,
                                   Signature = req.Signature,
                                   Physicianid = req.Physicianid,
                                   Isnondisclosuredoc = req.Isnondisclosuredoc,
                                   Isagreementdoc = req.Isagreementdoc,
                                   Iscredentialdoc = req.Iscredentialdoc,
                                   Isbackgrounddoc = req.Isbackgrounddoc,
                                   Islicensedoc = req.Islicensedoc,
                                   Istrainingdoc = req.Istrainingdoc,

                               }).FirstOrDefaultAsync();
            //region
            var PhysicianSelectedRegion = from x1 in _db.Physicianregions
                                          where x1.Physicianid == model.Physicianid
                                          join x2 in _db.Regions on x1.Regionid equals x2.Regionid
                                          select x2;
            //non selected
            var NonSelectedRegion = _db.Regions.Except(PhysicianSelectedRegion);
            //var data = model;

            CreateProviderForm providerform = new()
            {
                physician = model,
            };

            ProviderProfileViewModel CombineModel = new()
            {
                ProviderFormData = providerform
            };
            CombineModel.selectedRegion = PhysicianSelectedRegion;
            CombineModel.nonSelectedRegion = NonSelectedRegion;
            return await Task.Run(() => CombineModel);
        }
        public async Task<bool> ProviderAspFormSubmit(ProviderProfileViewModel obj, int adminid, int phyid, IFormFile? filePhoto, IFormFile? fileSign, string? submitfor)
        {
            var phydata = await _db.Physicians.FirstOrDefaultAsync(x => x.Physicianid == phyid);
            //provider profile 
            if (submitfor == "ProviderProfileForm")
            {
                if (phydata != null)
                {
                    phydata.Businessname = obj.ProviderFormData.physician.Businessname;
                    phydata.Businesswebsite = obj.ProviderFormData.physician.Businesswebsite;
                    phydata.Adminnotes = obj.ProviderFormData.physician.Adminnotes;
                    //photo provider
                    if (filePhoto != null && filePhoto.Length > 0)
                    {
                        Guid myuuid = Guid.NewGuid();
                        var filename = Path.GetFileName(filePhoto.FileName);
                        var FinalFileName = myuuid.ToString() + filename;

                        //path
                        var filepath = Path.Combine(Environment.CurrentDirectory, "wwwroot", "uploads", "ProviderData", FinalFileName);

                        //copy in stream

                        using (var str = new FileStream(filepath, FileMode.Create))
                        {
                            //copy file
                            filePhoto.CopyTo(str);
                        }

                        //STORE DATA IN TABLE
                        phydata.Photo = FinalFileName;
                    }

                    if (fileSign != null && fileSign.Length > 0)
                    {
                        Guid myuuid = Guid.NewGuid();
                        var filename = Path.GetFileName(fileSign.FileName);
                        var FinalFileName = myuuid.ToString() + filename;

                        //path

                        var filepath = Path.Combine(Environment.CurrentDirectory, "wwwroot", "uploads", "ProviderData", FinalFileName);

                        //copy in stream

                        using (var str = new FileStream(filepath, FileMode.Create))
                        {
                            //copy file
                            fileSign.CopyTo(str);
                        }

                        //STORE DATA IN TABLE
                        phydata.Signature = FinalFileName;
                    }
                    phydata.Modifieddate = DateTime.Now;
                    //update with current admin
                    phydata.Modifiedby = "01";
                    _db.Physicians.Update(phydata);
                    await _db.SaveChangesAsync();
                    return true;
                }
                return false;
            }

            if (obj.ProviderFormData.physician.Firstname == null)
            {
                if (phydata != null)
                {
                    phydata.Address1 = obj.ProviderFormData.physician.Address1;
                    phydata.Address2 = obj.ProviderFormData.physician.Address2;
                    phydata.City = obj.ProviderFormData.physician.City;
                    phydata.Regionid = obj.ProviderFormData.physician.Regionid;
                    phydata.Zip = obj.ProviderFormData.physician.Zip;
                    phydata.Altphone ="+"+obj.CountryCode.Split("+")[2] + obj.ProviderFormData.physician.Altphone;
                    phydata.Modifieddate = DateTime.Now;
                    phydata.Modifiedby = "01";
                    //phydata.Lattitude = obj.ProviderFormData.physician.Lattitude;
                    //phydata.Longtitude = obj.ProviderFormData.physician.Longtitude;
                    //set lattitude
                    //var locationService = new GoogleLocationService();
                    //var point = locationService.GetLatLongFromAddress(phydata.Address1);
                    //phydata.Lattitude = point.Latitude;

                    _db.Physicians.Update(phydata);
                    _db.SaveChanges();
                    return true;
                }
                return false;
            }
            //for the asps
            if (phydata != null)
            {
                phydata.Firstname = obj.ProviderFormData.physician.Firstname;
                phydata.Lastname = obj.ProviderFormData.physician.Lastname;
                phydata.Email = obj.ProviderFormData.physician.Email;
                phydata.Mobile = "+" + obj.CountryCode.Split("+")[1]+ obj.ProviderFormData.physician.Mobile;
                phydata.Status = obj.ProviderFormData.physician.Status;
                phydata.Roleid = obj.ProviderFormData.physician.Roleid;
                _db.Physicians.Update(phydata);
                await _db.SaveChangesAsync();


                var aspdata = await _db.Aspnetusers.FirstOrDefaultAsync(x => x.Id == phydata.Aspnetuserid);

                if (aspdata != null)
                {
                    //user name cant be updated
                    //aspdata.Username = obj.ProviderFormData.physician.Aspnetuser.Username;
                    aspdata.Modifieddate = DateTime.Now;
                    _db.Aspnetusers.Update(aspdata);
                    await _db.SaveChangesAsync();
                }
            }

            //update regions

            var allRegionList = _db.Physicianregions;
            int count = 0;
            if (allRegionList.Count() != 0)
            {
                count = allRegionList.Max(x => x.Physicianregionid);
            }
            if (obj.RegionList == null)
            {
                foreach (var item in allRegionList)
                {
                    allRegionList.Remove(item);
                }
                _db.SaveChanges();
            }
            else
            {
                var SelectedRegionList = obj.RegionList.ToList();

                var PhysicianRegiontable = _db.Physicianregions.Where(x => x.Physicianid == phyid);
                foreach (var item in PhysicianRegiontable)
                {
                    var val = "";
                    foreach (var item2 in SelectedRegionList)
                    {
                        if (item.Regionid == item2)
                        {
                            val = item.ToString();
                            break;
                        }
                    }
                    if (val == "")
                    {
                        _db.Physicianregions.Remove(item);
                        SelectedRegionList.Remove((int)item.Regionid);
                    }
                    else
                    {
                        SelectedRegionList.Remove((int)item.Regionid);
                        val = "";
                    }
                }
                _db.SaveChanges();
                //Guid uuid = Guid.NewGuid();
                foreach (var item in SelectedRegionList)
                {

                    var newRegions = new Physicianregion();
                    newRegions.Regionid = item;
                    newRegions.Physicianregionid = ++count;
                    newRegions.Physicianid = phydata.Physicianid;
                    _db.Physicianregions.Add(newRegions);
                    await _db.SaveChangesAsync();
                }
            }

            var user = await _db.Users.FirstOrDefaultAsync(x => x.Aspnetuserid == phydata.Aspnetuserid);
            if (user != null)
            {
                user.Firstname = obj.ProviderFormData.physician.Firstname;
                user.Lastname = obj.ProviderFormData.physician.Lastname;
                user.Email = obj.ProviderFormData.physician.Email;
                user.Mobile = "+" + obj.CountryCode.Split("+")[1]+ obj.ProviderFormData.physician.Mobile;
                user.Regionid = obj.ProviderFormData.physician.Regionid;
                user.Status = obj.ProviderFormData.physician.Status;
                user.Modifieddate = DateTime.Now;
                user.Modifiedby = adminid.ToString();
                _db.Users.Update(user);
                await _db.SaveChangesAsync();
                return true;
            }
            return true;
            //return false;
        }
        public async Task<bool> DeleteProvider(int phyid, string adminid)
        {
            var phydata = await _db.Physicians.FirstOrDefaultAsync(x => x.Physicianid == phyid);
            if (phydata != null)
            {
                phydata.Isdeleted = true;
                _db.Physicians.Update(phydata);
                await _db.SaveChangesAsync();

                var aspdata = await _db.Aspnetusers.FirstOrDefaultAsync(x => x.Id == phydata.Aspnetuserid);
                if (aspdata != null)
                {
                    aspdata.IsDeleted = true;
                    _db.Aspnetusers.Update(aspdata);
                    await _db.SaveChangesAsync();
                }
                return true;
            }
            return false;
        }
        public async Task<bool> IsProviderDeleted(int phyid)
        {
            return await Task.Run(() => _db.Physicians.Any(x => x.Physicianid == phyid && x.Isdeleted == true));
        }
        public async Task<bool> ProviderDocumentsUpload(IFormFile? file1, IFormFile? file2, IFormFile? file3, IFormFile? file4, IFormFile? file5, int phyid, int adminid)
        {
            var phydata = _db.Physicians.FirstOrDefault(x => x.Physicianid == phyid && x.Isdeleted != true);
            if (phydata == null)
            {
                return false;
            }
            if (file1 != null)
            {
                string name = await UploadProviderData(file1);
                phydata.Isagreementdoc = true;
                var phydoc = _db.ProviderDocuments.FirstOrDefault(x => x.PhysicianId == phyid && x.FileType == 1);
                if (phydoc != null)
                {
                    phydoc.Filename = name;
                    _db.ProviderDocuments.Update(phydoc);
                }
                else
                {
                    int count = _db.ProviderDocuments.Count();
                    var doc = new ProviderDocument();
                    doc.ProviderDocumentId = count + 1;
                    doc.Filename = name;
                    doc.FileType = 1;
                    doc.PhysicianId = phyid;
                    _db.ProviderDocuments.Add(doc);

                }
                _db.SaveChanges();
            }
            if (file2 != null)
            {
                string name = await UploadProviderData(file2);
                phydata.Isbackgrounddoc = true;

                var phydoc = _db.ProviderDocuments.FirstOrDefault(x => x.PhysicianId == phyid && x.FileType == 2);
                if (phydoc != null)
                {
                    phydoc.Filename = name;
                    _db.ProviderDocuments.Update(phydoc);
                }
                else
                {
                    int count = _db.ProviderDocuments.Count();
                    var doc = new ProviderDocument();
                    doc.ProviderDocumentId = count + 1;
                    doc.Filename = name;
                    doc.FileType = 2;
                    doc.PhysicianId = phyid;
                    _db.ProviderDocuments.Add(doc);

                }
                _db.SaveChanges();
            }
            if (file3 != null)
            {
                string name = await UploadProviderData(file3);
                phydata.Istrainingdoc = true;
                var phydoc = _db.ProviderDocuments.FirstOrDefault(x => x.PhysicianId == phyid && x.FileType == 3);
                if (phydoc != null)
                {
                    phydoc.Filename = name;
                    _db.ProviderDocuments.Update(phydoc);
                }
                else
                {
                    int count = _db.ProviderDocuments.Count();
                    var doc = new ProviderDocument();
                    doc.ProviderDocumentId = count + 1;
                    doc.Filename = name;
                    doc.FileType = 3;
                    doc.PhysicianId = phyid;
                    _db.ProviderDocuments.Add(doc);

                }
                _db.SaveChanges();
            }
            if (file4 != null)
            {
                string name = await UploadProviderData(file4);
                phydata.Isnondisclosuredoc = true;
                var phydoc = _db.ProviderDocuments.FirstOrDefault(x => x.PhysicianId == phyid && x.FileType == 4);
                if (phydoc != null)
                {
                    phydoc.Filename = name;
                    _db.ProviderDocuments.Update(phydoc);
                }
                else
                {
                    int count = _db.ProviderDocuments.Count();
                    var doc = new ProviderDocument();
                    doc.ProviderDocumentId = count + 1;
                    doc.Filename = name;
                    doc.FileType = 4;
                    doc.PhysicianId = phyid;
                    _db.ProviderDocuments.Add(doc);

                }
                _db.SaveChanges();
            }
            if (file5 != null)
            {
                string name = await UploadProviderData(file5);
                phydata.Islicensedoc = true;
                var phydoc = _db.ProviderDocuments.FirstOrDefault(x => x.PhysicianId == phyid && x.FileType == 5);
                if (phydoc != null)
                {
                    phydoc.Filename = name;
                    _db.ProviderDocuments.Update(phydoc);
                }
                else
                {
                    int count = _db.ProviderDocuments.Count();
                    var doc = new ProviderDocument();
                    doc.ProviderDocumentId = count + 1;
                    doc.Filename = name;
                    doc.FileType = 5;
                    doc.PhysicianId = phyid;
                    _db.ProviderDocuments.Add(doc);

                }
                _db.SaveChanges();
            }
            _db.Physicians.Update(phydata);
            _db.SaveChanges();
            return true;
        }
        public async Task<string> UploadProviderData(IFormFile? doc)
        {
            string FinalFileName = "";
            if (doc != null && doc.Length > 0)
            {
                Guid myuuid = Guid.NewGuid();
                var filename = Path.GetFileName(doc.FileName);
                FinalFileName = myuuid.ToString() + filename;
                //path
                var filepath = Path.Combine(Environment.CurrentDirectory, "wwwroot", "uploads", "ProviderDocument", FinalFileName);
                //copy in stream
                using (var str = new FileStream(filepath, FileMode.Create))
                {
                    //copy file
                    doc.CopyTo(str);
                }
                return await Task.Run(() => FinalFileName);
            }
            return await Task.Run(() => FinalFileName);
        }
        public async Task<string?> showProviderDocuments(int phyid, int filetype)
        {
            var data = await _db.ProviderDocuments.FirstOrDefaultAsync(x => x.PhysicianId == phyid && x.FileType == filetype);
            if (data != null)
            {
                if (data.Filename != null)
                {
                    return data.Filename;
                }
            }
            return "";
        }
        public async Task<bool> DeleteRecords(int reqid)
        {
            var reqdata = await _db.Requests.FirstOrDefaultAsync(x => x.Requestid == reqid);
            if (reqdata != null)
            {
                reqdata.Isdeleted = true;
                _db.Requests.Update(reqdata);
                _db.SaveChanges();
                return true;
            }
            return false;
        }
        public async Task<bool> CreatePhysician(ProviderProfileViewModel obj, CreateProviderForm Filedata, int adminid)
        {

            var admindata = _db.Admins.FirstOrDefault(x => x.Adminid == adminid);
            //aspnetuser
            if (admindata == null) { return false; }
            var pass = BCrypt.Net.BCrypt.HashPassword(obj.ProviderFormData.aspnetuser.Passwordhash);
            Aspnetuser user = new Aspnetuser()
            {
                Id = ((500 + _db.Aspnetusers.Count()) + 1).ToString(),
                Username = obj.ProviderFormData.aspnetuser.Username,
                Passwordhash = pass,
                Email = obj.ProviderFormData.physician.Email,
                Phonenumber ="+" + obj.CountryCode.Split("+")[1] + obj.ProviderFormData.physician.Mobile,
                Createddate = DateTime.Now,
                Accounttype = 1,
            };
            _db.Aspnetusers.Add(user);
            _db.SaveChanges();

            //aspnetroles
            var roles = _db.Roles.FirstOrDefault(x => x.Roleid == obj.ProviderFormData.physician.Roleid);
            if (roles != null)
            {
                Aspnetuserrole role = new()
                {
                    Userid = user.Id,
                    Name = roles.Name
                };
                _db.Aspnetuserroles.Add(role);
                _db.SaveChanges();
            }


            //physician
            Physician phy = new()
            {
                Physicianid = _db.Physicians.Count() + 1,
                Aspnetuserid = user.Id,
                Firstname = obj.ProviderFormData.physician.Firstname,
                Lastname = obj.ProviderFormData.physician.Lastname,
                Email = obj.ProviderFormData.physician.Email,
                Mobile = "+" + obj.CountryCode.Split("+")[1]+ obj.ProviderFormData.physician.Mobile,
                Adminnotes = obj.ProviderFormData.physician.Adminnotes,
                Address1 = obj.ProviderFormData.physician.Address1,
                Address2 = obj.ProviderFormData.physician.Address2,
                City = obj.ProviderFormData.physician.City,
                Regionid = obj.ProviderFormData.physician.Regionid,
                Zip = obj.ProviderFormData.physician.Zip,
                Altphone = "+" + obj.CountryCode.Split("+")[2]+ obj.ProviderFormData.physician.Altphone,
                Createdby = (admindata.Aspnetuserid != null) ? admindata.Aspnetuserid : "01",
                Createddate = DateTime.Now,
                Status = 4,
                Businessname = obj.ProviderFormData.physician.Businessname,
                Businesswebsite = obj.ProviderFormData.physician.Businesswebsite,
                Roleid = obj.ProviderFormData.physician.Roleid,
                Lattitude = obj.ProviderFormData.physician.Lattitude,
                Longtitude = obj.ProviderFormData.physician.Longtitude,

            };
            _db.Physicians.Add(phy);
            _db.SaveChanges();
            if (obj.ProviderFormData.ProviderPhoto != null && obj.ProviderFormData.ProviderPhoto.Length > 0)
            {
                Guid myuuid = Guid.NewGuid();
                var filename = Path.GetFileName(obj.ProviderFormData.ProviderPhoto.FileName);
                var FinalFileName = myuuid.ToString() + filename;

                //path
                var filepath = Path.Combine(Environment.CurrentDirectory, "wwwroot", "uploads", "ProviderData", FinalFileName);

                //copy in stream

                using (var str = new FileStream(filepath, FileMode.Create))
                {
                    //copy file
                    obj.ProviderFormData.ProviderPhoto.CopyTo(str);
                }
                //STORE DATA IN TABLE
                phy.Photo = FinalFileName;
            }
            _db.Physicians.Update(phy);
            _db.SaveChanges();
            //provider documents
            bool status = await ProviderDocumentsUpload(Filedata.file1, Filedata.file2, Filedata.file3, Filedata.file4, Filedata.file5, phy.Physicianid, adminid);
            if (status == false)
            {
                return false;
            }

            //notification
            Physiciannotification notify = new()
            {
                Physicianid = phy.Physicianid,
                Isnotificationstopped = false
            };
            _db.Physiciannotifications.Add(notify);
            _db.SaveChanges();


            phy.PhyNotificationid = notify.Id;
            _db.Physicians.Update(phy);
            _db.SaveChanges();

            //physician location
            Physicianlocation location = new()
            {
                Physicianid = phy.Physicianid,
                Createddate = DateTime.Now,
                Physicianname = obj.ProviderFormData.physician.Firstname + " " + obj.ProviderFormData.physician.Lastname,
                Address = obj.ProviderFormData.physician.Address1
            };
            _db.Physicianlocations.Add(location);
            _db.SaveChanges();

            //physion region do once do check boxes
            //adminregions add
            var allRegionList = _db.Physicianregions;
            if (obj.RegionList != null)
            {
                var SelectedRegionList = obj.RegionList.ToList();
                int count = 1;
                if(allRegionList != null)
                {
                 count = allRegionList.Max(x => x.Physicianregionid);
                }

                foreach (var item in SelectedRegionList)
                {
                    var newRegions = new Physicianregion();
                    newRegions.Regionid = item;
                    newRegions.Physicianregionid = ++count;
                    newRegions.Physicianid = phy.Physicianid;
                    _db.Physicianregions.Add(newRegions);
                    await _db.SaveChangesAsync();
                }
            }
            return true;
        }
        public async Task<IEnumerable<Physician>?> LoadProviderLocationPartial(int adminid)
        {
            var admindata = await _db.Admins.FirstOrDefaultAsync(x => x.Adminid == adminid);
            IEnumerable<Physician>? phy = null;
            if (admindata != null)
            {
                if (admindata.Aspnetuserid != null)
                {
                    return _db.Physicians.Where(x => x.Isdeleted != true && x.Createdby == admindata.Aspnetuserid && x.Lattitude != null && x.Longtitude != null);
                }
            }
            return phy;
        }
        #endregion
        #region AdminAccount
        public async Task<AdminProfileViewModel> AdminProfile(int adminid)
        {
            var AdminData = new Admin();
            AdminProfileViewModel model = new();
            try
            {

                AdminData = await (from req in _db.Admins
                                   where req.Adminid == adminid
                                   join Roles in _db.Roles on req.Roleid equals Roles.Roleid into temp
                                   from roles in temp.DefaultIfEmpty()
                                   join Status in _db.Statuses on req.Status equals Status.StatusId into temp2
                                   from status in temp2.DefaultIfEmpty()
                                   join aspnet in _db.Aspnetusers on req.Aspnetuserid equals aspnet.Id into temp3
                                   from asp in temp3.DefaultIfEmpty()
                                   join Regions in _db.Regions on req.Regionid equals Regions.Regionid into temp4
                                   from regions in temp4.DefaultIfEmpty()
                                   select new Admin
                                   {
                                       Adminid = adminid,
                                       Address1 = req.Address1,
                                       Address2 = req.Address2,
                                       Firstname = req.Firstname,
                                       Lastname = req.Lastname,
                                       Email = req.Email,
                                       Regionid = req.Regionid,
                                       Mobile = req.Mobile,
                                       City = req.City,
                                       Region = regions,
                                       Zip = req.Zip,
                                       Altphone = req.Altphone,
                                       Role = roles,
                                       StatusNavigation = status,
                                       Aspnetuser = asp,
                                   }).FirstOrDefaultAsync();
                //await _db.Admins.FirstOrDefaultAsync(x => x.Adminid == adminid);
                model.AdminData = AdminData;

                //region
                var adminSelectedRegion = from x1 in _db.Adminregions
                                          where x1.Adminid == AdminData.Adminid
                                          join x2 in _db.Regions on x1.Regionid equals x2.Regionid
                                          select x2;
                //non selected
                var NonSelectedRegion = _db.Regions.Except(adminSelectedRegion);

                model.selectedRegion = adminSelectedRegion;
                model.nonSelectedRegion = NonSelectedRegion;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return model;
        }
        public async Task<bool> AdminAspFormSubmit(AdminProfileViewModel admin, int adminid)
        {
            //update details in the admin table
            var admintabledata = await _db.Admins.FirstOrDefaultAsync(X => X.Adminid == adminid);
            if (admintabledata == null)
            {
                return false;
            }
            var aspdata = await _db.Aspnetusers.FirstOrDefaultAsync(x => x.Id == admintabledata.Aspnetuserid);
            if (aspdata == null)
            {
                return false;
            }
            string[] splitCountryCode = admin.CountryCode.Split("+");
            string aspMobile = splitCountryCode[1];
            string AltMobile = splitCountryCode[2];

            if (admin.AdminData != null && admin.AdminData.Firstname == null)
            {
                //implement business things here
                if (admintabledata != null)
                {
                    admintabledata.Address1 = admin.AdminData.Address1;
                    admintabledata.Address2 = admin.AdminData.Address2;
                    admintabledata.City = admin.AdminData.City;
                    admintabledata.Regionid = admin.AdminData.Regionid;
                    admintabledata.Zip = admin.AdminData.Zip;
                    admintabledata.Altphone = "+"+ AltMobile +admin.AdminData.Altphone;
                    _db.Admins.Update(admintabledata);
                    await _db.SaveChangesAsync();
                    return true;
                }
                return false;
            }

            if (admintabledata != null)
            {
                admintabledata.Firstname = admin.AdminData.Firstname;
                admintabledata.Lastname = admin.AdminData.Lastname;
                admintabledata.Email = admin.AdminData.Email;
                admintabledata.Mobile = "+"+aspMobile + admin.AdminData.Mobile;
                admintabledata.Status = admin.AdminData.Status;
                admintabledata.Roleid = admin.AdminData.Roleid;
                _db.Admins.Update(admintabledata);
                await _db.SaveChangesAsync();
                //checked on already selected
                var allRegionList = _db.Adminregions;
                var AdminRegiontable = _db.Adminregions.Where(x => x.Adminid == adminid);
                int count = 0;
                if (allRegionList.Count() != 0)
                {
                    count = allRegionList.Max(x => x.Adminregionid);
                }
                if (admin.RegionList == null)
                {
                    foreach (var item in allRegionList)
                    {
                        _db.Adminregions.Remove(item);
                    }
                    _db.SaveChanges();
                }
                else
                {
                    var SelectedRegionList = admin.RegionList.ToList();
                    foreach (var obj in AdminRegiontable)
                    {
                        var val = "";
                        foreach (var item in SelectedRegionList)
                        {
                            if (obj.Regionid == item)
                            {
                                val = item.ToString();
                                break;
                            }
                        }
                        if (val == "")
                        {
                            _db.Adminregions.Remove(obj);
                            SelectedRegionList.Remove((int)obj.Regionid);
                        }
                        else
                        {
                            SelectedRegionList.Remove((int)obj.Regionid);
                            val = "";
                        }
                    }
                    _db.SaveChanges();
                    Guid uuid = Guid.NewGuid();

                    foreach (var item in SelectedRegionList)
                    {

                        var newRegions = new Adminregion();
                        newRegions.Regionid = item;
                        newRegions.Adminregionid = ++count;
                        newRegions.Adminid = adminid;
                        _db.Adminregions.Add(newRegions);
                        await _db.SaveChangesAsync();
                    }
                }
            }

            if (admin.AdminData.Aspnetuser == null)
            {
                return false;
            }
            else
            {

                aspdata.Username = admin.AdminData.Aspnetuser.Username;
                _db.Aspnetusers.Update(aspdata);
                await _db.SaveChangesAsync();
                return true;
            }
        }
        public async Task<IEnumerable<Adminregion>> regionCheckbox(int adminid)
        {
            IEnumerable<Adminregion> obj = from x1 in _db.Adminregions
                                           join x2 in _db.Regions on x1.Regionid equals x2.Regionid into temp
                                           from X2 in temp.DefaultIfEmpty()
                                           where x1.Adminid == adminid
                                           select new Adminregion
                                           {
                                               Regionid = x1.Regionid,
                                               Region = X2
                                           };
            return await Task.Run(() => obj);
        }
        public async Task<bool> CreateAdminPost(AdminProfileViewModel obj)
        {
            
            int ApNetUserlength = _db.Aspnetusers.Count();
            Aspnetuser newuser = new();
            if (obj.AdminData.Aspnetuser != null)
            {
                newuser.Id = ApNetUserlength + 1.ToString();
                newuser.Username = obj.AdminData.Aspnetuser.Username;
                newuser.Passwordhash = BCrypt.Net.BCrypt.HashPassword(obj.AdminData.Aspnetuser.Passwordhash);
                newuser.Email = obj.AdminData.Email;
                newuser.Phonenumber = "+" + obj.CountryCode.Split("+")[1] + obj.AdminData.Mobile;
                newuser.Createddate = DateTime.Now;
                newuser.Accounttype = 2;
                _db.Aspnetusers.Add(newuser);
                _db.SaveChanges();
            }
            else
            {
                return false;
            }
            //User user = new();
            //if(obj.Aspnetuser != null)
            //{
            //    user.Aspnetuserid = newuser.Id;
            //    user.Firstname = obj.Firstname;
            //    user.Lastname= obj.Lastname;
            //    user.Email = obj.Email;
            //    user.Mobile = obj.Mobile;
            //    user.Createddate = DateTime.Now;
            //}
            Admin admin = new()
            {
                Aspnetuserid = newuser.Id,
                Firstname = obj.AdminData.Firstname,
                Lastname = obj.AdminData.Lastname,
                Email = obj.AdminData.Email,
                Mobile = "+" + obj.CountryCode.Split("+")[1] + obj.AdminData.Mobile,
                Createddate = DateTime.Now,
                Address1 = obj.AdminData.Address1,
                Address2 = obj.AdminData.Address2,
                City = obj.AdminData.City,
                Regionid = obj.AdminData.Regionid,
                Zip = obj.AdminData.Zip,
                Status = 1,
                Altphone = "+" + obj.CountryCode.Split("+")[2] +obj.AdminData.Altphone,
                Createdby = "01",
                Roleid = obj.AdminData.Roleid,
            };
            _db.Admins.Add(admin);
            _db.SaveChanges();

            var roles = _db.Roles.FirstOrDefault(x => x.Roleid == obj.AdminData.Roleid);
            if (roles != null)
            {
                Aspnetuserrole role = new()
                {
                    Userid = newuser.Id,
                    Name = roles.Name
                };
                _db.Aspnetuserroles.Add(role);
                _db.SaveChanges();
            }

            //adminregions add
            var allRegionList = _db.Adminregions;
            var SelectedRegionList = obj.RegionList.ToList();
            int count = allRegionList.Max(x => x.Adminregionid);
            foreach (var item in SelectedRegionList)
            {
                var newRegions = new Adminregion();
                newRegions.Regionid = item;
                newRegions.Adminregionid = ++count;
                newRegions.Adminid = admin.Adminid;
                _db.Adminregions.Add(newRegions);
                await _db.SaveChangesAsync();
            }

            return true;
        }
        #endregion
        #region Access
        public async Task<Healthprofessional?> GetBusinessData(int vendorid)
        {
            return await _db.Healthprofessionals.SingleOrDefaultAsync(x => x.Vendorid == vendorid);
        }
        public async Task<IEnumerable<Healthprofessional>> GetBusinessByProfession(int ProfessionId)
        {
            return await Task.Run(() => _db.Healthprofessionals.Where(x => x.Profession == ProfessionId).Select(x => new Healthprofessional
            {
                Vendorid = x.Vendorid,
                Vendorname = x.Vendorname,
            }));
        }
        public async Task<IEnumerable<Healthprofessionaltype>> OrderCheckOutPage(int ReqId)
        {
            return await Task.Run(() => _db.Healthprofessionaltypes.Select(s => new Healthprofessionaltype
            {
                Healthprofessionalid = s.Healthprofessionalid,
                Professionname = s.Professionname,
            }));

        }
        public async Task<AcccountAccessViewModel> Accountaccess(int? PageNumber)
        {
            var model = from x1 in _db.Roles
                        where x1.Isdeleted == false
                        select new Role
                        {
                            Roleid = x1.Roleid,
                            Name = x1.Name,
                            Accounttype = x1.Accounttype,
                            AccounttypeNavigation = x1.AccounttypeNavigation,
                        };

            int count = model.Count();
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
                        PageNumber = count / 5 + 1;
                    else
                    {
                        PageNumber = 1;
                    }
                }
                page = (int)PageNumber;
            }
            var Paginateddata = model.Skip((page - 1) * 5).Take(5);

            AcccountAccessViewModel data = new();
            data.roles = Paginateddata;
            data.maxPage = maxPage;
            data.PageNumber = page;
            return await Task.Run(() => data);

        }
        public async Task<IEnumerable<AccountType>> CreateAccess()
        {
            return await Task.Run(() => _db.AccountTypes);
        }
        public async Task<bool> CreateAccessRole(List<string> arrayPage, short accounttype, string rolename)
        {
            //add in role
            var newrole = new Role();
            var counts = _db.Roles.Count();
            newrole.Roleid = counts + 1;
            newrole.Name = rolename;
            newrole.Createddate = DateTime.Now;
            newrole.Createdby = "01";
            newrole.Accounttype = accounttype;
            _db.Roles.Add(newrole);
            _db.SaveChanges();
            //in rolemenu 

            var table = _db.Rolemenus.ToList();
            int count = table.Max(x => x.Rolemenuid);
            foreach (var item in arrayPage)
            {
                //var menuid = _db.Menus.Where(x => x.Name == item).FirstOrDefault(); 

                var rolemenuadd = new Rolemenu();
                rolemenuadd.Rolemenuid = ++count;
                rolemenuadd.Roleid = newrole.Roleid;
                rolemenuadd.Menuid = int.Parse(item);
                _db.Rolemenus.Add(rolemenuadd);
                _db.SaveChanges();
            }
            return await Task.Run(() => true);
        }
        public async Task<bool> DeleteAccess(int roleid)
        {
            var data = await _db.Roles.FirstOrDefaultAsync(x => x.Roleid == roleid);
            if (data != null)
            {
                data.Isdeleted = true;
                _db.Roles.Update(data);
                _db.SaveChanges();
                return true;
            }
            return false;
        }
        public async Task<IEnumerable<Menu>> getAccountRoleMenu(int? accountType)
        {
            return await Task.Run(() => _db.Menus.Where(x => x.Accounttype == accountType || accountType == null));
        }
        public async Task<Role?> EditAccessPage(int roleid)
        {
            return await _db.Roles.FirstOrDefaultAsync(x => x.Roleid == roleid);
        }
        public async Task<IEnumerable<Menu>> getRequestWiseAccess(int roleid)
        {
            var data = from menu in _db.Menus
                       join rolemenu in _db.Rolemenus on menu.Menuid equals rolemenu.Menuid into temp
                       from rolemenus in temp.DefaultIfEmpty()
                       where rolemenus.Roleid == roleid
                       select new Menu()
                       {
                           Menuid = menu.Menuid,
                           Name = menu.Name,
                       };
            return await Task.Run(() => data);
        }
        public async Task<bool> EditAccess(string roleid, List<string> arrayPage, short accounttype, string rolename)
        {
            var data = _db.Roles.FirstOrDefault(x => x.Roleid == int.Parse(roleid));

            if (data != null)
            {
                data.Name = rolename;
                data.Accounttype = accounttype;
                data.Modifieddate = DateTime.Now;
                //change here
                data.Modifiedby = "01";
                _db.Roles.Update(data);
                _db.SaveChanges();
            }
            else
            {
                return false;
            }
            var table = _db.Rolemenus.ToList();
            int count = table.Max(x => x.Rolemenuid);

            var rolemenu = _db.Rolemenus.Where(x => x.Roleid == int.Parse(roleid));

            foreach (var obj in rolemenu)
            {
                var val = "";
                foreach (var item in arrayPage)
                {
                    if (obj.Menuid == int.Parse(item))
                    {
                        val = item;
                        break;
                    }
                }
                if (val == "")
                {
                    _db.Rolemenus.Remove(obj);
                    arrayPage.Remove(val);
                }
                else
                {
                    arrayPage.Remove(val);
                    val = "";
                }
            }
            _db.SaveChanges();
            Guid uuid = Guid.NewGuid();
            foreach (var item in arrayPage)
            {

                var rolemenuadd = new Rolemenu();
                rolemenuadd.Rolemenuid = ++count;
                rolemenuadd.Roleid = int.Parse(roleid);
                rolemenuadd.Menuid = int.Parse(item);
                _db.Rolemenus.Add(rolemenuadd);
                await _db.SaveChangesAsync();
            }
            return true;
        }
        #endregion
        #region userAccess
        public async Task<UserAccessViewModel?> UserAccess(int? roletype, int adminid, int? PageNumber)
        {
            //set all the roles in the search 
            //
            //change once region Based Request Done

            var TotalRequests = _db.Requests.Where(x => x.Isdeleted != true && (x.Status != 3 && x.Status != 8 && x.Status != 9 && x.Status != 16) ).Count();
            //get admin regions 
            var Adminquery = from x1 in _db.Aspnetusers
                             join x2 in _db.Admins on x1.Id equals x2.Aspnetuserid
                             join x3 in _db.Regions on x2.Regionid equals x3.Regionid into temp
                             from X3 in temp.DefaultIfEmpty()
                             join x4 in _db.Statuses on x2.Status equals x4.StatusId into temp2
                             from X4 in temp2.DefaultIfEmpty()
                             where x1.IsDeleted != true && x2.Isdeleted != true && (roletype == null || x2.Roleid == roletype)
                             //condition for remove self account
                             && x2.Adminid != adminid
                             select new UserAccessIndexViewModel()
                             {
                                 aspid = x1.Id,
                                 AccountPOC = x2.Firstname + "," + x2.Lastname,
                                 Status = X4.Statusname,
                                 AccountType = "Admin",
                                 AdminId = x2.Adminid,
                                 Phone = x2.Mobile,
                                 OpenRequest = TotalRequests,
                             };
            var Physicinaquery = from x1 in _db.Aspnetusers
                                 join x2 in _db.Physicians on x1.Id equals x2.Aspnetuserid
                                 join x3 in _db.Regions on x2.Regionid equals x3.Regionid into temp
                                 from X3 in temp.DefaultIfEmpty()
                                 join x4 in _db.Statuses on x2.Status equals x4.StatusId into temp2
                                 from X4 in temp2.DefaultIfEmpty()
                                 join x5 in _db.Requests on x2.Physicianid equals x5.Physicianid into temp3
                                 from X5 in temp3.DefaultIfEmpty()
                                 join x6 in _db.Requeststatuses on X5.Status equals x6.Requeststatusid into temp4
                                 from X6 in temp4.DefaultIfEmpty()
                                 where x1.IsDeleted != true && x2.Isdeleted != true && (roletype == null || x2.Roleid == roletype)
                                 &&(X6==null || X6.Requeststatusid == 6 || X6.Requeststatusid == 2 || X6.Requeststatusid == 11 || X6.Requeststatusid == 4 || X6.Requeststatusid == 21)
                                 group X5 by new { x1.Id, x2.Firstname, x2.Lastname, X4.Statusname, x2.Physicianid, x2.Mobile } into RequestGroup
                                 select new UserAccessIndexViewModel()
                                 {
                                     aspid = RequestGroup.Key.Id,
                                     AccountPOC = RequestGroup.Key.Firstname + "," + RequestGroup.Key.Lastname,
                                     Status = RequestGroup.Key.Statusname,
                                     AccountType = "Physicians",
                                     PhysicianId = RequestGroup.Key.Physicianid,
                                     Phone = RequestGroup.Key.Mobile,
                                     OpenRequest = RequestGroup.Count(x => x != null)
                                 };
            List<UserAccessIndexViewModel> GlobalStrings = Adminquery.ToList();
            List<UserAccessIndexViewModel> localStrings = Physicinaquery.ToList();
            GlobalStrings.AddRange(localStrings);

            UserAccessViewModel model = new();

            int count = GlobalStrings.Count();
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
                        PageNumber = count / 5 + 1;
                    else
                    {
                        PageNumber = 1;
                    }
                }
                page = (int)PageNumber;
            }
            var Paginateddata = GlobalStrings.Skip((page - 1) * 5).Take(5);
            model.UserAccessIndexViewModel = Paginateddata;
            model.maxPage = maxPage;
            model.PageNumber = page;
            return await Task.Run(() => model);
        }
        #endregion
        #region Partners
        public async Task<IEnumerable<Healthprofessionaltype>?> getProfession()
        {
            return await Task.Run(() => _db.Healthprofessionaltypes.Where(x => x.Isdeleted != true));
        }
        public async Task<IEnumerable<Healthprofessional>?> PartnerDataOnly(string? search, int? profId)
        {
            if (profId == -1)
            {
                profId = null;
            }
            var query = from hp in _db.Healthprofessionals
                        join hpt in _db.Healthprofessionaltypes on hp.Profession equals hpt.Healthprofessionalid into temp
                        from HPT in temp.DefaultIfEmpty()
                        where hp.Isdeleted != true
                        && (search == null || hp.Vendorname.ToLower().Contains(search.ToLower()))
                        && (profId == null || hp.Profession == profId)
                        select new Healthprofessional()
                        {
                            Faxnumber = hp.Faxnumber,
                            Vendorname = hp.Vendorname,
                            Phonenumber = hp.Phonenumber,
                            Email = hp.Email,
                            Businesscontact = hp.Businesscontact,
                            Vendorid = hp.Vendorid,
                            ProfessionNavigation = HPT,
                            Profession = hp.Profession,

                        };
            return await Task.Run(() => query);

        }
        public async Task<PartnerInformationViewModel> PartnerData(string? search, int? profId, int? PageNumber)
        {
            if (profId == -1)
            {
                profId = null;
            }
            var query = from hp in _db.Healthprofessionals
                        join hpt in _db.Healthprofessionaltypes on hp.Profession equals hpt.Healthprofessionalid into temp
                        from HPT in temp.DefaultIfEmpty()
                        where hp.Isdeleted != true
                        && (search == null || hp.Vendorname.ToLower().Contains(search.ToLower()))
                        && (profId == null || hp.Profession == profId)
                        select new Healthprofessional()
                        {
                            Faxnumber = hp.Faxnumber,
                            Vendorname = hp.Vendorname,
                            Phonenumber = hp.Phonenumber,
                            Email = hp.Email,
                            Businesscontact = hp.Businesscontact,
                            Vendorid = hp.Vendorid,
                            ProfessionNavigation = HPT,
                            Profession = hp.Profession,

                        };

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
                        PageNumber = count / 5 + 1;
                    else
                    {
                        PageNumber = 1;
                    }
                }
                page = (int)PageNumber;
            }
            var Paginateddata = query.Skip((page - 1) * 5).Take(5);


            PartnerInformationViewModel model = new();
            model.healthprofessionals = Paginateddata;
            model.maxPage = maxPage;
            model.PageNumber = page;
            return await Task.Run(() => model);
        }
        public async Task<Healthprofessional?> VendorData(int vendorid)
        {
            Healthprofessional? query = await (from hp in _db.Healthprofessionals
                                               join hpt in _db.Healthprofessionaltypes on hp.Profession equals hpt.Healthprofessionalid into temp
                                               from HPT in temp.DefaultIfEmpty()
                                               where hp.Vendorid == vendorid
                                               select new Healthprofessional()
                                               {
                                                   Faxnumber = hp.Faxnumber,
                                                   Vendorname = hp.Vendorname,
                                                   Phonenumber = hp.Phonenumber,
                                                   Email = hp.Email,
                                                   Businesscontact = hp.Businesscontact,
                                                   Vendorid = hp.Vendorid,
                                                   ProfessionNavigation = HPT,
                                                   Profession = hp.Profession,
                                               }).FirstOrDefaultAsync();

            return query;
        }
        public async Task<bool> UpdateBusiness(Healthprofessional hp, int? vendorId, int toggler)
        {
            Healthprofessional? data = null;
            //is vendors have any account?
            //Aspnetuser aspnetvendor = new();
            if (toggler == 1 && vendorId != null)
            {
                data = await _db.Healthprofessionals.FirstOrDefaultAsync(x => x.Vendorid == vendorId);
                if (data == null)
                {
                    return false;
                }
                data.Modifieddate = DateTime.Now;
            }
            else
            {
                data = new Healthprofessional();
                data.Createddate = DateTime.Now;
            }
            data.Vendorname = hp.Vendorname;
            data.Profession = hp.Profession;
            data.Faxnumber = hp.Faxnumber;
            data.Phonenumber = hp.Phonenumber;
            data.Email = hp.Email;
            data.Businesscontact = hp.Businesscontact;
            data.Address = hp.Address;
            data.City = hp.City;
            data.State = hp.State;
            data.Zip = hp.Zip;

            if (toggler == 1)
                _db.Healthprofessionals.Update(data);
            else
                _db.Healthprofessionals.Add(data);
            _db.SaveChanges();


            return true;
        }
        public async Task<bool> DeleteBusiness(int vendorId)
        {
            Healthprofessional? data = await _db.Healthprofessionals.FirstOrDefaultAsync(x => x.Vendorid == vendorId);
            if (data != null)
            {
                data.Isdeleted = true;
                _db.Healthprofessionals.Update(data);
                _db.SaveChanges();
                return true;
            }
            return false;
        }
        #endregion
        #region records
        public async Task<SearchRecordsViewModel?> SearchRecords(SearchFilterModel? SearchData, int? PageNumber)
        {
            if (SearchData != null && SearchData.RequestType == -1)
            {
                SearchData.RequestType = null;
            }
            if (SearchData != null && SearchData.reqstatus == -1)
            {
                SearchData.reqstatus = null;
            }

            IEnumerable<SearchRecords>? model = from x1 in _db.Requests
                                                join x6 in _db.Requesttypes on x1.Requesttypeid equals x6.Requesttypeid into temp5
                                                from X6 in temp5.DefaultIfEmpty()
                                                join x2 in _db.Requeststatuses on x1.Status equals x2.Requeststatusid into temp
                                                from X2 in temp.DefaultIfEmpty()
                                                join x3 in _db.Requestclients on x1.Requestid equals x3.Requestid into temp2
                                                from X3 in temp2.DefaultIfEmpty()
                                                join x4 in _db.Requestnotes on x1.Requestid equals x4.Requestid into temp3
                                                from X4 in temp3.DefaultIfEmpty()
                                                join x5 in _db.Physicians on x1.Physicianid equals x5.Physicianid into temp4
                                                from X5 in temp4.DefaultIfEmpty()
                                                where (x1.Isdeleted != true)
                                                && (SearchData == null || SearchData.reqstatus == null || x1.Status == SearchData.reqstatus)
                                                && (SearchData == null || SearchData.patName == null || (X3.Firstname + X3.Lastname).ToLower().Contains(SearchData.patName.ToLower()))
                                                && (SearchData == null || SearchData.RequestType == null || (x1.Requesttypeid == SearchData.RequestType))
                                                && (SearchData == null || SearchData.ProviderName == null || ((X5.Firstname + X5.Lastname).ToLower().Contains(SearchData.ProviderName.ToLower())))
                                                && (SearchData == null || SearchData.Email == null || X3.Email == SearchData.Email)
                                                //&& (fromDate == null || x1.Accepteddate == fromDate)
                                                && (SearchData == null || SearchData.PhoneNumber == null || X3.Phonenumber == SearchData.PhoneNumber)
                                                && (SearchData == null || SearchData.FromDate == null || SearchData.FromDate <= x1.Accepteddate)
                                                && (SearchData == null || SearchData.FromDate == null || SearchData.ToDate >= x1.Accepteddate)
                                                select new SearchRecords()
                                                {
                                                    PatientName = X3.Firstname + "," + X3.Lastname,
                                                    Requestor = x1.Firstname + "," + x1.Lastname,
                                                    DateOfService = x1.Accepteddate.ToString(),
                                                    CloseCaseDate = (x1.Status == 8) ? x1.Modifieddate.ToString() : "-",
                                                    Email = X3.Email,
                                                    PhoneNumber = X3.Phonenumber,
                                                    Address = X3.Street + X3.City + X3.State,
                                                    Zip = X3.Zipcode,
                                                    requeststatus = X2.Name,
                                                    PhysicianName = X5.Firstname + X5.Lastname,
                                                    PhysicianNotes = (X4.Physiciannotes != null) ? X4.Physiciannotes : null,
                                                    AdminNotes = (X4.Adminnotes != null) ? X4.Adminnotes : null,
                                                    CancelledByProviderNotes = x1.Casetagphysician,
                                                    PatientNotes = X3.Notes,
                                                    RequestClientId = X3.Requestclientid,
                                                    RequestId = x1.Requestid,
                                                    RequestType = X6.Name
                                                };
            int count = model.Count();
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
                        PageNumber = count / 5 + 1;
                    else
                    {
                        PageNumber = 1;
                    }
                }
                page = (int)PageNumber;
            }
            var Paginateddata = model.Skip((page - 1) * 5).Take(5);

            SearchRecordsViewModel data = new();
            data.SearchRecords = Paginateddata;
            data.maxPage = maxPage;
            data.PageNumber = page;

            return await Task.Run(() => data);
        }
        public async Task<SearchFilterModel?> SearchRecordsIndex()
        {
            var PaymentStatus = _db.Requeststatuses.Where(x => x.Requeststatusid == 17 || x.Requeststatusid == 18 || x.Requeststatusid == 19 || x.Requeststatusid == 20 || x.Requeststatusid == 16);
            var reqtype = _db.Requesttypes;
            SearchFilterModel? searchfilter = new();
            searchfilter.requesttypes = reqtype;
            searchfilter.requeststatuses = PaymentStatus;
            return await Task.Run(() => searchfilter);
        }
        public async Task<IEnumerable<PatientRecords>> PatientRecords(SearchPatientRecords? obj)
        {
            var query = from user in _db.Users
                        where user.Isdeleted != true
                        && (obj == null || obj.FirstName == null || user.Firstname.ToLower().Contains(obj.FirstName.ToLower()))
                        && (obj == null || obj.LastName == null || user.Lastname == null || user.Lastname.ToLower().Contains(obj.LastName.ToLower()))
                        && (obj == null || obj.EmailAddress == null || user.Email.ToLower().Contains(obj.EmailAddress.ToLower()))
                        && (obj == null || obj.phoneNumber == null || user.Mobile == null || user.Mobile.ToLower().Contains(obj.phoneNumber.ToLower()))
                        orderby user.Userid
                        select new PatientRecords()
                        {
                            FirstName = user.Firstname,
                            LastName = user.Lastname,
                            PhoneNumber = user.Mobile,
                            Email = user.Email,
                            Address = user.Street + " " + user.City + " " + user.State + " " + user.Zipcode,
                            UserId = user.Userid
                        };
            return await Task.Run(() => query);
        }
        public async Task<IEnumerable<PatientHistory>?> PatientHistoryIndex(int userid)
        {
            IEnumerable<PatientHistory> query = from x1 in _db.Requests
                                                join x4 in _db.Requestwisefiles on x1.Requestid equals x4.Requestid into temp3
                                                from X4 in temp3.DefaultIfEmpty()
                                                where x1.Isdeleted != true && x1.Userid == userid && (X4 == null || X4.Isdeleted != true)

                                                join x2 in _db.Requeststatuses on x1.Status equals x2.Requeststatusid into temp
                                                from X2 in temp.DefaultIfEmpty()
                                                join x3 in _db.Encounterformdetails on x1.Requestid equals x3.ReqId into temp2
                                                from X3 in temp2.DefaultIfEmpty()
                                                join x5 in _db.Physicians on x1.Physicianid equals x5.Physicianid into temp4
                                                from X5 in temp4.DefaultIfEmpty()
                                                group x1 by new { x1.Requestid, x1.Firstname, x1.Createddate, x1.Confirmationnumber, provider = (X5.Firstname + " " + X5.Lastname), X2.Name, X3.Encounterformdetailsid } into g
                                                select new PatientHistory()
                                                {
                                                    Member = g.Key.Firstname,
                                                    CreateDate = g.Key.Createddate,
                                                    ConfirmationNumber = g.Key.Confirmationnumber,
                                                    Provider = g.Key.provider,
                                                    status = g.Key.Name,
                                                    concludedDate = g.Key.Createddate,
                                                    reportID = g.Key.Encounterformdetailsid,
                                                    DocCount = g.Count(),
                                                    requestid = g.Key.Requestid
                                                };
            return await Task.Run(() => query);
        }
        public async Task<EmailAndSMSCombineViewmodel> EmailLogRecords(SearchEmailLogsRecords? obj, int type, int? PageNumber)
        {
            if (obj != null && obj.RoleId != null && obj.RoleId == -1)
            {
                obj.RoleId = null;
            }
            IEnumerable<EmailLogsData>? query = null;
            if (type == 1)
            {

                query = from x1 in _db.Emaillogs
                        join x2 in _db.Roles on x1.Roleid equals x2.Roleid into temp
                        from X2 in temp.DefaultIfEmpty()
                        join x3 in _db.Requestclients on x1.Requestid equals x3.Requestid into temp2
                        from X3 in temp2.DefaultIfEmpty()
                        join x4 in _db.Admins on x1.Adminid equals x4.Adminid into temp3
                        from X4 in temp3.DefaultIfEmpty()
                        join x5 in _db.Physicians on x1.Physicianid equals x5.Physicianid into temp4
                        from X5 in temp4.DefaultIfEmpty()
                        where
                        (obj == null || obj.RoleId == null || obj.RoleId == x1.Roleid)
                        && (obj == null || obj.Receiver == null || (((X3 == null) ? (X4 == null) ? (X5 == null) ? "" : X5.Firstname + X5.Lastname : X4.Firstname + X4.Lastname : X3.Firstname + X3.Lastname)).ToLower().Contains(obj.Receiver.ToLower()))
                        && (obj == null || obj.Email == null || x1.Emailid.ToLower().Contains(obj.Email))
                        && (obj == null || obj.Created == null || x1.Createdate.Date == obj.Created)
                        && (obj == null || obj.SentDate == null || x1.Sentdate == obj.SentDate)
                        //&& (obj == null || obj.SentDate == null || x1.Sentdate == obj.SentDate)
                        select new EmailLogsData()
                        {
                            Email = x1.Emailid,
                            //actionName=x1.Action,
                            ConfirmationNumber = x1.Confirmationnumber,
                            RoleName = X2.Name,
                            CreatedDate = x1.Createdate.ToString(),
                            Recipient = (X3 == null) ? (X4 == null) ? (X5 == null) ? null : X5.Firstname + X5.Lastname : X4.Firstname + X4.Lastname : X3.Firstname + X3.Lastname,
                            sent = x1.Isemailsent.ToString(),
                            SentDate = x1.Sentdate.ToString(),
                            sentTrie = x1.Senttries.ToString(),
                        };

            }
            //for sms
            else
            {
                query = from x1 in _db.Smslogs
                        join x2 in _db.Roles on x1.Roleid equals x2.Roleid into temp
                        from X2 in temp.DefaultIfEmpty()
                        join x3 in _db.Requestclients on x1.Requestid equals x3.Requestid into temp2
                        from X3 in temp2.DefaultIfEmpty()
                        join x4 in _db.Admins on x1.Adminid equals x4.Adminid into temp3
                        from X4 in temp3.DefaultIfEmpty()
                        join x5 in _db.Physicians on x1.Physicianid equals x5.Physicianid into temp4
                        from X5 in temp4.DefaultIfEmpty()
                        where
                        (obj == null || obj.RoleId == null || obj.RoleId == x1.Roleid)
                        && (obj == null || obj.Receiver == null || (((X3 == null) ? (X4 == null) ? (X5 == null) ? "" : X5.Firstname + X5.Lastname : X4.Firstname + X4.Lastname : X3.Firstname + X3.Lastname)).ToLower().Contains(obj.Receiver.ToLower()))
                        && (obj == null || obj.mobile == null || x1.Mobilenumber.ToLower().Contains(obj.mobile))
                        && (obj == null || obj.Created == null || x1.Createdate.Date == obj.Created)
                        && (obj == null || obj.SentDate == null || x1.Sentdate == obj.SentDate)
                        select new EmailLogsData()
                        {
                            Mobile = x1.Mobilenumber,
                            //actionName=x1.Action,
                            ConfirmationNumber = x1.Confirmationnumber,
                            RoleName = X2.Name,
                            CreatedDate = x1.Createdate.ToString(),
                            Recipient = (X3 == null) ? (X4 == null) ? (X5 == null) ? null : X5.Firstname + X5.Lastname : X4.Firstname + X4.Lastname : X3.Firstname + X3.Lastname,
                            sent = x1.Issmssent.ToString(),
                            SentDate = x1.Sentdate.ToString(),
                            sentTrie = x1.Senttries.ToString(),
                        };

            }
            EmailAndSMSCombineViewmodel model = new();
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
                        PageNumber = count / 5 + 1;
                    else
                    {
                        PageNumber = 1;
                    }
                }
                page = (int)PageNumber;
            }
            var Paginateddata = query.Skip((page - 1) * 5).Take(5);

            model.emailLogsDatas = Paginateddata;
            model.maxPage = maxPage;
            model.PageNumber = page;
            return await Task.Run(() => model);
        }
        public async Task<IEnumerable<Role>?> getRoles()
        {
            return await Task.Run(() => _db.Roles.Where(x => x.Isdeleted != true));
        }
        public async Task<BlockRecordsViewmodel> BlockHistoryIndex(SearchFilterModel? obj, int? PageNumber)
        {
            IEnumerable<BlockRecords>? query = from x1 in _db.Blockrequests
                                               join x2 in _db.Requests on x1.ReqId equals x2.Requestid into temp
                                               from X2 in temp.DefaultIfEmpty()
                                               join x3 in _db.Requestclients on X2.Requestid equals x3.Requestid into temp2
                                               from X3 in temp2.DefaultIfEmpty()
                                               where
                                               (obj == null || obj.patName == null || ((X3.Firstname + X3.Lastname).ToLower().Contains(obj.patName.ToLower())))
                                                && (obj == null || obj.Email == null || x1.Email == null || x1.Email.ToLower().Contains(obj.Email.ToLower()))
                                                //&& (fromDate == null || x1.Accepteddate == fromDate)
                                                && (obj == null || obj.PhoneNumber == null || x1.Phonenumber == null || x1.Phonenumber.ToLower().Contains(obj.PhoneNumber.ToLower()))
                                                && (obj == null || obj.FromDate == null || x1.Createddate.Date == obj.FromDate)
                                               select new BlockRecords()
                                               {
                                                   FirstName = X3.Firstname + X3.Lastname,
                                                   BlockNotes = x1.Reason,
                                                   CreatedDate = x1.Createddate.ToString(),
                                                   Email = x1.Email,
                                                   PhoneNumber = x1.Phonenumber,
                                                   isactive = x1.Isactive,
                                                   reqid = x1.ReqId,
                                                   Blockid = x1.Blockrequestid
                                               };
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
                        PageNumber = count / 5 + 1;
                    else
                    {
                        PageNumber = 1;
                    }
                }
                page = (int)PageNumber;
            }
            var Paginateddata = query.Skip((page - 1) * 5).Take(5);
       
            BlockRecordsViewmodel  blockRecordsViewmodel= new ();
            blockRecordsViewmodel.maxPage = maxPage;
            blockRecordsViewmodel.PageNumber = page;
            blockRecordsViewmodel.BlockRecords = query;
            return await Task.Run(() => blockRecordsViewmodel);
        }
        public async Task<bool> UnBlockPatient(int reqid, int blockid)
        {
            //change status of block 
            var blockdata = await _db.Blockrequests.FirstOrDefaultAsync(x => x.Blockrequestid == blockid);
            if (blockdata != null)
            {
                blockdata.Isactive = false;
                blockdata.Modifieddate = DateTime.Now;
                _db.Blockrequests.Update(blockdata);
                _db.SaveChanges();
            }
            else
            {
                return false;
            }
            var requestdata = await _db.Requests.FirstOrDefaultAsync(x => x.Requestid == reqid);
            if (requestdata != null)
            {
                requestdata.Status = 1;
                requestdata.Modifieddate = DateTime.Now;
                _db.Requests.Update(requestdata);
                _db.SaveChanges();
                return true;
            }
            return false;
        }
        #endregion
    }
}
