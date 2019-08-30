﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Rent.Data;
using Rent.Models;
using System;

namespace Rent.Migrations
{
    [DbContext(typeof(RentContext))]
    [Migration("20180420113325_UpdateToLocationHour")]
    partial class UpdateToLocationHour
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Rent.Models.Area", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CleaningPlanID");

                    b.Property<string>("Description");

                    b.Property<int?>("TranslationID");

                    b.HasKey("ID");

                    b.HasIndex("CleaningPlanID");

                    b.ToTable("Area");
                });

            modelBuilder.Entity("Rent.Models.CleaningPlan", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<bool>("HasFloors");

                    b.Property<int?>("TranslationID");

                    b.HasKey("ID");

                    b.ToTable("CleaningPlan");
                });

            modelBuilder.Entity("Rent.Models.CleaningTask", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<int>("AreaID");

                    b.Property<string>("Comment");

                    b.Property<DateTime?>("FirstCleaned");

                    b.Property<int?>("FloorID");

                    b.Property<string>("Frequency");

                    b.Property<int>("LocationID");

                    b.Property<int>("SquareMeters");

                    b.Property<byte?>("TimesOfYear");

                    b.Property<int?>("UserResposibleID");

                    b.HasKey("ID");

                    b.HasIndex("AreaID");

                    b.HasIndex("FloorID");

                    b.HasIndex("UserResposibleID");

                    b.ToTable("CleaningTask");
                });

            modelBuilder.Entity("Rent.Models.CleaningTaskCompleted", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CleaningTaskID");

                    b.Property<string>("Comment");

                    b.Property<int>("CompletedByUserID");

                    b.Property<DateTime>("CompletedDate");

                    b.Property<bool>("Confirmed");

                    b.HasKey("ID");

                    b.HasIndex("CleaningTaskID");

                    b.HasIndex("CompletedByUserID");

                    b.ToTable("CleaningTaskCompleted");
                });

            modelBuilder.Entity("Rent.Models.Conversation", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("NewestMessageID");

                    b.Property<bool>("Open");

                    b.Property<string>("Title");

                    b.HasKey("ID");

                    b.HasIndex("NewestMessageID")
                        .IsUnique()
                        .HasFilter("[NewestMessageID] IS NOT NULL");

                    b.ToTable("Conversation");
                });

            modelBuilder.Entity("Rent.Models.ConversationUsers", b =>
                {
                    b.Property<int>("ConversationID");

                    b.Property<int>("UserID");

                    b.Property<int?>("LastSeenMessageID");

                    b.Property<bool>("NotificationsOn");

                    b.Property<int>("UnseenMessages");

                    b.HasKey("ConversationID", "UserID");

                    b.HasIndex("LastSeenMessageID");

                    b.HasIndex("UserID");

                    b.ToTable("ConversationUsers");
                });

            modelBuilder.Entity("Rent.Models.Customer", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Comment");

                    b.Property<int?>("ConversationID");

                    b.Property<DateTime>("Created");

                    b.Property<int>("GeneralFolderID");

                    b.Property<bool>("HasStandardFolders");

                    b.Property<string>("ImageLocation");

                    b.Property<int?>("KeyAccountManagerID");

                    b.Property<int?>("MainUserID");

                    b.Property<int>("ManagementFolderID");

                    b.Property<string>("Name");

                    b.Property<int>("PrivateFolderID");

                    b.Property<int?>("SalesRepID");

                    b.Property<int>("Status");

                    b.HasKey("ID");

                    b.HasIndex("ConversationID");

                    b.HasIndex("GeneralFolderID");

                    b.HasIndex("KeyAccountManagerID");

                    b.HasIndex("MainUserID");

                    b.HasIndex("ManagementFolderID");

                    b.HasIndex("PrivateFolderID");

                    b.HasIndex("SalesRepID");

                    b.ToTable("Customer");
                });

            modelBuilder.Entity("Rent.Models.CustomerDocument", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Category");

                    b.Property<string>("Name");

                    b.Property<string>("URL");

                    b.HasKey("ID");

                    b.ToTable("CustomerDocument");
                });

            modelBuilder.Entity("Rent.Models.DocumentFolder", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("HasParentPermissions");

                    b.Property<int?>("ParentDocumentFolderID");

                    b.Property<bool>("Removable");

                    b.Property<bool>("Standard");

                    b.Property<string>("Title");

                    b.Property<bool>("VisibleToAllRoles");

                    b.HasKey("ID");

                    b.ToTable("DocumentFolder");
                });

            modelBuilder.Entity("Rent.Models.DocumentItem", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("DocumentLocation");

                    b.Property<int>("RootDocumentFolderID");

                    b.Property<string>("Title");

                    b.HasKey("ID");

                    b.ToTable("DocumentItem");
                });

            modelBuilder.Entity("Rent.Models.Floor", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<int?>("TranslationID");

                    b.HasKey("ID");

                    b.ToTable("Floor");
                });

            modelBuilder.Entity("Rent.Models.FolderPermission", b =>
                {
                    b.Property<int>("FolderID");

                    b.Property<int>("RoleID");

                    b.Property<bool>("Edit");

                    b.Property<bool>("Read");

                    b.HasKey("FolderID", "RoleID");

                    b.ToTable("FolderPermission");
                });

            modelBuilder.Entity("Rent.Models.Location", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address");

                    b.Property<int>("CleaningplanFolderID");

                    b.Property<string>("Comment");

                    b.Property<int?>("CustomerContactID");

                    b.Property<int?>("CustomerConversationID");

                    b.Property<int?>("CustomerID");

                    b.Property<int>("GeneralFolderID");

                    b.Property<string>("ImageLocation");

                    b.Property<int>("IntervalOfServiceLeaderMeeting");

                    b.Property<string>("Name");

                    b.Property<string>("Phone");

                    b.Property<int?>("ProjectNumber");

                    b.Property<int?>("ServiceLeaderID");

                    b.Property<int?>("TeamConversationID");

                    b.HasKey("ID");

                    b.HasIndex("CleaningplanFolderID");

                    b.HasIndex("CustomerContactID");

                    b.HasIndex("CustomerConversationID");

                    b.HasIndex("CustomerID");

                    b.HasIndex("GeneralFolderID");

                    b.HasIndex("ServiceLeaderID");

                    b.HasIndex("TeamConversationID");

                    b.ToTable("Location");
                });

            modelBuilder.Entity("Rent.Models.LocationEconomy", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("LocationID");

                    b.Property<int>("PricePerHourCategory");

                    b.Property<float>("PriceRegularCleaning");

                    b.Property<float>("PriceWindowCleaning");

                    b.Property<DateTime?>("StartDate");

                    b.HasKey("ID");

                    b.ToTable("LocationEconomy");
                });

            modelBuilder.Entity("Rent.Models.LocationHour", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("ChristAscension");

                    b.Property<bool>("ChristmasDay");

                    b.Property<bool>("ChristmasEve");

                    b.Property<bool>("DifferentWeeks");

                    b.Property<bool>("EasterDay");

                    b.Property<bool>("GoodFriday");

                    b.Property<float>("L_Fri");

                    b.Property<float>("L_Mon");

                    b.Property<float>("L_Sat");

                    b.Property<float>("L_Sun");

                    b.Property<float>("L_Thu");

                    b.Property<float>("L_Tue");

                    b.Property<float>("L_Wed");

                    b.Property<int>("LocationID");

                    b.Property<bool>("MaundyThursday");

                    b.Property<bool>("NewyearsDay");

                    b.Property<bool>("NewyearsEve");

                    b.Property<bool>("Palmsunday");

                    b.Property<bool>("PrayerDay");

                    b.Property<bool>("SecondEasterDay");

                    b.Property<bool>("SndChristmasDay");

                    b.Property<bool>("SndPentecost");

                    b.Property<float>("U_Fri");

                    b.Property<float>("U_Mon");

                    b.Property<float>("U_Sat");

                    b.Property<float>("U_Sun");

                    b.Property<float>("U_Thu");

                    b.Property<float>("U_Tue");

                    b.Property<float>("U_Wed");

                    b.Property<bool>("WhitSunday");

                    b.HasKey("ID");

                    b.ToTable("LocationHour");
                });

            modelBuilder.Entity("Rent.Models.LocationLog", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateCreated");

                    b.Property<int>("LocationID");

                    b.Property<string>("Log");

                    b.Property<string>("Title");

                    b.Property<int>("UserID");

                    b.HasKey("ID");

                    b.ToTable("LocationLog");
                });

            modelBuilder.Entity("Rent.Models.LocationUser", b =>
                {
                    b.Property<int>("LocationID");

                    b.Property<int>("UserID");

                    b.Property<string>("Title");

                    b.HasKey("LocationID", "UserID");

                    b.HasIndex("UserID");

                    b.ToTable("LocationUser");
                });

            modelBuilder.Entity("Rent.Models.Login", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Password");

                    b.Property<string>("UserName");

                    b.HasKey("ID");

                    b.ToTable("Login");
                });

            modelBuilder.Entity("Rent.Models.Message", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ConversationID");

                    b.Property<string>("MessageText");

                    b.Property<DateTime>("SentTime");

                    b.Property<int?>("SpecialMessageID");

                    b.Property<string>("Type");

                    b.Property<int>("UserID");

                    b.HasKey("ID");

                    b.HasIndex("SpecialMessageID");

                    b.HasIndex("UserID");

                    b.ToTable("Message");
                });

            modelBuilder.Entity("Rent.Models.MoreWork", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("ActualCompletedTime");

                    b.Property<int>("CleaningPlanID");

                    b.Property<int>("CreatedByUserID");

                    b.Property<string>("Description");

                    b.Property<DateTime>("ExpectedCompletedTime");

                    b.Property<int>("LocationID");

                    b.Property<int?>("UserID");

                    b.HasKey("ID");

                    b.ToTable("MoreWork");
                });

            modelBuilder.Entity("Rent.Models.Permission", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.Property<bool>("OnlyMasterCanChange");

                    b.HasKey("ID");

                    b.ToTable("Permission");
                });

            modelBuilder.Entity("Rent.Models.PermissionsTemplate", b =>
                {
                    b.Property<int>("PermissionID");

                    b.Property<int>("RoleID");

                    b.Property<bool>("Create");

                    b.Property<bool>("Delete");

                    b.Property<bool>("Read");

                    b.Property<bool>("Update");

                    b.HasKey("PermissionID", "RoleID");

                    b.ToTable("PermissionsTemplate");
                });

            modelBuilder.Entity("Rent.Models.QualityReport", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("CompletedTime");

                    b.Property<int>("LocationID");

                    b.Property<int?>("RatingID");

                    b.Property<DateTime>("Time");

                    b.Property<int>("UserID");

                    b.HasKey("ID");

                    b.HasIndex("LocationID");

                    b.HasIndex("RatingID");

                    b.HasIndex("UserID");

                    b.ToTable("QualityReport");
                });

            modelBuilder.Entity("Rent.Models.QualityReportItem", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CleaningTaskID");

                    b.Property<string>("Comment");

                    b.Property<string>("ImageLocation");

                    b.Property<int>("QualityReportID");

                    b.Property<byte>("Rating");

                    b.HasKey("ID");

                    b.HasIndex("CleaningTaskID");

                    b.HasIndex("QualityReportID");

                    b.ToTable("QualityReportItem");
                });

            modelBuilder.Entity("Rent.Models.Rating", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Title");

                    b.HasKey("ID");

                    b.ToTable("Rating");
                });

            modelBuilder.Entity("Rent.Models.RatingItem", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Comment");

                    b.Property<int>("RatedByID");

                    b.Property<int>("RatingID");

                    b.Property<DateTime>("TimeRated");

                    b.Property<string>("Title");

                    b.Property<int>("Value");

                    b.HasKey("ID");

                    b.HasIndex("RatedByID");

                    b.HasIndex("RatingID");

                    b.ToTable("RatingItem");
                });

            modelBuilder.Entity("Rent.Models.Role", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.Property<int>("Rank");

                    b.HasKey("ID");

                    b.ToTable("Role");
                });

            modelBuilder.Entity("Rent.Models.SpecialMessage", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Discriminator")
                        .IsRequired();

                    b.HasKey("ID");

                    b.ToTable("SpecialMessage");

                    b.HasDiscriminator<string>("Discriminator").HasValue("SpecialMessage");
                });

            modelBuilder.Entity("Rent.Models.Translation", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.HasKey("ID");

                    b.ToTable("Translation");
                });

            modelBuilder.Entity("Rent.Models.TranslationEntry", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("Language");

                    b.Property<string>("Text");

                    b.Property<int>("TranslationID");

                    b.HasKey("ID");

                    b.ToTable("TranslationEntry");
                });

            modelBuilder.Entity("Rent.Models.User", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Comment");

                    b.Property<int?>("CustomerID");

                    b.Property<string>("Email");

                    b.Property<int?>("EmployeeNumber");

                    b.Property<string>("FirstName");

                    b.Property<string>("ImageLocation");

                    b.Property<string>("LastName");

                    b.Property<int>("LoginID");

                    b.Property<string>("Phone");

                    b.Property<int>("RoleID");

                    b.Property<string>("Title");

                    b.HasKey("ID");

                    b.HasIndex("CustomerID");

                    b.HasIndex("LoginID");

                    b.HasIndex("RoleID");

                    b.ToTable("User");
                });

            modelBuilder.Entity("Rent.Models.UserPermissions", b =>
                {
                    b.Property<int>("PermissionID");

                    b.Property<int>("UserID");

                    b.Property<bool>("Create");

                    b.Property<bool>("Delete");

                    b.Property<bool>("Read");

                    b.Property<bool>("Update");

                    b.HasKey("PermissionID", "UserID");

                    b.HasIndex("UserID");

                    b.ToTable("UserPermissions");
                });

            modelBuilder.Entity("Rent.Models.MessageComplaint", b =>
                {
                    b.HasBaseType("Rent.Models.SpecialMessage");

                    b.Property<string>("ImageLocator");

                    b.Property<int>("Status");

                    b.Property<DateTime>("Time");

                    b.ToTable("Complaint");

                    b.HasDiscriminator().HasValue("MessageComplaint");
                });

            modelBuilder.Entity("Rent.Models.MessageImage", b =>
                {
                    b.HasBaseType("Rent.Models.SpecialMessage");

                    b.Property<string>("ImageLocator")
                        .HasColumnName("MessageImage_ImageLocator");

                    b.ToTable("Image");

                    b.HasDiscriminator().HasValue("MessageImage");
                });

            modelBuilder.Entity("Rent.Models.MessageMeeting", b =>
                {
                    b.HasBaseType("Rent.Models.SpecialMessage");

                    b.Property<int>("Status")
                        .HasColumnName("MessageMeeting_Status");

                    b.Property<DateTime>("Time")
                        .HasColumnName("MessageMeeting_Time");

                    b.ToTable("Meeting");

                    b.HasDiscriminator().HasValue("MessageMeeting");
                });

            modelBuilder.Entity("Rent.Models.MessageMoreWork", b =>
                {
                    b.HasBaseType("Rent.Models.SpecialMessage");

                    b.Property<int>("Status")
                        .HasColumnName("MessageMoreWork_Status");

                    b.Property<DateTime>("Time")
                        .HasColumnName("MessageMoreWork_Time");

                    b.ToTable("MoreWork");

                    b.HasDiscriminator().HasValue("MessageMoreWork");
                });

            modelBuilder.Entity("Rent.Models.MessageVideo", b =>
                {
                    b.HasBaseType("Rent.Models.SpecialMessage");

                    b.Property<string>("ThumbnailLocator");

                    b.Property<string>("VideoLocator");

                    b.ToTable("Video");

                    b.HasDiscriminator().HasValue("MessageVideo");
                });

            modelBuilder.Entity("Rent.Models.Area", b =>
                {
                    b.HasOne("Rent.Models.CleaningPlan", "CleaningPlan")
                        .WithMany()
                        .HasForeignKey("CleaningPlanID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Rent.Models.CleaningTask", b =>
                {
                    b.HasOne("Rent.Models.Area", "Area")
                        .WithMany()
                        .HasForeignKey("AreaID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Rent.Models.Floor", "Floor")
                        .WithMany()
                        .HasForeignKey("FloorID");

                    b.HasOne("Rent.Models.User", "UserResposible")
                        .WithMany()
                        .HasForeignKey("UserResposibleID");
                });

            modelBuilder.Entity("Rent.Models.CleaningTaskCompleted", b =>
                {
                    b.HasOne("Rent.Models.CleaningTask", "CleaningTask")
                        .WithMany()
                        .HasForeignKey("CleaningTaskID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Rent.Models.User", "CompletedByUser")
                        .WithMany()
                        .HasForeignKey("CompletedByUserID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Rent.Models.Conversation", b =>
                {
                    b.HasOne("Rent.Models.Message", "NewestMessage")
                        .WithOne("Conversation")
                        .HasForeignKey("Rent.Models.Conversation", "NewestMessageID");
                });

            modelBuilder.Entity("Rent.Models.ConversationUsers", b =>
                {
                    b.HasOne("Rent.Models.Conversation", "Conversation")
                        .WithMany()
                        .HasForeignKey("ConversationID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Rent.Models.Message", "LastSeenMessage")
                        .WithMany()
                        .HasForeignKey("LastSeenMessageID");

                    b.HasOne("Rent.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Rent.Models.Customer", b =>
                {
                    b.HasOne("Rent.Models.Conversation", "Conversation")
                        .WithMany()
                        .HasForeignKey("ConversationID");

                    b.HasOne("Rent.Models.DocumentFolder", "GeneralFolder")
                        .WithMany()
                        .HasForeignKey("GeneralFolderID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Rent.Models.User", "KeyAccountManager")
                        .WithMany()
                        .HasForeignKey("KeyAccountManagerID");

                    b.HasOne("Rent.Models.User", "MainUser")
                        .WithMany()
                        .HasForeignKey("MainUserID");

                    b.HasOne("Rent.Models.DocumentFolder", "ManagementFolder")
                        .WithMany()
                        .HasForeignKey("ManagementFolderID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Rent.Models.DocumentFolder", "PrivateFolder")
                        .WithMany()
                        .HasForeignKey("PrivateFolderID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Rent.Models.User", "SalesRep")
                        .WithMany()
                        .HasForeignKey("SalesRepID");
                });

            modelBuilder.Entity("Rent.Models.FolderPermission", b =>
                {
                    b.HasOne("Rent.Models.DocumentFolder", "Folder")
                        .WithMany("FolderPermissions")
                        .HasForeignKey("FolderID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Rent.Models.Location", b =>
                {
                    b.HasOne("Rent.Models.DocumentFolder", "CleaningplanFolder")
                        .WithMany()
                        .HasForeignKey("CleaningplanFolderID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Rent.Models.User", "CustomerContact")
                        .WithMany()
                        .HasForeignKey("CustomerContactID");

                    b.HasOne("Rent.Models.Conversation", "CustomerConversation")
                        .WithMany()
                        .HasForeignKey("CustomerConversationID");

                    b.HasOne("Rent.Models.Customer", "Customer")
                        .WithMany()
                        .HasForeignKey("CustomerID");

                    b.HasOne("Rent.Models.DocumentFolder", "GeneralFolder")
                        .WithMany()
                        .HasForeignKey("GeneralFolderID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Rent.Models.User", "ServiceLeader")
                        .WithMany()
                        .HasForeignKey("ServiceLeaderID");

                    b.HasOne("Rent.Models.Conversation", "TeamConversation")
                        .WithMany()
                        .HasForeignKey("TeamConversationID");
                });

            modelBuilder.Entity("Rent.Models.LocationUser", b =>
                {
                    b.HasOne("Rent.Models.Location", "Location")
                        .WithMany()
                        .HasForeignKey("LocationID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Rent.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Rent.Models.Message", b =>
                {
                    b.HasOne("Rent.Models.SpecialMessage", "SpecialMessage")
                        .WithMany()
                        .HasForeignKey("SpecialMessageID");

                    b.HasOne("Rent.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Rent.Models.PermissionsTemplate", b =>
                {
                    b.HasOne("Rent.Models.Permission", "Permission")
                        .WithMany()
                        .HasForeignKey("PermissionID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Rent.Models.QualityReport", b =>
                {
                    b.HasOne("Rent.Models.Location", "Location")
                        .WithMany()
                        .HasForeignKey("LocationID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Rent.Models.Rating", "Rating")
                        .WithMany()
                        .HasForeignKey("RatingID");

                    b.HasOne("Rent.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Rent.Models.QualityReportItem", b =>
                {
                    b.HasOne("Rent.Models.CleaningTask", "CleaningTask")
                        .WithMany()
                        .HasForeignKey("CleaningTaskID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Rent.Models.QualityReport", "QualityReport")
                        .WithMany("QualityReportItems")
                        .HasForeignKey("QualityReportID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Rent.Models.RatingItem", b =>
                {
                    b.HasOne("Rent.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("RatedByID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Rent.Models.Rating", "Rating")
                        .WithMany("RatingItems")
                        .HasForeignKey("RatingID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Rent.Models.User", b =>
                {
                    b.HasOne("Rent.Models.Customer", "Customer")
                        .WithMany("Users")
                        .HasForeignKey("CustomerID");

                    b.HasOne("Rent.Models.Login", "Login")
                        .WithMany()
                        .HasForeignKey("LoginID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Rent.Models.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Rent.Models.UserPermissions", b =>
                {
                    b.HasOne("Rent.Models.Permission", "Permission")
                        .WithMany()
                        .HasForeignKey("PermissionID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Rent.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
