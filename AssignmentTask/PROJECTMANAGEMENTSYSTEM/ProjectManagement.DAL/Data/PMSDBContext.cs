using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using ProjectManagement.DAL.DataModels;

namespace ProjectManagement.DAL.Data;

public partial class PMSDBContext : DbContext
{
    public PMSDBContext()
    {
    }

    public PMSDBContext(DbContextOptions<PMSDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Domain> Domains { get; set; }

    public virtual DbSet<Project> Projects { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("User ID = postgres;Password=rutvik10@#;Server=localhost;Port=5432;Database=PMSDataBase;Integrated Security=true;Pooling=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Domain>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Domain_pkey");
        });

        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Project_pkey");

            entity.HasOne(d => d.DomainNavigation).WithMany(p => p.Projects)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Domain_FKey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
