using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.DAL.ViewModals.AdminDashBoardViewModels
{
    public  class PendingRequestViewModel
    {
        public string? Firstname { get; set; }

        public string? DateOfBirth { get; set; }

        public string? Requestor { get; set; }

        public string? PhysicianName { get; set; }

        public DateTime? DateOfService { get; set; }

        public string? Phonenumber { get; set; }
        public int? PhysicianId{ get; set; }    
        public string? Address { get; set; }

        public string? Notes { get; set; }

        public int? RequestType { get; set; }

        public string? MobileRequestor { get; set; }

        public int? RequestId { get; set; }

        public string? Email {  get; set; }

        public string? Region { get; set; }
    }
}
