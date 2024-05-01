using HalloDoc.DAL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.DAL.ViewModals.AdminDashBoardActions
{
    public class AdminProfileViewModel
    { 
        public Admin? AdminData {  get; set; }
        public string? CountryCode {  get; set; }
        public IEnumerable<Region>? selectedRegion { get; set; }
        public IEnumerable<Region>? nonSelectedRegion { get; set; }
        public List<int>? RegionList { get; set; }
    }
}
