using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.DAL.ViewModals.AdminDashBoardViewModels
{
    public class ConcludeRequestViewModel
    {
        public string? Firstname { get; set; }

        public string? DateOfBirth { get; set; }

        public string? Requestor { get; set; }

        public string? PhysicianName { get; set; }

        public DateTime? DateOfService { get; set; }

        public string? Phonenumber { get; set; }

        public string? Address { get; set; }

        public int? RequestType { get; set; }

        public string? MobileRequestor { get; set; }

        public string? Email { get; set; }

        public int? RequestId { get; set; }
        public bool? IsFinalize { get; set; }
    }
}
