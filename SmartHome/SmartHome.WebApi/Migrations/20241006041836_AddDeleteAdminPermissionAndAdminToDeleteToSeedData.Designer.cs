﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SmartHome.DataLayer;

#nullable disable

namespace SmartHome.WebApi.Migrations
{
    [DbContext(typeof(SmartHomeDbContext))]
    [Migration("20241006041836_AddDeleteAdminPermissionAndAdminToDeleteToSeedData")]
    partial class AddDeleteAdminPermissionAndAdminToDeleteToSeedData
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Home", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("MemberCount")
                        .HasColumnType("int");

                    b.Property<string>("OwnerEmail")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("OwnerEmail");

                    b.ToTable("Homes");
                });

            modelBuilder.Entity("RoleSystemPermission", b =>
                {
                    b.Property<string>("RoleName")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("SystemPermissionName")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("RoleName", "SystemPermissionName");

                    b.HasIndex("SystemPermissionName");

                    b.ToTable("RoleSystemPermission");

                    b.HasData(
                        new
                        {
                            RoleName = "homeOwner",
                            SystemPermissionName = "createHome"
                        },
                        new
                        {
                            RoleName = "homeOwner",
                            SystemPermissionName = "addMember"
                        },
                        new
                        {
                            RoleName = "admin",
                            SystemPermissionName = "createAdmin&CompanyOwner"
                        },
                        new
                        {
                            RoleName = "admin",
                            SystemPermissionName = "getUsers"
                        },
                        new
                        {
                            RoleName = "homeOwner",
                            SystemPermissionName = "getMembers"
                        },
                        new
                        {
                            RoleName = "companyOwner",
                            SystemPermissionName = "createCompany"
                        },
                        new
                        {
                            RoleName = "admin",
                            SystemPermissionName = "getCompanies"
                        },
                        new
                        {
                            RoleName = "admin",
                            SystemPermissionName = "deleteAdmin"
                        },
                        new
                        {
                            RoleName = "companyOwner",
                            SystemPermissionName = "createDevice"
                        });
                });

            modelBuilder.Entity("SmartHome.BusinessLogic.Feature_Company.Company", b =>
                {
                    b.Property<string>("RUT")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LogoUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OwnerEmail")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("RUT");

                    b.HasIndex("OwnerEmail");

                    b.ToTable("Companies");

                    b.HasData(
                        new
                        {
                            RUT = "111111111111",
                            LogoUrl = "logo",
                            Name = "Cameras SA",
                            OwnerEmail = "sa@smarthome.com"
                        });
                });

            modelBuilder.Entity("SmartHome.BusinessLogic.Feature_Device.Sensor", b =>
                {
                    b.Property<string>("ModelNumber")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CompanyRUT")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasMaxLength(8)
                        .HasColumnType("nvarchar(8)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Photos")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ModelNumber");

                    b.HasIndex("CompanyRUT");

                    b.ToTable("Devices");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Sensor");

                    b.UseTphMappingStrategy();

                    b.HasData(
                        new
                        {
                            ModelNumber = "77",
                            CompanyRUT = "111111111111",
                            Description = "yard sensor 01",
                            Name = "Sensor1",
                            Photos = "[\"photo13\",\"photo278\"]"
                        });
                });

            modelBuilder.Entity("SmartHome.BusinessLogic.Feature_DeviceTypes.DeviceType", b =>
                {
                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Name");

                    b.ToTable("DeviceTypes");

                    b.HasData(
                        new
                        {
                            Name = "camera"
                        },
                        new
                        {
                            Name = "sensor"
                        });
                });

            modelBuilder.Entity("SmartHome.BusinessLogic.Feature_Home.Coordinates", b =>
                {
                    b.Property<string>("Latitude")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Longitude")
                        .HasColumnType("nvarchar(450)");

                    b.Property<Guid>("HomeId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Latitude", "Longitude");

                    b.HasIndex("HomeId")
                        .IsUnique();

                    b.ToTable("Coordinates");
                });

            modelBuilder.Entity("SmartHome.BusinessLogic.Feature_Home.Hardware", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("Connected")
                        .HasColumnType("bit");

                    b.Property<Guid>("HomeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("SensorModelNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("HomeId");

                    b.HasIndex("SensorModelNumber");

                    b.ToTable("Hardwares");
                });

            modelBuilder.Entity("SmartHome.BusinessLogic.Feature_Home.HomePermission", b =>
                {
                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<Guid>("MemberId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Name", "MemberId");

                    b.HasIndex("MemberId");

                    b.ToTable("HomePermissions");
                });

            modelBuilder.Entity("SmartHome.BusinessLogic.Feature_Home.Location", b =>
                {
                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("DoorNumber")
                        .HasColumnType("nvarchar(450)");

                    b.Property<Guid>("HomeId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Address", "DoorNumber");

                    b.HasIndex("HomeId")
                        .IsUnique();

                    b.ToTable("Locations");
                });

            modelBuilder.Entity("SmartHome.BusinessLogic.Feature_Home.Member", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("HomeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("UserEmail")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("HomeId");

                    b.HasIndex("UserEmail");

                    b.ToTable("Members");
                });

            modelBuilder.Entity("SmartHome.BusinessLogic.Feature_Home.NotiAction", b =>
                {
                    b.Property<Guid>("MemberId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("NotificationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("HomeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsRead")
                        .HasColumnType("bit");

                    b.HasKey("MemberId", "NotificationId");

                    b.HasIndex("HomeId");

                    b.HasIndex("NotificationId");

                    b.ToTable("NotiActions");
                });

            modelBuilder.Entity("SmartHome.BusinessLogic.Feature_Home.Notification", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("HardwareId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("HardwareId");

                    b.ToTable("Notifications");
                });

            modelBuilder.Entity("SmartHome.BusinessLogic.Feature_User.Role", b =>
                {
                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Name");

                    b.ToTable("Roles");

                    b.HasData(
                        new
                        {
                            Name = "admin"
                        },
                        new
                        {
                            Name = "homeOwner"
                        },
                        new
                        {
                            Name = "companyOwner"
                        });
                });

            modelBuilder.Entity("SmartHome.BusinessLogic.Feature_User.SystemPermission", b =>
                {
                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Name");

                    b.ToTable("SystemPermissions");

                    b.HasData(
                        new
                        {
                            Name = "getUsers"
                        },
                        new
                        {
                            Name = "createAdmin&CompanyOwner"
                        },
                        new
                        {
                            Name = "createHome"
                        },
                        new
                        {
                            Name = "addMember"
                        },
                        new
                        {
                            Name = "getMembers"
                        },
                        new
                        {
                            Name = "createCompany"
                        },
                        new
                        {
                            Name = "getCompanies"
                        },
                        new
                        {
                            Name = "sendNotifications"
                        },
                        new
                        {
                            Name = "deleteAdmin"
                        },
                        new
                        {
                            Name = "createDevice"
                        });
                });

            modelBuilder.Entity("SmartHome.BusinessLogic.Feature_User.User", b =>
                {
                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("AccountCreation")
                        .HasColumnType("datetime2");

                    b.Property<string>("Lastname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProfilePicturePath")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Email");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Email = "sa@smarthome.com",
                            AccountCreation = new DateTime(2024, 10, 6, 1, 18, 35, 372, DateTimeKind.Local).AddTicks(2517),
                            Lastname = "Admin",
                            Name = "System",
                            Password = "sA$a1234",
                            ProfilePicturePath = "path"
                        },
                        new
                        {
                            Email = "delete@smarthome.com",
                            AccountCreation = new DateTime(2024, 10, 6, 1, 18, 35, 372, DateTimeKind.Local).AddTicks(2670),
                            Lastname = "me",
                            Name = "delete",
                            Password = "sA$a1234",
                            ProfilePicturePath = "path"
                        },
                        new
                        {
                            Email = "ho@smarthome.com",
                            AccountCreation = new DateTime(2024, 10, 6, 1, 18, 35, 372, DateTimeKind.Local).AddTicks(2689),
                            Lastname = "owner",
                            Name = "home",
                            Password = "sA$a1234",
                            ProfilePicturePath = "path"
                        },
                        new
                        {
                            Email = "co@smarthome.com",
                            AccountCreation = new DateTime(2024, 10, 6, 1, 18, 35, 372, DateTimeKind.Local).AddTicks(2706),
                            Lastname = "owner",
                            Name = "company",
                            Password = "sA$a1234",
                            ProfilePicturePath = "path"
                        });
                });

            modelBuilder.Entity("UserRole", b =>
                {
                    b.Property<string>("RoleName")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("UserEmail")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("RoleName", "UserEmail");

                    b.HasIndex("UserEmail");

                    b.ToTable("UserRole");

                    b.HasData(
                        new
                        {
                            RoleName = "admin",
                            UserEmail = "sa@smarthome.com"
                        },
                        new
                        {
                            RoleName = "admin",
                            UserEmail = "delete@smarthome.com"
                        },
                        new
                        {
                            RoleName = "homeOwner",
                            UserEmail = "ho@smarthome.com"
                        },
                        new
                        {
                            RoleName = "companyOwner",
                            UserEmail = "co@smarthome.com"
                        });
                });

            modelBuilder.Entity("SmartHome.BusinessLogic.Feature_Device.Camera", b =>
                {
                    b.HasBaseType("SmartHome.BusinessLogic.Feature_Device.Sensor");

                    b.Property<bool>("HasMovementDetection")
                        .HasColumnType("bit");

                    b.Property<bool>("HasPersonDetection")
                        .HasColumnType("bit");

                    b.Property<bool>("IsIndoor")
                        .HasColumnType("bit");

                    b.Property<bool>("IsOutdoor")
                        .HasColumnType("bit");

                    b.HasDiscriminator().HasValue("Camera");

                    b.HasData(
                        new
                        {
                            ModelNumber = "10",
                            CompanyRUT = "111111111111",
                            Description = "Camera for living room",
                            Name = "Camera1",
                            Photos = "[\"photo1\",\"photo2\"]",
                            HasMovementDetection = false,
                            HasPersonDetection = false,
                            IsIndoor = true,
                            IsOutdoor = false
                        });
                });

            modelBuilder.Entity("Home", b =>
                {
                    b.HasOne("SmartHome.BusinessLogic.Feature_User.User", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerEmail")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("RoleSystemPermission", b =>
                {
                    b.HasOne("SmartHome.BusinessLogic.Feature_User.Role", null)
                        .WithMany()
                        .HasForeignKey("RoleName")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SmartHome.BusinessLogic.Feature_User.SystemPermission", null)
                        .WithMany()
                        .HasForeignKey("SystemPermissionName")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SmartHome.BusinessLogic.Feature_Company.Company", b =>
                {
                    b.HasOne("SmartHome.BusinessLogic.Feature_User.User", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerEmail")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("SmartHome.BusinessLogic.Feature_Device.Sensor", b =>
                {
                    b.HasOne("SmartHome.BusinessLogic.Feature_Company.Company", "Company")
                        .WithMany()
                        .HasForeignKey("CompanyRUT")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Company");
                });

            modelBuilder.Entity("SmartHome.BusinessLogic.Feature_Home.Coordinates", b =>
                {
                    b.HasOne("Home", "Home")
                        .WithOne("Coordinates")
                        .HasForeignKey("SmartHome.BusinessLogic.Feature_Home.Coordinates", "HomeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Home");
                });

            modelBuilder.Entity("SmartHome.BusinessLogic.Feature_Home.Hardware", b =>
                {
                    b.HasOne("Home", "Home")
                        .WithMany("Hardwares")
                        .HasForeignKey("HomeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SmartHome.BusinessLogic.Feature_Device.Sensor", "Device")
                        .WithMany("Hardwares")
                        .HasForeignKey("SensorModelNumber")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Device");

                    b.Navigation("Home");
                });

            modelBuilder.Entity("SmartHome.BusinessLogic.Feature_Home.HomePermission", b =>
                {
                    b.HasOne("SmartHome.BusinessLogic.Feature_Home.Member", null)
                        .WithMany("HomePermissions")
                        .HasForeignKey("MemberId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SmartHome.BusinessLogic.Feature_Home.Location", b =>
                {
                    b.HasOne("Home", "Home")
                        .WithOne("Location")
                        .HasForeignKey("SmartHome.BusinessLogic.Feature_Home.Location", "HomeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Home");
                });

            modelBuilder.Entity("SmartHome.BusinessLogic.Feature_Home.Member", b =>
                {
                    b.HasOne("Home", "Home")
                        .WithMany("Members")
                        .HasForeignKey("HomeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SmartHome.BusinessLogic.Feature_User.User", "User")
                        .WithMany("Members")
                        .HasForeignKey("UserEmail")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Home");

                    b.Navigation("User");
                });

            modelBuilder.Entity("SmartHome.BusinessLogic.Feature_Home.NotiAction", b =>
                {
                    b.HasOne("Home", "Home")
                        .WithMany()
                        .HasForeignKey("HomeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SmartHome.BusinessLogic.Feature_Home.Member", "Member")
                        .WithMany("NotiActions")
                        .HasForeignKey("MemberId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("SmartHome.BusinessLogic.Feature_Home.Notification", "Notification")
                        .WithMany("NotiActions")
                        .HasForeignKey("NotificationId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Home");

                    b.Navigation("Member");

                    b.Navigation("Notification");
                });

            modelBuilder.Entity("SmartHome.BusinessLogic.Feature_Home.Notification", b =>
                {
                    b.HasOne("SmartHome.BusinessLogic.Feature_Home.Hardware", "Hardware")
                        .WithMany()
                        .HasForeignKey("HardwareId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Hardware");
                });

            modelBuilder.Entity("UserRole", b =>
                {
                    b.HasOne("SmartHome.BusinessLogic.Feature_User.Role", null)
                        .WithMany()
                        .HasForeignKey("RoleName")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SmartHome.BusinessLogic.Feature_User.User", null)
                        .WithMany()
                        .HasForeignKey("UserEmail")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Home", b =>
                {
                    b.Navigation("Coordinates");

                    b.Navigation("Hardwares");

                    b.Navigation("Location");

                    b.Navigation("Members");
                });

            modelBuilder.Entity("SmartHome.BusinessLogic.Feature_Device.Sensor", b =>
                {
                    b.Navigation("Hardwares");
                });

            modelBuilder.Entity("SmartHome.BusinessLogic.Feature_Home.Member", b =>
                {
                    b.Navigation("HomePermissions");

                    b.Navigation("NotiActions");
                });

            modelBuilder.Entity("SmartHome.BusinessLogic.Feature_Home.Notification", b =>
                {
                    b.Navigation("NotiActions");
                });

            modelBuilder.Entity("SmartHome.BusinessLogic.Feature_User.User", b =>
                {
                    b.Navigation("Members");
                });
#pragma warning restore 612, 618
        }
    }
}
