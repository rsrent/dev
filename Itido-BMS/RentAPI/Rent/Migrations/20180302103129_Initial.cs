using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Rent.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Area",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(nullable: true),
                    TranslationID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Area", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "CleaningDescription",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(nullable: true),
                    PricePerSquareMeter = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CleaningDescription", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "CustomerDocument",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Category = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    URL = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerDocument", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Floor",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(nullable: true),
                    TranslationID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Floor", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Login",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Password = table.Column<string>(nullable: true),
                    UserName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Login", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Permission",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permission", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Rank = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SpecialMessage",
                columns: table => new
                {
                    ImageLocator = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: true),
                    Time = table.Column<DateTime>(nullable: true),
                    MessageImage_ImageLocator = table.Column<string>(nullable: true),
                    MessageMeeting_Status = table.Column<int>(nullable: true),
                    MessageMeeting_Time = table.Column<DateTime>(nullable: true),
                    MessageMoreWork_Status = table.Column<int>(nullable: true),
                    MessageMoreWork_Time = table.Column<DateTime>(nullable: true),
                    ThumbnailLocator = table.Column<string>(nullable: true),
                    VideoLocator = table.Column<string>(nullable: true),
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Discriminator = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpecialMessage", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Translation",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Translation", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "TranslationEntry",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Language = table.Column<int>(nullable: true),
                    Text = table.Column<string>(nullable: true),
                    TranslationID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TranslationEntry", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Comment = table.Column<string>(nullable: true),
                    CustomerID = table.Column<int>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    FirstName = table.Column<string>(nullable: true),
                    ImageLocation = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    LoginID = table.Column<int>(nullable: false),
                    Phone = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.ID);
                    table.ForeignKey(
                        name: "FK_User_Login_LoginID",
                        column: x => x.LoginID,
                        principalTable: "Login",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PermissionsTemplate",
                columns: table => new
                {
                    PermissionID = table.Column<int>(nullable: false),
                    RoleID = table.Column<int>(nullable: false),
                    Create = table.Column<bool>(nullable: false),
                    Decide = table.Column<bool>(nullable: false),
                    Delete = table.Column<bool>(nullable: false),
                    Read = table.Column<bool>(nullable: false),
                    Update = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionsTemplate", x => new { x.PermissionID, x.RoleID });
                    table.ForeignKey(
                        name: "FK_PermissionsTemplate_Permission_PermissionID",
                        column: x => x.PermissionID,
                        principalTable: "Permission",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserPermissions",
                columns: table => new
                {
                    PermissionID = table.Column<int>(nullable: false),
                    UserID = table.Column<int>(nullable: false),
                    Create = table.Column<bool>(nullable: false),
                    Decide = table.Column<bool>(nullable: false),
                    Delete = table.Column<bool>(nullable: false),
                    Read = table.Column<bool>(nullable: false),
                    Update = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPermissions", x => new { x.PermissionID, x.UserID });
                    table.ForeignKey(
                        name: "FK_UserPermissions_Permission_PermissionID",
                        column: x => x.PermissionID,
                        principalTable: "Permission",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CleaningTask",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Active = table.Column<bool>(nullable: false),
                    AreaID = table.Column<int>(nullable: false),
                    Comment = table.Column<string>(nullable: true),
                    FirstCleaned = table.Column<DateTime>(nullable: true),
                    FloorID = table.Column<int>(nullable: false),
                    Frequency = table.Column<string>(nullable: true),
                    LocationID = table.Column<int>(nullable: false),
                    PlanType = table.Column<int>(nullable: false),
                    SquareMeters = table.Column<int>(nullable: false),
                    TimesOfYear = table.Column<byte>(nullable: true),
                    UserResposibleID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CleaningTask", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CleaningTask_Area_AreaID",
                        column: x => x.AreaID,
                        principalTable: "Area",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CleaningTask_Floor_FloorID",
                        column: x => x.FloorID,
                        principalTable: "Floor",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CleaningTask_User_UserResposibleID",
                        column: x => x.UserResposibleID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LocationUser",
                columns: table => new
                {
                    LocationID = table.Column<int>(nullable: false),
                    UserID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocationUser", x => new { x.LocationID, x.UserID });
                    table.ForeignKey(
                        name: "FK_LocationUser_User_UserID",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Message",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ConversationID = table.Column<int>(nullable: false),
                    MessageText = table.Column<string>(nullable: true),
                    SentTime = table.Column<DateTime>(nullable: false),
                    SpecialMessageID = table.Column<int>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    UserID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Message", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Message_SpecialMessage_SpecialMessageID",
                        column: x => x.SpecialMessageID,
                        principalTable: "SpecialMessage",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Message_User_UserID",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CleaningTaskCompleted",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CleaningTaskID = table.Column<int>(nullable: false),
                    Comment = table.Column<string>(nullable: true),
                    CompletedByUserID = table.Column<int>(nullable: false),
                    CompletedDate = table.Column<DateTime>(nullable: false),
                    Confirmed = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CleaningTaskCompleted", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CleaningTaskCompleted_CleaningTask_CleaningTaskID",
                        column: x => x.CleaningTaskID,
                        principalTable: "CleaningTask",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CleaningTaskCompleted_User_CompletedByUserID",
                        column: x => x.CompletedByUserID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Conversation",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Closed = table.Column<bool>(nullable: false),
                    NewestMessageID = table.Column<int>(nullable: true),
                    Title = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Conversation", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Conversation_Message_NewestMessageID",
                        column: x => x.NewestMessageID,
                        principalTable: "Message",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ConversationUsers",
                columns: table => new
                {
                    ConversationID = table.Column<int>(nullable: false),
                    UserID = table.Column<int>(nullable: false),
                    LastSeenMessageID = table.Column<int>(nullable: true),
                    NotificationsOn = table.Column<bool>(nullable: false),
                    UnseenMessages = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConversationUsers", x => new { x.ConversationID, x.UserID });
                    table.ForeignKey(
                        name: "FK_ConversationUsers_Conversation_ConversationID",
                        column: x => x.ConversationID,
                        principalTable: "Conversation",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ConversationUsers_Message_LastSeenMessageID",
                        column: x => x.LastSeenMessageID,
                        principalTable: "Message",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ConversationUsers_User_UserID",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Comment = table.Column<string>(nullable: true),
                    ConversationID = table.Column<int>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    HRContactID = table.Column<int>(nullable: true),
                    ImageLocation = table.Column<string>(nullable: true),
                    MainUserID = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    SalesRepID = table.Column<int>(nullable: true),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Customer_Conversation_ConversationID",
                        column: x => x.ConversationID,
                        principalTable: "Conversation",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Customer_User_HRContactID",
                        column: x => x.HRContactID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Customer_User_MainUserID",
                        column: x => x.MainUserID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Customer_User_SalesRepID",
                        column: x => x.SalesRepID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Location",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Address = table.Column<string>(nullable: true),
                    Comment = table.Column<string>(nullable: true),
                    CustomerContactID = table.Column<int>(nullable: true),
                    CustomerConversationID = table.Column<int>(nullable: true),
                    CustomerID = table.Column<int>(nullable: true),
                    ImageLocation = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    ServiceLeaderID = table.Column<int>(nullable: true),
                    TeamConversationID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Location", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Location_User_CustomerContactID",
                        column: x => x.CustomerContactID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Location_Conversation_CustomerConversationID",
                        column: x => x.CustomerConversationID,
                        principalTable: "Conversation",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Location_Customer_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "Customer",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Location_User_ServiceLeaderID",
                        column: x => x.ServiceLeaderID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Location_Conversation_TeamConversationID",
                        column: x => x.TeamConversationID,
                        principalTable: "Conversation",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "QualityReport",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CompletedTime = table.Column<DateTime>(nullable: false),
                    LocationID = table.Column<int>(nullable: false),
                    Time = table.Column<DateTime>(nullable: false),
                    UserID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QualityReport", x => x.ID);
                    table.ForeignKey(
                        name: "FK_QualityReport_Location_LocationID",
                        column: x => x.LocationID,
                        principalTable: "Location",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QualityReport_User_UserID",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QualityReportItem",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CleaningTaskID = table.Column<int>(nullable: false),
                    Comment = table.Column<string>(nullable: true),
                    QualityReportID = table.Column<int>(nullable: false),
                    Rating = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QualityReportItem", x => x.ID);
                    table.ForeignKey(
                        name: "FK_QualityReportItem_CleaningTask_CleaningTaskID",
                        column: x => x.CleaningTaskID,
                        principalTable: "CleaningTask",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QualityReportItem_QualityReport_QualityReportID",
                        column: x => x.QualityReportID,
                        principalTable: "QualityReport",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CleaningTask_AreaID",
                table: "CleaningTask",
                column: "AreaID");

            migrationBuilder.CreateIndex(
                name: "IX_CleaningTask_FloorID",
                table: "CleaningTask",
                column: "FloorID");

            migrationBuilder.CreateIndex(
                name: "IX_CleaningTask_UserResposibleID",
                table: "CleaningTask",
                column: "UserResposibleID");

            migrationBuilder.CreateIndex(
                name: "IX_CleaningTaskCompleted_CleaningTaskID",
                table: "CleaningTaskCompleted",
                column: "CleaningTaskID");

            migrationBuilder.CreateIndex(
                name: "IX_CleaningTaskCompleted_CompletedByUserID",
                table: "CleaningTaskCompleted",
                column: "CompletedByUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Conversation_NewestMessageID",
                table: "Conversation",
                column: "NewestMessageID",
                unique: true,
                filter: "[NewestMessageID] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ConversationUsers_LastSeenMessageID",
                table: "ConversationUsers",
                column: "LastSeenMessageID");

            migrationBuilder.CreateIndex(
                name: "IX_ConversationUsers_UserID",
                table: "ConversationUsers",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_ConversationID",
                table: "Customer",
                column: "ConversationID");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_HRContactID",
                table: "Customer",
                column: "HRContactID");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_MainUserID",
                table: "Customer",
                column: "MainUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_SalesRepID",
                table: "Customer",
                column: "SalesRepID");

            migrationBuilder.CreateIndex(
                name: "IX_Location_CustomerContactID",
                table: "Location",
                column: "CustomerContactID");

            migrationBuilder.CreateIndex(
                name: "IX_Location_CustomerConversationID",
                table: "Location",
                column: "CustomerConversationID");

            migrationBuilder.CreateIndex(
                name: "IX_Location_CustomerID",
                table: "Location",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_Location_ServiceLeaderID",
                table: "Location",
                column: "ServiceLeaderID");

            migrationBuilder.CreateIndex(
                name: "IX_Location_TeamConversationID",
                table: "Location",
                column: "TeamConversationID");

            migrationBuilder.CreateIndex(
                name: "IX_LocationUser_UserID",
                table: "LocationUser",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Message_SpecialMessageID",
                table: "Message",
                column: "SpecialMessageID");

            migrationBuilder.CreateIndex(
                name: "IX_Message_UserID",
                table: "Message",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_QualityReport_LocationID",
                table: "QualityReport",
                column: "LocationID");

            migrationBuilder.CreateIndex(
                name: "IX_QualityReport_UserID",
                table: "QualityReport",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_QualityReportItem_CleaningTaskID",
                table: "QualityReportItem",
                column: "CleaningTaskID");

            migrationBuilder.CreateIndex(
                name: "IX_QualityReportItem_QualityReportID",
                table: "QualityReportItem",
                column: "QualityReportID");

            migrationBuilder.CreateIndex(
                name: "IX_User_LoginID",
                table: "User",
                column: "LoginID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CleaningDescription");

            migrationBuilder.DropTable(
                name: "CleaningTaskCompleted");

            migrationBuilder.DropTable(
                name: "ConversationUsers");

            migrationBuilder.DropTable(
                name: "CustomerDocument");

            migrationBuilder.DropTable(
                name: "LocationUser");

            migrationBuilder.DropTable(
                name: "PermissionsTemplate");

            migrationBuilder.DropTable(
                name: "QualityReportItem");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "Translation");

            migrationBuilder.DropTable(
                name: "TranslationEntry");

            migrationBuilder.DropTable(
                name: "UserPermissions");

            migrationBuilder.DropTable(
                name: "CleaningTask");

            migrationBuilder.DropTable(
                name: "QualityReport");

            migrationBuilder.DropTable(
                name: "Permission");

            migrationBuilder.DropTable(
                name: "Area");

            migrationBuilder.DropTable(
                name: "Floor");

            migrationBuilder.DropTable(
                name: "Location");

            migrationBuilder.DropTable(
                name: "Customer");

            migrationBuilder.DropTable(
                name: "Conversation");

            migrationBuilder.DropTable(
                name: "Message");

            migrationBuilder.DropTable(
                name: "SpecialMessage");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Login");
        }
    }
}
