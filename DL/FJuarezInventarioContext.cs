using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DL
{
    public partial class FJuarezInventarioContext : DbContext
    {
        public FJuarezInventarioContext()
        {
        }

        public FJuarezInventarioContext(DbContextOptions<FJuarezInventarioContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Almacen> Almacens { get; set; } = null!;
        public virtual DbSet<Articulo> Articulos { get; set; } = null!;
        public virtual DbSet<ArticuloAlmacen> ArticuloAlmacens { get; set; } = null!;
        public virtual DbSet<Marca> Marcas { get; set; } = null!;
        public virtual DbSet<Movimiento> Movimientos { get; set; } = null!;
        public virtual DbSet<Registro> Registros { get; set; } = null!;
        public virtual DbSet<Ubicacion> Ubicacions { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=.; Database= FJuarezInventario; TrustServerCertificate=True; User ID=sa; Password=pass@word1;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Almacen>(entity =>
            {
                entity.HasKey(e => e.IdAlmacen)
                    .HasName("PK__Almacen__7339837B1DA63321");

                entity.ToTable("Almacen");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Direccion).IsUnicode(false);

                entity.HasOne(d => d.IdUbicacionNavigation)
                    .WithMany(p => p.Almacens)
                    .HasForeignKey(d => d.IdUbicacion)
                    .HasConstraintName("FK__Almacen__IdUbica__3F466844");
            });

            modelBuilder.Entity<Articulo>(entity =>
            {
                entity.HasKey(e => e.IdArticulo)
                    .HasName("PK__Articulo__F8FF5D522610BCD6");

                entity.ToTable("Articulo");

                entity.Property(e => e.Descripcion).IsUnicode(false);

                entity.Property(e => e.Nombre)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdMarcaNavigation)
                    .WithMany(p => p.Articulos)
                    .HasForeignKey(d => d.IdMarca)
                    .HasConstraintName("FK__Articulo__IdMarc__4D94879B");
            });

            modelBuilder.Entity<ArticuloAlmacen>(entity =>
            {
                entity.HasKey(e => e.IdArticuloAlmacen)
                    .HasName("PK__Articulo__E68657BCE819187B");

                entity.ToTable("ArticuloAlmacen");

                entity.HasOne(d => d.IdAlmacenNavigation)
                    .WithMany(p => p.ArticuloAlmacens)
                    .HasForeignKey(d => d.IdAlmacen)
                    .HasConstraintName("FK__ArticuloA__Stock__5070F446");

                entity.HasOne(d => d.IdArticuloNavigation)
                    .WithMany(p => p.ArticuloAlmacens)
                    .HasForeignKey(d => d.IdArticulo)
                    .HasConstraintName("FK__ArticuloA__IdArt__5165187F");
            });

            modelBuilder.Entity<Marca>(entity =>
            {
                entity.HasKey(e => e.IdMarca)
                    .HasName("PK__Marca__4076A887170F79F4");

                entity.ToTable("Marca");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(150)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Movimiento>(entity =>
            {
                entity.HasKey(e => e.IdMovimiento)
                    .HasName("PK__Movimien__881A6AE067F92D91");

                entity.ToTable("Movimiento");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(150)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Registro>(entity =>
            {
                entity.HasKey(e => e.IdRegistro)
                    .HasName("PK__Registro__FFA45A9972E8EED8");

                entity.ToTable("Registro");

                entity.HasOne(d => d.IdArticuloAlmacenNavigation)
                    .WithMany(p => p.Registros)
                    .HasForeignKey(d => d.IdArticuloAlmacen)
                    .HasConstraintName("FK__Registro__IdArti__5535A963");

                entity.HasOne(d => d.IdMovimientoNavigation)
                    .WithMany(p => p.Registros)
                    .HasForeignKey(d => d.IdMovimiento)
                    .HasConstraintName("FK__Registro__IdMovi__5441852A");
            });

            modelBuilder.Entity<Ubicacion>(entity =>
            {
                entity.HasKey(e => e.IdUbicacion)
                    .HasName("PK__Ubicacio__778CAB1D2F2F4C00");

                entity.ToTable("Ubicacion");

                entity.Property(e => e.IdUbicacion).ValueGeneratedNever();

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(150)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
