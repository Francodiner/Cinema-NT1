﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ReservaEspectaculo.Data;

namespace ReservaEspectaculo.Migrations
{
    [DbContext(typeof(MiContexto))]
    partial class MiContextoModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.9");

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("TEXT");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasColumnType("TEXT")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");

                    b.HasDiscriminator<string>("Discriminator").HasValue("IdentityRole<int>");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ClaimType")
                        .HasColumnType("TEXT");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("TEXT");

                    b.Property<int>("RoleId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("TEXT");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .HasColumnType("TEXT")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("TEXT")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("TEXT")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash")
                        .HasColumnType("TEXT");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("TEXT");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("TEXT");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<string>("UserName")
                        .HasColumnType("TEXT")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("Usuarios");

                    b.HasDiscriminator<string>("Discriminator").HasValue("IdentityUser<int>");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ClaimType")
                        .HasColumnType("TEXT");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("TEXT");

                    b.Property<int>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("TEXT");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("TEXT");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("TEXT");

                    b.Property<int>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<int>", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("RoleId")
                        .HasColumnType("INTEGER");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("Value")
                        .HasColumnType("TEXT");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("ReservaEspectaculo.Models.Funcion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("ButacarDisponibles")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Confirmada")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Descripcion")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasMaxLength(100);

                    b.Property<DateTime>("Fecha")
                        .HasColumnType("TEXT");

                    b.Property<int>("PeliculaId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("SalaId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("PeliculaId");

                    b.HasIndex("SalaId");

                    b.ToTable("Funciones");
                });

            modelBuilder.Entity("ReservaEspectaculo.Models.Genero", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.ToTable("Generos");
                });

            modelBuilder.Entity("ReservaEspectaculo.Models.Pelicula", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Descripcion")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasMaxLength(300);

                    b.Property<DateTime>("FechaLanzamiento")
                        .HasColumnType("TEXT");

                    b.Property<int>("GeneroId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Titulo")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.HasIndex("GeneroId");

                    b.ToTable("Peliculas");
                });

            modelBuilder.Entity("ReservaEspectaculo.Models.Reserva", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Activa")
                        .HasColumnType("INTEGER");

                    b.Property<int>("CantidadButacas")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ClienteId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("FechaAlta")
                        .HasColumnType("TEXT");

                    b.Property<int>("FuncionId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ClienteId");

                    b.HasIndex("FuncionId");

                    b.ToTable("Reservas");
                });

            modelBuilder.Entity("ReservaEspectaculo.Models.Sala", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("CapacidadButacas")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Numero")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TipoSalaId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("TipoSalaId");

                    b.ToTable("Salas");
                });

            modelBuilder.Entity("ReservaEspectaculo.Models.TipoSala", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasMaxLength(50);

                    b.Property<decimal>("Precio")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.ToTable("TipoSalas");
                });

            modelBuilder.Entity("ReservaEspectaculo.Models.Rol", b =>
                {
                    b.HasBaseType("Microsoft.AspNetCore.Identity.IdentityRole<int>");

                    b.HasDiscriminator().HasValue("Rol");
                });

            modelBuilder.Entity("ReservaEspectaculo.Models.Usuario", b =>
                {
                    b.HasBaseType("Microsoft.AspNetCore.Identity.IdentityUser<int>");

                    b.Property<string>("Apellido")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasMaxLength(50);

                    b.Property<string>("Direccion")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasMaxLength(100);

                    b.Property<string>("Dni")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("FechaAlta")
                        .HasColumnType("TEXT");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasMaxLength(50);

                    b.Property<string>("Telefono")
                        .HasColumnType("TEXT")
                        .HasMaxLength(20);

                    b.HasDiscriminator().HasValue("Usuario");
                });

            modelBuilder.Entity("ReservaEspectaculo.Models.Cliente", b =>
                {
                    b.HasBaseType("ReservaEspectaculo.Models.Usuario");

                    b.HasDiscriminator().HasValue("Cliente");
                });

            modelBuilder.Entity("ReservaEspectaculo.Models.Empleado", b =>
                {
                    b.HasBaseType("ReservaEspectaculo.Models.Usuario");

                    b.Property<string>("Legajo")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasMaxLength(7);

                    b.HasDiscriminator().HasValue("Empleado");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<int>", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser<int>", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser<int>", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<int>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<int>", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser<int>", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser<int>", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ReservaEspectaculo.Models.Funcion", b =>
                {
                    b.HasOne("ReservaEspectaculo.Models.Pelicula", "Pelicula")
                        .WithMany("Funciones")
                        .HasForeignKey("PeliculaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ReservaEspectaculo.Models.Sala", "Sala")
                        .WithMany("Funciones")
                        .HasForeignKey("SalaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ReservaEspectaculo.Models.Pelicula", b =>
                {
                    b.HasOne("ReservaEspectaculo.Models.Genero", "Genero")
                        .WithMany("Peliculas")
                        .HasForeignKey("GeneroId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ReservaEspectaculo.Models.Reserva", b =>
                {
                    b.HasOne("ReservaEspectaculo.Models.Cliente", "Cliente")
                        .WithMany("Reservas")
                        .HasForeignKey("ClienteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ReservaEspectaculo.Models.Funcion", "Funcion")
                        .WithMany("Reservas")
                        .HasForeignKey("FuncionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ReservaEspectaculo.Models.Sala", b =>
                {
                    b.HasOne("ReservaEspectaculo.Models.TipoSala", "TipoSala")
                        .WithMany()
                        .HasForeignKey("TipoSalaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
