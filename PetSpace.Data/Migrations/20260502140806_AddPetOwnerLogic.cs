using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetSpace.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPetOwnerLogic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PetOwners_AspNetUsers_UserId",
                table: "PetOwners");

            migrationBuilder.AddForeignKey(
                name: "FK_PetOwners_AspNetUsers_UserId",
                table: "PetOwners",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PetOwners_AspNetUsers_UserId",
                table: "PetOwners");

            migrationBuilder.AddForeignKey(
                name: "FK_PetOwners_AspNetUsers_UserId",
                table: "PetOwners",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
