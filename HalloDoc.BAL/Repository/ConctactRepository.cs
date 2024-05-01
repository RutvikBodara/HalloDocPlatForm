using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics.X86;
using System.Security.Permissions;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using HalloDoc.BAL.Interface;
using HalloDoc.DAL.Data;
using HalloDoc.DAL.DataModels;
using HalloDoc.DAL.ViewModals;
using HalloDoc.DAL.ViewModals.Family;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Org.BouncyCastle.Asn1.Cmp;
using Org.BouncyCastle.Crypto.Macs;

namespace HalloDoc.BAL.Repository
{
    /// <summary>
    /// Platform 4 type of Request and some Common Data fetch will take place from this repository
    /// </summary>
    public class ConctactRepository : IContactRepository
    {
        private readonly HalloDocDBContext _db;
        private readonly IAdminPartialsRepo _adminPartialsRepo;
        private readonly IProviderRepo _ProviderRepo;
        public ConctactRepository(HalloDocDBContext db, IAdminPartialsRepo adminPartialsRepo, IProviderRepo providerRepo)
        {
            _db = db;
            _adminPartialsRepo = adminPartialsRepo;
            _ProviderRepo = providerRepo;
        }
        #region FromPatientAccount
        public async Task MeRequest(CreatePatientRequestviewmodel obj, IFormFile? uploadFile)
        {
            IList ApNetUserList = _db.Aspnetusers.ToList();
            IList UserList = _db.Users.ToList();


            int ApNetUserlength = ApNetUserList.Count;
            int Userlength = UserList.Count;
            var usercheck = _db.Aspnetusers.Where(x => x.Username == obj.Email).FirstOrDefault();
            if (usercheck == null)
            {
                return;
            }
            var patUserId = _db.Users.Where(x => x.Aspnetuserid == usercheck.Id).FirstOrDefault();
            if (patUserId == null)
            {
                return;
            }
            var finalUid = patUserId.Userid;

            //request table
            var requestsdata = new Request()
            {
                Requesttypeid = 2,
                Firstname = obj.Firstname,
                Lastname = obj.Lastname,
                Phonenumber = obj.Phonenumber,
                Email = obj.Email,
                Status = 1,
                Userid = finalUid,
                Isdeleted=false,
                Createddate = DateTime.Now,
            };
            _db.Requests.Add(requestsdata);
            await _db.SaveChangesAsync();

            //request clients
            var requestsClientdata = new Requestclient()
            {

                Requestid = requestsdata.Requestid,
                Firstname = obj.Firstname,
                Lastname = obj.Lastname,
                Phonenumber = obj.Phonenumber,
                Notes = obj.Notes,
                Email = obj.Email,
                Street = obj.Street,
                City = obj.City,
                State = obj.State,
                Zipcode = obj.Zipcode,
                Regionid = obj.regid,
                Strmonth = obj.dateofbirth.ToString()
            };
            _db.Requestclients.Add(requestsClientdata);
            await _db.SaveChangesAsync();


            var statuslogs = new Requeststatuslog()
            {
                Requestid = requestsdata.Requestid,
                Status = 1,
                Createddate = DateTime.Now,
            };
            _db.Requeststatuslogs.Add(statuslogs);
            await _db.SaveChangesAsync();

            if (uploadFile != null && uploadFile.Length > 0)
            {
                Guid myuuid = Guid.NewGuid();
                var filename = Path.GetFileName(uploadFile.FileName);
                var FinalFileName = myuuid.ToString() + filename;

                //path

                var filepath = Path.Combine(Environment.CurrentDirectory, "wwwroot", "uploads", FinalFileName);

                //copy in stream

                using (var str = new FileStream(filepath, FileMode.Create))
                {
                    //copy file
                    await uploadFile.CopyToAsync(str);
                }

                //STORE DATA IN TABLE
                var fileupload = new Requestwisefile()
                {

                    Requestid = requestsdata.Requestid,
                    Filename = FinalFileName,
                    Createddate = DateTime.Now,
                };

                _db.Requestwisefiles.Add(fileupload);
                await _db.SaveChangesAsync();
            }
        }
        public async Task SomeOneRequest(FriendRequest obj, string ASPID)
        {
            IList ApNetUserList = _db.Aspnetusers.ToList();
            IList UserList = _db.Users.ToList();

            int ApNetUserlength = ApNetUserList.Count;
            int Userlength = UserList.Count;

            var userFriendcheck = _db.Aspnetusers.Where(x => x.Username == obj.PatientEmail).FirstOrDefault();
            var requesterdata = _db.Users.Where(x => x.Aspnetuserid == ASPID).FirstOrDefault();

            if (requesterdata == null)
            {
                return;
            }
            var guid = Guid.NewGuid();
            if (userFriendcheck == null)
            {

                //var HASHPASS = BCrypt.Net.BCrypt.HashPassword(obj.password);
                var AspNetUserFrienddata = new Aspnetuser()
                {
                    Id = guid.ToString(),
                    Username = obj.PatientEmail,
                    //Passwordhash = HASHPASS,
                    Email = obj.PatientEmail,
                    Phonenumber = obj.PatientPhonenumber,
                    Createddate = DateTime.Now,
                    Accounttype = 3
                };
                _db.Aspnetusers.Add(AspNetUserFrienddata);
                _db.SaveChanges();

                //aspnetusers
                var usersdata = new User()
                {
                    Userid = Userlength + 1,
                    Aspnetuserid = AspNetUserFrienddata.Id,
                    Firstname = obj.patientFirstname,
                    Lastname = obj.patientLastname,
                    Email = obj.PatientEmail,
                    Mobile = obj.PatientPhonenumber,
                    Street = obj.Street,
                    City = obj.City,
                    State = obj.State,
                    Regionid = obj.regid,
                    Zipcode = obj.Zipcode,
                    Createddate = DateTime.Now,
                    Createdby = requesterdata.Email,
                    Strmonth = obj.dateofbirth.ToString()
                };
                _db.Users.Add(usersdata);
                _db.SaveChanges();

                var aspnetroles = new Aspnetuserrole()
                {
                    Userid = AspNetUserFrienddata.Id,
                    Name = "patient"
                };
                _db.Aspnetuserroles.Add(aspnetroles);
                _db.SaveChanges();

                var requestsdata = new Request()
                {
                    Requesttypeid = 3,
                    Firstname = requesterdata.Firstname,
                    Lastname = requesterdata.Lastname,
                    Phonenumber = requesterdata.Mobile,
                    Email = requesterdata.Email,
                    Status = 1,
                    Isdeleted = false,
                    //if want to show on requestor history
                    //Userid = requesterdata.Userid,
                    Userid = usersdata.Userid,
                    Createddate = DateTime.Now,
                };
                _db.Requests.Add(requestsdata);
                _db.SaveChanges();

                var requestsClientdata = new Requestclient()
                {
                    Requestid = requestsdata.Requestid,
                    Firstname = obj.patientFirstname,
                    Lastname = obj.patientLastname,
                    Phonenumber = obj.PatientPhonenumber,
                    Notes = obj.patientNotes,
                    Email = obj.PatientEmail,
                    Street = obj.Street,
                    City = obj.City,
                    State = obj.State,
                    Regionid = obj.regid,
                    Zipcode = obj.Zipcode,
                };
                _db.Requestclients.Add(requestsClientdata);
                _db.SaveChanges();

                var statuslogs = new Requeststatuslog()
                {
                    Requestid = requestsdata.Requestid,
                    Status = 1,
                    Createddate = DateTime.Now,
                };
                _db.Requeststatuslogs.Add(statuslogs);
                _db.SaveChanges();


                if (obj.uploadFile != null && obj.uploadFile.Length > 0)
                {
                    Guid myuuid = Guid.NewGuid();
                    var filename = Path.GetFileName(obj.uploadFile.FileName);
                    var FinalFileName = myuuid.ToString() + filename;

                    //path

                    var filepath = Path.Combine(Environment.CurrentDirectory, "wwwroot", "uploads", FinalFileName);

                    //copy in stream

                    using (var str = new FileStream(filepath, FileMode.Create))
                    {
                        //copy file
                        obj.uploadFile.CopyTo(str);
                    }

                    //STORE DATA IN TABLE
                    var fileupload = new Requestwisefile()
                    {

                        Requestid = requestsdata.Requestid,
                        Filename = FinalFileName,
                        Createddate = DateTime.Now,
                    };




                    _db.Requestwisefiles.Add(fileupload);
                    _db.SaveChanges();
                }
                //send email to the registered
                string from = "hallodocpms@gmail.com";
                string to = obj.PatientEmail;
                string subject = "Regiter yourself";
                var link = $"https://localhost:44313/PatientSide/Register ";
                string body = $"Hi,<br /><br />Please click on the following link to Register on HelloDoc:<br /><br/><a>{link}<a> ";
                bool status = await _adminPartialsRepo.sendEmail(from, to, subject, body, null, requestsdata.Requestid, null, null);
            }
            else
            {

                //common things
                var patUserId = _db.Users.Where(x => x.Aspnetuserid == userFriendcheck.Id).FirstOrDefault();

                if (patUserId == null)
                {
                    return;
                }
                var finalUid = patUserId.Userid;


                //find id of patient and put the id

                var requestsdata = new Request()
                {
                    Requesttypeid = 3,
                    Firstname = requesterdata.Firstname,
                    Lastname = requesterdata.Lastname,
                    Phonenumber = requesterdata.Mobile,
                    Email = requesterdata.Email,
                    Status = 1,
                    Userid = finalUid,
                    Isdeleted = false,
                    Createddate = DateTime.Now,

                };
                _db.Requests.Add(requestsdata);
                _db.SaveChanges();

                var requestsClientdata = new Requestclient()
                {
                    Requestid = requestsdata.Requestid,
                    Firstname = obj.patientFirstname,
                    Lastname = obj.patientLastname,
                    Phonenumber = obj.PatientPhonenumber,
                    Notes = obj.patientNotes,
                    Email = obj.PatientEmail,
                    Street = obj.Street,
                    City = obj.City,
                    State = obj.State,
                    Regionid = obj.regid,
                    Zipcode = obj.Zipcode,
                    Strmonth = obj.dateofbirth.ToString()
                };
                _db.Requestclients.Add(requestsClientdata);
                _db.SaveChanges();

                var statuslogs = new Requeststatuslog()
                {
                    Requestid = requestsdata.Requestid,
                    Status = 1,
                    Createddate = DateTime.Now,
                };
                _db.Requeststatuslogs.Add(statuslogs);
                _db.SaveChanges();


                if (obj.uploadFile != null && obj.uploadFile.Length > 0)
                {
                    Guid myuuid = Guid.NewGuid();
                    var filename = Path.GetFileName(obj.uploadFile.FileName);
                    var FinalFileName = myuuid.ToString() + filename;

                    //path

                    var filepath = Path.Combine(Environment.CurrentDirectory, "wwwroot", "uploads", FinalFileName);

                    //copy in stream

                    using (var str = new FileStream(filepath, FileMode.Create))
                    {
                        //copy file
                        obj.uploadFile.CopyTo(str);
                    }

                    //STORE DATA IN TABLE
                    var fileupload = new Requestwisefile()
                    {

                        Requestid = requestsdata.Requestid,
                        Filename = FinalFileName,
                        Createddate = DateTime.Now,
                    };

                    _db.Requestwisefiles.Add(fileupload);
                    _db.SaveChanges();
                }
            }
        }
        #endregion
        #region 4TypeOfRequest
        public async Task<bool> ADDPatientRequestData(CreatePatientRequestviewmodel obj, string? createdBy, int? requestType)
        {

            IList ApNetUserList = _db.Aspnetusers.ToList();
            IList UserList = _db.Users.ToList();
            //request table

            //          confirmation number:it will show the confirmation number for patient request which
            //was created at the time of submitting a request.It is created by
            //the patient’s region and datetime of submit a request.It will be
            //unique for each patient. The first 2 characters will represent the
            //region abbreviation, then next 4 numbers will represent the date
            //of created date, then next 2 characters will represent first 2
            //characters of last - name, then next 2 characters will represent
            //first 2 characters of first - name, then next 4 dig

            //get state
            var stateData = _db.Regions.FirstOrDefault(x => x.Regionid == obj.regid);
            if(stateData == null)
            {
                return false;
            }
            obj.State = stateData.Name;
            if (obj.State == null)
            {
                obj.State = "XX";
            }
            if (obj.Lastname == null)
            {
                obj.Lastname = "XX";
            }
            var totalDayrequest = _db.Requests.Where(x => x.Createddate.Date == DateTime.Now.Date).Count();
            var uniqueNum = "";
            if (totalDayrequest <= 9)
            {
                uniqueNum = "000" + totalDayrequest + 1;
            }
            if (totalDayrequest <= 99)
            {
                uniqueNum = "00" + totalDayrequest + 1;
            }
            if (totalDayrequest <= 999)
            {
                uniqueNum = "0" + totalDayrequest + 1;
            }
            else
            {
                uniqueNum = totalDayrequest.ToString();
            }
            var uniqueNumber = obj.State[..2] + DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + obj.Lastname[..2] + obj.Firstname[..2] + uniqueNum;

            int ApNetUserlength = ApNetUserList.Count + 1;
            int Userlength = UserList.Count;

            var usercheck = _db.Aspnetusers.Where(x => x.Username == obj.Email).FirstOrDefault();
            String? HASHPASS = null;
            if (obj.password != null)
            {
                HASHPASS = BCrypt.Net.BCrypt.HashPassword(obj.password);
            }
            //aspnetusers
            if (usercheck == null)
            {

                var AspNetUserdata = new Aspnetuser()
                {
                    Id = ApNetUserlength.ToString(),
                    Username = obj.Email,
                    Passwordhash = HASHPASS,
                    Email = obj.Email,
                    Phonenumber = obj.CountryCode + obj.Phonenumber,
                    Createddate = DateTime.Now,
                    Accounttype = 3
                };
                _db.Aspnetusers.Add(AspNetUserdata);
                _db.SaveChanges();
                //aspnetusers
                var usersdata = new User()
                {
                    Userid = Userlength + 1,
                    Aspnetuserid = AspNetUserdata.Id,
                    Firstname = obj.Firstname,
                    Lastname = obj.Lastname,
                    Email = AspNetUserdata.Email,
                    Mobile = AspNetUserdata.Phonenumber,
                    Street = obj.Street,
                    City = obj.City,
                    State = obj.State,
                    Zipcode = obj.Zipcode,
                    Createddate = DateTime.Now,
                    Strmonth = obj.dateofbirth.ToString(),
                    Regionid = obj.regid,
                };
                Admin? admin = null;
                Physician? physician = null;
                if (requestType == 1)
                {
                    admin = _db.Admins.FirstOrDefault(x => x.Aspnetuserid == createdBy);
                    if (admin == null)
                    {
                        return false;
                    }
                    if (admin.Aspnetuserid == null)
                    {
                        return false;
                    }
                    usersdata.Createdby = admin.Aspnetuserid;
                }
                //provider
                else if (requestType == 2 && createdBy != null)
                {

                    physician = _db.Physicians.FirstOrDefault(x => x.Physicianid == int.Parse(createdBy));
                    if (physician == null)
                    {
                        return false;
                    }
                    if (physician.Aspnetuserid == null)
                    {
                        return false;
                    }
                    usersdata.Createdby = physician.Aspnetuserid;
                }
                else
                {
                    //patient creates itself
                    usersdata.Createdby = obj.Email;
                }
                _db.Users.Add(usersdata);
                _db.SaveChanges();

                var aspnetroles = new Aspnetuserrole()
                {
                    Userid = AspNetUserdata.Id,
                    Name = "patient"
                };
                _db.Aspnetuserroles.Add(aspnetroles);
                _db.SaveChanges();

                var requestsdata = new Request()
                {
                    Requesttypeid = 2,
                    Firstname = obj.Firstname,
                    Lastname = obj.Lastname,
                    Phonenumber = obj.CountryCode + obj.Phonenumber,
                    Email = obj.Email,
                    Status = 1,
                    Isdeleted = false,
                    Userid = usersdata.Userid,
                    Createddate = DateTime.Now,
                    Confirmationnumber = uniqueNumber
                };
                _db.Requests.Add(requestsdata);
                _db.SaveChanges();

                //request clients
                var requestsClientdata = new Requestclient()
                {

                    Requestid = requestsdata.Requestid,
                    Firstname = obj.Firstname,
                    Lastname = obj.Lastname,
                    Phonenumber = obj.CountryCode + obj.Phonenumber,
                    Notes = obj.Notes,
                    Email = obj.Email,
                    Street = obj.Street,
                    Regionid = obj.regid,
                    Strmonth = obj.dateofbirth.ToString(),
                    City = obj.City,
                    State = obj.State,
                    Zipcode = obj.Zipcode,
                };
                _db.Requestclients.Add(requestsClientdata);
                _db.SaveChanges();

                var statuslogs = new Requeststatuslog()
                {
                    Requestid = requestsdata.Requestid,
                    Status = 1,
                    Createddate = DateTime.Now,
                };
                _db.Requeststatuslogs.Add(statuslogs);
                _db.SaveChanges();

                if (obj.uploadFile != null && obj.uploadFile.Length > 0)
                {
                    Guid myuuid = Guid.NewGuid();
                    var filename = Path.GetFileName(obj.uploadFile.FileName);
                    var FinalFileName = myuuid.ToString() + filename;

                    //path

                    var filepath = Path.Combine(Environment.CurrentDirectory, "wwwroot", "uploads", FinalFileName);

                    //copy in stream

                    using (var str = new FileStream(filepath, FileMode.Create))
                    {
                        //copy file
                        obj.uploadFile.CopyTo(str);
                    }

                    //STORE DATA IN TABLE
                    var fileupload = new Requestwisefile()
                    {

                        Requestid = requestsdata.Requestid,
                        Filename = FinalFileName,
                        Createddate = DateTime.Now,
                    };

                    _db.Requestwisefiles.Add(fileupload);
                    _db.SaveChanges();
                }

                if (requestType == 2 && physician != null)
                {
                    //assign case from here
                    await _adminPartialsRepo.AssignPhysician(obj.regid, physician.Physicianid, obj.PhysicianNotes, requestsdata.Requestid, physician.Physicianid, "assignPhysician", 2);
                    bool status = await _ProviderRepo.AcceptCaseByProvider(requestsdata.Requestid);
                    //accept by default case
                    return status;
                }

                if (requestType == 1 && admin != null)
                {
                    //send email if not registered
                    string from = "hallodocpms@gmail.com";
                    string to = obj.Email;
                    string subject = "Register Your Self";
                    var link = $"https://localhost:44313/PatientSide/Register ";
                    string body = $"Hi,<br /><br />Please click on the following link to Register on HelloDoc:<br /><br/><a href={link}>{link}<a>";
                    bool status = await _adminPartialsRepo.sendEmail(from, to, subject, body, null, requestsdata.Requestid, null, null);
                }
                //request notes for admin
                if (requestType == 1 && admin != null && obj.AdminNotes != null)
                {
                    await _adminPartialsRepo.AddAdminNotes(obj.AdminNotes, requestsdata.Requestid, admin.Adminid);
                }

                return true;
            }
            else
            {
                //common things
                var patUserId = _db.Users.Where(x => x.Aspnetuserid == usercheck.Id).FirstOrDefault();
                if (patUserId == null) { return false; }
                var finalUid = patUserId.Userid;

                //request table
                var requestsdata = new Request()
                {
                    Requesttypeid = 2,
                    Firstname = obj.Firstname,
                    Lastname = obj.Lastname,
                    Phonenumber = obj.CountryCode + obj.Phonenumber,
                    Email = obj.Email,
                    Status = 1,
                    Confirmationnumber = uniqueNumber,
                    Userid = finalUid,
                    Createddate = DateTime.Now,
                    Isdeleted = false,
                };
                _db.Requests.Add(requestsdata);
                _db.SaveChanges();

                //request clients
                var requestsClientdata = new Requestclient()
                {

                    Requestid = requestsdata.Requestid,
                    Firstname = obj.Firstname,
                    Lastname = obj.Lastname,
                    Phonenumber = obj.CountryCode + obj.Phonenumber,
                    Notes = obj.Notes,
                    Email = obj.Email,
                    Street = obj.Street,
                    Strmonth = obj.dateofbirth.ToString(),
                    Regionid = obj.regid,
                    City = obj.City,
                    State = obj.State,
                    Zipcode = obj.Zipcode,
                };
                _db.Requestclients.Add(requestsClientdata);
                _db.SaveChanges();


                var statuslogs = new Requeststatuslog()
                {
                    Requestid = requestsdata.Requestid,
                    Status = 1,
                    Createddate = DateTime.Now,
                };
                _db.Requeststatuslogs.Add(statuslogs);
                _db.SaveChanges();


                if (obj.uploadFile != null && obj.uploadFile.Length > 0)
                {
                    Guid myuuid = Guid.NewGuid();
                    var filename = Path.GetFileName(obj.uploadFile.FileName);
                    var FinalFileName = myuuid.ToString() + filename;

                    //path

                    var filepath = Path.Combine(Environment.CurrentDirectory, "wwwroot", "uploads", FinalFileName);

                    //copy in stream

                    using (var str = new FileStream(filepath, FileMode.Create))
                    {
                        //copy file
                        obj.uploadFile.CopyTo(str);
                    }

                    //STORE DATA IN TABLE
                    var fileupload = new Requestwisefile()
                    {

                        Requestid = requestsdata.Requestid,
                        Filename = FinalFileName,
                        Createddate = DateTime.Now,
                    };
                    _db.Requestwisefiles.Add(fileupload);
                    _db.SaveChanges();
                }
                if (requestType == 2 && createdBy != null)
                {
                    //assign case from here
                    await _adminPartialsRepo.AssignPhysician(obj.regid, int.Parse(createdBy), obj.PhysicianNotes, requestsdata.Requestid, int.Parse(createdBy), "assignPhysician", 2);
                    bool status = await _ProviderRepo.AcceptCaseByProvider(requestsdata.Requestid);
                    //accept by default case
                    return status;
                }
                return true;
            }
        }
        public async Task AddFamilyFriendData(FriendRequest obj)
        {
            IList ApNetUserList = _db.Aspnetusers.ToList();
            IList UserList = _db.Users.ToList();
            var stateData = _db.Regions.FirstOrDefault(x => x.Regionid == obj.regid);
            if (stateData == null)
            {
                return;
            }
            obj.State = stateData.Name;
            if (obj.State == null)
            {
                obj.State = "XX";
            }
            if (obj.Lastname == null)
            {
                obj.Lastname = "XX";
            }
            if (obj.Firstname == null)
            {
                obj.Firstname = "XX";
            }
            var totalDayrequest = _db.Requests.Where(x => x.Createddate.Date == DateTime.Now.Date).Count();
            var uniqueNum = "";
            if (totalDayrequest <= 9)
            {
                uniqueNum = "000" + totalDayrequest + 1;
            }
            if (totalDayrequest <= 99)
            {
                uniqueNum = "00" + totalDayrequest + 1;
            }
            if (totalDayrequest <= 999)
            {
                uniqueNum = "0" + totalDayrequest + 1;
            }
            else
            {
                uniqueNum = totalDayrequest.ToString();
            }
            var uniqueNumber = obj.State[..2] + DateTime.Now.Day.ToString() + DateTime.Now.Month + obj.Lastname[..2] + obj.Firstname[..2] + uniqueNum;

            int ApNetUserlength = ApNetUserList.Count + 1;
            int Userlength = UserList.Count;

            var userFriendcheck = _db.Aspnetusers.Where(x => x.Username == obj.PatientEmail).FirstOrDefault();

            var GetCountry = obj.FamilyCountryCode.Split("+");
            obj.FamilyCountryCode = GetCountry[1];
            obj.PatientCountryCode = GetCountry[2];


            if (userFriendcheck == null)
            {
                //var HASHPASS = BCrypt.Net.BCrypt.HashPassword(obj.password);
                var AspNetUserFrienddata = new Aspnetuser()
                {
                    Id = ApNetUserlength.ToString(),
                    Username = obj.PatientEmail,
                    //update once user complete reg..
                    //Passwordhash = HASHPASS,
                    Email = obj.PatientEmail,
                    Phonenumber ="+"+obj.PatientCountryCode+ " "+obj.PatientPhonenumber,
                    Createddate = DateTime.Now,
                    Accounttype = 3
                };
                _db.Aspnetusers.Add(AspNetUserFrienddata);
                _db.SaveChanges();

                //aspnetusers
                var usersdata = new User()
                {
                    Userid = Userlength + 1,
                    Aspnetuserid = AspNetUserFrienddata.Id,
                    Firstname = obj.patientFirstname,
                    Lastname = obj.patientLastname,
                    Email = obj.PatientEmail,
                    Mobile = "+" + obj.PatientCountryCode + obj.PatientPhonenumber,
                    Street = obj.Street,
                    City = obj.City,
                    State = obj.State,
                    Zipcode = obj.Zipcode,
                    Strmonth = obj.dateofbirth.ToString(),
                    Createddate = DateTime.Now,
                    Createdby = obj.Email,
                    Regionid = obj.regid,
                };
                _db.Users.Add(usersdata);
                _db.SaveChanges();

                var aspnetroles = new Aspnetuserrole()
                {
                    Userid = AspNetUserFrienddata.Id,
                    Name = "patient"
                };

                _db.Aspnetuserroles.Add(aspnetroles);
                _db.SaveChanges();

                var requestsdata = new Request()
                {
                    Requesttypeid = 3,
                    Firstname = obj.Firstname,
                    Lastname = obj.Lastname,
                    Phonenumber = "+" + obj.FamilyCountryCode + obj.Phonenumber,
                    Email = obj.Email,
                    Status = 1,
                    Userid = usersdata.Userid,
                    Isdeleted = false,
                    Createddate = DateTime.Now,
                    Confirmationnumber = uniqueNumber
                };
                _db.Requests.Add(requestsdata);
                _db.SaveChanges();

                var requestsClientdata = new Requestclient()
                {
                    Requestid = requestsdata.Requestid,
                    Firstname = obj.patientFirstname,
                    Lastname = obj.patientLastname,
                    Phonenumber = "+" + obj.PatientCountryCode + obj.PatientPhonenumber,
                    Notes = obj.patientNotes,
                    Email = obj.PatientEmail,
                    Regionid = obj.regid,
                    Strmonth = obj.dateofbirth.ToString(),
                    Street = obj.Street,
                    City = obj.City,
                    State = obj.State,
                    Zipcode = obj.Zipcode,
                };
                _db.Requestclients.Add(requestsClientdata);
                _db.SaveChanges();

                var statuslogs = new Requeststatuslog()
                {
                    Requestid = requestsdata.Requestid,
                    Status = 1,
                    Createddate = DateTime.Now,
                };
                _db.Requeststatuslogs.Add(statuslogs);
                _db.SaveChanges();


                if (obj.uploadFile != null && obj.uploadFile.Length > 0)
                {
                    Guid myuuid = Guid.NewGuid();
                    var filename = Path.GetFileName(obj.uploadFile.FileName);
                    var FinalFileName = myuuid.ToString() + filename;

                    //path

                    var filepath = Path.Combine(Environment.CurrentDirectory, "wwwroot", "uploads", FinalFileName);

                    //copy in stream

                    using (var str = new FileStream(filepath, FileMode.Create))
                    {
                        //copy file
                        obj.uploadFile.CopyTo(str);
                    }

                    //STORE DATA IN TABLE
                    var fileupload = new Requestwisefile()
                    {
                        Requestid = requestsdata.Requestid,
                        Filename = FinalFileName,
                        Createddate = DateTime.Now,
                    };
                    _db.Requestwisefiles.Add(fileupload);
                    _db.SaveChanges();
                }


                string from = "hallodocpms@gmail.com";
                string to = obj.PatientEmail;
                string subject = "Register Your Self";
                var link = $"https://localhost:44313/PatientSide/Register ";
                string body = $"Hi,<br /><br />Please click on the following link to Register on HelloDoc:<br /><br/><a href={link}>{link}<a>";
                bool status = await _adminPartialsRepo.sendEmail(from, to, subject, body, null, requestsdata.Requestid, null, null);
            }
            else
            {

                //common things
                var patUserId = _db.Users.Where(x => x.Aspnetuserid == userFriendcheck.Id).FirstOrDefault();

                if (patUserId == null)
                {
                    return;
                }
                var finalUid = patUserId.Userid;

                //find id of patient and put the id

                var requestsdata = new Request()
                {
                    Requesttypeid = 3,
                    Firstname = obj.Firstname,
                    Lastname = obj.Lastname,
                    Phonenumber = "+" + obj.FamilyCountryCode + obj.Phonenumber,
                    Email = obj.Email,
                    Status = 1,
                    Userid = finalUid,
                    Createddate = DateTime.Now,
                    Confirmationnumber = uniqueNumber

                };
                _db.Requests.Add(requestsdata);
                _db.SaveChanges();

                var requestsClientdata = new Requestclient()
                {
                    Requestid = requestsdata.Requestid,
                    Firstname = obj.patientFirstname,
                    Lastname = obj.patientLastname,
                    Phonenumber = "+" + obj.PatientCountryCode + obj.PatientPhonenumber,
                    Notes = obj.patientNotes,
                    Email = obj.PatientEmail,
                    Street = obj.Street,
                    City = obj.City,
                    State = obj.State,
                    Zipcode = obj.Zipcode,
                };
                _db.Requestclients.Add(requestsClientdata);
                _db.SaveChanges();

                var statuslogs = new Requeststatuslog()
                {
                    Requestid = requestsdata.Requestid,
                    Status = 1,
                    Createddate = DateTime.Now,
                };
                _db.Requeststatuslogs.Add(statuslogs);
                _db.SaveChanges();


                if (obj.uploadFile != null && obj.uploadFile.Length > 0)
                {
                    Guid myuuid = Guid.NewGuid();
                    var filename = Path.GetFileName(obj.uploadFile.FileName);
                    var FinalFileName = myuuid.ToString() + filename;

                    //path

                    var filepath = Path.Combine(Environment.CurrentDirectory, "wwwroot", "uploads", FinalFileName);

                    //copy in stream

                    using (var str = new FileStream(filepath, FileMode.Create))
                    {
                        //copy file
                        obj.uploadFile.CopyTo(str);
                    }

                    //STORE DATA IN TABLE
                    var fileupload = new Requestwisefile()
                    {

                        Requestid = requestsdata.Requestid,
                        Filename = FinalFileName,
                        Createddate = DateTime.Now,
                    };

                    _db.Requestwisefiles.Add(fileupload);
                    _db.SaveChanges();
                }
            }

        }
        public async Task ADDConciergeData(ConciergeRequestviewmodel obj)
        {
            IList ApNetUserList = _db.Aspnetusers.ToList();
            IList UserList = _db.Users.ToList();
            IList RequestConciergeList = _db.Requestconcierges.ToList();
            IList conciergeList = _db.Concierges.ToList();
            if (obj.Street == null)
            {
                obj.Street = "XX";
            }
            if (obj.Lastname == null)
            {
                obj.Lastname = "XX";
            }
            var totalDayrequest = _db.Requests.Where(x => x.Createddate.Date == DateTime.Now.Date).Count();
            var uniqueNum = "";
            if (totalDayrequest <= 9)
            {
                uniqueNum = "000" + totalDayrequest + 1;
            }
            if (totalDayrequest <= 99)
            {
                uniqueNum = "00" + totalDayrequest + 1;
            }
            if (totalDayrequest <= 999)
            {
                uniqueNum = "0" + totalDayrequest + 1;
            }
            else
            {
                uniqueNum = totalDayrequest.ToString();
            }
            var uniqueNumber = obj.Street[..2] + DateTime.Now.Day.ToString() + DateTime.Now.Month + obj.Lastname[..2] + obj.Firstname[..2] + uniqueNum;


            int ApNetUserlength = ApNetUserList.Count + 1;
            int Userlength = UserList.Count;
            int RequestConciergeListLength = RequestConciergeList.Count;
            int conciergeListLength = conciergeList.Count;


            //var HASHPASS = BCrypt.Net.BCrypt.HashPassword(obj.password
            var GetCountry = obj.ConciergeCountryCode.Split("+");
            obj.ConciergeCountryCode = GetCountry[1];
            obj.PatientCountryCode = GetCountry[2];

            //var HASHPASS = BCrypt.Net.BCrypt.HashPassword(obj.password);

            var userFriendcheck = _db.Aspnetusers.Where(x => x.Username == obj.Email).FirstOrDefault();
            if (userFriendcheck == null)
            {

                var AspNetUserCociergeData = new Aspnetuser()
                {
                    Id = ApNetUserlength.ToString(),
                    Username = obj.Email,
                    //Passwordhash = HASHPASS,
                    Email = obj.Email,
                    Phonenumber ="+"+obj.PatientCountryCode + obj.Phonenumber,
                    Createddate = DateTime.Now,
                    Accounttype = 3

                };
                _db.Aspnetusers.Add(AspNetUserCociergeData);
                _db.SaveChanges();

                //aspnetusers
                var usersdata = new User()
                {
                    Userid = Userlength + 1,
                    Aspnetuserid = AspNetUserCociergeData.Id,
                    Firstname = obj.Firstname,
                    Lastname = obj.Lastname,
                    Email = obj.Email,
                    Strmonth = obj.dateofbirth.ToString(),
                    Mobile = "+" + obj.PatientCountryCode + obj.Phonenumber,
                    Createddate = DateTime.Now,
                    Createdby = obj.ConciergeEmail,
                    Regionid = obj.regid,
                };
                _db.Users.Add(usersdata);
                _db.SaveChanges();


                var aspnetroles = new Aspnetuserrole()
                {
                    Userid = AspNetUserCociergeData.Id,
                    Name = "patient"
                };
                _db.Aspnetuserroles.Add(aspnetroles);
                _db.SaveChanges();
                //common things
                var regions = _db.Regions.FirstOrDefault(x => x.Regionid == obj.regid);
                if (regions == null)
                {
                    return;
                }
                var conciergedata = new Concierge()
                {
                    Conciergeid = conciergeListLength + 1,
                    Conciergename = obj.Conciergename,
                    Address = obj.Address,
                    Street = obj.Street,
                    City = obj.City,
                    State = regions.Name,
                    Zipcode = obj.Zipcode.ToString(),
                    Createddate = DateTime.Now,
                };

                _db.Concierges.Add(conciergedata);
                _db.SaveChanges();

                var requestsdata = new Request()
                {
                    Requesttypeid = 4,
                    Firstname = obj.Conciergename,
                    Lastname = obj.Conciergelastname,
                    Phonenumber = "+" + obj.ConciergeCountryCode + obj.ConciergePhonenumber,
                    Email = obj.ConciergeEmail,
                    Status = 1,
                    Isdeleted = false,
                    Userid = usersdata.Userid,
                    Createddate = DateTime.Now,
                    Confirmationnumber = uniqueNumber

                };
                _db.Requests.Add(requestsdata);
                _db.SaveChanges();

                var statuslogs = new Requeststatuslog()
                {
                    Requestid = requestsdata.Requestid,
                    Status = 1,
                    Createddate = DateTime.Now,
                };
                _db.Requeststatuslogs.Add(statuslogs);
                _db.SaveChanges();

                var requestsClientdata = new Requestclient()
                {
                    Requestid = requestsdata.Requestid,
                    Firstname = obj.Firstname,
                    Lastname = obj.Lastname,
                    Phonenumber = "+" + obj.PatientCountryCode + obj.Phonenumber,
                    Notes = obj.Notes,
                    Regionid = obj.regid,
                    Strmonth = obj.dateofbirth.ToString(),
                    Email = obj.Email,

                };
                _db.Requestclients.Add(requestsClientdata);
                _db.SaveChanges();

                var RequestConciergedata = new Requestconcierge()
                {
                    Id = RequestConciergeListLength + 1,
                    Requestid = requestsdata.Requestid,
                    Conciergeid = conciergedata.Conciergeid
                };
                _db.Requestconcierges.Add(RequestConciergedata);
                _db.SaveChanges();

                string from = "hallodocpms@gmail.com";
                string to = obj.Email;
                string subject = "Register your self";
                var link = $"https://localhost:44313/PatientSide/Register ";
                string body = $"Hi,<br /><br />Please click on the following link to Register on HelloDoc:<br /><br/> <a href={link}>{link}<a>";
                bool status = await _adminPartialsRepo.sendEmail(from, to, subject, body, null, requestsdata.Requestid, null, null);

            }
            else
            {
                //common things
                var patUserId = _db.Users.Where(x => x.Aspnetuserid == userFriendcheck.Id).FirstOrDefault();

                if (patUserId == null) { return; }
                var finalUid = patUserId.Userid;
                var regions = _db.Regions.FirstOrDefault(x => x.Regionid == obj.regid);
                if (regions == null)
                {
                    return;
                }
                var conciergedata = new Concierge()
                {
                    Conciergeid = conciergeListLength + 1,
                    Conciergename = obj.Conciergename,
                    Address = obj.Address,
                    Street = obj.Street,
                    City = obj.City,
                    State = regions.Name,
                    Zipcode = obj.Zipcode.ToString(),
                    Createddate = DateTime.Now,
                };

                _db.Concierges.Add(conciergedata);
                _db.SaveChanges();

                var requestsdata = new Request()
                {
                    Requesttypeid = 4,
                    Firstname = obj.Conciergename,
                    Lastname = obj.Conciergelastname,
                    Phonenumber = "+" + obj.ConciergeCountryCode + obj.ConciergePhonenumber,
                    Email = obj.ConciergeEmail,
                    Status = 1,
                    Createddate = DateTime.Now,
                    Isdeleted = false,
                    Userid = finalUid,
                    Confirmationnumber = uniqueNumber
                };
                _db.Requests.Add(requestsdata);
                _db.SaveChanges();

                var statuslogs = new Requeststatuslog()
                {
                    Requestid = requestsdata.Requestid,
                    Status = 1,
                    Createddate = DateTime.Now,
                };
                _db.Requeststatuslogs.Add(statuslogs);
                _db.SaveChanges();

                var requestsClientdata = new Requestclient()
                {
                    Requestid = requestsdata.Requestid,
                    Firstname = obj.Firstname,
                    Lastname = obj.Lastname,
                    Phonenumber = "+" + obj.PatientCountryCode + obj.Phonenumber,
                    Notes = obj.Notes,
                    Email = obj.Email,
                    Strmonth = obj.dateofbirth.ToString(),
                    Regionid = obj.regid,
                };
                _db.Requestclients.Add(requestsClientdata);
                _db.SaveChanges();

                var RequestConciergedata = new Requestconcierge()
                {
                    Id = RequestConciergeListLength + 1,
                    Requestid = requestsdata.Requestid,
                    Conciergeid = conciergedata.Conciergeid
                };
                _db.Requestconcierges.Add(RequestConciergedata);
                _db.SaveChanges();

            }

        }
        public async Task ADDBusinessData(BusinessRequestviewmodel obj)
        {
            IList ApNetUserList = _db.Aspnetusers.ToList();
            IList UserList = _db.Users.ToList();

            var stateData = _db.Regions.FirstOrDefault(x => x.Regionid == obj.regid);
            if (stateData == null)
            {
                return;
            }
            obj.State = stateData.Name;
            if (obj.State == null)
            {
                obj.State = "XX";
            }
            if (obj.PatientLastname == null)
            {
                obj.PatientLastname = "XX";
            }
            var totalDayrequest = _db.Requests.Where(x => x.Createddate.Date == DateTime.Now.Date).Count();
            var uniqueNum = "";
            if (totalDayrequest <= 9)
            {
                uniqueNum = "000" + totalDayrequest + 1;
            }
            if (totalDayrequest <= 99)
            {
                uniqueNum = "00" + totalDayrequest + 1;
            }
            if (totalDayrequest <= 999)
            {
                uniqueNum = "0" + totalDayrequest + 1;
            }
            else
            {
                uniqueNum = totalDayrequest.ToString();
            }
            var uniqueNumber = obj.State[..2] + DateTime.Now.Day.ToString() + DateTime.Now.Month + obj.PatientLastname[..2] + obj.PatientFirstname[..2] + uniqueNum;
            int ApNetUserlength = ApNetUserList.Count + 1;
            int Userlength = UserList.Count;

            //var HASHPASS = BCrypt.Net.BCrypt.HashPassword(obj.password
            var GetCountry = obj.BusinessCountryCode.Split("+");
            obj.BusinessCountryCode = GetCountry[1];
            obj.PatientCountryCode = GetCountry[2];


            var userBusinesscheck = _db.Aspnetusers.Where(x => x.Username == obj.PatientEmail).FirstOrDefault();
            if (userBusinesscheck == null)
            {
                var AspNetUserBusinessData = new Aspnetuser()
                {
                    Id = ApNetUserlength.ToString(),
                    Username = obj.PatientEmail,
                    //Passwordhash = HASHPASS,
                    Email = obj.PatientEmail,
                    Phonenumber = "+" + obj.PatientCountryCode + " " + obj.PatientPhonenumber,
                    Createddate = DateTime.Now,
                    Accounttype = 3
                };
                _db.Aspnetusers.Add(AspNetUserBusinessData);
                _db.SaveChanges();

                var usersdata = new User()
                {
                    Userid = Userlength + 1,
                    Aspnetuserid = AspNetUserBusinessData.Id,
                    Firstname = obj.PatientFirstname,
                    Strmonth = obj.dateofbirth.ToString(),
                    Lastname = obj.PatientLastname,
                    Email = obj.PatientEmail,
                    Mobile = "+" + obj.PatientCountryCode + " " + obj.PatientPhonenumber,
                    Createddate = DateTime.Now,
                    Createdby = obj.BusinessEmail,
                    Regionid = obj.regid,
                };
                _db.Users.Add(usersdata);
                _db.SaveChanges();

                var aspnetroles = new Aspnetuserrole()
                {
                    Userid = AspNetUserBusinessData.Id,
                    Name = "patient"
                };
                _db.Aspnetuserroles.Add(aspnetroles);
                _db.SaveChanges();

                //common things
                var requestsdata = new Request()
                {
                    Requesttypeid = 1,
                    Firstname = obj.BusinessFirstname,
                    Lastname = obj.BusinessLastname,
                    Phonenumber = "+" + obj.BusinessCountryCode + " " + obj.BusinessPhonenumber,
                    Email = obj.BusinessEmail,
                    Status = 1,
                    Casenumber = obj.Casenumber,
                    Isdeleted = false,
                    Userid = usersdata.Userid,
                    Createddate = DateTime.Now,
                    Confirmationnumber = uniqueNumber
                };
                _db.Requests.Add(requestsdata);
                _db.SaveChanges();


                var statuslogs = new Requeststatuslog()
                {
                    Requestid = requestsdata.Requestid,
                    Status = 1,
                    Createddate = DateTime.Now,
                };
                _db.Requeststatuslogs.Add(statuslogs);
                _db.SaveChanges();

                var requestsClientdata = new Requestclient()
                {
                    Requestid = requestsdata.Requestid,
                    Firstname = obj.PatientFirstname,
                    Lastname = obj.PatientLastname,
                    Phonenumber = "+" + obj.PatientCountryCode + " " + obj.PatientPhonenumber,
                    Notes = obj.Notes,
                    Email = obj.PatientEmail,
                    Strmonth = obj.dateofbirth.ToString(),
                    Street = obj.Street,
                    City = obj.City,
                    Regionid = obj.regid,
                    State = obj.State,
                    Zipcode = obj.Zipcode
                };
                _db.Requestclients.Add(requestsClientdata);
                _db.SaveChanges();

                string from = "hallodocpms@gmail.com";
                string to = obj.PatientEmail;
                string subject = "Regiter yourself";
                var link = $"https://localhost:44313/PatientSide/Register ";
                string body = $"Hi,<br /><br />Please click on the following link to Register on HelloDoc:<br /><br/><a>{link}<a> ";
                bool status = await _adminPartialsRepo.sendEmail(from, to, subject, body, null, requestsdata.Requestid, null, null);

                //ServicePointManager.ServerCertificateValidationCallback =
                //(sender, certificate, chain, sslPolicyErrors) => true;


                //MailMessage message = new MailMessage();
                //message.From = new MailAddress("hallodocpms@gmail.com");
                //message.Subject = "Regiter yourself";
                //message.To.Add(new MailAddress(obj.PatientEmail));
                //message.IsBodyHtml = true;
                //using (var smtpClient = new SmtpClient("sandbox.smtp.mailtrap.io"))
                //{
                //    smtpClient.Port = 587;
                //    smtpClient.Credentials = new NetworkCredential("5ddcdcba9543c7", "d44ecbea64732c");
                //    smtpClient.EnableSsl = true;

                //    smtpClient.Send(message);
                //}

                //business 2 table not require as of now             
            }
            else
            {
                //common things
                var patUserId = _db.Users.Where(x => x.Aspnetuserid == userBusinesscheck.Id).FirstOrDefault();

                if (patUserId == null) { return; }
                var finalUid = patUserId.Userid;


                var requestsdata = new Request()
                {
                    Requesttypeid = 1,
                    Firstname = obj.BusinessFirstname,
                    Lastname = obj.BusinessLastname,
                    Phonenumber = obj.BusinessCountryCode + " " + obj.BusinessPhonenumber,
                    Email = obj.BusinessEmail,
                    Status = 1,
                    Casenumber = obj.Casenumber,
                    Createddate = DateTime.Now,
                    Isdeleted = false,
                    Userid = finalUid,
                    Confirmationnumber = uniqueNumber

                };
                _db.Requests.Add(requestsdata);
                _db.SaveChanges();

                var statuslogs = new Requeststatuslog()
                {
                    Requestid = requestsdata.Requestid,
                    Status = 1,
                    Createddate = DateTime.Now,
                };
                _db.Requeststatuslogs.Add(statuslogs);
                _db.SaveChanges();


                var requestsClientdata = new Requestclient()
                {
                    Requestid = requestsdata.Requestid,
                    Firstname = obj.PatientFirstname,
                    Lastname = obj.PatientLastname,
                    Phonenumber = obj.PatientCountryCode + " " + obj.PatientPhonenumber,
                    Notes = obj.Notes,
                    Regionid = obj.regid,
                    Email = obj.PatientEmail,
                    Street = obj.Street,
                    Strmonth = obj.dateofbirth.ToString(),
                    City = obj.City,
                    State = obj.State,
                    Zipcode = obj.Zipcode
                };
                _db.Requestclients.Add(requestsClientdata);
                _db.SaveChanges();
            }
        }
        #endregion
        # region CommonMethod
        public Admin? Admin(string id)
        {
            return _db.Admins.FirstOrDefault(x => x.Aspnetuserid == id);
        }
        public async Task<Aspnetuser?> AspnetUserData(string username)
        {
            return await _db.Aspnetusers.FirstOrDefaultAsync(x => (x.Username == username || x.Email == username) && x.IsDeleted != true);
        }
        public async Task<AccountType?> GetAcountType(int accountType)
        {
            return await _db.AccountTypes.FirstOrDefaultAsync(x => x.AccountTypeId == accountType);
        }
        public async Task<Physician?> getPhysicianData(string AspId)
        {
            return await _db.Physicians.FirstOrDefaultAsync(x => x.Aspnetuserid == AspId && x.Isdeleted != true);
        }
        public async Task<IEnumerable<Status>?> loadStatus()
        {
            return await Task.Run(() => _db.Statuses);
        }
        public async Task<Role?> getRole(int? RoleId)
        {
            return await _db.Roles.FirstOrDefaultAsync(x => (x.Roleid == RoleId || RoleId == null) && x.Isdeleted != true);

        }
        public async Task<IEnumerable<Rolemenu>?> getRoleMenu(int roleid)
        {
            return await Task.Run(() => _db.Rolemenus.Where(x => x.Roleid == roleid));
        }
        public async Task<User?> getUserData(string ID)
        {
            return await _db.Users.FirstOrDefaultAsync(x => x.Aspnetuserid == ID && x.Isdeleted != true);
        }
        public async Task<bool> ResetPassword(string email, ResetPass obj)
        {
            var userexist = await AspnetUserData(email);
            if (userexist != null)
            {
                userexist.Passwordhash = BCrypt.Net.BCrypt.HashPassword(obj.password);
                _db.Aspnetusers.Update(userexist);
                _db.SaveChanges();
                return true;
            }
            return false;
        }
        public async Task<bool> RegisterOnPlatForm(Aspnetuser? aspUser, string password)
        {
            if (aspUser != null)
            {
                aspUser.Passwordhash = BCrypt.Net.BCrypt.HashPassword(password);
                _db.Aspnetusers.Update(aspUser);
                _db.SaveChanges();
                return await Task.Run(() => true);
            }
            return await Task.Run(() => false);
        }
        public async Task<Requestclient?> RequestclientsData(int RequestId)
        {
            return await _db.Requestclients.SingleOrDefaultAsync(x => x.Requestid == RequestId);

        }
        #endregion
    }
}
