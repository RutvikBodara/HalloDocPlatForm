using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.DAL.ViewModals
{
    public class ConciergeRequestviewmodel
    {
        [Column("conciergename")]
        [StringLength(100)]
        public string Conciergename { get; set; } = null!;

        [Column("conciergeLastname")]
        [StringLength(100)]
        public string Conciergelastname { get; set; } = null!;

        [Column("email")]
        [StringLength(50)]
        [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "Invalid email format.")]
        public required string ConciergeEmail { get; set; }

        [Column("phonenumber")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be 10 digits")]
        public string? ConciergePhonenumber { get; set; }

        [Column("street")]
        [StringLength(50)]
        public string Street { get; set; } = null!;

        [Column("city")]
        [StringLength(50)]
        public string City { get; set; } = null!;

        [Column("address")]
        [StringLength(150)]
        public string? Address { get; set; }

        [Column("zipcode")]
        [StringLength(50)]
        public string Zipcode { get; set; } = null!;

        [Column("notes")]
        [StringLength(500)]
        public string? Notes { get; set; }

        [Column("firstname")]
        [StringLength(100)]
        public string Firstname { get; set; } = null!;

        [Column("lastname")]
        [StringLength(100)]
        public string? Lastname { get; set; }

        [Column("phonenumber")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be 10 digits")]
        public string? Phonenumber { get; set; }

        [Column("email")]
        [StringLength(50)]
        [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "Invalid email format.")]
        public required string Email { get; set; }

        [Column("createddate", TypeName = "timestamp without time zone")]
        public DateTime Createddate { get; set; }

        [Required]
        public int regid { get; set; }

        public DateOnly dateofbirth { get; set; }

        public required string PatientCountryCode { get; set; }

        public required string ConciergeCountryCode { get; set; }

    }
}
