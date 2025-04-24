using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class _34576u8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Blocks_Tests_FinalTestId",
                table: "Blocks");

            migrationBuilder.DropIndex(
                name: "IX_Blocks_FinalTestId",
                table: "Blocks");

            migrationBuilder.RenameColumn(
                name: "TestId",
                table: "Blocks",
                newName: "FinalTestId1");

            migrationBuilder.CreateIndex(
                name: "IX_Blocks_FinalTestId1",
                table: "Blocks",
                column: "FinalTestId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Blocks_Tests_FinalTestId1",
                table: "Blocks",
                column: "FinalTestId1",
                principalTable: "Tests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Blocks_Tests_FinalTestId1",
                table: "Blocks");

            migrationBuilder.DropIndex(
                name: "IX_Blocks_FinalTestId1",
                table: "Blocks");

            migrationBuilder.RenameColumn(
                name: "FinalTestId1",
                table: "Blocks",
                newName: "TestId");

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
    }
}
