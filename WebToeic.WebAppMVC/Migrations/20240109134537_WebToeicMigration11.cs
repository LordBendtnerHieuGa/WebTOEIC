using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebToeic.WebAppMVC.Migrations
{
    public partial class WebToeicMigration11 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1F563B11-0536-44EC-8804-08686259BBBE",
                column: "ConcurrencyStamp",
                value: "9a75e14d-e81a-48f8-b512-3cc72b90a482");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "503B6BDD-7CC7-454E-9CFE-170D9D266450",
                column: "ConcurrencyStamp",
                value: "f315931c-c306-4764-a7d3-673acb36b4dc");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "AD02894A-8C79-41FF-9BCA-CA2CFF5F959D",
                column: "ConcurrencyStamp",
                value: "a2db8e6e-63e7-408d-a1db-206249844e0a");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6EF4554C-0501-436A-B2EB-6F2A574BAD31",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "c1ae49c0-17dd-4f79-aeb2-73a58637d59b", "AQAAAAEAACcQAAAAEITnQmuW4eA3WIQZwrYFZJDsB5MnelPD72jrOwKkqOyaeQO7UOWkaCCESf9fbQtYFQ==" });
        }
    }
}
