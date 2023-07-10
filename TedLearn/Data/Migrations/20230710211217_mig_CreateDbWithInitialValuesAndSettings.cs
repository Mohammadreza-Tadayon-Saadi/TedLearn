using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class mig_CreateDbWithInitialValuesAndSettings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Products");

            migrationBuilder.EnsureSchema(
                name: "Persons");

            migrationBuilder.EnsureSchema(
                name: "Sales");

            migrationBuilder.CreateTable(
                name: "CourseGroups",
                schema: "Products",
                columns: table => new
                {
                    GroupId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SubGroupId = table.Column<int>(type: "int", nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    Version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false, comment: "This Column Is For Concurrency Check")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseGroups", x => x.GroupId);
                    table.ForeignKey(
                        name: "FK_CourseGroups_CourseGroups_SubGroupId",
                        column: x => x.SubGroupId,
                        principalSchema: "Products",
                        principalTable: "CourseGroups",
                        principalColumn: "GroupId");
                });

            migrationBuilder.CreateTable(
                name: "CourseStatuses",
                schema: "Products",
                columns: table => new
                {
                    StatusId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StatusTitle = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseStatuses", x => x.StatusId);
                });

            migrationBuilder.CreateTable(
                name: "Discounts",
                schema: "Persons",
                columns: table => new
                {
                    DiscountId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DiscountCode = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    Percent = table.Column<byte>(type: "tinyint", nullable: false),
                    UsableCount = table.Column<short>(type: "smallint", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    Version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Discounts", x => x.DiscountId);
                    table.CheckConstraint("CK_Discounts_Percent", "[Percent] > 0 And [Percent] <= 100");
                    table.CheckConstraint("CK_UsableCount", "UsableCount > 0");
                });

            migrationBuilder.CreateTable(
                name: "Permissions",
                schema: "Persons",
                columns: table => new
                {
                    PermissionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PermissionTitle = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false),
                    ParentId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.PermissionId);
                    table.ForeignKey(
                        name: "FK_Permissions_Permissions_ParentId",
                        column: x => x.ParentId,
                        principalSchema: "Persons",
                        principalTable: "Permissions",
                        principalColumn: "PermissionId");
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                schema: "Persons",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    CanDeleteOrEdit = table.Column<bool>(type: "bit", nullable: false),
                    Version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false, comment: "This Column Is For Concurrency Check")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "TransactionTypes",
                schema: "Persons",
                columns: table => new
                {
                    TypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeTitle = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionTypes", x => x.TypeId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "Persons",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    UserAvatar = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Biography = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    WebsiteAddress = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PasswordHash = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    RegisterDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastLoginDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ActiveCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreateActiveCode = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    Version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false, comment: "This Column Is For Concurrency Check")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "RolePermissions",
                schema: "Persons",
                columns: table => new
                {
                    RolePermissionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    PermissionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermissions", x => x.RolePermissionId);
                    table.ForeignKey(
                        name: "FK_RolePermissions_Permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalSchema: "Persons",
                        principalTable: "Permissions",
                        principalColumn: "PermissionId");
                    table.ForeignKey(
                        name: "FK_RolePermissions_Roles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "Persons",
                        principalTable: "Roles",
                        principalColumn: "RoleId");
                });

            migrationBuilder.CreateTable(
                name: "Courses",
                schema: "Products",
                columns: table => new
                {
                    CourseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupId = table.Column<int>(type: "int", nullable: false),
                    SubGroupId = table.Column<int>(type: "int", nullable: true),
                    TeacherId = table.Column<int>(type: "int", nullable: false),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    CourseLevel = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CourseTitle = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    CoursePrice = table.Column<decimal>(type: "money", nullable: false),
                    CourseDemoFile = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    CourseImage = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    CourseDescription = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    CourseRequirement = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CourseTags = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    Version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false, comment: "This Column Is For Concurrency Check")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.CourseId);
                    table.CheckConstraint("CK_CoursePrice", "CoursePrice >= 0");
                    table.ForeignKey(
                        name: "FK_Courses_CourseGroups_GroupId",
                        column: x => x.GroupId,
                        principalSchema: "Products",
                        principalTable: "CourseGroups",
                        principalColumn: "GroupId");
                    table.ForeignKey(
                        name: "FK_Courses_CourseGroups_SubGroupId",
                        column: x => x.SubGroupId,
                        principalSchema: "Products",
                        principalTable: "CourseGroups",
                        principalColumn: "GroupId");
                    table.ForeignKey(
                        name: "FK_Courses_CourseStatuses_StatusId",
                        column: x => x.StatusId,
                        principalSchema: "Products",
                        principalTable: "CourseStatuses",
                        principalColumn: "StatusId");
                    table.ForeignKey(
                        name: "FK_Courses_Users_TeacherId",
                        column: x => x.TeacherId,
                        principalSchema: "Persons",
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                schema: "Sales",
                columns: table => new
                {
                    OrderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    IsFinaly = table.Column<bool>(type: "bit", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Discount = table.Column<decimal>(type: "money", nullable: false, defaultValue: 0m)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK_Orders_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "Persons",
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                schema: "Persons",
                columns: table => new
                {
                    TransactionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "money", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    IsPay = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.TransactionId);
                    table.CheckConstraint("CK_Amount", "[Amount] >= 1000 And [Amount] <= 2500000");
                    table.ForeignKey(
                        name: "FK_Transactions_TransactionTypes_TypeId",
                        column: x => x.TypeId,
                        principalSchema: "Persons",
                        principalTable: "TransactionTypes",
                        principalColumn: "TypeId");
                    table.ForeignKey(
                        name: "FK_Transactions_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "Persons",
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "UserDiscounts",
                schema: "Persons",
                columns: table => new
                {
                    UserDiscountId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    DiscountId = table.Column<int>(type: "int", nullable: false),
                    Percent = table.Column<byte>(type: "tinyint", nullable: false),
                    UseDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDiscounts", x => x.UserDiscountId);
                    table.CheckConstraint("CK_UserDiscounts_Percent", "[Percent] > 0 And [Percent] <= 100");
                    table.ForeignKey(
                        name: "FK_UserDiscounts_Discounts_DiscountId",
                        column: x => x.DiscountId,
                        principalSchema: "Persons",
                        principalTable: "Discounts",
                        principalColumn: "DiscountId");
                    table.ForeignKey(
                        name: "FK_UserDiscounts_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "Persons",
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                schema: "Persons",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "Persons",
                        principalTable: "Roles",
                        principalColumn: "RoleId");
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "Persons",
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "AnswerQuestions",
                schema: "Products",
                columns: table => new
                {
                    AQ_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    AnswerId = table.Column<int>(type: "int", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HasRightAnswer = table.Column<bool>(type: "bit", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnswerQuestions", x => x.AQ_Id);
                    table.ForeignKey(
                        name: "FK_AnswerQuestions_AnswerQuestions_AnswerId",
                        column: x => x.AnswerId,
                        principalSchema: "Products",
                        principalTable: "AnswerQuestions",
                        principalColumn: "AQ_Id");
                    table.ForeignKey(
                        name: "FK_AnswerQuestions_Courses_CourseId",
                        column: x => x.CourseId,
                        principalSchema: "Products",
                        principalTable: "Courses",
                        principalColumn: "CourseId");
                    table.ForeignKey(
                        name: "FK_AnswerQuestions_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "Persons",
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "CourseComments",
                schema: "Products",
                columns: table => new
                {
                    CommentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ParentId = table.Column<int>(type: "int", nullable: true),
                    Comment = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseComments", x => x.CommentId);
                    table.ForeignKey(
                        name: "FK_CourseComments_CourseComments_ParentId",
                        column: x => x.ParentId,
                        principalSchema: "Products",
                        principalTable: "CourseComments",
                        principalColumn: "CommentId");
                    table.ForeignKey(
                        name: "FK_CourseComments_Courses_CourseId",
                        column: x => x.CourseId,
                        principalSchema: "Products",
                        principalTable: "Courses",
                        principalColumn: "CourseId");
                    table.ForeignKey(
                        name: "FK_CourseComments_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "Persons",
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "CourseSeasons",
                schema: "Products",
                columns: table => new
                {
                    SeasonId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    SeasonTitle = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    Version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseSeasons", x => x.SeasonId);
                    table.ForeignKey(
                        name: "FK_CourseSeasons_Courses_CourseId",
                        column: x => x.CourseId,
                        principalSchema: "Products",
                        principalTable: "Courses",
                        principalColumn: "CourseId");
                });

            migrationBuilder.CreateTable(
                name: "UserCourses",
                schema: "Products",
                columns: table => new
                {
                    UC_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CourseId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCourses", x => x.UC_Id);
                    table.ForeignKey(
                        name: "FK_UserCourses_Courses_CourseId",
                        column: x => x.CourseId,
                        principalSchema: "Products",
                        principalTable: "Courses",
                        principalColumn: "CourseId");
                    table.ForeignKey(
                        name: "FK_UserCourses_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "Persons",
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "OrderDetails",
                schema: "Sales",
                columns: table => new
                {
                    DetailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "money", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetails", x => x.DetailId);
                    table.ForeignKey(
                        name: "FK_OrderDetails_Courses_CourseId",
                        column: x => x.CourseId,
                        principalSchema: "Products",
                        principalTable: "Courses",
                        principalColumn: "CourseId");
                    table.ForeignKey(
                        name: "FK_OrderDetails_Orders_OrderId",
                        column: x => x.OrderId,
                        principalSchema: "Sales",
                        principalTable: "Orders",
                        principalColumn: "OrderId");
                });

            migrationBuilder.CreateTable(
                name: "CourseEpisodes",
                schema: "Products",
                columns: table => new
                {
                    EpisodeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SeasonId = table.Column<int>(type: "int", nullable: false),
                    EpisodeTitle = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    EpisodeFile = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    EpisodeTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsFree = table.Column<bool>(type: "bit", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    Version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseEpisodes", x => x.EpisodeId);
                    table.ForeignKey(
                        name: "FK_CourseEpisodes_CourseSeasons_SeasonId",
                        column: x => x.SeasonId,
                        principalSchema: "Products",
                        principalTable: "CourseSeasons",
                        principalColumn: "SeasonId");
                });

            migrationBuilder.InsertData(
                schema: "Products",
                table: "CourseGroups",
                columns: new[] { "GroupId", "IsDelete", "SubGroupId", "Title" },
                values: new object[] { 1, false, null, "گروه های دوره ها" });

            migrationBuilder.InsertData(
                schema: "Products",
                table: "CourseStatuses",
                columns: new[] { "StatusId", "StatusTitle" },
                values: new object[,]
                {
                    { 1, "درحال برگزاری" },
                    { 2, "پایان یافته" }
                });

            migrationBuilder.InsertData(
                schema: "Persons",
                table: "Roles",
                columns: new[] { "RoleId", "CanDeleteOrEdit", "CreateDate", "IsDelete", "RoleName" },
                values: new object[,]
                {
                    { 11001, true, new DateTime(2023, 7, 11, 0, 42, 17, 585, DateTimeKind.Local).AddTicks(6899), false, "کاربر عادی" },
                    { 11002, true, new DateTime(2023, 7, 11, 0, 42, 17, 585, DateTimeKind.Local).AddTicks(6915), false, "ادمین" },
                    { 11003, true, new DateTime(2023, 7, 11, 0, 42, 17, 585, DateTimeKind.Local).AddTicks(6917), false, "استاد" },
                    { 11004, true, new DateTime(2023, 7, 11, 0, 42, 17, 585, DateTimeKind.Local).AddTicks(6918), false, "مدیر سایت" }
                });

            migrationBuilder.InsertData(
                schema: "Persons",
                table: "TransactionTypes",
                columns: new[] { "TypeId", "TypeTitle" },
                values: new object[,]
                {
                    { 1, "واریز" },
                    { 2, "برداشت" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnswerQuestions_AnswerId",
                schema: "Products",
                table: "AnswerQuestions",
                column: "AnswerId");

            migrationBuilder.CreateIndex(
                name: "IX_AnswerQuestions_CourseId",
                schema: "Products",
                table: "AnswerQuestions",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_AnswerQuestions_UserId",
                schema: "Products",
                table: "AnswerQuestions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseComments_CourseId",
                schema: "Products",
                table: "CourseComments",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseComments_ParentId",
                schema: "Products",
                table: "CourseComments",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseComments_UserId",
                schema: "Products",
                table: "CourseComments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseEpisodes_EpisodeTime",
                schema: "Products",
                table: "CourseEpisodes",
                column: "EpisodeTime");

            migrationBuilder.CreateIndex(
                name: "IX_CourseEpisodes_EpisodeTitle",
                schema: "Products",
                table: "CourseEpisodes",
                column: "EpisodeTitle");

            migrationBuilder.CreateIndex(
                name: "IX_CourseEpisodes_SeasonId",
                schema: "Products",
                table: "CourseEpisodes",
                column: "SeasonId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseGroups_SubGroupId",
                schema: "Products",
                table: "CourseGroups",
                column: "SubGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseGroups_Title",
                schema: "Products",
                table: "CourseGroups",
                column: "Title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Courses_CourseTitle",
                schema: "Products",
                table: "Courses",
                column: "CourseTitle",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Courses_GroupId",
                schema: "Products",
                table: "Courses",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_StatusId",
                schema: "Products",
                table: "Courses",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_SubGroupId",
                schema: "Products",
                table: "Courses",
                column: "SubGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_TeacherId",
                schema: "Products",
                table: "Courses",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseSeasons_CourseId",
                schema: "Products",
                table: "CourseSeasons",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseSeasons_SeasonTitle",
                schema: "Products",
                table: "CourseSeasons",
                column: "SeasonTitle",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Discounts_DiscountCode",
                schema: "Persons",
                table: "Discounts",
                column: "DiscountCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_CourseId",
                schema: "Sales",
                table: "OrderDetails",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_OrderId",
                schema: "Sales",
                table: "OrderDetails",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserId",
                schema: "Sales",
                table: "Orders",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_ParentId",
                schema: "Persons",
                table: "Permissions",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_PermissionId",
                schema: "Persons",
                table: "RolePermissions",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_RoleId",
                schema: "Persons",
                table: "RolePermissions",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_RoleName",
                schema: "Persons",
                table: "Roles",
                column: "RoleName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_TypeId",
                schema: "Persons",
                table: "Transactions",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_UserId",
                schema: "Persons",
                table: "Transactions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserCourses_CourseId",
                schema: "Products",
                table: "UserCourses",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_UserCourses_UserId",
                schema: "Products",
                table: "UserCourses",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserDiscounts_DiscountId",
                schema: "Persons",
                table: "UserDiscounts",
                column: "DiscountId");

            migrationBuilder.CreateIndex(
                name: "IX_UserDiscounts_UserId",
                schema: "Persons",
                table: "UserDiscounts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                schema: "Persons",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_PhoneNumber",
                schema: "Persons",
                table: "Users",
                column: "PhoneNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserName",
                schema: "Persons",
                table: "Users",
                column: "UserName",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnswerQuestions",
                schema: "Products");

            migrationBuilder.DropTable(
                name: "CourseComments",
                schema: "Products");

            migrationBuilder.DropTable(
                name: "CourseEpisodes",
                schema: "Products");

            migrationBuilder.DropTable(
                name: "OrderDetails",
                schema: "Sales");

            migrationBuilder.DropTable(
                name: "RolePermissions",
                schema: "Persons");

            migrationBuilder.DropTable(
                name: "Transactions",
                schema: "Persons");

            migrationBuilder.DropTable(
                name: "UserCourses",
                schema: "Products");

            migrationBuilder.DropTable(
                name: "UserDiscounts",
                schema: "Persons");

            migrationBuilder.DropTable(
                name: "UserRoles",
                schema: "Persons");

            migrationBuilder.DropTable(
                name: "CourseSeasons",
                schema: "Products");

            migrationBuilder.DropTable(
                name: "Orders",
                schema: "Sales");

            migrationBuilder.DropTable(
                name: "Permissions",
                schema: "Persons");

            migrationBuilder.DropTable(
                name: "TransactionTypes",
                schema: "Persons");

            migrationBuilder.DropTable(
                name: "Discounts",
                schema: "Persons");

            migrationBuilder.DropTable(
                name: "Roles",
                schema: "Persons");

            migrationBuilder.DropTable(
                name: "Courses",
                schema: "Products");

            migrationBuilder.DropTable(
                name: "CourseGroups",
                schema: "Products");

            migrationBuilder.DropTable(
                name: "CourseStatuses",
                schema: "Products");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "Persons");
        }
    }
}
