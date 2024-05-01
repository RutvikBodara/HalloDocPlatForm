using Org.BouncyCastle.Bcpg.OpenPgp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.DAL.ViewModals.Records
{
    public class SearchPatientRecords
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? phoneNumber { get; set; }
        public string? EmailAddress { get; set;}
    }
}
