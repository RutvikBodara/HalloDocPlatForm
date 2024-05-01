using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HalloDoc.DAL.DataModels;

[Table("ProviderDocument")]
[Index("FileType", Name = "fki_ProviderDocumentTypes_fkey")]
public partial class ProviderDocument
{
    [Key]
    public int ProviderDocumentId { get; set; }

    [Column("PhysicianID")]
    public int? PhysicianId { get; set; }

    [Column(TypeName = "character varying")]
    public string? Filename { get; set; }

    public bool? IsDeleted { get; set; }

    public int? FileType { get; set; }

    [ForeignKey("FileType")]
    [InverseProperty("ProviderDocuments")]
    public virtual ProviderDocumentType? FileTypeNavigation { get; set; }

    [ForeignKey("PhysicianId")]
    [InverseProperty("ProviderDocuments")]
    public virtual Physician? Physician { get; set; }
}
