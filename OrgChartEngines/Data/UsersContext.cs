using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace OrgChartEngines.Models.Users
{
    public partial class UsersContext : DbContext
    {
        public UsersContext()
        {
        }

        public UsersContext(DbContextOptions<UsersContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AspNetRole> AspNetRoles { get; set; } = null!;
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; } = null!;
        public virtual DbSet<DatabaseCatalog> DatabaseCatalogs { get; set; } = null!;
        public virtual DbSet<UsersDatabase> UsersDatabases { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                //optionsBuilder.UseSqlServer("data source=DESKTOP-B4BPL0M;initial catalog=MantenimientoDB;user id=sa;password=Insytech;Integrated Security=false");
                //optionsBuilder.UseSqlServer(_configuration.GetConnectionString("UserConnection"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("Latin1_General_CI_AI");

            modelBuilder.Entity<AspNetRole>(entity =>
            {
                entity.HasIndex(e => e.Name, "RoleNameIndex")
                    .IsUnique();

                entity.Property(e => e.Id).HasMaxLength(128);

                entity.Property(e => e.ConcurrencyStamp)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.NormalizedName)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<AspNetUser>(entity =>
            {
                entity.HasIndex(e => e.UserName, "UserNameIndex")
                    .IsUnique();

                entity.Property(e => e.Id).HasMaxLength(128);

                entity.Property(e => e.ConcurrencyStamp)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.LockoutEnd).HasColumnType("datetime");

                entity.Property(e => e.LockoutEndDateUtc).HasColumnType("datetime");

                entity.Property(e => e.NormalizedEmail)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.NormalizedUserName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UserName).HasMaxLength(256);

                entity.HasMany(d => d.Roles)
                    .WithMany(p => p.Users)
                    .UsingEntity<Dictionary<string, object>>(
                        "AspNetUserRole",
                        l => l.HasOne<AspNetRole>().WithMany().HasForeignKey("RoleId").HasConstraintName("FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId"),
                        r => r.HasOne<AspNetUser>().WithMany().HasForeignKey("UserId").HasConstraintName("FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId"),
                        j =>
                        {
                            j.HasKey("UserId", "RoleId").HasName("PK_dbo.AspNetUserRoles");

                            j.ToTable("AspNetUserRoles");

                            j.HasIndex(new[] { "RoleId" }, "IX_RoleId");

                            j.HasIndex(new[] { "UserId" }, "IX_UserId");

                            j.IndexerProperty<string>("UserId").HasMaxLength(128);

                            j.IndexerProperty<string>("RoleId").HasMaxLength(128);
                        });
            });

            modelBuilder.Entity<DatabaseCatalog>(entity =>
            {
                entity.ToTable("DatabaseCatalog");

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<UsersDatabase>(entity =>
            {
                entity.ToTable("UsersDatabase");

                entity.Property(e => e.IdUser).HasMaxLength(128);

                entity.HasOne(d => d.IdDatabaseNavigation)
                    .WithMany(p => p.UsersDatabases)
                    .HasForeignKey(d => d.IdDatabase)
                    .HasConstraintName("FK_UsersDatabase_DatabaseCatalog");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.UsersDatabases)
                    .HasForeignKey(d => d.IdUser)
                    .HasConstraintName("FK_UsersDatabase_AspNetUsers");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
