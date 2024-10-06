using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpaceDash.Migrations
{
    /// <inheritdoc />
    public partial class cat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Challenges_Devices_DeviceId",
                table: "Challenges");

            migrationBuilder.DropForeignKey(
                name: "FK_Challenges_GameSessions_GameSessionId",
                table: "Challenges");

            migrationBuilder.DropForeignKey(
                name: "FK_GameSessions_Challenges_CurrentChallengeId",
                table: "GameSessions");

            migrationBuilder.AlterColumn<int>(
                name: "CurrentChallengeId",
                table: "GameSessions",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "DeviceId",
                table: "Challenges",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Challenges_Devices_DeviceId",
                table: "Challenges",
                column: "DeviceId",
                principalTable: "Devices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Challenges_GameSessions_GameSessionId",
                table: "Challenges",
                column: "GameSessionId",
                principalTable: "GameSessions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GameSessions_Challenges_CurrentChallengeId",
                table: "GameSessions",
                column: "CurrentChallengeId",
                principalTable: "Challenges",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Challenges_Devices_DeviceId",
                table: "Challenges");

            migrationBuilder.DropForeignKey(
                name: "FK_Challenges_GameSessions_GameSessionId",
                table: "Challenges");

            migrationBuilder.DropForeignKey(
                name: "FK_GameSessions_Challenges_CurrentChallengeId",
                table: "GameSessions");

            migrationBuilder.AlterColumn<int>(
                name: "CurrentChallengeId",
                table: "GameSessions",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DeviceId",
                table: "Challenges",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_Challenges_Devices_DeviceId",
                table: "Challenges",
                column: "DeviceId",
                principalTable: "Devices",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Challenges_GameSessions_GameSessionId",
                table: "Challenges",
                column: "GameSessionId",
                principalTable: "GameSessions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GameSessions_Challenges_CurrentChallengeId",
                table: "GameSessions",
                column: "CurrentChallengeId",
                principalTable: "Challenges",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
