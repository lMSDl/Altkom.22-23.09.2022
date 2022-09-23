using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.SqlServer.Migrations
{
    public partial class EditPersonToTeporal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterTable(
                name: "Person")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "PersonHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "End")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "From");

            migrationBuilder.AddColumn<DateTime>(
                name: "End",
                table: "Person",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified))
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "End")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "From");

            migrationBuilder.AddColumn<DateTime>(
                name: "From",
                table: "Person",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified))
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "End")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "From");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "End",
                table: "Person")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "PersonHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "End")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "From");

            migrationBuilder.DropColumn(
                name: "From",
                table: "Person")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "PersonHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "End")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "From");

            migrationBuilder.AlterTable(
                name: "Person")
                .OldAnnotation("SqlServer:IsTemporal", true)
                .OldAnnotation("SqlServer:TemporalHistoryTableName", "PersonHistory")
                .OldAnnotation("SqlServer:TemporalHistoryTableSchema", null)
                .OldAnnotation("SqlServer:TemporalPeriodEndColumnName", "End")
                .OldAnnotation("SqlServer:TemporalPeriodStartColumnName", "From");
        }
    }
}
