using HalloDoc.DAL.DataModels;
using HalloDoc.DAL.ViewModals;
using HalloDoc.DAL.ViewModals.Family;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.BAL.Interface
{
    /// <summary>
    /// Platform 4 type of Request and some Common Data fetch will take place from this repository
    /// </summary>
    public interface IContactRepository
    {
        #region 4TypeOfRequest
        Task<bool> ADDPatientRequestData(CreatePatientRequestviewmodel obj, string? createdBy,int? requestType);
        Task AddFamilyFriendData(FriendRequest obj);
        Task ADDConciergeData(ConciergeRequestviewmodel obj);
        Task ADDBusinessData(BusinessRequestviewmodel obj);
        #endregion
        #region FromPatientAccount
        Task MeRequest(CreatePatientRequestviewmodel obj, IFormFile? uploadFile);
        Task SomeOneRequest(FriendRequest obj, string? ASPID);
        #endregion
        #region CommonMethod
        Admin? Admin(string adminId);
        Task<Aspnetuser?> AspnetUserData(string username);
        Task<AccountType?> GetAcountType(int accountType);
        Task<Physician?> getPhysicianData(string AspId);
        Task<Role?> getRole(int? RoleId);
        Task<IEnumerable<Rolemenu>?> getRoleMenu(int roleid);
        Task<User?> getUserData(string ID);
        Task<bool> ResetPassword(string email, ResetPass obj);
        Task<bool> RegisterOnPlatForm(Aspnetuser aspUser,string password);
        Task<Requestclient?> RequestclientsData(int RequestId);
        Task<IEnumerable<Status>?> loadStatus();
        #endregion
    }
}
