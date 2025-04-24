using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class _12342576 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Blocks_Tests_TestId",
                table: "Blocks");

            migrationBuilder.DropIndex(
                name: "IX_Blocks_TestId",
                table: "Blocks");

            migrationBuilder.AddColumn<int>(
                name: "BlockId",
                table: "Tests",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FinalTestId",
                table: "Blocks",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Blocks_FinalTestId",
                table: "Blocks",
                column: "FinalTestId");

            migrationBuilder.AddForeignKey(
                name: "FK_Blocks_Tests_FinalTestId",
                table: "Blocks",
                column: "FinalTestId",
                principalTable: "Tests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Blocks_Tests_FinalTestId",
                table: "Blocks");

            migrationBuilder.DropIndex(
                name: "IX_Blocks_FinalTestId",
                table: "Blocks");

            migrationBuilder.DropColumn(
                name: "BlockId",
                table: "Tests");

            migrationBuilder.DropColumn(
                name: "FinalTestId",
                table: "Blocks");

            migrationBuilder.CreateIndex(
                name: "IX_Blocks_TestId",
                table: "Blocks",
                column: "TestId");

            migrationBuilder.AddForeignKey(
                name: "FK_Blocks_Tests_TestId",
                table: "Blocks",
                column: "TestId",
                principalTable: "Tests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
