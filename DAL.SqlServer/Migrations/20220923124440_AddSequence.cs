using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.SqlServer.Migrations
{
    public partial class AddSequence : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "sequences");

            migrationBuilder.CreateSequence<int>(
                name: "ProductPrice",
                schema: "sequences",
                startValue: 100L,
                incrementBy: 33,
                minValue: 30L,
                maxValue: 300L,
                cyclic: true);

            migrationBuilder.AlterColumn<float>(
                name: "Price",
                table: "Product",
                type: "real",
                nullable: false,
                defaultValueSql: "NEXT VALUE FOR sequences.ProductPrice",
                oldClrType: typeof(float),
                oldType: "real",
                oldDefaultValue: 0.5f);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropSequence(
                name: "ProductPrice",
                schema: "sequences");

            migrationBuilder.AlterColumn<float>(
                name: "Price",
                table: "Product",
                type: "real",
                nullable: false,
                defaultValue: 0.5f,
                oldClrType: typeof(float),
                oldType: "real",
                oldDefaultValueSql: "NEXT VALUE FOR sequences.ProductPrice");
        }
    }
}
