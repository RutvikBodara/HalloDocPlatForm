using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HalloDoc.DAL.DataModels;

[Table("AccountType")]
public partial class AccountType
{
    [Key]
    public short AccountTypeId { get; set; }

    [Column("name", TypeName = "character varying")]
    public string? Name { get; set; }

    [InverseProperty("AccounttypeNavigation")]
    public virtual ICollection<Role> Roles { get; } = new List<Role>();
}
