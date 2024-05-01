using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.DAL.ViewModals.Records
{
    public class PatientRecords
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email {  get; set; }
        public string? PhoneNumber { get; set; }    
        public string? Address { get; set; }    
        public int? UserId { get; set; }
    }
}
