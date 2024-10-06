using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpaceDash.Migrations
{
    /// <inheritdoc />
    public partial class today : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "EndTime",
                table: "GameSessions",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Challenges",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Challenges",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Challenges_GameSessionId",
                table: "Challenges",
                column: "GameSessionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Challenges_GameSessions_GameSessionId",
                table: "Challenges",
                column: "GameSessionId",
                principalTable: "GameSessions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Challenges_GameSessions_GameSessionId",
                table: "Challenges");

            migrationBuilder.DropIndex(
                name: "IX_Challenges_GameSessionId",
                table: "Challenges");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Challenges");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Challenges");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndTime",
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
