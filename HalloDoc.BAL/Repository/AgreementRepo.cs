using HalloDoc.BAL.Interface;
using HalloDoc.DAL.Data;
using HalloDoc.DAL.DataModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.BAL.Repository
{
    public class AgreementRepo : IAgreementRepo
    {
        private readonly HalloDocDBContext _db;
        public AgreementRepo (HalloDocDBContext db)
        {
            _db = db;
        }
        public async  Task<bool> ConfirmCancel(int reqid, string cancelNotes)
        {
            var req = await _db.Requests.FirstOrDefaultAsync(x => x.Requestid == reqid);
            if(req != null)
            {
                req.Status = 3;
                req.Casetag = cancelNotes;
                _db.Requests.Update(req);
                _db.SaveChanges();
            }
            else
            {
                return false;
            }
            var data = new Requeststatuslog()
            {
                Requestid = reqid,
                Status = 3,
                Notes = cancelNotes
            };
            _db.Requeststatuslogs.Add(data);
            _db.SaveChanges();
         return true;
        }
        public async Task<bool> ConfirmAgreement(int reqid)
        {
            var obj = _db.Requests.Where(x=>x.Requestid == reqid).FirstOrDefault();
            if(obj != null)
            {
                obj.Status = 2;
                _db.Requests.Update(obj);
                await _db.SaveChangesAsync();
                return true;
            }

            return false;
        }
        public async Task<bool> CheckAgreementProcessCompleted(int RequestId)
        {
            return await Task.Run(()=> _db.Requests.Any(x => x.Requestid == RequestId && (x.Status == 2 || x.Status == 6)));
        }
    }
}
