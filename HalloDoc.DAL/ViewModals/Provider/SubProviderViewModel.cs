using HalloDoc.DAL.DataModels;
using Org.BouncyCastle.Asn1.X509;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.DAL.ViewModals.Provider
{
    public class SubProviderViewModel
    {
        public bool? IsNotificationStopped { get; set; }
        public int? PhysicianId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? RoleName { get; set; }
        public string? StatusName { get; set; }
        public string? Mobile {  get; set; }
        public string? Email {  get; set; }


        //public Physician? physician {  get; set; }
        //public Status? status { get; set; }
        //public Role? role { get; set; }
        //public Physiciannotification? Physiciannotification { get; set; }
        public string? ProviderOnCall { get; set; }  

    }
}