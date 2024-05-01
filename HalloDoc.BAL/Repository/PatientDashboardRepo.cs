using HalloDoc.BAL.Interface;
using HalloDoc.DAL.Data;
using HalloDoc.DAL.DataModels;
using HalloDoc.DAL.ViewModals;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.BAL.Repository
{
    public class PatientDashboardRepo : IPatientDashboardRepo
    {
        private readonly HalloDocDBContext _db;

        public PatientDashboardRepo(HalloDocDBContext db)
        {
            _db = db;
        }

        public async Task<User?> User(string AspId)
        {
            var userdetails = await _db.Users.FirstOrDefaultAsync(x => x.Aspnetuserid == AspId);
            return userdetails;
        }
        public async Task<IEnumerable<Region>> LoadRegion()
        {
            return await Task.Run(() => _db.Regions);
        }
        public async Task<IEnumerable<PatientDashboardViewModel>> PatientRequestTable(User patUserId)
        {
            var requestdata = from req in _db.Requests
                              join requestfiles in _db.Requestwisefiles
                              on req.Requestid equals requestfiles.Requestid into files
                              from requestsfiles in files.DefaultIfEmpty()
                              join x3 in _db.Requeststatuses on req.Status equals x3.Requeststatusid
                              where req.Userid == patUserId.Userid
                              group requestsfiles by new { req.Requestid, req.Createddate, x3.Name } into groups
                              select new PatientDashboardViewModel
                              {

                                  StatusName = groups.Key.Name,
                                  //file name can be null here for not show btn
                                  Filename = groups.FirstOrDefault().Filename ?? null,
                                  docCount = groups.Count(),
                                  RequestId = groups.Key.Requestid,
                                  Createddate = groups.Key.Createddate,
                                  UserId = patUserId.Userid,
                              };

            return await Task.Run(() => requestdata);
        }

        public async Task<PatientProfileEdit> PatientProfileData(User patUserId, string Patient_asp_id)
        {
            var useriddata = new PatientProfileEdit
            {
                Firstname = patUserId.Firstname,
                Lastname = patUserId.Lastname,
                Mobile = patUserId.Mobile,
                Email = patUserId.Email,
                Street = patUserId.Street,
                City = patUserId.City,
                State = patUserId.State,
                Zipcode = patUserId.Zipcode,
                aspnetid = Patient_asp_id,
                regid = patUserId.Regionid,
                userid = patUserId.Userid.ToString(),
                DateOfBirth = patUserId.Strmonth
            };
            return await Task.Run(() => useriddata);
        }

        public async Task<Request?> CheckRequest(int id)
        {
            var requestdetails = await _db.Requests.FirstOrDefaultAsync(x => x.Requestid == id);
            return requestdetails;
        }

        public async Task<IEnumerable<PatientRequestWiseDocument>> RequestWiseDocument(int id)
        {

            var RequestWiseDoc = from reqfiles in _db.Requestwisefiles
                                 join req in _db.Requests on reqfiles.Requestid equals req.Requestid
                                 where reqfiles.Requestid == id
                                 select new PatientRequestWiseDocument
                                 {
                                     document = reqfiles.Filename,
                                     uploaddate = reqfiles.Createddate,
                                     uploader = req.Firstname + req.Lastname,
                                     ReqId = req.Requestid,
                                 };

            return await Task.Run(() => RequestWiseDoc);
        }

        public async Task PatientDocumentRequestWise(IFormFile file, string reqid)
        {

            if (file != null && file.Length > 0)
            {
                Guid myuuid = Guid.NewGuid();
                var filename = Path.GetFileName(file.FileName);
                var FinalFileName = myuuid.ToString() + filename;

                //path

                var filepath = Path.Combine(Environment.CurrentDirectory, "wwwroot", "uploads", FinalFileName);

                //copy in stream

                using (var str = new FileStream(filepath, FileMode.Create))
                {
                    //copy file
                    file.CopyTo(str);
                }

                //STORE DATA IN TABLE
                var fileupload = new Requestwisefile()
                {
                    Requestid = int.Parse(reqid),
                    Filename = FinalFileName,
                    Createddate = DateTime.Now,
                };

                _db.Requestwisefiles.Add(fileupload);
                await _db.SaveChangesAsync();
            }
        }

        public async Task EditProfile(PatientProfileEdit obj, string aspid, int userid)
        {

            //update user data to user table
            var userdata = _db.Users.Where(x => x.Userid == userid).FirstOrDefault();

            var Aspnetdata = _db.Aspnetusers.Where(x => x.Id == aspid).FirstOrDefault();

            if (userdata != null)
            {
                _db.Requestclients.Where(x => x.Email == userdata.Email).ToList().ForEach(item =>
                {

                    item.Firstname = obj.Firstname;
                    item.Lastname = obj.Lastname;
                    item.Email = obj.Email;
                    item.Phonenumber = obj.countryCode + obj.Mobile;
                    item.Street = obj.Street;
                    item.City = obj.City;
                    item.State = obj.State;
                    item.Zipcode = obj.Zipcode;
                    item.Strmonth = obj.DateOfBirth;
                });


                userdata.Firstname = obj.Firstname;
                userdata.Lastname = obj.Lastname;
                userdata.Email = obj.Email;
                userdata.Mobile = obj.countryCode + obj.Mobile;
                userdata.Street = obj.Street;
                userdata.City = obj.City;
                userdata.State = obj.State;
                userdata.Zipcode = obj.Zipcode;
                userdata.Strmonth = obj.DateOfBirth;
                _db.Users.Update(userdata);
            }
            if (Aspnetdata != null)
            {
                Aspnetdata.Username = obj.Email;
                Aspnetdata.Email = obj.Email;
                Aspnetdata.Phonenumber = obj.countryCode + obj.Mobile;
                Aspnetdata.Modifieddate = DateTime.Now;
                _db.Aspnetusers.Update(Aspnetdata);
            }
            await _db.SaveChangesAsync(true);
        }

        public async Task<CreatePatientRequestviewmodel> MeRequestGet(string Patient_asp_id)
        {
            var patUserId = await _db.Users.FirstOrDefaultAsync(x => x.Aspnetuserid == Patient_asp_id);

            var meRequest = new CreatePatientRequestviewmodel
            {
                Firstname = patUserId.Firstname,
                Lastname = patUserId.Lastname,
                Phonenumber = patUserId.Mobile,
                Email = patUserId.Email,
                Street = patUserId.Street,
                City = patUserId.City,
                State = patUserId.State,
                Zipcode = patUserId.Zipcode,
                regid = (int)patUserId.Regionid,
            };
            return meRequest;
        }
        public async Task<IEnumerable<Requestwisefile>?> getRequestwisefiles(int RequestId)
        {
            return await Task.Run(() => _db.Requestwisefiles.Where(u => u.Requestid == RequestId));
        }

    }
}
