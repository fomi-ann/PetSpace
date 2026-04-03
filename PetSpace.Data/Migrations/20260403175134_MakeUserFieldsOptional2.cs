using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetSpace.Data.Migrations
{
    /// <inheritdoc />
    public partial class MakeUserFieldsOptional2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_AppointmentStatusCodes_StatusAppStatusCodeId",
                table: "Appointments");

            migrationBuilder.AlterColumn<int>(
                name: "StatusAppStatusCodeId",
                table: "Appointments",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_AppointmentStatusCodes_StatusAppStatusCodeId",
                table: "Appointments",
                column: "StatusAppStatusCodeId",
                principalTable: "AppointmentStatusCodes",
                principalColumn: "AppStatusCodeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_AppointmentStatusCodes_StatusAppStatusCodeId",
                table: "Appointments");

            migrationBuilder.AlterColumn<int>(
                name: "StatusAppStatusCodeId",
                table: "Appointments",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_AppointmentStatusCodes_StatusAppStatusCodeId",
                table: "Appointments",
                column: "StatusAppStatusCodeId",
                principalTable: "AppointmentStatusCodes",
                principalColumn: "AppStatusCodeId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
