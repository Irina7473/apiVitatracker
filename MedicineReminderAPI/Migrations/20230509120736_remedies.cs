using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedicineReminderAPI.Migrations
{
    /// <inheritdoc />
    public partial class remedies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Remedys_RemedyId",
                table: "Courses");

            migrationBuilder.DropForeignKey(
                name: "FK_HistoryRemedys_Remedys_RemedyId",
                table: "HistoryRemedys");

            migrationBuilder.DropForeignKey(
                name: "FK_Remedys_Users_UserId",
                table: "Remedys");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Remedys",
                table: "Remedys");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HistoryRemedys",
                table: "HistoryRemedys");

            migrationBuilder.RenameTable(
                name: "Remedys",
                newName: "Remedies");

            migrationBuilder.RenameTable(
                name: "HistoryRemedys",
                newName: "HistoryRemedies");

            migrationBuilder.RenameIndex(
                name: "IX_Remedys_UserId",
                table: "Remedies",
                newName: "IX_Remedies_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_HistoryRemedys_RemedyId",
                table: "HistoryRemedies",
                newName: "IX_HistoryRemedies_RemedyId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Remedies",
                table: "Remedies",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HistoryRemedies",
                table: "HistoryRemedies",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Remedies_RemedyId",
                table: "Courses",
                column: "RemedyId",
                principalTable: "Remedies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HistoryRemedies_Remedies_RemedyId",
                table: "HistoryRemedies",
                column: "RemedyId",
                principalTable: "Remedies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Remedies_Users_UserId",
                table: "Remedies",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Remedies_RemedyId",
                table: "Courses");

            migrationBuilder.DropForeignKey(
                name: "FK_HistoryRemedies_Remedies_RemedyId",
                table: "HistoryRemedies");

            migrationBuilder.DropForeignKey(
                name: "FK_Remedies_Users_UserId",
                table: "Remedies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Remedies",
                table: "Remedies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HistoryRemedies",
                table: "HistoryRemedies");

            migrationBuilder.RenameTable(
                name: "Remedies",
                newName: "Remedys");

            migrationBuilder.RenameTable(
                name: "HistoryRemedies",
                newName: "HistoryRemedys");

            migrationBuilder.RenameIndex(
                name: "IX_Remedies_UserId",
                table: "Remedys",
                newName: "IX_Remedys_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_HistoryRemedies_RemedyId",
                table: "HistoryRemedys",
                newName: "IX_HistoryRemedys_RemedyId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Remedys",
                table: "Remedys",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HistoryRemedys",
                table: "HistoryRemedys",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Remedys_RemedyId",
                table: "Courses",
                column: "RemedyId",
                principalTable: "Remedys",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HistoryRemedys_Remedys_RemedyId",
                table: "HistoryRemedys",
                column: "RemedyId",
                principalTable: "Remedys",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Remedys_Users_UserId",
                table: "Remedys",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
