using DAL.SqlServer.Properties;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.SqlServer.Migrations
{
    public partial class AddTriggerDeletedProducts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DeletedProducts",
                table: "Order",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.Sql(Resources.DeletedProductsTrigger);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedProducts",
                table: "Order");
            migrationBuilder.Sql("DROP TRIGGER PRODUCT_Delete");
        }
    }
}
