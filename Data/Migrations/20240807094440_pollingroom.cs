using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Voting_Test.Data.Migrations
{
    /// <inheritdoc />
    public partial class pollingroom : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Votes_Options_OptionId",
                table: "Votes");

            migrationBuilder.DropTable(
                name: "Options");

            migrationBuilder.RenameColumn(
                name: "OptionId",
                table: "Votes",
                newName: "PollingRoomId");

            migrationBuilder.RenameIndex(
                name: "IX_Votes_OptionId",
                table: "Votes",
                newName: "IX_Votes_PollingRoomId");

            migrationBuilder.AddColumn<DateTime>(
                name: "VoteDate",
                table: "Votes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "PollingRoomId",
                table: "Polls",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PollingRooms",
                columns: table => new
                {
                    PollingRoomId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PollingRooms", x => x.PollingRoomId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Polls_PollingRoomId",
                table: "Polls",
                column: "PollingRoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_Polls_PollingRooms_PollingRoomId",
                table: "Polls",
                column: "PollingRoomId",
                principalTable: "PollingRooms",
                principalColumn: "PollingRoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_Votes_PollingRooms_PollingRoomId",
                table: "Votes",
                column: "PollingRoomId",
                principalTable: "PollingRooms",
                principalColumn: "PollingRoomId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Polls_PollingRooms_PollingRoomId",
                table: "Polls");

            migrationBuilder.DropForeignKey(
                name: "FK_Votes_PollingRooms_PollingRoomId",
                table: "Votes");

            migrationBuilder.DropTable(
                name: "PollingRooms");

            migrationBuilder.DropIndex(
                name: "IX_Polls_PollingRoomId",
                table: "Polls");

            migrationBuilder.DropColumn(
                name: "VoteDate",
                table: "Votes");

            migrationBuilder.DropColumn(
                name: "PollingRoomId",
                table: "Polls");

            migrationBuilder.RenameColumn(
                name: "PollingRoomId",
                table: "Votes",
                newName: "OptionId");

            migrationBuilder.RenameIndex(
                name: "IX_Votes_PollingRoomId",
                table: "Votes",
                newName: "IX_Votes_OptionId");

            migrationBuilder.CreateTable(
                name: "Options",
                columns: table => new
                {
                    OptionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PollId = table.Column<int>(type: "int", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Options", x => x.OptionId);
                    table.ForeignKey(
                        name: "FK_Options_Polls_PollId",
                        column: x => x.PollId,
                        principalTable: "Polls",
                        principalColumn: "PollId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Options_PollId",
                table: "Options",
                column: "PollId");

            migrationBuilder.AddForeignKey(
                name: "FK_Votes_Options_OptionId",
                table: "Votes",
                column: "OptionId",
                principalTable: "Options",
                principalColumn: "OptionId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
