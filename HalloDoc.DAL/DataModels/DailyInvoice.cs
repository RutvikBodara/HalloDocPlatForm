using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HalloDoc.DAL.DataModels;

[Table("DailyInvoice")]
[Index("WeeklyInvoiceId", Name = "fki_WeeklyInvoice_fkey")]
public partial class DailyInvoice
{
    [Key]
    public int Id { get; set; }

    public int WeeklyInvoiceId { get; set; }

    public double? TotalHours { get; set; }

    public bool? IsHoliday { get; set; }

    public int? CountHouseCall { get; set; }

    public int? CountPhoneConsult { get; set; }

    [Column("Modifiend_date")]
    public DateTime? ModifiendDate { get; set; }

    public bool? IsFinalized { get; set; }

    public DateOnly Date { get; set; }

    [ForeignKey("WeeklyInvoiceId")]
    [InverseProperty("DailyInvoices")]
    public virtual WeeklyInvoice WeeklyInvoice { get; set; } = null!;
}
