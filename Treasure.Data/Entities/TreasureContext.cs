using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace Treasure.Data.Entities;

public partial class TreasureContext : DbContext
{
    public TreasureContext()
    {
    }

    public TreasureContext(DbContextOptions<TreasureContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Problem> Problems { get; set; }

    public virtual DbSet<ProblemData> ProblemData { get; set; }

    public virtual DbSet<ProblemResult> ProblemResults { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_general_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Problem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("problem");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("created_at");
            entity.Property(e => e.Title)
                .HasColumnType("text")
                .HasColumnName("title");
            entity.Property(e => e.UpdatedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<ProblemData>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("problem_data");

            entity.HasIndex(e => e.ProblemId, "problem_id").IsUnique();

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.ChestTypes)
                .HasDefaultValueSql("'0'")
                .HasColumnType("int(11)")
                .HasColumnName("chestTypes");
            entity.Property(e => e.Col)
                .HasColumnType("int(11)")
                .HasColumnName("col");
            entity.Property(e => e.Matrix)
                .HasColumnType("blob")
                .HasColumnName("matrix");
            entity.Property(e => e.ProblemId)
                .HasColumnType("int(11)")
                .HasColumnName("problem_id");
            entity.Property(e => e.Row)
                .HasColumnType("int(11)")
                .HasColumnName("row");

            entity.HasOne(d => d.Problem).WithOne(p => p.ProblemData)
                .HasForeignKey<ProblemData>(d => d.ProblemId)
                .HasConstraintName("problem_data_ibfk_1");
        });

        modelBuilder.Entity<ProblemResult>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("problem_result");

            entity.HasIndex(e => e.ProblemId, "problem_id").IsUnique();

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.IsResolved).HasColumnName("is_resolved");
            entity.Property(e => e.ProblemId)
                .HasColumnType("int(11)")
                .HasColumnName("problem_id");
            entity.Property(e => e.Result)
                .HasPrecision(10, 6)
                .HasColumnName("result");

            entity.HasOne(d => d.Problem).WithOne(p => p.ProblemResult)
                .HasForeignKey<ProblemResult>(d => d.ProblemId)
                .HasConstraintName("problem_result_ibfk_1");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
