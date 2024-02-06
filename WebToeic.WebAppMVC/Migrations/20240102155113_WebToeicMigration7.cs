using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebToeic.WebAppMVC.Migrations
{
    public partial class WebToeicMigration7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1F563B11-0536-44EC-8804-08686259BBBE",
                column: "ConcurrencyStamp",
                value: "04d16e4e-e582-407a-864e-28f722cb7b8e");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "503B6BDD-7CC7-454E-9CFE-170D9D266450",
                column: "ConcurrencyStamp",
                value: "820f4ee2-d435-4e4b-b80c-872ce841ca76");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "AD02894A-8C79-41FF-9BCA-CA2CFF5F959D",
                column: "ConcurrencyStamp",
                value: "d678b184-0b81-43e2-adfb-dd9dd3b0d9a2");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6EF4554C-0501-436A-B2EB-6F2A574BAD31",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "089aaa06-4c22-4c6c-b0aa-170b30ba051f", "AQAAAAEAACcQAAAAEAbN+mVvKFTpQOGv940udckwDvp5FUa81SGaNQpegL7lfXTxuzwW+48ZlN7CsINQ4A==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1F563B11-0536-44EC-8804-08686259BBBE",
                column: "ConcurrencyStamp",
                value: "602b2074-566d-4833-be71-008052b7cca7");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "503B6BDD-7CC7-454E-9CFE-170D9D266450",
                column: "ConcurrencyStamp",
                value: "68821d60-81b6-43c6-a0e8-5a9d03a46ce1");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "AD02894A-8C79-41FF-9BCA-CA2CFF5F959D",
                column: "ConcurrencyStamp",
                value: "5e3013c8-2f55-4c23-bbc5-d3ca96c82329");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6EF4554C-0501-436A-B2EB-6F2A574BAD31",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "cdf72c60-5c8b-4b77-b2b2-b002fc8a75d6", "AQAAAAEAACcQAAAAENEwmRNHDdXUgtimkj8Iep3T0OFBKhJJMKZ2z6UZE/tbetWEJVJuxzNFHXdwLy9wug==" });
        }
    }
}
