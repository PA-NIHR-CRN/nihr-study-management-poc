using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace NIHR.StudyManagement.Infrastructure.Migrations
{
    public partial class _02RDD619 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "griResearchStudyStatus",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    GriMappingId = table.Column<int>(type: "int", nullable: false),
                    code = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    FromDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ToDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    created = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_griResearchStudyStatus", x => x.id);
                    table.ForeignKey(
                        name: "fk_griResearchStudyStatus_griMapping",
                        column: x => x.GriMappingId,
                        principalTable: "griMapping",
                        principalColumn: "id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "personRole",
                keyColumn: "id",
                keyValue: 1,
                column: "created",
                value: new DateTime(2024, 3, 1, 11, 48, 42, 13, DateTimeKind.Local).AddTicks(6382));

            migrationBuilder.UpdateData(
                table: "personType",
                keyColumn: "id",
                keyValue: 1,
                column: "created",
                value: new DateTime(2024, 3, 1, 11, 48, 42, 13, DateTimeKind.Local).AddTicks(7023));

            migrationBuilder.UpdateData(
                table: "researchInitiativeIdentifierType",
                keyColumn: "id",
                keyValue: 1,
                column: "created",
                value: new DateTime(2024, 3, 1, 11, 48, 42, 14, DateTimeKind.Local).AddTicks(3080));

            migrationBuilder.UpdateData(
                table: "researchInitiativeIdentifierType",
                keyColumn: "id",
                keyValue: 2,
                column: "created",
                value: new DateTime(2024, 3, 1, 11, 48, 42, 14, DateTimeKind.Local).AddTicks(3094));

            migrationBuilder.UpdateData(
                table: "researchInitiativeType",
                keyColumn: "id",
                keyValue: 1,
                column: "created",
                value: new DateTime(2024, 3, 1, 11, 48, 42, 14, DateTimeKind.Local).AddTicks(3469));

            migrationBuilder.UpdateData(
                table: "sourceSystem",
                keyColumn: "id",
                keyValue: 1,
                column: "created",
                value: new DateTime(2024, 3, 1, 11, 48, 42, 14, DateTimeKind.Local).AddTicks(9837));

            migrationBuilder.UpdateData(
                table: "sourceSystem",
                keyColumn: "id",
                keyValue: 2,
                column: "created",
                value: new DateTime(2024, 3, 1, 11, 48, 42, 14, DateTimeKind.Local).AddTicks(9850));

            migrationBuilder.CreateIndex(
                name: "IX_griResearchStudyStatus_GriMappingId",
                table: "griResearchStudyStatus",
                column: "GriMappingId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "griResearchStudyStatus");

            migrationBuilder.UpdateData(
                table: "personRole",
                keyColumn: "id",
                keyValue: 1,
                column: "created",
                value: new DateTime(2024, 2, 20, 10, 33, 23, 980, DateTimeKind.Local).AddTicks(5038));

            migrationBuilder.UpdateData(
                table: "personType",
                keyColumn: "id",
                keyValue: 1,
                column: "created",
                value: new DateTime(2024, 2, 20, 10, 33, 23, 980, DateTimeKind.Local).AddTicks(5695));

            migrationBuilder.UpdateData(
                table: "researchInitiativeIdentifierType",
                keyColumn: "id",
                keyValue: 1,
                column: "created",
                value: new DateTime(2024, 2, 20, 10, 33, 23, 981, DateTimeKind.Local).AddTicks(5896));

            migrationBuilder.UpdateData(
                table: "researchInitiativeIdentifierType",
                keyColumn: "id",
                keyValue: 2,
                column: "created",
                value: new DateTime(2024, 2, 20, 10, 33, 23, 981, DateTimeKind.Local).AddTicks(5922));

            migrationBuilder.UpdateData(
                table: "researchInitiativeType",
                keyColumn: "id",
                keyValue: 1,
                column: "created",
                value: new DateTime(2024, 2, 20, 10, 33, 23, 981, DateTimeKind.Local).AddTicks(6579));

            migrationBuilder.UpdateData(
                table: "sourceSystem",
                keyColumn: "id",
                keyValue: 1,
                column: "created",
                value: new DateTime(2024, 2, 20, 10, 33, 23, 982, DateTimeKind.Local).AddTicks(8192));

            migrationBuilder.UpdateData(
                table: "sourceSystem",
                keyColumn: "id",
                keyValue: 2,
                column: "created",
                value: new DateTime(2024, 2, 20, 10, 33, 23, 982, DateTimeKind.Local).AddTicks(8221));
        }
    }
}
