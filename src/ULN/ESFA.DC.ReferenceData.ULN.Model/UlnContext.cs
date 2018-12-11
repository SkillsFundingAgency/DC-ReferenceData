using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ESFA.DC.ReferenceData.ULN.Model
{
    public partial class UlnContext : DbContext
    {
        public UlnContext()
        {
        }

        public UlnContext(DbContextOptions<UlnContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Import> Imports { get; set; }
        public virtual DbSet<UniqueLearnerNumber> UniqueLearnerNumbers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=.\\;Database=ESFA.DC.ReferenceData.ULN.Database;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.0-rtm-35687");

            modelBuilder.Entity<Import>(entity =>
            {
                entity.ToTable("Import");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.DateTime).HasColumnType("datetime");

                entity.Property(e => e.Filename)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.NewUlnsInFileCount).HasColumnName("NewULNsInFileCount");

                entity.Property(e => e.UlnsInFileCount).HasColumnName("ULNsInFileCount");
            });

            modelBuilder.Entity<UniqueLearnerNumber>(entity =>
            {
                entity.HasKey(e => e.Uln)
                    .HasName("PK__UniqueLe__C5B14FF6832DDA4F");

                entity.ToTable("UniqueLearnerNumber");

                entity.Property(e => e.Uln)
                    .HasColumnName("ULN")
                    .ValueGeneratedNever();

                entity.HasOne(d => d.Import)
                    .WithMany(p => p.UniqueLearnerNumbers)
                    .HasForeignKey(d => d.ImportId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UniqueLearnerNumber_ToImport");
            });
        }
    }
}
