using Microsoft.AspNetCore.Http.Features;
using Org.BouncyCastle.Asn1.Crmf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.DAL.ViewModals.Records
{
    public class PatientHistory
    {
        public string? Member {  get; set; }
        public DateTime? CreateDate { get; set; }
        public string? ConfirmationNumber { get; set; }
        public string? Provider {  get; set; }
        public DateTime? concludedDate { get; set; }
        public string? status { get; set; }
        public int? requestid { get; set; }
        public int? reportID { get; set; }
        public int? DocCount { get; set; }  
    }
}
