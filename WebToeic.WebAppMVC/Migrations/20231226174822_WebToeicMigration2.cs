using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebToeic.WebAppMVC.Migrations
{
    public partial class WebToeicMigration2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "503B6BDD-7CC7-454E-9CFE-170D9D266450",
                column: "ConcurrencyStamp",
                value: "425940d3-7a85-4950-a728-64c3345d291b");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6EF4554C-0501-436A-B2EB-6F2A574BAD31",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "3b5b7f53-af41-472e-8985-dd9072d089a9", "AQAAAAEAACcQAAAAENrUCzVdMrIZiviEr0wrWyHT1dIA+wZ5yEfILkMWeQPfWCXmuihXsyMs6Hb2QaPfCA==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "503B6BDD-7CC7-454E-9CFE-170D9D266450",
                column: "ConcurrencyStamp",
                value: "bdbf8c42-a2ee-428f-941f-a7b98b606f70");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6EF4554C-0501-436A-B2EB-6F2A574BAD31",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "1b8ac455-320c-4e9a-9b71-a24413305521", "AQAAAAEAACcQAAAAELJoWlBbFVgcTUm3t05y/aTdA5VfzNiWe+/3e3YOjjO1zwDG9IEu0OPPbVKLa9t4Rw==" });
        }
    }
}
