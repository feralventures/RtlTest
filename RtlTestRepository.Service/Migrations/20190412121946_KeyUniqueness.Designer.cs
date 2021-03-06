﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RtlTestRepository;

namespace RtlTestRepository.Migrations
{
    [DbContext(typeof(Context))]
    [Migration("20190412121946_KeyUniqueness")]
    partial class KeyUniqueness
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

                    b.Property<long>("ShowId");

                    b.Property<long>("TvMazeId");

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

                    b.Property<long>("TvMazeId");

                    b.Property<long>("Updated");

                    b.HasKey("Id");

                    b.HasIndex("TvMazeId")
                        .IsUnique();

                    b.ToTable("Show");
                });

            modelBuilder.Entity("RtlTestRepository.Models.Person", b =>
                {
                    b.HasOne("RtlTestRepository.Models.Show", "Show")
                        .WithMany("Cast")
                        .HasForeignKey("ShowId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
