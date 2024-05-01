using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using HalloDoc.DAL.DataModels;

namespace HalloDoc.DAL.ViewModals.AdminDashBoardViewModels
{
    public class NewRequestViewModel
    {
        public string? Firstname { get; set; }

        public string? DateOfBirth { get; set; }

        public string? Requestor { get; set; }

        public DateTime? Requesterdate { get; set; }

        public string? Phonenumber { get; set; }

        public string? Address { get; set; }  
        
        public string? Notes { get; set; }

        public int? RequestType { get; set; }

        public int? RequestId { get; set; }

        public string? Email { get; set; }

        public string? MobileRequestor {  get; set; }

        public string? Region { get; set; }

        public int? status {get; set; }
    }
}
