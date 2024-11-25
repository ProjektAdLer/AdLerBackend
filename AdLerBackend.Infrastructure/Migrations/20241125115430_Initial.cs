using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdLerBackend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PlayerData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    PlayerGender = table.Column<int>(type: "INTEGER", nullable: false),
                    PlayerWorldColor = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerData", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Worlds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    LmsWorldId = table.Column<int>(type: "INTEGER", nullable: false),
                    AuthorId = table.Column<int>(type: "INTEGER", nullable: false),
                    AtfJson = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Worlds", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "H5PLocationEntity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Path = table.Column<string>(type: "TEXT", nullable: false),
                    ElementId = table.Column<int>(type: "INTEGER", nullable: true),
                    WorldEntityId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_H5PLocationEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_H5PLocationEntity_Worlds_WorldEntityId",
                        column: x => x.WorldEntityId,
                        principalTable: "Worlds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_H5PLocationEntity_WorldEntityId",
                table: "H5PLocationEntity",
                column: "WorldEntityId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "H5PLocationEntity");

            migrationBuilder.DropTable(
                name: "PlayerData");

            migrationBuilder.DropTable(
                name: "Worlds");
        }
    }
}
