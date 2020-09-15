﻿// <auto-generated />
using System;
using Dwapi.Exchange.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Dwapi.Exchange.Infrastructure.Migrations
{
    [DbContext(typeof(RegistryContext))]
    [Migration("20200915164541_ExchangeInitial")]
    partial class ExchangeInitial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Dwapi.Exchange.Core.Domain.Definitions.ExtractRequest", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("RecordCount")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("Refreshed")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("RegistryId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("SqlScript")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Updated")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("RegistryId");

                    b.ToTable("ExtractRequests");
                });

            modelBuilder.Entity("Dwapi.Exchange.Core.Domain.Definitions.Registry", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Code")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Purpose")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Registries");
                });

            modelBuilder.Entity("Dwapi.Exchange.Core.Domain.Definitions.ExtractRequest", b =>
                {
                    b.HasOne("Dwapi.Exchange.Core.Domain.Definitions.Registry", null)
                        .WithMany("ExtractRequests")
                        .HasForeignKey("RegistryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
