﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using OmMediaWorkManagement.ApiService.DataContext;

#nullable disable

namespace OmMediaWorkManagement.ApiService.Migrations
{
    [DbContext(typeof(OmContext))]
    partial class OmContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("OmMediaWorkManagement.ApiService.Models.JobImages", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ImagePath")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("JobTodoId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("JobTodoId");

                    b.ToTable("JobImages");
                });

            modelBuilder.Entity("OmMediaWorkManagement.ApiService.Models.JobToDo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("CompanyName")
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<bool>("IsStatus")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("JobPostedDateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("JobStatusType")
                        .HasColumnType("integer");

                    b.Property<int?>("PostedBy")
                        .HasColumnType("integer");

                    b.Property<double?>("Quantity")
                        .HasColumnType("double precision");

                    b.HasKey("Id");

                    b.ToTable("JobToDo");
                });

            modelBuilder.Entity("OmMediaWorkManagement.ApiService.Models.JobTypeStatus", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("JobStatusName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("JobStatusType")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("JobTypeStatus");
                });

            modelBuilder.Entity("OmMediaWorkManagement.ApiService.Models.OmClient", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("CompanyName")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<string>("MobileNumber")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("OmClient");
                });

            modelBuilder.Entity("OmMediaWorkManagement.ApiService.Models.OmClientWork", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("OmClientId")
                        .HasColumnType("integer");

                    b.Property<int>("Price")
                        .HasColumnType("integer");

                    b.Property<int>("PrintCount")
                        .HasColumnType("integer");

                    b.Property<string>("Remarks")
                        .HasColumnType("text");

                    b.Property<int>("Total")
                        .HasColumnType("integer");

                    b.Property<DateTime>("WorkDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("WorkDetails")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("OmClientId");

                    b.ToTable("OmClientWork");
                });

            modelBuilder.Entity("OmMediaWorkManagement.ApiService.Models.OmEmployee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("CompanyName")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("OmEmployee");
                });

            modelBuilder.Entity("OmMediaWorkManagement.ApiService.Models.OmMachines", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsRunning")
                        .HasColumnType("boolean");

                    b.Property<string>("MachineDescription")
                        .HasColumnType("text");

                    b.Property<string>("MachineName")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("OmMachines");
                });

            modelBuilder.Entity("OmMediaWorkManagement.ApiService.Models.JobImages", b =>
                {
                    b.HasOne("OmMediaWorkManagement.ApiService.Models.JobToDo", "JobToDo")
                        .WithMany("JobImages")
                        .HasForeignKey("JobTodoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("JobToDo");
                });

            modelBuilder.Entity("OmMediaWorkManagement.ApiService.Models.OmClientWork", b =>
                {
                    b.HasOne("OmMediaWorkManagement.ApiService.Models.OmClient", "OmClient")
                        .WithMany("OmClientWork")
                        .HasForeignKey("OmClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("OmClient");
                });

            modelBuilder.Entity("OmMediaWorkManagement.ApiService.Models.JobToDo", b =>
                {
                    b.Navigation("JobImages");
                });

            modelBuilder.Entity("OmMediaWorkManagement.ApiService.Models.OmClient", b =>
                {
                    b.Navigation("OmClientWork");
                });
#pragma warning restore 612, 618
        }
    }
}
