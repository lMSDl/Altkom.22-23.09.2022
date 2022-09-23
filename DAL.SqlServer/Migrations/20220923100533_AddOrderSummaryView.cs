using DAL.SqlServer.Properties;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.SqlServer.Migrations
{
    public partial class AddOrderSummaryView : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(Resources.OrderSummaryView);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW View_OrderSummary");
        }
    }
}
