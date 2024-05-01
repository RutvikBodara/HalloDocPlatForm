using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HalloDoc.DAL.DataModels;

[Table("WeeklyInvoice")]
public partial class WeeklyInvoice
{
    [Key]
    public int Id { get; set; }

    [Column("physicianid")]
    public int Physicianid { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    [Column("Modified_Date", TypeName = "timestamp without time zone")]
    public DateTime? ModifiedDate { get; set; }

    [Column("Created_Date", TypeName = "timestamp without time zone")]
    public DateTime CreatedDate { get; set; }

    [InverseProperty("WeeklyInvoice")]
    public virtual ICollection<DailyInvoice> DailyInvoices { get; } = new List<DailyInvoice>();
}
