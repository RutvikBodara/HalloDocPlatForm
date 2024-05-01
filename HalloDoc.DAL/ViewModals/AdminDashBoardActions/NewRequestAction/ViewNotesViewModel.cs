using HalloDoc.DAL.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.DAL.ViewModals.AdminDashBoardActions.NewRequestAction
{
    public class ViewNotesViewModel
    {
        public int RequestId { get; set; }
        public string? PhysicianNotes { get; set; }
        [Required(ErrorMessage = "Please add Notes!")]
        public string? AdminNotes { get; set; }
        public int? status { get; set; }
        public string? casetag { get; set; }
        public IEnumerable<ReqStatusLogViewModel>? ReqstatusLog { get; set; }
    }
}
