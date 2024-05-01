using AdminHalloDoc.Controllers.Login;
using HalloDoc.BAL.Interface;
using HalloDoc.DAL.Data;
using Humanizer.Localisation.TimeToClockNotation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;

namespace hellodocsrsmvc.Controllers
{

    public class AgreementController : Controller
    {
        //private readonly HalloDocDBContext _db;
        private readonly IAgreementRepo _agreementRepo;
        private readonly IContactRepository _contactRepository;

        public AgreementController(IAgreementRepo agreementRepo, IContactRepository contactRepository)
        {
            //_db = db;
            _agreementRepo = agreementRepo;
            _contactRepository = contactRepository;
        }
        public async Task<IActionResult> PatientAgreement(string reqid)
        {
            if (reqid == null)
            {
                return View("AgreementProcessCompleted");
            }
            reqid = Decode(reqid);
            bool requeststatus = await _agreementRepo.CheckAgreementProcessCompleted(int.Parse(reqid));
            //bool requeststatus = _db.Requests.Any(x => x.Requestid == int.Parse(reqid) && (x.Status == 3 || x.Status == 5));
            if (requeststatus)
            {
                return View("AgreementProcessCompleted");
            }
            var data = await _contactRepository.RequestclientsData(int.Parse(reqid));
            //var data =_db.Requestclients.Where(x=>x.Requestid == int.Parse(reqid)).FirstOrDefault();   
            return View(data);
        }
        public IActionResult AgreementProcessCompleted()
        {
            return View();
        }
        public async Task<IActionResult> ConfirmCancel(int reqid, string cancelNotes)
        {
            //send in the confirm cancel notes
            bool status = await _agreementRepo.ConfirmCancel(reqid, cancelNotes);
            return Json(status);
        }
        public async Task<IActionResult> ConfirmAgreement(int reqid)
        {
            bool status = await _agreementRepo.ConfirmAgreement(reqid);
            return Json(status);
        }
        public string Decode(string decodeMe)
        {
            byte[] encoded = Convert.FromBase64String(decodeMe);
            return System.Text.Encoding.UTF8.GetString(encoded);
        }
    }
}
