using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HalloDoc.DAL.DataModels;

[Table("physiciannotification")]
public partial class Physiciannotification
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("physicianid")]
    public int Physicianid { get; set; }

    [Column("isnotificationstopped")]
    public bool Isnotificationstopped { get; set; }

    [ForeignKey("Physicianid")]
    [InverseProperty("Physiciannotifications")]
    public virtual Physician Physician { get; set; } = null!;

    [InverseProperty("PhyNotification")]
    public virtual ICollection<Physician> Physicians { get; } = new List<Physician>();
}
