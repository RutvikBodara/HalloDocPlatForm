using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HalloDoc.DAL.DataModels;

[Table("status")]
public partial class Status
{
    [Key]
    [Column("statusID")]
    public int StatusId { get; set; }

    [Column("statusname", TypeName = "character varying")]
    public string Statusname { get; set; } = null!;

    [InverseProperty("StatusNavigation")]
    public virtual ICollection<Admin> Admins { get; } = new List<Admin>();

    [InverseProperty("StatusNavigation")]
    public virtual ICollection<Physician> Physicians { get; } = new List<Physician>();
}
