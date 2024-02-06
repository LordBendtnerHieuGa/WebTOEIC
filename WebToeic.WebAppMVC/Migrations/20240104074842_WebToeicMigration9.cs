using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebToeic.WebAppMVC.Migrations
{
    public partial class WebToeicMigration9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Order",
                table: "ReadingQuestions",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "Order",
                table: "ListeningQuestions",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1F563B11-0536-44EC-8804-08686259BBBE",
                column: "ConcurrencyStamp",
                value: "5e7502ff-11c5-4b94-9735-c31d5e0ace00");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "503B6BDD-7CC7-454E-9CFE-170D9D266450",
                column: "ConcurrencyStamp",
                value: "24f1bd74-7058-4d10-9945-62d3f2ae1842");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "AD02894A-8C79-41FF-9BCA-CA2CFF5F959D",
                column: "ConcurrencyStamp",
                value: "15ff34f0-b741-4723-9902-ddf08cd95155");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6EF4554C-0501-436A-B2EB-6F2A574BAD31",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "1d5bf173-66c7-4a7d-ae88-72bab2315ac9", "AQAAAAEAACcQAAAAEOmNORNcFdkPWqPG/JLtgOeIbT+2WZHw3TfYJe98Y7vXBpjhOTXtQ4sJhLQ/2lKOzg==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Order",
                table: "ReadingQuestions",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Order",
                table: "ListeningQuestions",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1F563B11-0536-44EC-8804-08686259BBBE",
                column: "ConcurrencyStamp",
                value: "c1e95923-1d01-45d3-844c-cf815397ac66");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "503B6BDD-7CC7-454E-9CFE-170D9D266450",
                column: "ConcurrencyStamp",
                value: "1d30415d-97e9-4567-abff-b84770e48d3f");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "AD02894A-8C79-41FF-9BCA-CA2CFF5F959D",
                column: "ConcurrencyStamp",
                value: "8c58333e-29c6-44a0-81b7-12df6f3db53b");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6EF4554C-0501-436A-B2EB-6F2A574BAD31",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "b3b7d70c-c116-44dd-ac40-5d8c139dcacc", "AQAAAAEAACcQAAAAEJhPIv6V2RriHJ6VIgpKz7yu9/5pncXK1T/TBt7W+HhJgNLp/3l13MLLIjC79Zi9dA==" });
        }
    }
}
