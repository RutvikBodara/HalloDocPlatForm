using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.BAL.Interface
{
    public interface IAgreementRepo
    {
        public Task<bool> ConfirmCancel(int reqid, string cancelNotes);
        public Task<bool> ConfirmAgreement(int reqid);
        public Task<bool> CheckAgreementProcessCompleted(int RequestId);

    }
}
