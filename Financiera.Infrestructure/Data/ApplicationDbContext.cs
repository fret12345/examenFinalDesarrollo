//using Bogus;
using Financiera.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Financiera.Infrestructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<Usuarios, IdentityRole<int>, int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options){}
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Rutas> Rutas { get; set; }
        public DbSet<Clientes> Clientes { get; set; }
        public DbSet<Prestamos> Prestamos { get; set; }
        public DbSet<Cuota> Cuota { get; set; }
        public DbSet<CuentasPorCobrar> CuentasPorCobrar { get; set; }
        public DbSet<Pagos> Pagos { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //builder.Entity<Rol>(static entity =>
            //{
            //    entity.HasKey(R => R.Id);
            //    entity.Property(R => R.Id)
            //    .ValueGeneratedOnAdd();

            //    entity.Property(R => R.NombreRol)
            //    .IsRequired()
            //    .HasMaxLength(15);

            //    entity.HasIndex(R => R.NombreRol).IsUnique();
            //});

            builder.Entity<Rutas>(static entity =>
            {
                entity.HasKey(R => R.id);
                entity.Property(R => R.id)
                .ValueGeneratedOnAdd();

                entity.Property(R => R.NombreRuta)
                .IsRequired()
                .HasMaxLength(15);

                entity.HasIndex(R => R.NombreRuta).IsUnique();
            });

            builder.Entity<Clientes>(static entity =>
            {
                entity.HasKey(C => C.Id);
                entity.Property(R => R.Id)
                .ValueGeneratedOnAdd();

                entity.Property(C => C.idRuta)
                .IsRequired();

                entity.Property(C => C.Nombres)
                .IsRequired()
                .HasMaxLength(60);

                entity.Property(C => C.Apellidos)
               .IsRequired()
               .HasMaxLength(60);

                entity.Property(C => C.Cedula)
               .IsRequired()
               .HasMaxLength(20);

                entity.Property(C => C.DireccionDomiciliar)
               .IsRequired()
               .HasMaxLength(250);

                entity.Property(C => C.DireccionNegocio)
               .IsRequired()
               .HasMaxLength(250);

                entity.Property(C => C.Telefono)
               .IsRequired()
               .HasMaxLength(15);

                entity.Property(C => C.Estado)
               .IsRequired()
               .HasDefaultValue(true);

                entity.HasIndex(U => U.Cedula).IsUnique();
                entity.HasIndex(U => U.Telefono).IsUnique();

                //Relacion con la tabla Rutas
                entity.HasOne(R => R.Rutas)
                .WithMany(C => C.Clientes)
                .HasForeignKey(R => R.idRuta)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();
            });

            //Aqui tabla usuario
            builder.Entity<Usuarios>(static entity =>
            {
                //entity.HasKey(U => U.Id);
                //entity.Property(U => U.Id)
                //.ValueGeneratedOnAdd();

                //entity.Property(U => U.IdRol)
                //.IsRequired();

                entity.Property(U => U.NombresCompleto)
                .IsRequired()
                .HasMaxLength(60);

               // entity.Property(U => U.Apellidos)
               //.IsRequired()
               //.HasMaxLength(60);

               // entity.Property(U => U.Usuario)
               //.IsRequired()
               //.HasMaxLength(30);

               // entity.Property(U => U.Clave)
               //.IsRequired()
               //.HasMaxLength(60);

               //entity.Property(U => U.Estado)
               //.IsRequired()
               //.HasDefaultValue(true);

                //entity.HasIndex(U => U.Usuario).IsUnique();
                //entity.HasIndex(U => U.Clave).IsUnique();


                //Relacion Con la taba ROl
              //  entity.HasOne(R => R.Rol)
              //.WithMany(U => U.Usuarios)
              //.HasForeignKey(R => R.IdRol)
              //.OnDelete(DeleteBehavior.Restrict)
              //.IsRequired();

            });

            builder.Entity<Prestamos>(static entity =>
            {
                entity.HasKey(P => P.Id);
                entity.Property(P => P.Id)
                .ValueGeneratedOnAdd();

                entity.Property(P => P.IdCliente)
                .IsRequired();

                entity.Property(P => P.IdUsuarios)
               .IsRequired();

                entity.Property(P => P.NumPrestamos)
               .IsRequired();

                entity.Property(P => P.Moneda)
               .IsRequired();

                entity.Property(P => P.TipoCambio)
               .IsRequired();

                entity.Property(P => P.TasaInteres)
               .IsRequired();

                entity.Property(P => P.TotalAPAgar)
               .IsRequired();

                entity.Property(P => P.PladoMeses)
               .IsRequired();

                entity.Property(P => P.FrecuenciaPago)
               .IsRequired();

                entity.Property(P => P.EstadoPrestamo)
                .IsRequired()
                .HasDefaultValue("Pendiente"); 

                entity.Property(P => P.FechaDesembolso)
               .IsRequired()
               .HasColumnType("date");

                entity.Property(P => P.FechaVensimiento)
               .IsRequired()
               .HasColumnType("date");

                entity.Property(P => P.FechaRegistro)
               .IsRequired()
               .HasColumnType("date");

                entity.Property(P => P.Estado)
               .IsRequired()
               .HasDefaultValue(true);

                entity.HasIndex(U => U.NumPrestamos).IsUnique();

                //Relacion Con la taba Cliente
                entity.HasOne(R => R.Clientes)
                .WithMany(U => U.Prestamos)
                .HasForeignKey(R => R.IdCliente)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

                //Relacion Con la taba Usuario
                entity.HasOne(R => R.Usuarios)
              .WithMany(U => U.Prestamos)
              .HasForeignKey(R => R.IdUsuarios)
              .OnDelete(DeleteBehavior.Restrict)
              .IsRequired();

                //restriccion de estado rpestamo
                entity.ToTable(table =>
                {
                    
                    table.HasCheckConstraint("CK_Prestamos_EstadoPrestamo",
                    "\"EstadoPrestamo\" IN('Pendiente', 'Aprovado', 'Cancelado', 'Rechazado')");
                });
            });

            builder.Entity<Cuota>(static entity =>
            {
                entity.HasKey(C => C.Id);
                entity.Property(C => C.Id)
                .ValueGeneratedOnAdd();

                entity.Property(C => C.IdPrestamos)
                .IsRequired();

                entity.Property(C => C.NumCuota)
               .IsRequired();

                entity.Property(C => C.MontoCuota)
               .IsRequired();

                entity.Property(C => C.FechaVensimiento)
               .IsRequired()
               .HasColumnType("date");

                entity.Property(C => C.CobroMora)
               .IsRequired();

                entity.Property(C => C.DiasAtraso)
               .IsRequired();

                entity.Property(C => C.MontoCuota)
               .IsRequired();

                entity.Property(U => U.Estado)
               .IsRequired()
               .HasDefaultValue(true);

                //entity.HasIndex(U => U.NumCuota).IsUnique();

                //Relacion Con la taba Prestamo
                entity.HasOne(R => R.Prestamos)
              .WithMany(U => U.Cuotas)
              .HasForeignKey(R => R.IdPrestamos)
              .OnDelete(DeleteBehavior.Restrict)
              .IsRequired();
            });

            builder.Entity<Pagos>(static entity =>
            {
                entity.HasKey(P => P.Id);
                entity.Property(P => P.Id)
                .ValueGeneratedOnAdd();

                entity.Property(P => P.IdCuota)
                .IsRequired();

                entity.Property(P => P.IdUsuario)
               .IsRequired();

                entity.Property(P => P.MontoRecibido)
               .IsRequired();

                entity.Property(P => P.SaldoPendiente)
               .IsRequired();

                entity.Property(P => P.MontoCuota)
               .IsRequired();

                entity.Property(P => P.ImpuestoExtra)
               .IsRequired();

                entity.Property(P => P.FechaPago)
               .IsRequired()
               .HasColumnType("date");

                entity.Property(P => P.FechaRegistro)
              .IsRequired()
              .HasColumnType("date");

                //Relacion Con la taba cuota
                entity.HasOne(R => R.cuota)
                .WithMany(U => U.Pagos)
                .HasForeignKey(R => R.IdCuota)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

                //Relacion Con la taba usuario
                entity.HasOne(R => R.usuarios)
                .WithMany(U => U.Pagos)
                .HasForeignKey(R => R.IdUsuario)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();
            });

            builder.Entity<CuentasPorCobrar>(static entity =>
            {
                entity.HasKey(C => C.Id);
                entity.Property(C => C.Id)
                .ValueGeneratedOnAdd();

                entity.Property(C => C.IdPrestammo)
                .IsRequired();

                entity.Property(C => C.MontoTotal)
                .IsRequired();

                entity.Property(C => C.SaldoPendiente)
                .IsRequired();

                // Ojo: En tu entidad escribiste 'TotalMoraAcumuada' (sin la l) según tu foto de Domain. 
                // Asegurate de que coincida con tu clase física.
                entity.Property(C => C.TotalMoraAcumuada)
                .IsRequired();

                entity.Property(C => C.TotalImpuestoExtra)
                .IsRequired();

                entity.Property(U => U.Estado)
               .IsRequired()
               .HasDefaultValue(true);

                // =========================================================================
                // ¡ESTO ES LO QUE SOLUCIONA EL ERROR! Amarramos la relación con IdPrestammo
                // =========================================================================
                entity.HasOne(c => c.prestamos)
                      .WithMany() // Dejalo vacío si Préstamos no tiene una colección de CuentasPorCobrar
                      .HasForeignKey(c => c.IdPrestammo)
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }

    }
}
