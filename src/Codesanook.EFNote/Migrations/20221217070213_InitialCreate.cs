using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Codesanook.EFNote.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "notebook",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "notebookHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "period_end")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "period_start"),
                    name = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "notebookHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "period_end")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "period_start"),
                    description = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "notebookHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "period_end")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "period_start"),
                    periodend = table.Column<DateTime>(name: "period_end", type: "datetime2", nullable: false)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "notebookHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "period_end")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "period_start"),
                    periodstart = table.Column<DateTime>(name: "period_start", type: "datetime2", nullable: false)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "notebookHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "period_end")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "period_start"),
                    Metadata = table.Column<string>(type: "nvarchar(max)", nullable: true)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "notebookHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "period_end")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "period_start")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_notebook", x => x.id);
                })
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "notebookHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "period_end")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "period_start");

            migrationBuilder.CreateTable(
                name: "tag",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tag", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "note",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    title = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    content = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    createdutc = table.Column<DateTime>(name: "created_utc", type: "datetime2", nullable: false),
                    updatedutc = table.Column<DateTime>(name: "updated_utc", type: "datetime2", nullable: true),
                    isdeleted = table.Column<bool>(name: "is_deleted", type: "bit", nullable: false),
                    viewcount = table.Column<int>(name: "view_count", type: "int", nullable: false),
                    notebookid = table.Column<int>(name: "notebook_id", type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_note", x => x.id);
                    table.ForeignKey(
                        name: "fk_note_notebook_notebook_id",
                        column: x => x.notebookid,
                        principalTable: "notebook",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "note_tag",
                columns: table => new
                {
                    notesid = table.Column<int>(name: "notes_id", type: "int", nullable: false),
                    tagsid = table.Column<int>(name: "tags_id", type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_note_tag", x => new { x.notesid, x.tagsid });
                    table.ForeignKey(
                        name: "fk_note_tag_notes_notes_id",
                        column: x => x.notesid,
                        principalTable: "note",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_note_tag_tags_tags_id",
                        column: x => x.tagsid,
                        principalTable: "tag",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_note_notebook_id",
                table: "note",
                column: "notebook_id");

            migrationBuilder.CreateIndex(
                name: "ix_note_tag_tags_id",
                table: "note_tag",
                column: "tags_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "note_tag");

            migrationBuilder.DropTable(
                name: "note");

            migrationBuilder.DropTable(
                name: "tag");

            migrationBuilder.DropTable(
                name: "notebook")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "notebookHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "period_end")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "period_start");
        }
    }
}
