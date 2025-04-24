using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class _4к35е67г8ш : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Blocks_Tests_FinalTestId1",
                table: "Blocks");

            migrationBuilder.DropColumn(
                name: "BlockId",
                table: "Tests");

            migrationBuilder.DropColumn(
                name: "FinalTestId",
                table: "Blocks");

            migrationBuilder.RenameColumn(
                name: "FinalTestId1",
                table: "Blocks",
                newName: "TestId");

            migrationBuilder.RenameIndex(
                name: "IX_Blocks_FinalTestId1",
                table: "Blocks",
                newName: "IX_Blocks_TestId");

            migrationBuilder.AddForeignKey(
                name: "FK_Blocks_Tests_TestId",
                table: "Blocks",
                column: "TestId",
                principalTable: "Tests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Blocks_Tests_TestId",
                table: "Blocks");

            migrationBuilder.RenameColumn(
                name: "TestId",
                table: "Blocks",
                newName: "FinalTestId1");

            migrationBuilder.RenameIndex(
                name: "IX_Blocks_TestId",
                table: "Blocks",
                newName: "IX_Blocks_FinalTestId1");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Blocks_Tests_FinalTestId1",
                table: "Blocks",
                column: "FinalTestId1",
                principalTable: "Tests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
