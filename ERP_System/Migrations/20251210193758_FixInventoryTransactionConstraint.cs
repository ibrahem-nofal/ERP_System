using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP_System.Migrations
{
    /// <inheritdoc />
    public partial class FixInventoryTransactionConstraint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_InventoryTransaction_TransactionType",
                table: "InventoryTransactions");

            migrationBuilder.AddCheckConstraint(
                name: "CK_InventoryTransaction_TransactionType",
                table: "InventoryTransactions",
                sql: "[TransactionType] IN ('Purchase', 'PurchaseReturn', 'Sale', 'SaleReturn', 'Adjustment')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_InventoryTransaction_TransactionType",
                table: "InventoryTransactions");

            migrationBuilder.AddCheckConstraint(
                name: "CK_InventoryTransaction_TransactionType",
                table: "InventoryTransactions",
                sql: "[TransactionType] IN ('Purchase', 'PurchaseReturn', 'Sales', 'SalesReturn', 'Adjustment')");
        }
    }
}
