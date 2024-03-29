﻿using System;
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
                    name = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true)
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
                    period_end = table.Column<DateTime>(type: "datetime2", nullable: false)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "notebookHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "period_end")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "period_start"),
                    period_start = table.Column<DateTime>(type: "datetime2", nullable: false)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "notebookHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "period_end")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "period_start"),
                    Settings = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                    name = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true)
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
                    created_utc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    utc_updates = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    view_count = table.Column<int>(type: "int", nullable: false),
                    notebook_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_note", x => x.id);
                    table.ForeignKey(
                        name: "fk_note_notebook_notebook_id",
                        column: x => x.notebook_id,
                        principalTable: "notebook",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "note_tag",
                columns: table => new
                {
                    notes_id = table.Column<int>(type: "int", nullable: false),
                    tags_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_note_tag", x => new { x.notes_id, x.tags_id });
                    table.ForeignKey(
                        name: "fk_note_tag_notes_notes_id",
                        column: x => x.notes_id,
                        principalTable: "note",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_note_tag_tags_tags_id",
                        column: x => x.tags_id,
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
