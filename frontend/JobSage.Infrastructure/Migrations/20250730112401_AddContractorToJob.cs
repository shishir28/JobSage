using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobSage.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddContractorToJob : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ContractorId",
                table: "Jobs",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Contractors",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Trade = table.Column<string>(type: "text", nullable: false),
                    Rating = table.Column<float>(type: "real", nullable: false),
                    Availability = table.Column<string>(type: "text", nullable: false),
                    ContactInfo = table.Column<string>(type: "text", nullable: false),
                    Location = table.Column<string>(type: "text", nullable: false),
                    HourlyRate = table.Column<float>(type: "real", nullable: false),
                    Preferred = table.Column<bool>(type: "boolean", nullable: false),
                    WarrantyApproved = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contractors", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_ContractorId",
                table: "Jobs",
                column: "ContractorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_Contractors_ContractorId",
                table: "Jobs",
                column: "ContractorId",
                principalTable: "Contractors",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_Contractors_ContractorId",
                table: "Jobs");

            migrationBuilder.DropTable(
                name: "Contractors");

            migrationBuilder.DropIndex(
                name: "IX_Jobs_ContractorId",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "ContractorId",
                table: "Jobs");
        }
    }
}
