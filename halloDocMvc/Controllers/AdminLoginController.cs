
//using HalloDoc.DAL.ViewModals;
//using Microsoft.AspNetCore.Mvc;
//using System.Net.Mail;
//using System.Net;
//using HalloDoc.DAL.Data;
//using HalloDoc.DAL.DataModels;
//using HalloDoc.BAL.Interface;
//using Newtonsoft.Json.Linq;
//using Org.BouncyCastle.Asn1.Ocsp;
//using System.IdentityModel.Tokens.Jwt;
//using Microsoft.EntityFrameworkCore;

//namespace HalloDocAdmin.Controllers
//{
//    public class AdminLoginController : Controller
//    {
//        private readonly HalloDocDBContext _db;
//        private readonly IJwtAuthInterface _JwtAuth;

//        //db object
//        public AdminLoginController(HalloDocDBContext db, IJwtAuthInterface JwtAuth)
//        {
//            _db = db;
//            _JwtAuth = JwtAuth;
//        }
//        public IActionResult LoginPage()
//        {

//            //REDIRECT TO DASHBOARD IF ALREADY LOGGEED IN

//            //if (HttpContext.Session.GetString("ASPID") != null)
//            //{
//            //    return RedirectToAction("AdminDashboard", "AdminSite", new { AdminID = HttpContext.Session.GetString("ASPID").ToString() });
//            ////}
//            //var JwtCookie = Request.Cookies["Jwt"];

//            //if (JwtCookie != null)
//            //{
//            //    if (HttpContext.Session.GetString("ASPID").ToString() != null)
//            //    {
//            //        return RedirectToAction("AdminDashboard", "AdminSite", new { AdminID = HttpContext.Session.GetString("ASPID") });
//            //    }
//            //}
//            return View();
//        }
//        public IActionResult AccessDenied()
//        {
//            return View();        
//        }

//        [HttpPost]
//        public async Task<IActionResult> LoginPage(Loginviewmodel obj)
//        {
//            if (ModelState.IsValid)
//            {
//                var user =await _db.Aspnetusers.FirstOrDefaultAsync(x => x.Username == obj.Username);

//                if(user == null)
//                {
//                    TempData["error"] = "User Not Found!";
//                    return View();
//                }

//                var admincheck = _db.Aspnetuserroles.Where(x => x.Userid == user.Id).FirstOrDefault();

//                if(admincheck == null)
//                {
//                    TempData["error"] = "Role Not Defined For you!";
//                    return View();
//                }

//                var Usercheck = _db.Users.Where(x => x.Aspnetuserid == user.Id).FirstOrDefault();
//                if (Usercheck == null)
//                {
//                    return View();
//                }
//                //check in the netuser roles if he is admin or not
//                if (admincheck.Name != null)
//                {
//                    if (BCrypt.Net.BCrypt.Verify(obj.Passwordhash, user.Passwordhash))
//                    {


//                        //HttpContext.Session.SetString("ASPID", @user.Id.ToString());
//                        //HttpContext.Session.SetString("Roles", admincheck.Name.ToString());

//                        //var admin_asp_id = HttpContext.Session.GetString("ASPID");

//                        //ViewBag.ASPID = admin_asp_id;

//                        //var AdminRoles = HttpContext.Session.GetString("Roles");

//                        //ViewBag.Roles = AdminRoles;
                     
//                        UserDataViewModel authuser = new();
//                        authuser.Username = user.Username;
//                        authuser.UserId= user.Id;
//                        authuser.Role = admincheck.Name;
//                        authuser.FirstName = Usercheck.Firstname;
//                        authuser.LastName = Usercheck.Lastname;

//                        var jwtToken = _JwtAuth.GenerateJWTAuthetication(authuser);
//                        Response.Cookies.Append("Jwt", jwtToken);

//                        var token = Request.Cookies["Jwt"];
                        
