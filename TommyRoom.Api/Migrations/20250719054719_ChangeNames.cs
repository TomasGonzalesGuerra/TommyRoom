using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TommyRoom.Api.Migrations
{
    /// <inheritdoc />
    public partial class ChangeNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_AspNetUsers_UserId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_AspNetUsers_UserId",
                table: "Rooms");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Rooms",
                newName: "OwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_Rooms_UserId",
                table: "Rooms",
                newName: "IX_Rooms_OwnerId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Bookings",
                newName: "GuestId");

            migrationBuilder.RenameIndex(
                name: "IX_Bookings_UserId",
                table: "Bookings",
                newName: "IX_Bookings_GuestId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_AspNetUsers_GuestId",
                table: "Bookings",
                column: "GuestId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_AspNetUsers_OwnerId",
                table: "Rooms",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_AspNetUsers_GuestId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_AspNetUsers_OwnerId",
                table: "Rooms");

            migrationBuilder.RenameColumn(
                name: "OwnerId",
                table: "Rooms",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Rooms_OwnerId",
                table: "Rooms",
                newName: "IX_Rooms_UserId");

            migrationBuilder.RenameColumn(
                name: "GuestId",
                table: "Bookings",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Bookings_GuestId",
                table: "Bookings",
                newName: "IX_Bookings_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_AspNetUsers_UserId",
                table: "Bookings",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_AspNetUsers_UserId",
                table: "Rooms",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
