using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HalloDoc.DAL.DataModels;

public partial class ProviderDocumentType
{
    [Key]
    public int ProviderDocumentTypesId { get; set; }

    [Column(TypeName = "character varying")]
    public string? Name { get; set; }

    [InverseProperty("FileTypeNavigation")]
    public virtual ICollection<ProviderDocument> ProviderDocuments { get; } = new List<ProviderDocument>();
}
