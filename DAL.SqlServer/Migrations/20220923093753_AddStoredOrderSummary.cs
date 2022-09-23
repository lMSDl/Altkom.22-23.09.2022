using DAL.SqlServer.Properties;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.SqlServer.Migrations
{
    public partial class AddStoredOrderSummary : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(Resources.OrderSummary);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE OrderSummary");
        }
    }
}
