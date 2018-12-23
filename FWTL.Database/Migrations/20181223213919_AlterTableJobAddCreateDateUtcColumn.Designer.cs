﻿// <auto-generated />
using System;
using FWTL.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace FWTL.Database.Migrations
{
    [DbContext(typeof(JobDatabaseContext))]
    [Migration("20181223213919_AlterTableJobAddCreateDateUtcColumn")]
    partial class AlterTableJobAddCreateDateUtcColumn
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("FWTL.Core.Entities.Job", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreateDateUtc");

                    b.Property<int>("MaxId");

                    b.Property<int>("PeerId");

                    b.Property<int>("PeerType");

                    b.Property<int>("State");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Job");
                });
#pragma warning restore 612, 618
        }
    }
}
