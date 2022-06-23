using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using OrgChartEngines.Models.OrgChart;
using System.Configuration;

namespace OrgChartEngines.Data
{
    public partial class Engines_PCContext : DbContext
    {
        public Engines_PCContext()
        {
        }

        public Engines_PCContext(DbContextOptions<Engines_PCContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Departament> Departaments { get; set; } = null!;
        public virtual DbSet<Line> Lines { get; set; } = null!;
        public virtual DbSet<OrgChartPosition> OrgChartPositions { get; set; } = null!;
        public virtual DbSet<PositionUser> PositionUsers { get; set; } = null!;
        public virtual DbSet<RolesDepartment> RolesDepartments { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                //optionsBuilder.UseSqlServer("data source=DESKTOP-B4BPL0M;initial catalog=Weld_PC;user id=sa;password=Insytech;Integrated Security=false");
                //optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Departament>(entity =>
            {
                entity.ToTable("Departament");

                entity.Property(e => e.DepartamentName).HasMaxLength(50);
            });

            modelBuilder.Entity<Line>(entity =>
            {
                entity.ToTable("Line");

                entity.HasKey(e => e.line_id);

                entity.Property(e => e.line_name).HasMaxLength(50);
            });

            modelBuilder.Entity<OrgChartPosition>(entity =>
            {
                entity.HasKey(e => e.IdPosition);

                entity.ToTable("OrgChart_Positions");

                entity.Property(e => e.Assemblies).HasMaxLength(50);

                entity.Property(e => e.IdRol).HasMaxLength(128);

                entity.Property(e => e.IdUser).HasMaxLength(128);

                entity.Property(e => e.Title).HasMaxLength(100);
            });

            modelBuilder.Entity<PositionUser>(entity =>
            {
                entity.HasKey(e => e.IdPositionUser);

                entity.ToTable("PositionUser");

                entity.Property(e => e.IdUser).HasMaxLength(128);

                entity.HasOne(d => d.IdPositionNavigation)
                    .WithMany(p => p.PositionUsers)
                    .HasForeignKey(d => d.IdPosition)
                    .HasConstraintName("FK_PositionUser_OrgChart_Positions");
            });

            modelBuilder.Entity<RolesDepartment>(entity =>
            {
                entity.HasKey(e => e.IdRolDepartment);

                entity.Property(e => e.IdRol).HasMaxLength(128);

                entity.HasOne(d => d.IdDepartmentNavigation)
                    .WithMany(p => p.RolesDepartments)
                    .HasForeignKey(d => d.IdDepartment)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RolesDepartments_Departament");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
