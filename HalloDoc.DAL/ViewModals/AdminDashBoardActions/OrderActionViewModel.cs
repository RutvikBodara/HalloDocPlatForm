using HalloDoc.DAL.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.DAL.ViewModals.AdminDashBoardActions
{
    public class OrderActionViewModel
    {
        [Required]
        public IEnumerable<Healthprofessionaltype>? Profession { get; set; }

        [Required]
        public int professionName { get; set; }

        [Required]
        public int BusinessName { get; set; }

        [Required]
        public string? BusinessContact { get; set; }

        [Required]
        [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "Invalid email format.")]
        public string? BusinessEmail { get; set; }

        [Required]
        //[RegularExpression(@"\+1[2-9][0-9]{9}", ErrorMessage ="Invalid Fax formet")]
        public string? Faxnumber { get; set; }

        [Required]
        public string? Pres { get; set; }

        [Required]
        public int? refill { get; set; }

    }
}
