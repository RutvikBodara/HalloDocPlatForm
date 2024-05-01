using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ProjectManagement.DAL.DataModels;

[Table("Domain")]
public partial class Domain
{
    [Key]
    public int Id { get; set; }

    [Column(TypeName = "character varying")]
    public string Name { get; set; } = null!;

    [InverseProperty("DomainNavigation")]
    public virtual ICollection<Project> Projects { get; set; } = new List<Project>();
}
