using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using hellodocsrsmvc.Models;
using System.Web;
using HalloDoc.DAL.Data;
using HalloDoc.DAL.ViewModals;
using HalloDoc.DAL.DataModels;
using Microsoft.AspNetCore.Identity;
using System.Collections;
using Microsoft.DiaSymReader;
using RestSharp;
using Microsoft.AspNetCore.Http;
using System.Drawing;
using System.Net.Mail;
using System.Net;
using NuGet.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Filters;
using HalloDoc.BAL.Interface;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Org.BouncyCastle.Security.Certificates;


namespace hellodocsrsmvc.Controllers;

public class PatientSideController : Controller
{
    //private readonly HalloDocDBContext _db;
    private readonly IJwtAuthInterface _JwtAuth;
    private readonly IContactRepository _ContactRepository;
    //db object
    public PatientSideController(IContactRepository contactRepository, IJwtAuthInterface jwtAuthInterface)
    {
        //_db = db;
        _JwtAuth = jwtAuthInterface;
        _ContactRepository = contactRepository;
    }

    public async Task<IActionResult> LoginPage()
    {
        var token = Request.Cookies["Jwt"];
        if (token != null)
        {
            UserDataViewModel jwtData = _JwtAuth.AccessData(token);
            if (jwtData.UserId != null)
            {
                var method = "";
                var controller = "";
                if (jwtData.Role == "admin")
                {
                    method = "AdminDashboard";
                    controller = "AdminSite";
                    var admindata = _ContactRepository.Admin(jwtData.UserId);
                    //var admindata =_db.Admins.FirstOrDefault(x=>x.Aspnetuserid == jwtData.UserId);
                    if (admindata == null)
                    {
                        return View();
                    }
                    return RedirectToAction(method, controller, new { id = admindata.Adminid, roleid = jwtData.roleid });
                }
                else if (jwtData.Role == "physician")
                {
                    controller = "ProviderSite";
                    method = "ProviderDataLoader";
                }
                else if (jwtData.Role == "patient")
                {
                    controller = "PatientDashBoard";
                    method = "PatientDashboard";
                }
                //check role is exist 

                return RedirectToAction(method, controller, new { id = jwtData.UserId, roleid = jwtData.roleid });
            }
        }
        return await Task.Run(() => View());
    }

