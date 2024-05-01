using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HalloDoc.DAL.DataModels;

[Table("physician")]
[Index("PhyNotificationid", Name = "fki_Phy_notify_pkey")]
[Index("Regionid", Name = "fki_phy_region_pkey")]
[Index("Roleid", Name = "fki_role_f_key")]
[Index("Status", Name = "fki_status_f_key")]
public partial class Physician
{
    [Key]
    [Column("physicianid")]
    public int Physicianid { get; set; }

    [Column("aspnetuserid")]
    [StringLength(128)]
    public string? Aspnetuserid { get; set; }

    [Column("firstname")]
    [StringLength(100)]
    public string Firstname { get; set; } = null!;

    [Column("lastname")]
    [StringLength(100)]
    public string? Lastname { get; set; }

    [Column("email")]
    [StringLength(50)]
    public string Email { get; set; } = null!;

    [Column("mobile")]
    [StringLength(20)]
    public string? Mobile { get; set; }

    [Column("medicallicense")]
    [StringLength(500)]
    public string? Medicallicense { get; set; }

    [Column("photo")]
    [StringLength(100)]
    public string? Photo { get; set; }

    [Column("adminnotes")]
    [StringLength(500)]
    public string? Adminnotes { get; set; }

    [Column("isagreementdoc")]
    public bool? Isagreementdoc { get; set; }

    [Column("isbackgrounddoc")]
    public bool? Isbackgrounddoc { get; set; }

    [Column("istrainingdoc")]
    public bool? Istrainingdoc { get; set; }

    [Column("isnondisclosuredoc")]
    public bool? Isnondisclosuredoc { get; set; }

    [Column("address1")]
    [StringLength(500)]
    public string? Address1 { get; set; }

    [Column("address2")]
    [StringLength(500)]
    public string? Address2 { get; set; }

    [Column("city")]
    [StringLength(100)]
    public string? City { get; set; }

    [Column("regionid")]
    public int? Regionid { get; set; }

    [Column("zip")]
    [StringLength(10)]
    public string? Zip { get; set; }

    [Column("altphone")]
    [StringLength(20)]
    public string? Altphone { get; set; }

    [Column("createdby")]
    [StringLength(128)]
    public string Createdby { get; set; } = null!;

    [Column("createddate", TypeName = "timestamp(0) without time zone")]
    public DateTime Createddate { get; set; }

    [Column("modifiedby")]
    [StringLength(128)]
    public string? Modifiedby { get; set; }

    [Column("modifieddate", TypeName = "timestamp without time zone")]
    public DateTime? Modifieddate { get; set; }

    [Column("status")]
    public int? Status { get; set; }

    [Column("businessname")]
    [StringLength(100)]
    public string Businessname { get; set; } = null!;

    [Column("businesswebsite")]
    [StringLength(200)]
    public string Businesswebsite { get; set; } = null!;

    [Column("isdeleted")]
    public bool? Isdeleted { get; set; }

    [Column("roleid")]
    public int? Roleid { get; set; }

    [Column("npinumber")]
    [StringLength(500)]
    public string? Npinumber { get; set; }

    [Column("islicensedoc")]
    public bool? Islicensedoc { get; set; }

    [Column("signature")]
    [StringLength(100)]
    public string? Signature { get; set; }

    [Column("iscredentialdoc")]
    public bool? Iscredentialdoc { get; set; }

    [Column("istokengenerate")]
    public bool? Istokengenerate { get; set; }

    [Column("syncemailaddress")]
    [StringLength(50)]
    public string? Syncemailaddress { get; set; }

    public int? PhyNotificationid { get; set; }

    [Column("lattitude")]
    public double? Lattitude { get; set; }

    [Column("longtitude")]
    public double? Longtitude { get; set; }

    [ForeignKey("Aspnetuserid")]
    [InverseProperty("PhysicianAspnetusers")]
    public virtual Aspnetuser? Aspnetuser { get; set; }

    [ForeignKey("Createdby")]
    [InverseProperty("PhysicianCreatedbyNavigations")]
    public virtual Aspnetuser CreatedbyNavigation { get; set; } = null!;

    [InverseProperty("Physician")]
    public virtual ICollection<Encounterformdetail> Encounterformdetails { get; } = new List<Encounterformdetail>();

    [ForeignKey("Modifiedby")]
    [InverseProperty("PhysicianModifiedbyNavigations")]
    public virtual Aspnetuser? ModifiedbyNavigation { get; set; }

    [ForeignKey("PhyNotificationid")]
    [InverseProperty("Physicians")]
    public virtual Physiciannotification? PhyNotification { get; set; }

    [InverseProperty("Physician")]
    public virtual ICollection<Physicianlocation> Physicianlocations { get; } = new List<Physicianlocation>();

    [InverseProperty("Physician")]
    public virtual ICollection<Physiciannotification> Physiciannotifications { get; } = new List<Physiciannotification>();

    [InverseProperty("Physician")]
    public virtual ICollection<Physicianregion> Physicianregions { get; } = new List<Physicianregion>();

    [InverseProperty("Physician")]
    public virtual ICollection<ProviderDocument> ProviderDocuments { get; } = new List<ProviderDocument>();

    [ForeignKey("Regionid")]
    [InverseProperty("Physicians")]
    public virtual Region? Region { get; set; }

    [InverseProperty("Physician")]
    public virtual ICollection<Request> Requests { get; } = new List<Request>();

    [InverseProperty("Physician")]
    public virtual ICollection<Requeststatuslog> RequeststatuslogPhysicians { get; } = new List<Requeststatuslog>();

    [InverseProperty("Transtophysician")]
    public virtual ICollection<Requeststatuslog> RequeststatuslogTranstophysicians { get; } = new List<Requeststatuslog>();

    [InverseProperty("Physician")]
    public virtual ICollection<Requestwisefile> Requestwisefiles { get; } = new List<Requestwisefile>();

    [ForeignKey("Roleid")]
    [InverseProperty("Physicians")]
    public virtual Role? Role { get; set; }

    [InverseProperty("Physician")]
    public virtual ICollection<Shift> Shifts { get; } = new List<Shift>();

    [ForeignKey("Status")]
    [InverseProperty("Physicians")]
    public virtual Status? StatusNavigation { get; set; }
}
