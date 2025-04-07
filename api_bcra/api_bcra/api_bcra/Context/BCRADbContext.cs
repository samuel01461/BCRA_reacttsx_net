using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using api_bcra.Models;

namespace api_bcra.Context;

public partial class BCRADbContext : DbContext
{
    public BCRADbContext()
    {
    }

    public BCRADbContext(DbContextOptions<BCRADbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Entity> Entities { get; set; }

    public virtual DbSet<Person> People { get; set; }

    public virtual DbSet<Query> Queries { get; set; }

    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Scoring> Scorings { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=localhost;Database=bcra_api;User=bcra_usr;Password=bcra123_;TrustServerCertificate=True;MultipleActiveResultSets=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Entity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Entities__3214EC07ABB0B4B0");

            entity.Property(e => e.Name).HasColumnType("varchar(255)");
        });

        modelBuilder.Entity<Person>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__People__3214EC07F44D3737");

            entity.Property(e => e.Cuit)
                .HasColumnType("varchar(255)")
                .HasColumnName("CUIT");
            entity.Property(e => e.Fullname).HasColumnType("varchar(255)");
        });

        modelBuilder.Entity<Query>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Queries__3214EC077789A770");

            entity.Property(e => e.DateQuery)
                .HasColumnType("datetime")
                .HasColumnName("date_query");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.Queries)
                .HasForeignKey(d => d.IdUser)
                .HasConstraintName("FK__Queries__IdUser__5070F446");
        });

        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Refresh___3214EC0707B4DE2B");

            entity.ToTable("Refresh_tokens");

            entity.Property(e => e.CreationDate)
                .HasColumnType("datetime")
                .HasColumnName("Creation_date");
            entity.Property(e => e.ExpiryDate)
                .HasColumnType("datetime")
                .HasColumnName("Expiry_date");
            entity.Property(e => e.IdUser).HasColumnName("Id_user");
            entity.Property(e => e.Token)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Used)
        .HasColumnName("Used")
        .HasColumnType("tinyint");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.RefreshTokens)
                .HasForeignKey(d => d.IdUser)
                .HasConstraintName("FK__Refresh_t__Id_us__6FE99F9F");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Roles__3214EC07D56B1D0A");

            entity.Property(e => e.Name).HasColumnType("varchar(255)");
        });

        modelBuilder.Entity<Scoring>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Scoring__3214EC07C9359FE1");

            entity.ToTable("Scoring");

            entity.Property(e => e.Period).HasColumnType("varchar(255)");

            entity.HasOne(d => d.EntityNavigation).WithMany(p => p.Scorings)
                .HasForeignKey(d => d.Entity)
                .HasConstraintName("FK_Scoring_Entities");

            entity.HasOne(d => d.PersonNavigation).WithMany(p => p.Scorings)
                .HasForeignKey(d => d.Person)
                .HasConstraintName("FK__Scoring__idPerso__70DDC3D8");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC073B4BF41A");

            entity.Property(e => e.Email).HasColumnType("varchar(255)");
            entity.Property(e => e.Name).HasColumnType("varchar(255)");
            entity.Property(e => e.Password).HasColumnType("varchar(255)");
            entity.Property(e => e.Username).HasColumnType("varchar(255)");
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UserRole__3214EC07EF4EC3D5");

            entity.HasOne(d => d.IdRoleNavigation).WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.IdRole)
                .HasConstraintName("FK__UserRoles__IdRol__5535A963");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.IdUser)
                .HasConstraintName("FK__UserRoles__IdUse__5629CD9C");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
