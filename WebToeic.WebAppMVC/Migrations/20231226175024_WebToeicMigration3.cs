using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebToeic.WebAppMVC.Migrations
{
    public partial class WebToeicMigration3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "503B6BDD-7CC7-454E-9CFE-170D9D266450",
                column: "ConcurrencyStamp",
                value: "c4962b8a-b9e9-45b6-a4e4-b23f8dab264b");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6EF4554C-0501-436A-B2EB-6F2A574BAD31",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "f1573771-6f27-4d84-be04-ee83ea943410", "AQAAAAEAACcQAAAAEDvWdThpAPd46K0bOKSRd4KcuqfGfigxNo9f5fcttqFbn8dZMA7tXwFln1iwYWSkNw==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

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
    }
}
