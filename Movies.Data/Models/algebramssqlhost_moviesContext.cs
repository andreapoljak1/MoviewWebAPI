using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Movies.Data.Models
{
    public partial class algebramssqlhost_moviesContext : DbContext
    {
        public algebramssqlhost_moviesContext()
        {
        }

        public algebramssqlhost_moviesContext(DbContextOptions<algebramssqlhost_moviesContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Movie> Movies { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=mssql9.mojsite.com,1555; Database=algebramssqlhost_movies; User Id=algebramssqlhost_ol_oasp_dev_01_21_l_nek; Password=2021_alge_TESTDEV_2022; MultipleActiveResultSets=true;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("algebramssqlhost_ol_oasp_dev_01_21_l_nek");

            modelBuilder.Entity<Movie>(entity =>
            {
                entity.ToTable("Movies", "dbo");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Genre).HasMaxLength(60);

                entity.Property(e => e.ReleaseYear)
                    .HasMaxLength(4)
                    .IsFixedLength();

                entity.Property(e => e.Title).HasMaxLength(150);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
