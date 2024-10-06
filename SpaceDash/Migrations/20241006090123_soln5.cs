using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpaceDash.Migrations
{
    /// <inheritdoc />
    public partial class soln5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SessionId",
                table: "GameSessions",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TimeTaken",
                table: "GameSessions",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SessionId",
                table: "GameSessions");

            migrationBuilder.DropColumn(
                name: "TimeTaken",
                table: "GameSessions");
        }
    }
}
