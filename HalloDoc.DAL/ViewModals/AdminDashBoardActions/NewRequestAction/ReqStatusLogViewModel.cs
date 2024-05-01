using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.DAL.ViewModals.AdminDashBoardActions.NewRequestAction
{
    public class ReqStatusLogViewModel
    {
        public int? Status { get; set; }
        public int? PhysicianId { get; set; }
        public int? AdminId { get; set; }
        public int? TransPhyId { get; set; }
        public string? Notes { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Boolean? IsTransferBackToAdmin { get; set; }

    }
}
