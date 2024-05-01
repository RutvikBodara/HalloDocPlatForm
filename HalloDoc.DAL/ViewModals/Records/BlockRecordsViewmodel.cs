using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.DAL.ViewModals.Records
{
    public class BlockRecordsViewmodel
    {
        public IEnumerable<BlockRecords> BlockRecords { get; set; }
        public int? PageNumber { get; set; }
        public int? maxPage { get; set; }
    }
}
