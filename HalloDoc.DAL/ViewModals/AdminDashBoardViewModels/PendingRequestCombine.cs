﻿using HalloDoc.DAL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.DAL.ViewModals.AdminDashBoardViewModels
{
    public class PendingRequestCombine
    {
        public IEnumerable<PendingRequestViewModel>? PendingRequestViewModels { get; set; }
        public IList<Region>? Regions { get; set; }
        public IList<Physician>? physicians { get; set; }
        public string? Search { get; set; }
        public int PageNumber { get; set; }
        public int maxPage { get; set; }
    }
}
