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
            migrationBuilder.AddColumn<int>(
                name: "userid",
                table: "Properties",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Properties_userid",
                table: "Properties",
                column: "userid");

            migrationBuilder.AddForeignKey(
                name: "FK_Properties_Users_userid",
                table: "Properties",
                column: "userid",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Properties_Users_userid",
                table: "Properties");

            migrationBuilder.DropIndex(
                name: "IX_Properties_userid",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "userid",
                table: "Properties");
        }
    }
}
