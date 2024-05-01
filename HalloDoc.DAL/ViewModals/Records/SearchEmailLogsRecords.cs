using HalloDoc.DAL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.DAL.ViewModals.Records
{
    public class SearchEmailLogsRecords
    {
        public int? RoleId { get; set; }
        public string? Receiver {  get; set; }
        public string? Email { get; set; }
        public string? mobile { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? SentDate { get; set;}
        public IEnumerable<Role>? RoleData { get; set; }
    }
}
