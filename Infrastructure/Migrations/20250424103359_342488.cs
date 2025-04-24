using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class _342488 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TargetId",
                table: "UserProgresses");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "UserProgresses",
                newName: "BlockId");

            migrationBuilder.RenameColumn(
                name: "CompletionDate",
                table: "UserProgresses",
                newName: "PassedAt");

            migrationBuilder.AlterColumn<bool>(
                name: "IsCompleted",
                table: "UserProgresses",
                type: "boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "UserProgresses",
                type: "character varying(34)",
                maxLength: 34,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsBlockCompleted",
                table: "UserProgresses",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsPassed",
                table: "UserProgresses",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TopicId",
                table: "UserProgresses",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "UserProgresses",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "TestId",
                table: "Blocks",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UserProgresses_BlockId",
                table: "UserProgresses",
                column: "BlockId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_UserProgresses_Blocks_BlockId",
                table: "UserProgresses",
                column: "BlockId",
                principalTable: "Blocks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Blocks_Tests_TestId",
                table: "Blocks");

            migrationBuilder.DropForeignKey(
                name: "FK_UserProgresses_Blocks_BlockId",
                table: "UserProgresses");

            migrationBuilder.DropIndex(
                name: "IX_UserProgresses_BlockId",
                table: "UserProgresses");

            migrationBuilder.DropIndex(
                name: "IX_Blocks_TestId",
                table: "Blocks");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "UserProgresses");

            migrationBuilder.DropColumn(
                name: "IsBlockCompleted",
                table: "UserProgresses");

            migrationBuilder.DropColumn(
                name: "IsPassed",
                table: "UserProgresses");

            migrationBuilder.DropColumn(
                name: "TopicId",
                table: "UserProgresses");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "UserProgresses");

            migrationBuilder.DropColumn(
                name: "TestId",
                table: "Blocks");

            migrationBuilder.RenameColumn(
                name: "PassedAt",
                table: "UserProgresses",
                newName: "CompletionDate");

            migrationBuilder.RenameColumn(
                name: "BlockId",
                table: "UserProgresses",
                newName: "Type");

            migrationBuilder.AlterColumn<bool>(
                name: "IsCompleted",
                table: "UserProgresses",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TargetId",
                table: "UserProgresses",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
