using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.DAL.ViewModals
{
    public class PatientProfileEdit
    {

        public required string Firstname { get; set; }
        public string? Lastname { get; set; }
        public string? Mobile { get; set; }
        public string? DateOfBirth { get; set; }
        public required string Email { get; set; }
        public string? City { get; set; }
        public string? Street { get; set; }
        public string? State { get; set; }
        public string? Zipcode { get; set; }
        public string? aspnetid { get; set; }
        public string? userid { get; set; }
        public int? regid { get; set; }
        public String? countryCode { get; set; }

    }
}

