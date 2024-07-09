﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using OmMediaWorkManagement.ApiService.DataContext;

#nullable disable

namespace OmMediaWorkManagement.ApiService.Migrations
{
    [DbContext(typeof(OmContext))]
    [Migration("20240709094458_updatedtodoomemploye")]
    partial class updatedtodoomemploye
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("text");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("RoleId")
                        .HasColumnType("text");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Value")
                        .HasColumnType("text");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

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

                    b.Property<int>("OmClientId")
                        .HasColumnType("integer");

                    b.Property<int>("OmEmpId")
                        .HasColumnType("integer");

                    b.Property<int?>("PostedBy")
                        .HasColumnType("integer");

                    b.Property<int?>("Price")
                        .HasColumnType("integer");

                    b.Property<double?>("Quantity")
                        .HasColumnType("double precision");

                    b.Property<int?>("total")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("OmClientId");

                    b.HasIndex("OmEmpId");

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

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("OmClient");
                });

            modelBuilder.Entity("OmMediaWorkManagement.ApiService.Models.OmClientWork", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsEmailSent")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsPaid")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsPushSent")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsSMSSent")
                        .HasColumnType("boolean");

                    b.Property<int>("OmClientId")
                        .HasColumnType("integer");

                    b.Property<int>("Price")
                        .HasColumnType("integer");

                    b.Property<int>("PrintCount")
                        .HasColumnType("integer");

                    b.Property<string>("Remarks")
                        .HasColumnType("text");

                    b.Property<int?>("Total")
                        .HasColumnType("integer");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("WorkDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("WorkDetails")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("OmClientId");

                    b.HasIndex("UserId");

                    b.ToTable("OmClientWork");
                });

            modelBuilder.Entity("OmMediaWorkManagement.ApiService.Models.OmEmployee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("AppPin")
                        .HasColumnType("integer");

                    b.Property<string>("CompanyName")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<string>("EmployeeProfilePath")
                        .HasColumnType("text");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsSalaryPaid")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("OTP")
                        .HasColumnType("text");

                    b.Property<int>("OTPAttempts")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("OTPExpireTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("OTPGeneratedTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<decimal>("SalaryAmount")
                        .HasColumnType("numeric");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("OmEmployee");
                });

            modelBuilder.Entity("OmMediaWorkManagement.ApiService.Models.OmEmployeeDocuments", b =>
                {
                    b.Property<int>("OmEmployeeDocumentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("OmEmployeeDocumentId"));

                    b.Property<string>("EmployeeDocumentsPath")
                        .HasColumnType("text");

                    b.Property<int>("OmEmployeeId")
                        .HasColumnType("integer");

                    b.HasKey("OmEmployeeDocumentId");

                    b.HasIndex("OmEmployeeId");

                    b.ToTable("OmEmployeeDocuments");
                });

            modelBuilder.Entity("OmMediaWorkManagement.ApiService.Models.OmEmployeeSalaryManagement", b =>
                {
                    b.Property<int>("EmployeeSalaryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("EmployeeSalaryId"));

                    b.Property<decimal?>("AdvancePayment")
                        .HasColumnType("numeric");

                    b.Property<DateTime?>("AdvancePaymentDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<decimal?>("DueBalance")
                        .HasColumnType("numeric");

                    b.Property<int>("OmEmployeeId")
                        .HasColumnType("integer");

                    b.Property<decimal?>("OverBalance")
                        .HasColumnType("numeric");

                    b.Property<decimal?>("OverTimeSalary")
                        .HasColumnType("numeric");

                    b.HasKey("EmployeeSalaryId");

                    b.HasIndex("OmEmployeeId");

                    b.ToTable("OmEmployeeSalaryManagement");
                });

            modelBuilder.Entity("OmMediaWorkManagement.ApiService.Models.OmEmployeeShift", b =>
                {
                    b.Property<int>("EmployeeShiftId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("EmployeeShiftId"));

                    b.Property<int>("OmEmployeeId")
                        .HasColumnType("integer");

                    b.Property<decimal>("OverTimePerHourCost")
                        .HasColumnType("numeric");

                    b.Property<string>("ShiftOverTime")
                        .HasColumnType("text");

                    b.Property<string>("ShiftTiming")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("EmployeeShiftId");

                    b.HasIndex("OmEmployeeId");

                    b.ToTable("OmEmployeeShift");
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

            modelBuilder.Entity("OmMediaWorkManagement.ApiService.Models.UserRegistration", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("integer");

                    b.Property<int?>("Attempts")
                        .HasColumnType("integer");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<DateTime?>("CreatedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("OTP")
                        .HasColumnType("text");

                    b.Property<DateTime?>("OTPExpireTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("RefreshToken")
                        .HasColumnType("text");

                    b.Property<DateTime>("RefreshTokenExpiryTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("text");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("boolean");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("OmMediaWorkManagement.ApiService.Models.UserRegistration", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("OmMediaWorkManagement.ApiService.Models.UserRegistration", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("OmMediaWorkManagement.ApiService.Models.UserRegistration", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("OmMediaWorkManagement.ApiService.Models.UserRegistration", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
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

            modelBuilder.Entity("OmMediaWorkManagement.ApiService.Models.JobToDo", b =>
                {
                    b.HasOne("OmMediaWorkManagement.ApiService.Models.OmClient", "OmClient")
                        .WithMany("JobToDo")
                        .HasForeignKey("OmClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("OmMediaWorkManagement.ApiService.Models.OmEmployee", "OmEmployee")
                        .WithMany()
                        .HasForeignKey("OmEmpId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("OmClient");

                    b.Navigation("OmEmployee");
                });

            modelBuilder.Entity("OmMediaWorkManagement.ApiService.Models.OmClient", b =>
                {
                    b.HasOne("OmMediaWorkManagement.ApiService.Models.UserRegistration", "UserRegistration")
                        .WithMany("OmClient")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UserRegistration");
                });

            modelBuilder.Entity("OmMediaWorkManagement.ApiService.Models.OmClientWork", b =>
                {
                    b.HasOne("OmMediaWorkManagement.ApiService.Models.OmClient", "OmClient")
                        .WithMany("OmClientWork")
                        .HasForeignKey("OmClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("OmMediaWorkManagement.ApiService.Models.UserRegistration", "UserRegistration")
                        .WithMany("OmClientWork")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("OmClient");

                    b.Navigation("UserRegistration");
                });

            modelBuilder.Entity("OmMediaWorkManagement.ApiService.Models.OmEmployee", b =>
                {
                    b.HasOne("OmMediaWorkManagement.ApiService.Models.UserRegistration", "UserRegistration")
                        .WithMany("OmEmployee")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UserRegistration");
                });

            modelBuilder.Entity("OmMediaWorkManagement.ApiService.Models.OmEmployeeDocuments", b =>
                {
                    b.HasOne("OmMediaWorkManagement.ApiService.Models.OmEmployee", "OmEmployee")
                        .WithMany("EmployeeDocuments")
                        .HasForeignKey("OmEmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("OmEmployee");
                });

            modelBuilder.Entity("OmMediaWorkManagement.ApiService.Models.OmEmployeeSalaryManagement", b =>
                {
                    b.HasOne("OmMediaWorkManagement.ApiService.Models.OmEmployee", "OmEmployee")
                        .WithMany()
                        .HasForeignKey("OmEmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("OmEmployee");
                });

            modelBuilder.Entity("OmMediaWorkManagement.ApiService.Models.OmEmployeeShift", b =>
                {
                    b.HasOne("OmMediaWorkManagement.ApiService.Models.OmEmployee", "OmEmployee")
                        .WithMany()
                        .HasForeignKey("OmEmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("OmEmployee");
                });

            modelBuilder.Entity("OmMediaWorkManagement.ApiService.Models.JobToDo", b =>
                {
                    b.Navigation("JobImages");
                });

            modelBuilder.Entity("OmMediaWorkManagement.ApiService.Models.OmClient", b =>
                {
                    b.Navigation("JobToDo");

                    b.Navigation("OmClientWork");
                });

            modelBuilder.Entity("OmMediaWorkManagement.ApiService.Models.OmEmployee", b =>
                {
                    b.Navigation("EmployeeDocuments");
                });

            modelBuilder.Entity("OmMediaWorkManagement.ApiService.Models.UserRegistration", b =>
                {
                    b.Navigation("OmClient");

                    b.Navigation("OmClientWork");

                    b.Navigation("OmEmployee");
                });
#pragma warning restore 612, 618
        }
    }
}
