
using Microsoft.AspNetCore.Mvc;
using HalloDoc.DAL.ViewModals;
using HalloDoc.DAL.Data;
using HalloDoc.DAL.DataModels;
using HalloDoc.DAL.ViewModals.Family;
using HalloDoc.BAL.Interface;


namespace hellodocsrsmvc.Controllers;

public class PatientRequestsController : Controller
{

    //private readonly HalloDocDBContext _db;
    private readonly IContactRepository _contactRepository;
    private readonly IAdminPartialsRepo _adminPartialsRepo;

    public PatientRequestsController(HalloDocDBContext db, IContactRepository contactRepository, IAdminPartialsRepo adminPartialsRepo)
    {
        _contactRepository = contactRepository;
        _adminPartialsRepo = adminPartialsRepo;
        //_db = db;
    }
    public IActionResult PatientRequestCreate(string? email)
    {
        return View();
    }

    public IActionResult PatientSubmitRequest()
    {
        return View();
    }
    //[HttpPost]
    //public IActionResult PatientEmailCheck(string email)
    //{
    //    bool emailavailable = _db.Aspnetusers.Any(x => x.Email == email);
    //    return Json(new {avail = emailavailable });

    //}

    [HttpPost]
    public async Task<IActionResult> emailcheck(string email)
    {
        //bool emailavailable = _db.Aspnetusers.Any(x => x.Email == email);
        var emailcheck =await _contactRepository.AspnetUserData(email);
        bool flag = false;
        if (emailcheck == null)
        {
            flag = true;
        }
        return Json(new { emails = flag });
    }

    [HttpPost]
    public IActionResult PatientRequestCreate(CreatePatientRequestviewmodel obj)
    {
        _contactRepository.ADDPatientRequestData(obj, null, null);
        TempData["requestSubmitted"] = "Request Submitted Successfully";
        return RedirectToAction("LoginPage", "PatientSide");
    }

    public IActionResult FamilyFriendRequest()
    {
        return View();
    }

    [HttpPost]
    public IActionResult FamilyFriendRequest(FriendRequest obj)
    {

        if (ModelState.IsValid)
        {
            _contactRepository.AddFamilyFriendData(obj);
            TempData["requestSubmitted"] = "Request Submitted Successfully";
            return RedirectToAction("LoginPage", "PatientSide");
        }

        TempData["!requestSubmitted"] = "Request Not Submitted";
        return View(obj);
    }

    public IActionResult ConciergeRequest()
    {
        return View();
    }

    [HttpPost]
    public IActionResult ConciergeRequest(ConciergeRequestviewmodel obj)
    {
        if (ModelState.IsValid)
        {
            _contactRepository.ADDConciergeData(obj);
            TempData["requestSubmitted"] = "Request Submitted Successfully";
            return RedirectToAction("LoginPage", "PatientSide");
        }
        TempData["!requestSubmitted"] = "Request Not Submitted";
        return View(obj);
    }


    public IActionResult BusinessRequest()
    {
        return View();
    }

    [HttpPost]
    public IActionResult BusinessRequest(BusinessRequestviewmodel obj)
    {
        if (ModelState.IsValid)
        {
            _contactRepository.ADDBusinessData(obj);
            TempData["requestSubmitted"] = "Request Submitted Successfully";

            return RedirectToAction("LoginPage", "PatientSide");
        }

        TempData["!requestSubmitted"] = "Request Not Submitted";
        return View(obj);
    }

    public async Task<IActionResult> LoadRegion()
    {
        IEnumerable<Region> region = await _adminPartialsRepo.loadRegion();
        //IEnumerable<Region> region = _db.Regions;
        return Json(new { region = region });
    }

    //public void emailsend(string email , string subject , string message)
    //{
    //    var emailtosend = new MimeMessage();
    //    emailtosend.From.Add(MailboxAddress.Parse("hallodocpms@gmail.com"));
    //    emailtosend.To.Add(MailboxAddress.Parse(email));
    //    emailtosend.Subject = subject;
    //    emailtosend.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = message };

    //    using( var emailclient =new SmtpClient())
    //    {

    //        emailclient.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
    //        emailclient.Authenticate("hallodocpms@gmail.com", "Rutvik10@#");
    //        emailclient.Send(emailtosend);
    //        emailclient.Disconnect(true);
    //    }
    //    //return Task.CompletedTask;
    //}
}
