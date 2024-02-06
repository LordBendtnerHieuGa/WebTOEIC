using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebToeic.WebAppMVC.Migrations
{
    public partial class WebToeicMigration4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "503B6BDD-7CC7-454E-9CFE-170D9D266450",
                column: "ConcurrencyStamp",
                value: "03fd1892-3b4d-4575-8202-94d3cf5c3264");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Description", "Discriminator", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1F563B11-0536-44EC-8804-08686259BBBE", "d6c426d4-877c-4f30-95b4-fedbc820248d", "Member role", "Role", "member", "member" },
                    { "AD02894A-8C79-41FF-9BCA-CA2CFF5F959D", "ca162fef-4329-400b-808a-a0efe586d953", "Employee role", "Role", "employee", "employee" }
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6EF4554C-0501-436A-B2EB-6F2A574BAD31",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "1cbebdf2-5734-4428-9a54-9dc643a92797", "AQAAAAEAACcQAAAAEM4A+RGf1RnE2gIanpVsJL5pT129+TQ5LKV95N/imPT1/LhOcy3DVXGu7WkCp2fIDA==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1F563B11-0536-44EC-8804-08686259BBBE");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "AD02894A-8C79-41FF-9BCA-CA2CFF5F959D");

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
    }
}
