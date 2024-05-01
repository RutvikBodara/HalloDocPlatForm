﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.DAL.ViewModals.Records
{
    public class SearchRecordsViewModel
    {
        public IEnumerable<SearchRecords>? SearchRecords { get; set; }
        public int PageNumber { get; set; }
        public int maxPage { get; set; }
    }
}
