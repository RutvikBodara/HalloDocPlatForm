using HalloDoc.DAL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.DAL.ViewModals.Access
{
    public class AcccountAccessViewModel
    {
        public IEnumerable<Role> roles {  get; set; }
        public int PageNumber { get; set; }
        public int maxPage { get; set; }

    }
}
