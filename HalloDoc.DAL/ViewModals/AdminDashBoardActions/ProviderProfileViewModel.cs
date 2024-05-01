using HalloDoc.DAL.DataModels;
using HalloDoc.DAL.ViewModals.Provider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.DAL.ViewModals.AdminDashBoardActions
{
    public class ProviderProfileViewModel
    {
        public required CreateProviderForm ProviderFormData { get; set; }
        public string? CountryCode { get; set; }
        public IEnumerable<Region>? selectedRegion { get; set; }
        public IEnumerable<Region>? nonSelectedRegion { get; set; }
        public List<int>? RegionList { get; set; }
    }
}
