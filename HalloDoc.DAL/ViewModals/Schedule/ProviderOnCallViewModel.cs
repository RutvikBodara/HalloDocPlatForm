using HalloDoc.DAL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.DAL.ViewModals.Schedule
{
    public class ProviderOnCallViewModel
    {
        public IQueryable<Physician>? ProviderOnCall { get; set; }
        public IEnumerable<Physician>? ProviderOffDuty { get; set; }
    }
}
