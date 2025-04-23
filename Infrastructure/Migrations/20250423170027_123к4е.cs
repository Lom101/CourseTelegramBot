using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class _123к4е : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AltText",
                table: "ContentItems",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AudioContent_Title",
                table: "ContentItems",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AudioUrl",
                table: "ContentItems",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "ContentItems",
                type: "character varying(21)",
                maxLength: 21,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "ContentItems",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FileUrl",
                table: "ContentItems",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageContent_Title",
                table: "ContentItems",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "ContentItems",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "ContentItems",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WordFileContent_FileName",
                table: "ContentItems",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WordFileContent_FileUrl",
                table: "ContentItems",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WordFileContent_Title",
                table: "ContentItems",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AltText",
                table: "ContentItems");

            migrationBuilder.DropColumn(
                name: "AudioContent_Title",
                table: "ContentItems");

            migrationBuilder.DropColumn(
                name: "AudioUrl",
                table: "ContentItems");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "ContentItems");

            migrationBuilder.DropColumn(
                name: "FileName",
                table: "ContentItems");

            migrationBuilder.DropColumn(
                name: "FileUrl",
                table: "ContentItems");

            migrationBuilder.DropColumn(
                name: "ImageContent_Title",
                table: "ContentItems");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "ContentItems");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "ContentItems");

            migrationBuilder.DropColumn(
                name: "WordFileContent_FileName",
                table: "ContentItems");

            migrationBuilder.DropColumn(
                name: "WordFileContent_FileUrl",
                table: "ContentItems");

            migrationBuilder.DropColumn(
                name: "WordFileContent_Title",
                table: "ContentItems");
        }
    }
}
