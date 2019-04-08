﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RtlTestRepository;

namespace RtlTestRepository.Migrations
{
    [DbContext(typeof(Context))]
    [Migration("20190408132147_Create")]
    partial class Create
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.3-servicing-35854")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("RtlTestRepository.Models.Person", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Birthday");

                    b.Property<string>("Name");

                    b.Property<long?>("ShowId");

                    b.HasKey("Id");

                    b.HasIndex("ShowId");

                    b.ToTable("Person");
                });

            modelBuilder.Entity("RtlTestRepository.Models.Show", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Show");
                });

            modelBuilder.Entity("RtlTestRepository.Models.Person", b =>
                {
                    b.HasOne("RtlTestRepository.Models.Show")
                        .WithMany("Cast")
                        .HasForeignKey("ShowId");
                });
#pragma warning restore 612, 618
        }
    }
}
