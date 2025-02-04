using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdLerBackend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedBackpackSkinfromstringtointaddedOther : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Clear existing SkinColor data
            migrationBuilder.Sql("UPDATE Avatars SET SkinColor = NULL");

            migrationBuilder.AlterColumn<int>(
                    name: "SkinColor",
                    table: "Avatars",
                    type: "int",
                    nullable: true,
                    oldClrType: typeof(string),
                    oldType: "longtext",
                    oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                    name: "Backpack",
                    table: "Avatars",
                    type: "longtext",
                    nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                    name: "Other",
                    table: "Avatars",
                    type: "longtext",
                    nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Backpack",
                table: "Avatars");

            migrationBuilder.DropColumn(
                name: "Other",
                table: "Avatars");

            migrationBuilder.AlterColumn<string>(
                name: "SkinColor",
                table: "Avatars",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
