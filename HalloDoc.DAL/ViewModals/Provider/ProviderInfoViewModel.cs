using HalloDoc.DAL.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.DAL.ViewModals.Provider
{
    public class ProviderInfoViewModel
    {
        public IEnumerable<SubProviderViewModel>? physicians { get; set; }
        public int PageNumber { get; set; }
        public int maxPage { get; set; }
    }
}
