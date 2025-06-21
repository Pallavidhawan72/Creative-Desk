using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CreativeDesk.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddProjectDesignersNavigation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProjectId",
                table: "Designers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Designers_ProjectId",
                table: "Designers",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Designers_Projects_ProjectId",
                table: "Designers",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "ProjectId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Designers_Projects_ProjectId",
                table: "Designers");

            migrationBuilder.DropIndex(
                name: "IX_Designers_ProjectId",
                table: "Designers");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "Designers");
        }
    }
}
