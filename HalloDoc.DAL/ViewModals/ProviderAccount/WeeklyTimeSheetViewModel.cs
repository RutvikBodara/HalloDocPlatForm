using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.DAL.ViewModals.ProviderAccount
{
    public class WeeklyTimeSheetViewModel
    {
        public DateOnly Date {  get; set; }
        public double?  OnCallHours { get; set; }
        public int? TotalHours { get; set; }
        public bool? IsHoliday { get; set; }
        public int? NumberOfHouseCall{ get; set;}
        public int? NumberOfPhoneConsult { get; set;}

        //For add Reciepients
        public string? Item {  get; set; }
        public int? Amount {  get; set; }
        public IFormFile? Bill{ get; set;}

        //FOR POSTMETHOD
        public List<int>? TotalHoursPost {  get; set; }
        public List<int>? HouseCallPost { get; set; }
        public List<int>? PhoneConsult { get; set; }
    }
}
