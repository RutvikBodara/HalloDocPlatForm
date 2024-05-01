using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.DAL.ViewModals.AdminDashBoardViewModels
{
    public class UnpaidCombine
    {
        public IEnumerable<UnPaidRequestViewModel>? UnPaidRequestViewModels {  get; set; }
        public int PageNumber { get; set; }
        public int maxPage { get; set; }
    }
}
