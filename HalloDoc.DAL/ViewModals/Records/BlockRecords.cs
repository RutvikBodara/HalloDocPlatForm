using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.DAL.ViewModals.Records
{
    public class BlockRecords
    {
        public string? FirstName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set;}
        public string? CreatedDate {  get; set; }
        public int? reqid { get; set;}
        public bool? isactive { get; set; }
        public string? BlockNotes { get; set; }
        public int? Blockid { get; set; }

    }
}
