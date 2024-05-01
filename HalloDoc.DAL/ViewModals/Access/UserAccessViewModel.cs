using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.DAL.ViewModals.Access
{
    public class UserAccessViewModel
    {
        public IEnumerable<UserAccessIndexViewModel>? UserAccessIndexViewModel { get; set; }
        public int PageNumber { get; set; }
        public int maxPage { get; set; }
    }
}
