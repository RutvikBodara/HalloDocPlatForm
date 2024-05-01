using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.DAL.ViewModals
{
    public class BusinessRequestviewmodel
    {
        [Column("firstname")]
        [StringLength(100)]
        public string? BusinessFirstname { get; set; }

        [Column("lastname")]
        [StringLength(100)]
        public string? BusinessLastname { get; set; }

        [Column("phonenumber")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be 10 digits")]
        public string? BusinessPhonenumber { get; set; }

        [Column("email")]
        [StringLength(50)]
        [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "Invalid email format.")]

        public required string BusinessEmail { get; set; }

        [Column("casenumber")]
        [StringLength(50)]
        public string? Casenumber { get; set; }

        [Column("address")]
        [StringLength(100)]
        public string? BusinessLocation { get; set; }

        [Column("notes")]
        [StringLength(500)]
        public string? Notes { get; set; }

        [Column("firstname")]
        [StringLength(100)]
        public string PatientFirstname { get; set; } = null!;

        [Column("lastname")]
        [StringLength(100)]
        public string? PatientLastname { get; set; }

        //partition dates

        [Column("phonenumber")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be 10 digits")]
        public string? PatientPhonenumber { get; set; }

        [Column("email")]
        [StringLength(50)]
        [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "Invalid email format.")]

        public required string PatientEmail { get; set; }

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
        [StringLength(10)]
        public string? Zipcode { get; set; }

        [Required]
        public int regid { get; set; }

        public DateOnly dateofbirth { get; set; }

        public required string BusinessCountryCode { get; set; }

        public required string PatientCountryCode { get; set; }

    }
}
