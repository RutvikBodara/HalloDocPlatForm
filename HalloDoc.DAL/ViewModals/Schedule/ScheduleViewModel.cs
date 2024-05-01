using HalloDoc.DAL.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.DAL.ViewModals.Schedule
{
    public class ScheduleViewModel
    {
        public IEnumerable<Physician>? Physician { get; set; }
        public IEnumerable<Shiftdetail>? Shiftdetail { get; set; }
        public IEnumerable<Shift>? shift { get; set; }
        public bool sunday { get; set; }
        public bool monday { get; set; }
        public bool Tuesday { get; set; }
        public bool Wednesday { get; set; }
        public bool Thursday { get; set; }
        public bool friday { get; set; }
        public bool saturday { get; set; }

        [Column("physicianid")]
        [Required]
        public int Physicianid { get; set; }

        [Required]
        [Column("startdate")]
        public DateOnly Startdate { get; set; }


        [Column("isrepeat")]
        public bool Isrepeat { get; set; }

        [Column("weekdays")]
        [StringLength(500)]
        public string? Weekdays { get; set; }

        [Column("repeatupto")]
        public int? Repeatupto { get; set; }

        [Required]
        [Column("shiftdate")]
        public DateOnly Shiftdate { get; set; }

        [Required]
        [Column("regionid")]
        public int Regionid { get; set; }

        [Required]
        [Column("starttime")]
        public TimeOnly Starttime { get; set; }

        [Required]
        [Column("endtime")]
        public TimeOnly Endtime { get; set; }

    }
}
