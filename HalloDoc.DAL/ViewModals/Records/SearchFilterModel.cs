using HalloDoc.DAL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.DAL.ViewModals.Records
{
    public class SearchFilterModel
    {
        public int? reqstatus {  get; set; }
        public string? patName {  get; set; }
        public int? RequestType { get; set; }

        public DateTime? FromDate {  get; set; }
        public DateTime? ToDate { get;set; }
        public string? ProviderName { get; set;}
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }    
        public IEnumerable<Requeststatus>? requeststatuses { get; set; }
        public IEnumerable<Requesttype>?  requesttypes { get; set; }
    }
}
