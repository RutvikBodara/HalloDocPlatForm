﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.DAL.ViewModals.AdminDashBoardViewModels
{
    public class ConcludeRequestCombine
    {
        public IEnumerable<ConcludeRequestViewModel>? concludeRequestViewModels { get; set; }
        public int PageNumber { get; set; }
        public int maxPage { get; set; }
    }
}