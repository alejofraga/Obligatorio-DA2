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
    [Migration("20241112151844_GetHomesPermission")]
    partial class GetHomesPermission
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

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
                            RoleName = "HomeOwner",
                            SystemPermissionName = "CreateHome"
                        },
                        new
                        {
                            RoleName = "HomeOwner",
                            SystemPermissionName = "BeHomeMember"
                        },
                        new
                        {
                            RoleName = "HomeOwner",
                            SystemPermissionName = "AddMember"
                        },
                        new
                        {
                            RoleName = "Admin",
                            SystemPermissionName = "CreateUserWithRole"
                        },
                        new
                        {
                            RoleName = "Admin",
                            SystemPermissionName = "GetUsers"
                        },
                        new
                        {
                            RoleName = "HomeOwner",
                            SystemPermissionName = "GetMembers"
                        },
                        new
                        {
                            RoleName = "CompanyOwner",
                            SystemPermissionName = "CreateCompany"
                        },
                        new
                        {
                            RoleName = "Admin",
                            SystemPermissionName = "GetCompanies"
                        },
                        new
                        {
                            RoleName = "Admin",
                            SystemPermissionName = "DeleteAdmin"
                        },
                        new
                        {
                            RoleName = "CompanyOwner",
                            SystemPermissionName = "CreateDevice"
                        },
                        new
                        {
                            RoleName = "HomeOwner",
                            SystemPermissionName = "GetHomes"
                        });
                });

            modelBuilder.Entity("SmartHome.BusinessLogic.Companies.Company", b =>
                {
                    b.Property<string>("RUT")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LogoUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OwnerEmail")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ValidatorTypeName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("RUT");

                    b.HasIndex("OwnerEmail");

                    b.ToTable("Companies");
                });

            modelBuilder.Entity("SmartHome.BusinessLogic.DeviceTypes.DeviceType", b =>
                {
                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Name");

                    b.ToTable("DeviceTypes");

                    b.HasData(
                        new
                        {
                            Name = "Camera"
                        },
                        new
                        {
                            Name = "Sensor"
                        },
                        new
                        {
                            Name = "MovementSensor"
                        },
                        new
                        {
                            Name = "Lamp"
                        });
                });

            modelBuilder.Entity("SmartHome.BusinessLogic.Devices.Device", b =>
                {
                    b.Property<string>("ModelNumber")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CompanyRUT")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DeviceType")
                        .IsRequired()
                        .HasMaxLength(8)
                        .HasColumnType("nvarchar(8)");

                    b.Property<string>("DeviceTypeName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Photos")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ModelNumber");

                    b.HasIndex("CompanyRUT");

                    b.ToTable("Devices");

                    b.HasDiscriminator<string>("DeviceType").HasValue("Device");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("SmartHome.BusinessLogic.Homes.Coordinates", b =>
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

            modelBuilder.Entity("SmartHome.BusinessLogic.Homes.Hardware", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("Connected")
                        .HasColumnType("bit");

                    b.Property<string>("DeviceModelNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasMaxLength(21)
                        .HasColumnType("nvarchar(21)");

                    b.Property<Guid>("HomeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("RoomId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("DeviceModelNumber");

                    b.HasIndex("HomeId");

                    b.HasIndex("RoomId");

                    b.ToTable("Hardwares");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Hardware");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("SmartHome.BusinessLogic.Homes.Home", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("MemberCount")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OwnerEmail")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("OwnerEmail");

                    b.ToTable("Homes");
                });

            modelBuilder.Entity("SmartHome.BusinessLogic.Homes.HomePermission", b =>
                {
                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<Guid>("MemberId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Name", "MemberId");

                    b.HasIndex("MemberId");

                    b.ToTable("HomePermissions");
                });

            modelBuilder.Entity("SmartHome.BusinessLogic.Homes.Location", b =>
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

            modelBuilder.Entity("SmartHome.BusinessLogic.Homes.Member", b =>
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

            modelBuilder.Entity("SmartHome.BusinessLogic.Homes.NotiAction", b =>
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

            modelBuilder.Entity("SmartHome.BusinessLogic.Homes.Notification", b =>
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

            modelBuilder.Entity("SmartHome.BusinessLogic.Homes.Room", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("HomeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("HomeId");

                    b.ToTable("Rooms");
                });

            modelBuilder.Entity("SmartHome.BusinessLogic.Sessions.Session", b =>
                {
                    b.Property<Guid>("SessionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("UserEmail")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("SessionId");

                    b.HasIndex("UserEmail")
                        .IsUnique()
                        .HasFilter("[UserEmail] IS NOT NULL");

                    b.ToTable("Sessions");
                });

            modelBuilder.Entity("SmartHome.BusinessLogic.Users.Role", b =>
                {
                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Name");

                    b.ToTable("Roles");

                    b.HasData(
                        new
                        {
                            Name = "Admin"
                        },
                        new
                        {
                            Name = "HomeOwner"
                        },
                        new
                        {
                            Name = "CompanyOwner"
                        });
                });

            modelBuilder.Entity("SmartHome.BusinessLogic.Users.SystemPermission", b =>
                {
                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Name");

                    b.ToTable("SystemPermissions");

                    b.HasData(
                        new
                        {
                            Name = "GetUsers"
                        },
                        new
                        {
                            Name = "BeHomeMember"
                        },
                        new
                        {
                            Name = "CreateUserWithRole"
                        },
                        new
                        {
                            Name = "CreateHome"
                        },
                        new
                        {
                            Name = "AddMember"
                        },
                        new
                        {
                            Name = "GetMembers"
                        },
                        new
                        {
                            Name = "CreateCompany"
                        },
                        new
                        {
                            Name = "GetCompanies"
                        },
                        new
                        {
                            Name = "DeleteAdmin"
                        },
                        new
                        {
                            Name = "CreateDevice"
                        },
                        new
                        {
                            Name = "GetHomes"
                        });
                });

            modelBuilder.Entity("SmartHome.BusinessLogic.Users.User", b =>
                {
                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("AccountCreation")
                        .HasColumnType("datetime2");

                    b.Property<string>("Lastname")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
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
                            AccountCreation = new DateTime(2024, 11, 12, 12, 18, 42, 918, DateTimeKind.Local).AddTicks(7525),
                            Lastname = "Admin",
                            Name = "System",
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
                            RoleName = "Admin",
                            UserEmail = "sa@smarthome.com"
                        });
                });

            modelBuilder.Entity("SmartHome.BusinessLogic.Devices.Camera", b =>
                {
                    b.HasBaseType("SmartHome.BusinessLogic.Devices.Device");

                    b.Property<bool?>("HasMovementDetection")
                        .HasColumnType("bit");

                    b.Property<bool?>("HasPersonDetection")
                        .HasColumnType("bit");

                    b.Property<bool?>("IsIndoor")
                        .HasColumnType("bit");

                    b.Property<bool?>("IsOutdoor")
                        .HasColumnType("bit");

                    b.HasDiscriminator().HasValue("Camera");
                });

            modelBuilder.Entity("SmartHome.BusinessLogic.Homes.LampHardware", b =>
                {
                    b.HasBaseType("SmartHome.BusinessLogic.Homes.Hardware");

                    b.Property<bool>("IsOn")
                        .HasColumnType("bit");

                    b.HasDiscriminator().HasValue("LampHardware");
                });

            modelBuilder.Entity("SmartHome.BusinessLogic.Homes.SensorHardware", b =>
                {
                    b.HasBaseType("SmartHome.BusinessLogic.Homes.Hardware");

                    b.Property<bool>("IsOpen")
                        .HasColumnType("bit");

                    b.HasDiscriminator().HasValue("SensorHardware");
                });

            modelBuilder.Entity("RoleSystemPermission", b =>
                {
                    b.HasOne("SmartHome.BusinessLogic.Users.Role", null)
                        .WithMany()
                        .HasForeignKey("RoleName")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SmartHome.BusinessLogic.Users.SystemPermission", null)
                        .WithMany()
                        .HasForeignKey("SystemPermissionName")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SmartHome.BusinessLogic.Companies.Company", b =>
                {
                    b.HasOne("SmartHome.BusinessLogic.Users.User", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerEmail")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("SmartHome.BusinessLogic.Devices.Device", b =>
                {
                    b.HasOne("SmartHome.BusinessLogic.Companies.Company", "Company")
                        .WithMany()
                        .HasForeignKey("CompanyRUT");

                    b.Navigation("Company");
                });

            modelBuilder.Entity("SmartHome.BusinessLogic.Homes.Coordinates", b =>
                {
                    b.HasOne("SmartHome.BusinessLogic.Homes.Home", "Home")
                        .WithOne("Coordinates")
                        .HasForeignKey("SmartHome.BusinessLogic.Homes.Coordinates", "HomeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Home");
                });

            modelBuilder.Entity("SmartHome.BusinessLogic.Homes.Hardware", b =>
                {
                    b.HasOne("SmartHome.BusinessLogic.Devices.Device", "Device")
                        .WithMany("Hardwares")
                        .HasForeignKey("DeviceModelNumber")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SmartHome.BusinessLogic.Homes.Home", "Home")
                        .WithMany("Hardwares")
                        .HasForeignKey("HomeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SmartHome.BusinessLogic.Homes.Room", "Room")
                        .WithMany("Hardwares")
                        .HasForeignKey("RoomId");

                    b.Navigation("Device");

                    b.Navigation("Home");

                    b.Navigation("Room");
                });

            modelBuilder.Entity("SmartHome.BusinessLogic.Homes.Home", b =>
                {
                    b.HasOne("SmartHome.BusinessLogic.Users.User", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerEmail")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("SmartHome.BusinessLogic.Homes.HomePermission", b =>
                {
                    b.HasOne("SmartHome.BusinessLogic.Homes.Member", null)
                        .WithMany("HomePermissions")
                        .HasForeignKey("MemberId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SmartHome.BusinessLogic.Homes.Location", b =>
                {
                    b.HasOne("SmartHome.BusinessLogic.Homes.Home", "Home")
                        .WithOne("Location")
                        .HasForeignKey("SmartHome.BusinessLogic.Homes.Location", "HomeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Home");
                });

            modelBuilder.Entity("SmartHome.BusinessLogic.Homes.Member", b =>
                {
                    b.HasOne("SmartHome.BusinessLogic.Homes.Home", "Home")
                        .WithMany("Members")
                        .HasForeignKey("HomeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SmartHome.BusinessLogic.Users.User", "User")
                        .WithMany("Members")
                        .HasForeignKey("UserEmail")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Home");

                    b.Navigation("User");
                });

            modelBuilder.Entity("SmartHome.BusinessLogic.Homes.NotiAction", b =>
                {
                    b.HasOne("SmartHome.BusinessLogic.Homes.Home", "Home")
                        .WithMany()
                        .HasForeignKey("HomeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SmartHome.BusinessLogic.Homes.Member", "Member")
                        .WithMany("NotiActions")
                        .HasForeignKey("MemberId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SmartHome.BusinessLogic.Homes.Notification", "Notification")
                        .WithMany("NotiActions")
                        .HasForeignKey("NotificationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Home");

                    b.Navigation("Member");

                    b.Navigation("Notification");
                });

            modelBuilder.Entity("SmartHome.BusinessLogic.Homes.Notification", b =>
                {
                    b.HasOne("SmartHome.BusinessLogic.Homes.Hardware", "Hardware")
                        .WithMany()
                        .HasForeignKey("HardwareId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Hardware");
                });

            modelBuilder.Entity("SmartHome.BusinessLogic.Homes.Room", b =>
                {
                    b.HasOne("SmartHome.BusinessLogic.Homes.Home", "Home")
                        .WithMany("Rooms")
                        .HasForeignKey("HomeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Home");
                });

            modelBuilder.Entity("SmartHome.BusinessLogic.Sessions.Session", b =>
                {
                    b.HasOne("SmartHome.BusinessLogic.Users.User", "User")
                        .WithOne("Session")
                        .HasForeignKey("SmartHome.BusinessLogic.Sessions.Session", "UserEmail");

                    b.Navigation("User");
                });

            modelBuilder.Entity("UserRole", b =>
                {
                    b.HasOne("SmartHome.BusinessLogic.Users.Role", null)
                        .WithMany()
                        .HasForeignKey("RoleName")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SmartHome.BusinessLogic.Users.User", null)
                        .WithMany()
                        .HasForeignKey("UserEmail")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SmartHome.BusinessLogic.Devices.Device", b =>
                {
                    b.Navigation("Hardwares");
                });

            modelBuilder.Entity("SmartHome.BusinessLogic.Homes.Home", b =>
                {
                    b.Navigation("Coordinates")
                        .IsRequired();

                    b.Navigation("Hardwares");

                    b.Navigation("Location")
                        .IsRequired();

                    b.Navigation("Members");

                    b.Navigation("Rooms");
                });

            modelBuilder.Entity("SmartHome.BusinessLogic.Homes.Member", b =>
                {
                    b.Navigation("HomePermissions");

                    b.Navigation("NotiActions");
                });

            modelBuilder.Entity("SmartHome.BusinessLogic.Homes.Notification", b =>
                {
                    b.Navigation("NotiActions");
                });

            modelBuilder.Entity("SmartHome.BusinessLogic.Homes.Room", b =>
                {
                    b.Navigation("Hardwares");
                });

            modelBuilder.Entity("SmartHome.BusinessLogic.Users.User", b =>
                {
                    b.Navigation("Members");

                    b.Navigation("Session")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
