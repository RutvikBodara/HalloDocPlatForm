using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ProjectManagement.DAL.DataModels;

[Table("Project")]
[Index("DomainId", Name = "fki_Domain_FKey")]
public partial class Project
{
    [Key]
    public int Id { get; set; }

    [Column(TypeName = "character varying")]
    public string ProjectName { get; set; } = null!;

    [Column(TypeName = "character varying")]
    public string Assignee { get; set; } = null!;

    [Column("DomainID")]
    public int DomainId { get; set; }

    [Column(TypeName = "character varying")]
    public string Description { get; set; } = null!;

    public DateOnly DueDate { get; set; }

    [Column(TypeName = "character varying")]
    public string Domain { get; set; } = null!;

    [Column(TypeName = "character varying")]
    public string City { get; set; } = null!;

    [ForeignKey("DomainId")]
    [InverseProperty("Projects")]
    public virtual Domain DomainNavigation { get; set; } = null!;
}
