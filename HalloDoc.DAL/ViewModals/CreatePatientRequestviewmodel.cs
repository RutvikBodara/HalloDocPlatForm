using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace HalloDoc.DAL.ViewModals
{
    public class CreatePatientRequestviewmodel
    {
        [Column("notes")]
        [StringLength(500)]
        public string? Notes { get; set; }

        [Column("firstname")]
        [StringLength(100)]
        public string Firstname { get; set; } = null!;

        [Column("lastname")]
        [StringLength(100)]
        public string? Lastname { get; set; }

        [Column("email")]
        [StringLength(50)]
        [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "Invalid email format.")]
        public required string Email { get; set; }
         
        [Column("phonenumber")]
        //[RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be 10 digits")]
        public string? Phonenumber { get; set; }

        [Column("street")]
        [StringLength(100)]
        public string? Street { get; set; }

        [Column("city")]
        [StringLength(100)]
        public string? City { get; set; }

        [Column("state")]
        [StringLength(100)]
        public string? State { get; set; }

        [Required]
        public int regid {get; set; }

        [Column("zipcode")]
        [StringLength(10)]
        public string? Zipcode { get; set; }

        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*()]).{8,}$", ErrorMessage = "Password must have at least 8 characters, one uppercase letter, one lowercase letter, one digit, and one special character.")]
        [DataType(DataType.Password)]
        public string? password { get; set; }

        [Compare(nameof(password), ErrorMessage = "Passwords do not match.")]
        [DataType(DataType.Password)]
        public string? Conpassword { get; set; }
        public DateOnly dateofbirth{get; set; }
        public IFormFile? uploadFile { get; set; }

        public string? AdminNotes { get; set;}

        public string? PhysicianNotes { get; set; }

        public string? DateOfBirth { get; set; }    

        public string? CountryCode {  get; set; }

    }
}
