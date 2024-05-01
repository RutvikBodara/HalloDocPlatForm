using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HalloDoc.DAL.DataModels;

[Table("encounterformdetails")]
[Index("ReqId", Name = "fki_Reqid_fkey")]
[Index("Adminid", Name = "fki_adminid_fkey")]
[Index("Physicianid", Name = "fki_physicianid_fkey")]
[Index("Reqestclientid", Name = "fki_reqclient_fkey")]
public partial class Encounterformdetail
{
    [Key]
    [Column("encounterformdetailsid")]
    public int Encounterformdetailsid { get; set; }

    [Column("adminid")]
    public int? Adminid { get; set; }

    [Column("physicianid")]
    public int? Physicianid { get; set; }

    [Column("reqestclientid")]
    public int? Reqestclientid { get; set; }

    [Column("injuryhistory", TypeName = "character varying")]
    public string? Injuryhistory { get; set; }

    [Column("medicalhistory", TypeName = "character varying")]
    public string? Medicalhistory { get; set; }

    [Column("medications", TypeName = "character varying")]
    public string? Medications { get; set; }

    [Column("allergies", TypeName = "character varying")]
    public string? Allergies { get; set; }

    [Column("temp", TypeName = "character varying")]
    public string? Temp { get; set; }

    [Column("hr", TypeName = "character varying")]
    public string? Hr { get; set; }

    [Column("rr", TypeName = "character varying")]
    public string? Rr { get; set; }

    [Column("bp(s)", TypeName = "character varying")]
    public string? BpS { get; set; }

    [Column("bp(d)", TypeName = "character varying")]
    public string? BpD { get; set; }

    [Column("o2", TypeName = "character varying")]
    public string? O2 { get; set; }

    [Column("pain", TypeName = "character varying")]
    public string? Pain { get; set; }

    [Column("heent", TypeName = "character varying")]
    public string? Heent { get; set; }

    [Column("cv", TypeName = "character varying")]
    public string? Cv { get; set; }

    [Column("chest", TypeName = "character varying")]
    public string? Chest { get; set; }

    [Column("abd", TypeName = "character varying")]
    public string? Abd { get; set; }

    [Column("extr", TypeName = "character varying")]
    public string? Extr { get; set; }

    [Column("skin", TypeName = "character varying")]
    public string? Skin { get; set; }

    [Column("neuro", TypeName = "character varying")]
    public string? Neuro { get; set; }

    [Column("other", TypeName = "character varying")]
    public string? Other { get; set; }

    [Column("disgnosis", TypeName = "character varying")]
    public string? Disgnosis { get; set; }

    [Column("treatmentplan", TypeName = "character varying")]
    public string? Treatmentplan { get; set; }

    [Column("medicationdispensed", TypeName = "character varying")]
    public string? Medicationdispensed { get; set; }

    [Column("procedure", TypeName = "character varying")]
    public string? Procedure { get; set; }

    [Column("followup")]
    public short? Followup { get; set; }

    public int ReqId { get; set; }

    [ForeignKey("Adminid")]
    [InverseProperty("Encounterformdetails")]
    public virtual Admin? Admin { get; set; }

    [ForeignKey("Physicianid")]
    [InverseProperty("Encounterformdetails")]
    public virtual Physician? Physician { get; set; }

    [ForeignKey("ReqId")]
    [InverseProperty("Encounterformdetails")]
    public virtual Request Req { get; set; } = null!;

    [ForeignKey("Reqestclientid")]
    [InverseProperty("Encounterformdetails")]
    public virtual Requestclient? Reqestclient { get; set; }
}