    [HttpPost]
    public async Task<IActionResult> LoginPage(Loginviewmodel obj)
    {
        if (ModelState.IsValid)
        {
            //var user =await _db.Aspnetusers.FirstOrDefaultAsync(x => x.Username == obj.Username && x.IsDeleted !=true);
            Aspnetuser? user = await _ContactRepository.AspnetUserData(obj.Username);

            //check in the netuser roles if he is admin or not         
            if (user == null)
            {
                TempData["error"] = "user not found!";
            }
            else
            {
                if (user.Passwordhash == null)
                {
                    TempData["error"] = "You are Not Registered! Please Check Email For registration!";
                    return View();
                }
                if (BCrypt.Net.BCrypt.Verify(obj.Passwordhash, user.Passwordhash))
                {
                    //var accounttype = _db.AccountTypes.FirstOrDefault(x=>x.AccountTypeId == user.Accounttype);
                    AccountType? accounttype = await _ContactRepository.GetAcountType(user.Accounttype);
                    if (accounttype == null)
                    {
                        TempData["error"] = "something went wrong";
                        return View();
                    }
                    UserDataViewModel authuser = new();
                    authuser.Username = user.Username;
                    authuser.UserId = user.Id;
                    //to access only specific account type
                    authuser.Role = accounttype.Name;

                    var firstname = "";
                    var lastname = "";
                    int accounttyepe = user.Accounttype;
                    var controller = "";
                    var method = "";
                    //var idvariable = "";
                    int IdValue = 0;
                    int roleid = 0;
                    IEnumerable<Rolemenu>? rolemenus = null;
                    switch (accounttyepe)
                    {
                        case 1:
                            Physician? Physiciandata = await _ContactRepository.getPhysicianData(user.Id);
                            //var Physiciandata =await _db.Physicians.FirstOrDefaultAsync(x => x.Aspnetuserid == user.Id && x.Isdeleted != true);
                            if (Physiciandata == null)
                            {
                                TempData["error"] = "user not found!";
                                return View("LoginPage");
                            }

                            Role? data = await _ContactRepository.getRole(Physiciandata.Roleid);
                            if (data == null)
                            {
                                TempData["error"] = "Your Role has been deleted";
                                return View(obj);
                            }

                            roleid = (int)Physiciandata.Roleid;
                            rolemenus = await _ContactRepository.getRoleMenu(roleid);
                            //rolemenus = _db.Rolemenus.Where(x => x.Roleid == roleid);
                            firstname = Physiciandata.Firstname;
                            lastname = Physiciandata.Lastname;
                            authuser.UserId = Physiciandata.Physicianid.ToString();
                            controller = "ProviderSite";
                            method = "ProviderDataLoader";
                            IdValue = Physiciandata.Physicianid;
                            //rolemenus = _db.Rolemenus.Where(_ => _.Roleid == roleid);
                            break;
                        case 2:

                            var Admindata = _ContactRepository.Admin(user.Id);
                            //var Admindata = await _db.Admins.FirstOrDefaultAsync(x => x.Aspnetuserid == user.Id && x.Isdeleted != true);
                            if (Admindata == null)
                            {
                                TempData["error"] = "user not found!";
                                return View("LoginPage");
                            }

                            Role? datas = await _ContactRepository.getRole(Admindata.Roleid);
                            //Role? datas = _db.Roles.FirstOrDefault(x => x.Roleid == Admindata.Roleid && x.Isdeleted != true);
                            if (datas == null)
                            {
                                TempData["error"] = "Your Role has been deleted";
                                return View(obj);
                            }

                            roleid = (int)Admindata.Roleid;
                            rolemenus = await _ContactRepository.getRoleMenu(roleid);
                            //rolemenus = _db.Rolemenus.Where(x => x.Roleid == roleid);
                            method = "AdminDashboard";
                            controller = "AdminSite";
                            //idvariable = "AdminId";
                            IdValue = Admindata.Adminid;
                            firstname = Admindata.Firstname; lastname = Admindata.Lastname;
                            break;
                        case 3:
                            var userdata = await _ContactRepository.getUserData(user.Id);
                            //var userdata = await _db.Users.FirstOrDefaultAsync(x => x.Aspnetuserid == user.Id && x.Isdeleted != true);
                            if (userdata == null)
                            {
                                TempData["error"] = "user not found!";
                                return View("LoginPage");
                            }
                            roleid = 0; rolemenus = null;
                            controller = "PatientDashBoard";
                            method = "PatientDashboard";
                            //idvariable = "patientID";
                            IdValue = userdata.Userid;
                            firstname = userdata.Firstname; lastname = userdata.Lastname;
                            break;
                    }
                    authuser.FirstName = firstname;
                    authuser.LastName = lastname;
                    authuser.roleid = roleid.ToString();
                    var jwtToken = _JwtAuth.GenerateJWTAuthetication(authuser);
                    string roles = "";
                    if (accounttyepe != 3)
                    {
                        var tempmenu = rolemenus.ToList();
                        if (tempmenu != null)
                        {
                            foreach (var role in tempmenu)
                            {
                                roles += role.Menuid + ",";
                            }
                            Response.Cookies.Append("RoleMenu", roles);
                        }
                        TempData["success"] = "Login Successfully";
                    }
                    else
                    {
                        TempData["success"] = "Login Successfully";
                    }
                    Response.Cookies.Append("Jwt", jwtToken);
                    return RedirectToAction(method, controller, new { id = IdValue, roleid = roleid });
                }
                else
                {
                    TempData["error"] = "Wrong Password!";
                }
            }
        }
        return View();
    }
    public async Task<IActionResult> logout()
    {
        HttpContext.Session.Clear();
        Response.Cookies.Delete("Jwt");
        return await Task.Run(() => RedirectToAction("LoginPage", "PatientSide"));
    }
    public IActionResult ForgetPassword()
    {
        return View();
    }
    public IActionResult AccessDenied()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> forgetpassword(ResetPass obj)
    {
        var UserData = await _ContactRepository.AspnetUserData(obj.email);
        //var val = await _db.Aspnetusers.FirstOrDefaultAsync(x => x.Email == obj.email);
        if (UserData == null)
        {
            TempData["emailnotfound"] = "email not found!";
            return View();
        }
        else
        {
            //email send
            //Send an email with the password reset link to the user's email address
            //var resetLink = Url.Action("Reset", "Home", new { email = obj.Email, token }, Request.Scheme);
            //Send email to user with reset password link
            //...
            string from = "hallodocpms@gmail.com";
            string to = UserData.Email;
            var link = $"https://localhost:44313" + Url.Action("ResetPassword", "PatientSide", new { email = await Encode(obj.email) });
            string body = $"<html><body>please reset your password threw following link: <br/><a href='link'> {link} </a></ <body></html>"; ;
            ResetMail(from, to, body);
        }
        return RedirectToAction("LoginPage");
    }

