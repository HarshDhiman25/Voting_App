using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Voting_Test.Data.Migrations
{
    /// <inheritdoc />
    public partial class pollingadd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Polls_PollingRooms_PollingRoomId",
                table: "Polls");

            migrationBuilder.AlterColumn<int>(
                name: "PollingRoomId",
                table: "Polls",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "PollingRooms",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Polls_PollingRooms_PollingRoomId",
                table: "Polls",
                column: "PollingRoomId",
                principalTable: "PollingRooms",
                principalColumn: "PollingRoomId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Polls_PollingRooms_PollingRoomId",
                table: "Polls");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "PollingRooms");

            migrationBuilder.AlterColumn<int>(
                name: "PollingRoomId",
                table: "Polls",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Polls_PollingRooms_PollingRoomId",
                table: "Polls",
                column: "PollingRoomId",
                principalTable: "PollingRooms",
                principalColumn: "PollingRoomId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
