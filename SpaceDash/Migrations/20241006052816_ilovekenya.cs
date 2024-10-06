using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpaceDash.Migrations
{
    /// <inheritdoc />
    public partial class ilovekenya : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Challenges_Devices_DeviceId",
                table: "Challenges");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Challenges_Devices_DeviceId",
                table: "Challenges");

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
        }
    }
}
