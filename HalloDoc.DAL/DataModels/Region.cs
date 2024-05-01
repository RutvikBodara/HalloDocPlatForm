using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HalloDoc.DAL.DataModels;

[Table("region")]
public partial class Region
{
    [Key]
    [Column("regionid")]
    public int Regionid { get; set; }

    [Column("name")]
    [StringLength(50)]
    public string Name { get; set; } = null!;

    [Column("abbreviation")]
    [StringLength(50)]
    public string? Abbreviation { get; set; }

    [InverseProperty("Region")]
    public virtual ICollection<Adminregion> Adminregions { get; } = new List<Adminregion>();

    [InverseProperty("Region")]
    public virtual ICollection<Admin> Admins { get; } = new List<Admin>();

    [InverseProperty("Region")]
    public virtual ICollection<Business> Businesses { get; } = new List<Business>();

    [InverseProperty("Region")]
    public virtual ICollection<Concierge> Concierges { get; } = new List<Concierge>();

    [InverseProperty("Region")]
    public virtual ICollection<Physicianregion> Physicianregions { get; } = new List<Physicianregion>();

    [InverseProperty("Region")]
    public virtual ICollection<Physician> Physicians { get; } = new List<Physician>();

    [InverseProperty("Region")]
    public virtual ICollection<Requestclient> Requestclients { get; } = new List<Requestclient>();

    [InverseProperty("Region")]
    public virtual ICollection<Shiftdetailregion> Shiftdetailregions { get; } = new List<Shiftdetailregion>();

    [InverseProperty("Region")]
    public virtual ICollection<Shiftdetail> Shiftdetails { get; } = new List<Shiftdetail>();

    [InverseProperty("Region")]
    public virtual ICollection<User> Users { get; } = new List<User>();
}
