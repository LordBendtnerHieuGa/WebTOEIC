using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebToeic.WebAppMVC.Migrations
{
    public partial class WebToeicMigration5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageU",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1F563B11-0536-44EC-8804-08686259BBBE",
                column: "ConcurrencyStamp",
                value: "36568208-c36c-473e-a9fc-a07d65f9c038");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "503B6BDD-7CC7-454E-9CFE-170D9D266450",
                column: "ConcurrencyStamp",
                value: "750c00c0-6768-41cb-ace1-2be066bbeb5c");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "AD02894A-8C79-41FF-9BCA-CA2CFF5F959D",
                column: "ConcurrencyStamp",
                value: "15a91d51-069b-4b83-ae02-8766a823e48d");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6EF4554C-0501-436A-B2EB-6F2A574BAD31",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "ab033952-f919-4024-8455-75d3b1bd0396", "AQAAAAEAACcQAAAAEJ7R9MJ2t1kSH4LMP3F/01eLfLUBxYwbhMcxVWHfNgbhmM2lIjv6zfbu+/koaSKbSg==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageU",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1F563B11-0536-44EC-8804-08686259BBBE",
                column: "ConcurrencyStamp",
                value: "d6c426d4-877c-4f30-95b4-fedbc820248d");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "503B6BDD-7CC7-454E-9CFE-170D9D266450",
                column: "ConcurrencyStamp",
                value: "03fd1892-3b4d-4575-8202-94d3cf5c3264");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "AD02894A-8C79-41FF-9BCA-CA2CFF5F959D",
                column: "ConcurrencyStamp",
                value: "ca162fef-4329-400b-808a-a0efe586d953");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6EF4554C-0501-436A-B2EB-6F2A574BAD31",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "1cbebdf2-5734-4428-9a54-9dc643a92797", "AQAAAAEAACcQAAAAEM4A+RGf1RnE2gIanpVsJL5pT129+TQ5LKV95N/imPT1/LhOcy3DVXGu7WkCp2fIDA==" });
        }
    }
}
