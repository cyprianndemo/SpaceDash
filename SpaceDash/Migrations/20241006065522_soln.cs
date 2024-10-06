using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpaceDash.Migrations
{
    /// <inheritdoc />
    public partial class soln : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "StartTime",
                table: "GameSessions",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddColumn<string>(
                name: "Solution",
                table: "Challenges",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SudokuPuzzle",
                table: "Challenges",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Solution",
                table: "Challenges");

            migrationBuilder.DropColumn(
                name: "SudokuPuzzle",
                table: "Challenges");

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartTime",
                table: "GameSessions",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);
        }
    }
}