//                        var admindata= _db.Admins.Where(x => x.Aspnetuserid==user.Id).FirstOrDefault(); 
//                        if(admindata == null)
//                        {
//                            //found as a patient
//                            TempData["success"] = "redirect to patient Login Portal";
//                            return RedirectToAction("LoginPage", "PatientSide");
//                        }

//                        TempData["success"] = "Login successfull";
//                        return RedirectToAction("AdminDashboard", "AdminSite" ,new { AdminId = admindata.Adminid });
//                    }     
//                }
//            }
//            return View();
//        }

//        public IActionResult ForgetPassword()
//        {
//            return View();
//        }

//        [HttpPost]
//        public IActionResult forgetpassword(ResetPass? obj)
//        {
//            var val = _db.Aspnetusers.Where(x => x.Email == obj.email).FirstOrDefault();
          

//            if (val == null)
//            {
//                TempData["emailnotfound"] = "email not found!";
//                return View();
//            }
//            else
//            {
//                //email send
//                //Send an email with the password reset link to the user's email address
//                //var resetLink = Url.Action("Reset", "Home", new { email = obj.Email, token }, Request.Scheme);
//                //Send email to user with reset password link
//                //...
//                var admincheck = _db.Aspnetuserroles.Where(x => x.Userid == val.Id).FirstOrDefault();

//                if(admincheck.Name != "admin")
//                {
//                    TempData["NotAdmin"] = "You are not an admin!";
//                    return View();
//                }

//                ServicePointManager.ServerCertificateValidationCallback =
//                    (sender, certificate, chain, sslPolicyErrors) => true;
//                //var link = Url.Action("ResetPassword", "PatientSide" , new { email = Encode(obj.email) });

//                //var link = "https://localhost:44356/AdminSide/ResetPassword?email=" + Encode(obj.email);
               
//                var link = $"https://localhost:44356" + Url.Action("ResetPassword", "Adminside", new { email = Encode(obj.email) });
//                MailMessage message = new MailMessage();
//                message.From = new MailAddress("hallodocpms@gmail.com");
//                message.Subject = "Password reset request";
//                message.To.Add(new MailAddress(val.Email));
//                message.Body = $"<html><body>please reset your password threw following link: <br/><a href='link'> {link} </a></ <body></html>";
//                message.IsBodyHtml = true;
//                using (var smtpClient = new SmtpClient("sandbox.smtp.mailtrap.io"))
//                {
//                    smtpClient.Port = 587;
//                    smtpClient.Credentials = new NetworkCredential("5ddcdcba9543c7", "d44ecbea64732c");
//                    smtpClient.EnableSsl = true;
//                    smtpClient.Send(message);
//                }
//            }

//            return RedirectToAction("LoginPage");

//        }

//        public IActionResult ResetPassword(string email)
//        {


//            ViewBag.emailval = email;
//            return View();
//        }

//        [HttpPost]
//        public IActionResult ResetPassword(ResetPass obj)
//        {

//            if (obj.password != obj.conpass)
//            {
//                TempData["paswornotmatch"] = "password must be same!";
//                return View();
//            }
//            var userexist = _db.Aspnetusers.Where(x => x.Email == Decode(obj.email)).FirstOrDefault();

//            if (userexist != null)
//            {
//                userexist.Passwordhash = BCrypt.Net.BCrypt.HashPassword(obj.password);

//                _db.Aspnetusers.Update(userexist);
//                _db.SaveChanges();
//            }
//            TempData["changepass"] = "password change successfully";
//            return RedirectToAction("LoginPage");
//        }

//        public string Encode(string encodeMe)
//        {
//            byte[] encoded = System.Text.Encoding.UTF8.GetBytes(encodeMe);
//            return Convert.ToBase64String(encoded);
//        }
//        public string Decode(string decodeMe)
//        {
//            byte[] encoded = Convert.FromBase64String(decodeMe);
//            return System.Text.Encoding.UTF8.GetString(encoded);
//        }
//    }
//}
