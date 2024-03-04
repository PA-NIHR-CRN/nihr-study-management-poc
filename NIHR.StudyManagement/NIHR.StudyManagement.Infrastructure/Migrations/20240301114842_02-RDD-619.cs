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

            migrationBuilder.CreateIndex(
                name: "IX_griResearchStudyStatus_GriMappingId",
                table: "griResearchStudyStatus",
                column: "GriMappingId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "griResearchStudyStatus");
        }
    }
}
