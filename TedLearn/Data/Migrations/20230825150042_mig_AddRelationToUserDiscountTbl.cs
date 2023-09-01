using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class mig_AddRelationToUserDiscountTbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                schema: "Persons",
                table: "UserDiscounts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                schema: "Persons",
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 11001,
                column: "CreateDate",
                value: new DateTime(2023, 8, 25, 18, 30, 41, 825, DateTimeKind.Local).AddTicks(8065));

            migrationBuilder.UpdateData(
                schema: "Persons",
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 11002,
                column: "CreateDate",
                value: new DateTime(2023, 8, 25, 18, 30, 41, 825, DateTimeKind.Local).AddTicks(8089));

            migrationBuilder.UpdateData(
                schema: "Persons",
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 11003,
                column: "CreateDate",
                value: new DateTime(2023, 8, 25, 18, 30, 41, 825, DateTimeKind.Local).AddTicks(8091));

            migrationBuilder.UpdateData(
                schema: "Persons",
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 11004,
                column: "CreateDate",
                value: new DateTime(2023, 8, 25, 18, 30, 41, 825, DateTimeKind.Local).AddTicks(8093));

            migrationBuilder.CreateIndex(
                name: "IX_UserDiscounts_OrderId",
                schema: "Persons",
                table: "UserDiscounts",
                column: "OrderId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UserDiscounts_Orders_OrderId",
                schema: "Persons",
                table: "UserDiscounts",
                column: "OrderId",
                principalSchema: "Sales",
                principalTable: "Orders",
                principalColumn: "OrderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserDiscounts_Orders_OrderId",
                schema: "Persons",
                table: "UserDiscounts");

            migrationBuilder.DropIndex(
                name: "IX_UserDiscounts_OrderId",
                schema: "Persons",
                table: "UserDiscounts");

            migrationBuilder.DropColumn(
                name: "OrderId",
                schema: "Persons",
                table: "UserDiscounts");

            migrationBuilder.UpdateData(
                schema: "Persons",
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 11001,
                column: "CreateDate",
                value: new DateTime(2023, 7, 11, 0, 42, 17, 585, DateTimeKind.Local).AddTicks(6899));

            migrationBuilder.UpdateData(
                schema: "Persons",
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 11002,
                column: "CreateDate",
                value: new DateTime(2023, 7, 11, 0, 42, 17, 585, DateTimeKind.Local).AddTicks(6915));

            migrationBuilder.UpdateData(
                schema: "Persons",
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 11003,
                column: "CreateDate",
                value: new DateTime(2023, 7, 11, 0, 42, 17, 585, DateTimeKind.Local).AddTicks(6917));

            migrationBuilder.UpdateData(
                schema: "Persons",
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 11004,
                column: "CreateDate",
                value: new DateTime(2023, 7, 11, 0, 42, 17, 585, DateTimeKind.Local).AddTicks(6918));
        }
    }
}
