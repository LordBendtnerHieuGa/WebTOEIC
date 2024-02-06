using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebToeic.WebAppMVC.Migrations
{
    public partial class WebToeicMigration13 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "VocaNameCmtV",
                table: "CommentVocabularies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "GrammarNameCmtG",
                table: "Comments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ReadingNameCmtR",
                table: "CommentReadings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ListenNameCmtL",
                table: "CommentListens",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1F563B11-0536-44EC-8804-08686259BBBE",
                column: "ConcurrencyStamp",
                value: "9e25c56c-a8ca-450a-83a5-50066c655b29");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "503B6BDD-7CC7-454E-9CFE-170D9D266450",
                column: "ConcurrencyStamp",
                value: "7286eba3-78bf-458b-8d04-0cc7613cb474");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "AD02894A-8C79-41FF-9BCA-CA2CFF5F959D",
                column: "ConcurrencyStamp",
                value: "696a1998-73c4-4042-892e-390761d18a2c");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6EF4554C-0501-436A-B2EB-6F2A574BAD31",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "53c57116-96d6-4891-bce2-500efe93c7f2", "AQAAAAEAACcQAAAAEPWj4CNkOFOsgxOZLOf0RrNPYUE6x1CO63gH9O9oJSPcyY6faUh17bagfLc8bV7MsQ==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VocaNameCmtV",
                table: "CommentVocabularies");

            migrationBuilder.DropColumn(
                name: "GrammarNameCmtG",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "ReadingNameCmtR",
                table: "CommentReadings");

            migrationBuilder.DropColumn(
                name: "ListenNameCmtL",
                table: "CommentListens");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1F563B11-0536-44EC-8804-08686259BBBE",
                column: "ConcurrencyStamp",
                value: "72d59847-0aad-4f90-b62b-79bc12dbd70e");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "503B6BDD-7CC7-454E-9CFE-170D9D266450",
                column: "ConcurrencyStamp",
                value: "19790c46-f1a9-4641-9e30-5335835dc0cf");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "AD02894A-8C79-41FF-9BCA-CA2CFF5F959D",
                column: "ConcurrencyStamp",
                value: "5824cd31-a2ea-4dc4-8655-2026d7bd8730");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6EF4554C-0501-436A-B2EB-6F2A574BAD31",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "862b510b-6b45-4382-ab95-0202eb13d022", "AQAAAAEAACcQAAAAEO2zt6wzZ1c7Wpqc605apnvl5tMj6oE+HibIzpxS2HvxTeiIEFETMH6EIBRT5gK9vw==" });
        }
    }
}
