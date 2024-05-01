using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.DAL.ViewModals.Records
{
    public class EmailLogsData
    {
        public string? Recipient { get; set; }
        public string? actionName { get; set; } 
        public string? RoleName { get; set; }   
        public string? Email { get; set; }
        public string? Mobile { get; set; }
        public string? CreatedDate { get; set;}
        public string? SentDate { get; set; }
        public string? sent {  get; set; }
        public string? sentTrie {  get; set; }
        public string? ConfirmationNumber {  get; set; }
    }
}
