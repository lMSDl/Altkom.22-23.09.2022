using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.SqlServer.Migrations
{
    public partial class AddSparse : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SomeValue",
                table: "Person",
                type: "int",
                nullable: true)
                .Annotation("SqlServer:Sparse", true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SomeValue",
                table: "Person")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "PersonHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null);
        }
    }
}