    public void ResetMail(string from, string to, string body)
    {
        ServicePointManager.ServerCertificateValidationCallback =
                (sender, certificate, chain, sslPolicyErrors) => true;
        //var link = Url.Action("ResetPassword", "PatientSide" , new { email = Encode(obj.email) });

        //var link = "https://localhost:44313/PatientSide/ResetPassword?email=" + Encode(obj.email);
        //var link = $"https://localhost:44313" + Url.Action("ResetPassword", "PatientSide", new { email = Encode(obj.email) });
        MailMessage message = new()
        {
            From = new MailAddress(from),
            Subject = "Password reset request"
        };
        message.To.Add(new MailAddress(to));
        message.Body = body;
        message.IsBodyHtml = true;
        using var smtpClient = new SmtpClient("sandbox.smtp.mailtrap.io");
        smtpClient.Port = 587;
        smtpClient.Credentials = new NetworkCredential("5ddcdcba9543c7", "d44ecbea64732c");
        smtpClient.EnableSsl = true;
        smtpClient.Send(message);
    }

    public IActionResult ResetPassword(string email)
    {
        ViewBag.emailval = email;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ResetPassword(ResetPass obj)
    {

        if (obj.password != obj.conpass)
        {
            TempData["paswornotmatch"] = "password must be same!";
            return View();
        }
        bool status = await _ContactRepository.ResetPassword(Decode(obj.email), obj);
        if (status)
            TempData["changepass"] = "password change successfully";
        else
        {
            TempData["error"] = "Something went wrong!";
        }
        return RedirectToAction("LoginPage");
    }

    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel obj)
    {


        var UserData = await _ContactRepository.AspnetUserData(obj.Email);

        if (UserData == null)
        {
            TempData["error"] = "Please Use that Email which is use to send Request";
            return View();
        }
        if (obj.Password != obj.ConfirmPassword)
        {
            TempData["paswornotmatch"] = "password must be same!";
            return View();
        }
        bool status = await _ContactRepository.RegisterOnPlatForm(UserData, obj.Password);
        if (status)
        {
            TempData["success"] = "Register successfully";
        }
        else
        {
            TempData["error"] = "Something Went Wrong";
        }
        return RedirectToAction("LoginPage");
    }
    public async Task<string> Encode(string encodeMe)
    {
        byte[] encoded = System.Text.Encoding.UTF8.GetBytes(encodeMe);
        return await Task.Run(() => Convert.ToBase64String(encoded));
    }
    public string Decode(string decodeMe)
    {
        byte[] encoded = Convert.FromBase64String(decodeMe);
        return System.Text.Encoding.UTF8.GetString(encoded);
    }

}
