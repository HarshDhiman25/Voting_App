using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Voting_Test.Migrations
{
    /// <inheritdoc />
    public partial class pollwinner : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PollWinnerss",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PollId = table.Column<int>(type: "int", nullable: false),
                    PollQuestion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PollingRoomName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalVotes = table.Column<int>(type: "int", nullable: false),
                    WinnerMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WinningDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PollWinnerss", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PollWinnerss");
        }
    }
}
