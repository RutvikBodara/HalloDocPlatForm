using HalloDoc.DAL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.DAL.ViewModals.Schedule
{
    public class PendingShiftCombine
    {
        public IEnumerable<ReqestedShiftDetailsviewmodel>? ReqestedShiftDetailsviewmodel { get; set; }
        public int PageNumber { get; set; }
        public int maxPage { get; set; }
        public IEnumerable<Region>? RegionData {get; set; }
    }
}
