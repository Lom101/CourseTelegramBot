using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class _3456 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tests_Topics_TopicId",
                table: "Tests");

            migrationBuilder.DropIndex(
                name: "IX_Tests_TopicId",
                table: "Tests");

            migrationBuilder.DropColumn(
                name: "TopicId",
                table: "Tests");

            migrationBuilder.DropColumn(
                name: "Options",
                table: "TestQuestions");

            migrationBuilder.CreateTable(
                name: "TestOption",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TestQuestionId = table.Column<int>(type: "integer", nullable: false),
                    OptionText = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestOption", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TestOption_TestQuestions_TestQuestionId",
                        column: x => x.TestQuestionId,
                        principalTable: "TestQuestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TestOption_TestQuestionId",
                table: "TestOption",
                column: "TestQuestionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TestOption");

            migrationBuilder.AddColumn<int>(
                name: "TopicId",
                table: "Tests",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<List<string>>(
                name: "Options",
                table: "TestQuestions",
                type: "text[]",
                nullable: false);

            migrationBuilder.CreateIndex(
                name: "IX_Tests_TopicId",
                table: "Tests",
                column: "TopicId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tests_Topics_TopicId",
                table: "Tests",
                column: "TopicId",
                principalTable: "Topics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
