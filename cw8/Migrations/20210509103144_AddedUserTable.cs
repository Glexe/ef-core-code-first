using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace cw8.Migrations
{
    public partial class AddedUserTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Prescription_Prescription_Medicament_IdPrescription_MedicamentNavigationIdMedicament_IdPrescription_MedicamentNavigationIdPr~",
                table: "Prescription");

            migrationBuilder.DropIndex(
                name: "IX_Prescription_IdPrescription_MedicamentNavigationIdMedicament_IdPrescription_MedicamentNavigationIdPrescription",
                table: "Prescription");

            migrationBuilder.DropColumn(
                name: "IdPrescription_MedicamentNavigationIdMedicament",
                table: "Prescription");

            migrationBuilder.DropColumn(
                name: "IdPrescription_MedicamentNavigationIdPrescription",
                table: "Prescription");

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Login = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PasswordHashed = table.Column<byte[]>(type: "varbinary(200)", maxLength: 200, nullable: false),
                    Salt = table.Column<byte[]>(type: "varbinary(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("User_PK", x => x.UserID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.AddColumn<int>(
                name: "IdPrescription_MedicamentNavigationIdMedicament",
                table: "Prescription",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IdPrescription_MedicamentNavigationIdPrescription",
                table: "Prescription",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Prescription_IdPrescription_MedicamentNavigationIdMedicament_IdPrescription_MedicamentNavigationIdPrescription",
                table: "Prescription",
                columns: new[] { "IdPrescription_MedicamentNavigationIdMedicament", "IdPrescription_MedicamentNavigationIdPrescription" });

            migrationBuilder.AddForeignKey(
                name: "FK_Prescription_Prescription_Medicament_IdPrescription_MedicamentNavigationIdMedicament_IdPrescription_MedicamentNavigationIdPr~",
                table: "Prescription",
                columns: new[] { "IdPrescription_MedicamentNavigationIdMedicament", "IdPrescription_MedicamentNavigationIdPrescription" },
                principalTable: "Prescription_Medicament",
                principalColumns: new[] { "IdMedicament", "IdPrescription" },
                onDelete: ReferentialAction.Restrict);
        }
    }
}
