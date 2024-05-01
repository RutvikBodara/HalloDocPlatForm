using HalloDoc.DAL.DataModels;
using HalloDoc.DAL.ViewModals;
using Microsoft.AspNetCore.Http;
using Org.BouncyCastle.Crypto.Operators;
using Org.BouncyCastle.Tsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.BAL.Interface
{
    /// <summary>
    /// Patient Action Method
    /// </summary>
    public interface IPatientDashboardRepo
    {
        #region PatientAction
        public Task<User?> User(string? id);
        public Task<IEnumerable<PatientDashboardViewModel>> PatientRequestTable(User patUserId);
        public Task<PatientProfileEdit> PatientProfileData(User patUserId, string Patient_asp_id);
        public Task<Request?> CheckRequest(int id);
        public Task<IEnumerable<PatientRequestWiseDocument>> RequestWiseDocument(int id);
        public Task PatientDocumentRequestWise(IFormFile file, string reqid);
        public Task EditProfile(PatientProfileEdit obj, string aspid, int userid);
        public Task<CreatePatientRequestviewmodel> MeRequestGet(string? Patient_asp_id);
        public Task<IEnumerable<Region>> LoadRegion();
        public Task<IEnumerable<Requestwisefile>?> getRequestwisefiles(int RequestId);
        #endregion
    }
}
