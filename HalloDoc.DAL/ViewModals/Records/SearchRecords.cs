using HalloDoc.DAL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.DAL.ViewModals.Records
{
    public class SearchRecords
    {
        public string? PatientName {  get; set; }
        public int? RequestId { get; set; }
        public int? RequestClientId { get; set; }
        public string? Requestor{ get; set;}
        public string? DateOfService { get; set; }
        public string? CloseCaseDate { get; set; }
        public string? Email {  get; set; }
        public string? PhoneNumber {  get; set; }
        public string? Address { get; set; }
        public string? Zip {  get; set; }
        public  string? requeststatus { get; set; }
        public string? PhysicianName {  get; set; }
        public string? PhysicianNotes {  get; set; }
        public string? CancelledByProviderNotes { get; set; }
        public string? AdminNotes {  get; set; }
        public string? PatientNotes { get; set;}
        public string? RequestType { get; set; }
    }
}
