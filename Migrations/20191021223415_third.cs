using Microsoft.EntityFrameworkCore.Migrations;

namespace MySQLIdentity.Migrations
{
    public partial class third : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Locations_LocationID",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_LocationID",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "LocationID",
                table: "Reservations");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LocationID",
                table: "Reservations",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_LocationID",
                table: "Reservations",
                column: "LocationID");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Locations_LocationID",
                table: "Reservations",
                column: "LocationID",
                principalTable: "Locations",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
