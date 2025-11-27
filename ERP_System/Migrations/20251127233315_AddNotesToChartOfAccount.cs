using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP_System.Migrations
{
    /// <inheritdoc />
    public partial class AddNotesToChartOfAccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "ChartOfAccounts",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Notes",
                table: "ChartOfAccounts");
        }
    }
}
