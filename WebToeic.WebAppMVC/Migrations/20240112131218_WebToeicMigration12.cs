using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebToeic.WebAppMVC.Migrations
{
    public partial class WebToeicMigration12 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsReply",
                table: "CommentVocabularies");

            migrationBuilder.DropColumn(
                name: "IsReply",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "IsReply",
                table: "CommentReadings");

            migrationBuilder.DropColumn(
                name: "IsReply",
                table: "CommentListens");

            migrationBuilder.AddColumn<string>(
                name: "UserNameCmtV",
                table: "CommentVocabularies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserNameCmtG",
                table: "Comments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserNameCmtR",
                table: "CommentReadings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserNameCmtL",
                table: "CommentListens",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserNameCmtV",
                table: "CommentVocabularies");

            migrationBuilder.DropColumn(
                name: "UserNameCmtG",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "UserNameCmtR",
                table: "CommentReadings");

            migrationBuilder.DropColumn(
                name: "UserNameCmtL",
                table: "CommentListens");

            migrationBuilder.AddColumn<bool>(
                name: "IsReply",
                table: "CommentVocabularies",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsReply",
                table: "Comments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsReply",
                table: "CommentReadings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsReply",
                table: "CommentListens",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1F563B11-0536-44EC-8804-08686259BBBE",
                column: "ConcurrencyStamp",
                value: "d0ecd29a-73a9-4f7b-8e6f-643318ea5b69");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "503B6BDD-7CC7-454E-9CFE-170D9D266450",
                column: "ConcurrencyStamp",
                value: "31cef7a9-9136-49d6-b956-be004f1df631");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "AD02894A-8C79-41FF-9BCA-CA2CFF5F959D",
                column: "ConcurrencyStamp",
                value: "178bf141-55d7-4007-a693-e0171624565a");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6EF4554C-0501-436A-B2EB-6F2A574BAD31",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "ef16e264-e4bc-4865-8d10-29f9ec7c1bf1", "AQAAAAEAACcQAAAAEEYiBZp3MT9L6VibzHKr25PLzJEG+bLSCcHZs2+zXB64PkpSDOnbMyvjaVuBrBmaiw==" });
        }
    }
}
