using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HalloDoc.DAL.DataModels;

[Table("requeststatus")]
public partial class Requeststatus
{
    [Key]
    [Column("requeststatusid")]
    public int Requeststatusid { get; set; }

    [Column("name", TypeName = "character varying")]
    public string? Name { get; set; }
}
