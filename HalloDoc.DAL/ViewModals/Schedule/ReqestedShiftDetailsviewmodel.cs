using HalloDoc.DAL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.DAL.ViewModals.Schedule
{
    public class ReqestedShiftDetailsviewmodel
    {
        public int? phyid {  get; set; }
        public string? Physicianname { get; set; }
        public Physician? physicians { get; set; }
        public DateOnly? ShiftDate { get; set; }
        public TimeOnly? Start { get; set; }
        public TimeOnly? End { get; set; }
        public string? Region { get; set; }
        public int? RegionId { get; set; }   
        public int ShiftdetailId {get;set;}
        public int? status { get; set;}
    }
}
