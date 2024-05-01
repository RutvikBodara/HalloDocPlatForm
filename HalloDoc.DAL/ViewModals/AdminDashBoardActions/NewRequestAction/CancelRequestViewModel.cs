using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.DAL.ViewModals.AdminDashBoardActions.NewRequestAction
{
    public class CancelRequestViewModel
    {
        public string? CancelReason { get; set; }
        public string? AdditionalNotes { get; set; }
        public int Requestid { get; set; }
        public int AdminId { get; set; }
    }
}
