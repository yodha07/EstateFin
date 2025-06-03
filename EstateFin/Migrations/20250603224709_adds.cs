using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EstateFin.Migrations
{
    /// <inheritdoc />
    public partial class adds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointment_Properties_PropertyId",
                table: "Appointment");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointment_Users_UserID",
                table: "Appointment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Appointment",
                table: "Appointment");

            migrationBuilder.RenameTable(
                name: "Appointment",
                newName: "appointment");

            migrationBuilder.RenameIndex(
                name: "IX_Appointment_UserID",
                table: "appointment",
                newName: "IX_appointment_UserID");

            migrationBuilder.RenameIndex(
                name: "IX_Appointment_PropertyId",
                table: "appointment",
                newName: "IX_appointment_PropertyId");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "appointment",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_appointment",
                table: "appointment",
                column: "AppointmentId");

            migrationBuilder.CreateTable(
                name: "slot",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Slottype = table.Column<string>(name: "Slot_type", type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_slot", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_appointment_Properties_PropertyId",
                table: "appointment",
                column: "PropertyId",
                principalTable: "Properties",
                principalColumn: "PropertyId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_appointment_Users_UserID",
                table: "appointment",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_appointment_Properties_PropertyId",
                table: "appointment");

            migrationBuilder.DropForeignKey(
                name: "FK_appointment_Users_UserID",
                table: "appointment");

            migrationBuilder.DropTable(
                name: "slot");

            migrationBuilder.DropPrimaryKey(
                name: "PK_appointment",
                table: "appointment");

            migrationBuilder.RenameTable(
                name: "appointment",
                newName: "Appointment");

            migrationBuilder.RenameIndex(
                name: "IX_appointment_UserID",
                table: "Appointment",
                newName: "IX_Appointment_UserID");

            migrationBuilder.RenameIndex(
                name: "IX_appointment_PropertyId",
                table: "Appointment",
                newName: "IX_Appointment_PropertyId");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Appointment",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Appointment",
                table: "Appointment",
                column: "AppointmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointment_Properties_PropertyId",
                table: "Appointment",
                column: "PropertyId",
                principalTable: "Properties",
                principalColumn: "PropertyId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointment_Users_UserID",
                table: "Appointment",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
