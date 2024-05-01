using HalloDoc.DAL.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.DAL.ViewModals.AdminDashBoardActions.NewRequestAction
{
    public class ViewCasesViewModel
    {
        public string? Notes { get; set; }
        public string? ConfirmationNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Date { get; set; }
        [Required]
        //[RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be 10 digits")]
        public string? PhoneNumber { get; set; }
        [Required]
        [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "Invalid email format.")]
        public string? Email { get; set; }
        public string? Region { get; set; }
        public string? Business { get; set; }
        public string? Room { get; set; }
        public int? RequestId { get; set; }
        public IList<Region>? Regions { get; set; }
        public IList<Physician>? physicians { get; set; }
        public IList<Casetag>? casetags { get; set; }

        public string? PatientCountryCode {  get; set; }
    }
}
