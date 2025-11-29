using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP_System.Migrations
{
    /// <inheritdoc />
    public partial class AddInvoiceTransfer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InvoiceTransferHeaders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FromStoreId = table.Column<int>(type: "int", nullable: false),
                    ToStoreId = table.Column<int>(type: "int", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AssignedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceTransferHeaders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvoiceTransferHeaders_Employees_AssignedBy",
                        column: x => x.AssignedBy,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InvoiceTransferHeaders_Stores_FromStoreId",
                        column: x => x.FromStoreId,
                        principalTable: "Stores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InvoiceTransferHeaders_Stores_ToStoreId",
                        column: x => x.ToStoreId,
                        principalTable: "Stores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InvoiceTransferDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HeaderId = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceTransferDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvoiceTransferDetails_InvoiceTransferHeaders_HeaderId",
                        column: x => x.HeaderId,
                        principalTable: "InvoiceTransferHeaders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InvoiceTransferDetails_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceTransferDetails_HeaderId",
                table: "InvoiceTransferDetails",
                column: "HeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceTransferDetails_ItemId",
                table: "InvoiceTransferDetails",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceTransferHeaders_AssignedBy",
                table: "InvoiceTransferHeaders",
                column: "AssignedBy");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceTransferHeaders_FromStoreId",
                table: "InvoiceTransferHeaders",
                column: "FromStoreId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceTransferHeaders_ToStoreId",
                table: "InvoiceTransferHeaders",
                column: "ToStoreId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InvoiceTransferDetails");

            migrationBuilder.DropTable(
                name: "InvoiceTransferHeaders");
        }
    }
}
