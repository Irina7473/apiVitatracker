﻿// <auto-generated />
using System;
using MedicineReminderAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace MedicineReminderAPI.Migrations
{
    [DbContext(typeof(AppApiContext))]
    [Migration("20230424155805_init_Identity")]
    partial class init_Identity
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("MedicineReminderAPI.Models.Course", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Comment")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("Created")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime(6)")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

                    b.Property<long?>("EndDate")
                        .HasColumnType("bigint");

                    b.Property<long?>("Interval")
                        .HasColumnType("bigint");

                    b.Property<bool>("IsFinished")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("tinyint(1)")
                        .HasDefaultValue(false);

                    b.Property<bool>("IsInfinite")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("tinyint(1)")
                        .HasDefaultValue(false);

                    b.Property<bool>("NotUsed")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("tinyint(1)")
                        .HasDefaultValue(false);

                    b.Property<int>("Regime")
                        .HasColumnType("int");

                    b.Property<int>("RemedyId")
                        .HasColumnType("int");

                    b.Property<long?>("RemindDate")
                        .HasColumnType("bigint");

                    b.Property<long>("StartDate")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("Updated")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime(6)")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP(6) ON UPDATE CURRENT_TIMESTAMP(6)");

                    b.HasKey("Id");

                    b.HasIndex("RemedyId");

                    b.ToTable("Courses");
                });

            modelBuilder.Entity("MedicineReminderAPI.Models.HistoryRemedy", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("Created")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime(6)")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

                    b.Property<int>("Dose")
                        .HasColumnType("int");

                    b.Property<int>("MeasureUnit")
                        .HasColumnType("int");

                    b.Property<int>("RemedyId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RemedyId");

                    b.ToTable("HistoryRemedys");
                });

            modelBuilder.Entity("MedicineReminderAPI.Models.NotificationSetting", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<bool>("IsEnabled")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("tinyint(1)")
                        .HasDefaultValue(true);

                    b.Property<bool>("IsFloat")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("tinyint(1)")
                        .HasDefaultValue(false);

                    b.Property<bool>("MedicalControl")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("tinyint(1)")
                        .HasDefaultValue(false);

                    b.Property<bool>("NextCourseStart")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("tinyint(1)")
                        .HasDefaultValue(true);

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("NotificationSettings");
                });

            modelBuilder.Entity("MedicineReminderAPI.Models.Remedy", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int?>("BeforeFood")
                        .HasColumnType("int");

                    b.Property<int>("Color")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.Property<string>("Comment")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("Created")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime(6)")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

                    b.Property<string>("Description")
                        .HasColumnType("longtext");

                    b.Property<int?>("Dose")
                        .HasColumnType("int");

                    b.Property<int>("Icon")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.Property<int?>("MeasureUnit")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("varchar(128)");

                    b.Property<bool>("NotUsed")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("tinyint(1)")
                        .HasDefaultValue(false);

                    b.Property<string>("Photo")
                        .HasColumnType("longtext");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<DateTime>("Updated")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime(6)")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP(6) ON UPDATE CURRENT_TIMESTAMP(6)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Remedys");
                });

            modelBuilder.Entity("MedicineReminderAPI.Models.Usage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("CourseId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Created")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime(6)")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

                    b.Property<long?>("FactUseTime")
                        .HasColumnType("bigint");

                    b.Property<bool>("NotUsed")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("tinyint(1)")
                        .HasDefaultValue(false);

                    b.Property<int?>("Quantity")
                        .HasColumnType("int");

                    b.Property<DateTime>("Updated")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime(6)")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP(6) ON UPDATE CURRENT_TIMESTAMP(6)");

                    b.Property<long>("UseTime")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("CourseId");

                    b.ToTable("Usages");
                });

            modelBuilder.Entity("MedicineReminderAPI.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("AccessFailedCount")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(5);

                    b.Property<string>("Avatar")
                        .HasColumnType("longtext");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("Created")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime(6)")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("varchar(64)");

                    b.Property<bool>("EmailConfirmed")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("tinyint(1)")
                        .HasDefaultValue(false);

                    b.Property<bool>("LockoutEnabled")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("tinyint(1)")
                        .HasDefaultValue(false);

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<bool>("NotUsed")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("tinyint(1)")
                        .HasDefaultValue(false);

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("varchar(16)")
                        .HasColumnName("Password");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("longtext");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("tinyint(1)")
                        .HasDefaultValue(false);

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("longtext");

                    b.Property<bool>("TwoFactorEnabled")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("tinyint(1)")
                        .HasDefaultValue(false);

                    b.Property<DateTime>("Updated")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime(6)")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP(6) ON UPDATE CURRENT_TIMESTAMP(6)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(120)
                        .HasColumnType("varchar(120)")
                        .HasColumnName("Name");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("Users", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ClaimType")
                        .HasColumnType("longtext");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("longtext");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("UserClaims", (string)null);

                    b.HasDiscriminator<string>("Discriminator").HasValue("IdentityUserClaim<int>");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("varchar(128)");

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(128)
                        .HasColumnType("varchar(128)");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("longtext");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.ToTable("UserLogins", (string)null);

                    b.HasDiscriminator<string>("Discriminator").HasValue("IdentityUserLogin<int>");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("varchar(128)");

                    b.Property<string>("Name")
                        .HasMaxLength(128)
                        .HasColumnType("varchar(128)");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Value")
                        .HasColumnType("longtext");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("UserTokens", (string)null);

                    b.HasDiscriminator<string>("Discriminator").HasValue("IdentityUserToken<int>");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("MedicineReminderAPI.Models.UserClaim", b =>
                {
                    b.HasBaseType("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>");

                    b.HasIndex("UserId");

                    b.HasDiscriminator().HasValue("UserClaim");
                });

            modelBuilder.Entity("MedicineReminderAPI.Models.UserLogin", b =>
                {
                    b.HasBaseType("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>");

                    b.HasIndex("UserId");

                    b.HasDiscriminator().HasValue("UserLogin");
                });

            modelBuilder.Entity("MedicineReminderAPI.Models.UserToken", b =>
                {
                    b.HasBaseType("Microsoft.AspNetCore.Identity.IdentityUserToken<int>");

                    b.HasDiscriminator().HasValue("UserToken");
                });

            modelBuilder.Entity("MedicineReminderAPI.Models.Course", b =>
                {
                    b.HasOne("MedicineReminderAPI.Models.Remedy", null)
                        .WithMany("Courses")
                        .HasForeignKey("RemedyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MedicineReminderAPI.Models.HistoryRemedy", b =>
                {
                    b.HasOne("MedicineReminderAPI.Models.Remedy", null)
                        .WithMany("HistoryRemedys")
                        .HasForeignKey("RemedyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MedicineReminderAPI.Models.NotificationSetting", b =>
                {
                    b.HasOne("MedicineReminderAPI.Models.User", null)
                        .WithOne("NotificationSetting")
                        .HasForeignKey("MedicineReminderAPI.Models.NotificationSetting", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MedicineReminderAPI.Models.Remedy", b =>
                {
                    b.HasOne("MedicineReminderAPI.Models.User", null)
                        .WithMany("Remedies")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MedicineReminderAPI.Models.Usage", b =>
                {
                    b.HasOne("MedicineReminderAPI.Models.Course", null)
                        .WithMany("Usages")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MedicineReminderAPI.Models.UserClaim", b =>
                {
                    b.HasOne("MedicineReminderAPI.Models.User", "User")
                        .WithMany("Claims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("MedicineReminderAPI.Models.UserLogin", b =>
                {
                    b.HasOne("MedicineReminderAPI.Models.User", "User")
                        .WithMany("Logins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("MedicineReminderAPI.Models.UserToken", b =>
                {
                    b.HasOne("MedicineReminderAPI.Models.User", "User")
                        .WithMany("Tokens")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("MedicineReminderAPI.Models.Course", b =>
                {
                    b.Navigation("Usages");
                });

            modelBuilder.Entity("MedicineReminderAPI.Models.Remedy", b =>
                {
                    b.Navigation("Courses");

                    b.Navigation("HistoryRemedys");
                });

            modelBuilder.Entity("MedicineReminderAPI.Models.User", b =>
                {
                    b.Navigation("Claims");

                    b.Navigation("Logins");

                    b.Navigation("NotificationSetting")
                        .IsRequired();

                    b.Navigation("Remedies");

                    b.Navigation("Tokens");
                });
#pragma warning restore 612, 618
        }
    }
}