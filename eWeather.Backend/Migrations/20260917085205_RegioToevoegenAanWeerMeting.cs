using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eWeather.Backend.Migrations
{
    /// <inheritdoc />
    public partial class RegioToevoegenAanWeerMeting : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Regio",
                table: "WeerMetingen",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Regio",
                table: "WeerMetingen");
        }
    }
}
