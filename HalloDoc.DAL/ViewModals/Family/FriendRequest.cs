using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace HalloDoc.DAL.ViewModals.Family
{
    public class FriendRequest
    {
        [Column("firstname")]
        [StringLength(100)]
        public string? Firstname { get; set; }


        [Column("lastname")]
        [StringLength(100)]
        public string? Lastname { get; set; }

        [Column("phonenumber")]
        //[RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be 10 digits")]
        public string? Phonenumber { get; set; }

        [Column("email")]
        [StringLength(50)]
        [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "Invalid email format.")]
        public string? Email { get; set; }

        [Column("relationname")]
        [StringLength(100)]
        public string? Relationname { get; set; }

        [Column("notes")]
        [StringLength(500)]
        public string? patientNotes { get; set; }

        [Column("firstname")]
        [StringLength(100)]
        public string patientFirstname { get; set; } = null!;

        [Column("lastname")]
        [StringLength(100)]
        public string? patientLastname { get; set; }

        [Column("createddate", TypeName = "timestamp without time zone")]
        public DateTime Createddate { get; set; }

        [Column("email")]
        [StringLength(50)]
        [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "Invalid email format.")]
        public required string PatientEmail { get; set; }

        [Required]
        public int regid { get; set; }

        [Column("phonenumber")]
        [StringLength(23)]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be 10 digits")]
        public string? PatientPhonenumber { get; set; }

        [Column("street")]
        [StringLength(100)]
        public string? Street { get; set; }

        [Column("city")]
        [StringLength(100)]
        public string? City { get; set; }

        [Column("state")]
        [StringLength(100)]
        public string? State { get; set; }

        [Column("zipcode")]
        [RegularExpression(@"^\d{6}$", ErrorMessage = "ZipCode must be 6 digits")]
        public string? Zipcode { get; set; }

        public DateOnly dateofbirth { get; set; }

        public IFormFile? uploadFile { get; set; }

        public  string? PatientCountryCode {get; set;}
        public  string? FamilyCountryCode { get; set; }
    }
}
