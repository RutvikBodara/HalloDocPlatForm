using HalloDoc.DAL.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.DAL.ViewModals
{
    public class PatientDashboardViewModel
    {
        [Column("createddate", TypeName = "timestamp without time zone")]
        public DateTime Createddate { get; set; }

        [Column("filename")]
        [StringLength(500)]
        public string StatusName { get; set; }

        public int RequestId { get; set; }

        [Column("filename")]
        public string Filename { get; set; } 

        public int docCount {  get; set; }    

        public int UserId {  get; set; }

    }
}
