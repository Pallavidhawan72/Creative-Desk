using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CreativeDesk.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProjectModel_RemoveIsPaid_AddAmounts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPaid",
                table: "Projects");

            migrationBuilder.AddColumn<decimal>(
                name: "AmountPaid",
                table: "Projects",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "RemainingAmount",
                table: "Projects",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalAmount",
                table: "Projects",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AmountPaid",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "RemainingAmount",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "TotalAmount",
                table: "Projects");

            migrationBuilder.AddColumn<bool>(
                name: "IsPaid",
                table: "Projects",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
