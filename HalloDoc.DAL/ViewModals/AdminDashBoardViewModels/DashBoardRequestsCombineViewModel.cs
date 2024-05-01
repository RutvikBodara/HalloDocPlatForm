using HalloDoc.DAL.ViewModals.AdminDashBoardActions.NewRequestAction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.DAL.ViewModals.AdminDashBoardViewModels
{
    public class DashBoardRequestsCombineViewModel
    {
        public DataCount? countData {  get; set; }

        public NewRequestCombineViewModel? newRequestViewModel { get; set; }

        public IEnumerable<PendingRequestViewModel> PendingRequestViewModels { get; set; }

        public IEnumerable<ActiveRequestViewModel> ActiveRequestViewModels { get; set; }

        public  IEnumerable<ConcludeRequestViewModel> ConcludeRequestViewModels { get; set; }
        public IEnumerable<ToCloseRequestViewModel> ToCloseRequestViewModels { get; set; }

        public IEnumerable<UnPaidRequestViewModel> UnPaidRequestViewModels { get; set; }
    }
}
