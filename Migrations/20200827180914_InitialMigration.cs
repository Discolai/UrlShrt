using Microsoft.EntityFrameworkCore.Migrations;

namespace UrlShrt.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UrlItems",
                columns: table => new
                {
                    Slug = table.Column<string>(nullable: false),
                    RedirectUrl = table.Column<string>(maxLength: 512, nullable: false),
                    Clicks = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UrlItems", x => x.Slug);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UrlItems");
        }
    }
}
