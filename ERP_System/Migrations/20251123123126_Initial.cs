using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP_System.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false),
                    Detail = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ChartOfAccounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ParentCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AccountType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsLeaf = table.Column<bool>(type: "bit", nullable: false),
                    NormalBalance = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChartOfAccounts", x => x.Id);
                    table.UniqueConstraint("AK_ChartOfAccounts_Code", x => x.Code);
                    table.CheckConstraint("CK_ChartOfAccount_AccountType", "[AccountType] IN ('Asset', 'Liability', 'Equity', 'Revenue', 'Expense')");
                    table.CheckConstraint("CK_ChartOfAccount_Level", "[Level] BETWEEN 1 AND 5");
                    table.CheckConstraint("CK_ChartOfAccount_NormalBalance", "[NormalBalance] IN ('Debit', 'Credit')");
                    table.ForeignKey(
                        name: "FK_ChartOfAccounts_ChartOfAccounts_ParentCode",
                        column: x => x.ParentCode,
                        principalTable: "ChartOfAccounts",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OtherDetails = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OtherDetails = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    RoleType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    IdNumber = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Qualification = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    State = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                    table.CheckConstraint("CK_Employee_RoleType", "[RoleType] IN ('delegate', 'emp')");
                });

            migrationBuilder.CreateTable(
                name: "ExpenseNames",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpenseNames", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RevenueNames",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RevenueNames", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Suppliers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    OwnerName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    OwnerPhone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ManagerName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ManagerPhone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suppliers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Units",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Details = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Units", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CompanyPhones",
                columns: table => new
                {
                    CompId = table.Column<int>(type: "int", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyPhones", x => new { x.CompId, x.Phone });
                    table.ForeignKey(
                        name: "FK_CompanyPhones_Companies_CompId",
                        column: x => x.CompId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CustomerPhones",
                columns: table => new
                {
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerPhones", x => new { x.CustomerId, x.Phone });
                    table.ForeignKey(
                        name: "FK_CustomerPhones_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "delegate",
                columns: table => new
                {
                    EmpId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_delegate", x => x.EmpId);
                    table.ForeignKey(
                        name: "FK_delegate_Employees_EmpId",
                        column: x => x.EmpId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmpImages",
                columns: table => new
                {
                    EmpId = table.Column<int>(type: "int", nullable: false),
                    EmpImageData = table.Column<byte[]>(type: "varbinary(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmpImages", x => x.EmpId);
                    table.ForeignKey(
                        name: "FK_EmpImages_Employees_EmpId",
                        column: x => x.EmpId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmpPhones",
                columns: table => new
                {
                    EmpId = table.Column<int>(type: "int", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmpPhones", x => new { x.EmpId, x.Phone });
                    table.ForeignKey(
                        name: "FK_EmpPhones_Employees_EmpId",
                        column: x => x.EmpId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Logins",
                columns: table => new
                {
                    Username = table.Column<string>(type: "nvarchar(35)", maxLength: 35, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(35)", maxLength: 35, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    EmpId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logins", x => x.Username);
                    table.ForeignKey(
                        name: "FK_Logins_Employees_EmpId",
                        column: x => x.EmpId,
                        principalTable: "Employees",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Stores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    OtherDetails = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StoreManager = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Stores_Employees_StoreManager",
                        column: x => x.StoreManager,
                        principalTable: "Employees",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SupplierPhones",
                columns: table => new
                {
                    SupplierId = table.Column<int>(type: "int", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupplierPhones", x => new { x.SupplierId, x.Phone });
                    table.ForeignKey(
                        name: "FK_SupplierPhones_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ActivityLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(35)", maxLength: 35, nullable: false),
                    ActDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ActTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    FormName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    OpName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CmpName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ActivityData = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActivityLogs_Logins_Username",
                        column: x => x.Username,
                        principalTable: "Logins",
                        principalColumn: "Username",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InvoicePurchaseHeaders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RefInvId = table.Column<int>(type: "int", nullable: true),
                    InvType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SupplierId = table.Column<int>(type: "int", nullable: true),
                    StoreId = table.Column<int>(type: "int", nullable: true),
                    OrderStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeliveryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AssignedBy = table.Column<int>(type: "int", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Discount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NetAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false, computedColumnSql: "[totalAmount] - [discount]", stored: true),
                    Paid = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Remain = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PayStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsPostpaid = table.Column<bool>(type: "bit", nullable: false),
                    PaymentDueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AssignedByEmployeeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoicePurchaseHeaders", x => x.Id);
                    table.CheckConstraint("CK_InvoicePurchaseHeader_InvType", "[InvType] IN ('PurchaseCash', 'PurchaseCredit', 'PurchaseReturn')");
                    table.CheckConstraint("CK_InvoicePurchaseHeader_OrderStatus", "[OrderStatus] IN ('wait', 'recieved', 'returned', 'returning')");
                    table.CheckConstraint("CK_InvoicePurchaseHeader_PayStatus", "[PayStatus] IN ('open', 'closed')");
                    table.ForeignKey(
                        name: "FK_InvoicePurchaseHeaders_Employees_AssignedByEmployeeId",
                        column: x => x.AssignedByEmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InvoicePurchaseHeaders_InvoicePurchaseHeaders_RefInvId",
                        column: x => x.RefInvId,
                        principalTable: "InvoicePurchaseHeaders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InvoicePurchaseHeaders_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_InvoicePurchaseHeaders_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "InvoiceSaleHeaders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RefInvId = table.Column<int>(type: "int", nullable: true),
                    InvType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CustomerId = table.Column<int>(type: "int", nullable: true),
                    DelegateId = table.Column<int>(type: "int", nullable: true),
                    StoreId = table.Column<int>(type: "int", nullable: true),
                    OrderStatus = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    DeliveryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AssignedBy = table.Column<int>(type: "int", nullable: true),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Discount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NetAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false, computedColumnSql: "[totalAmount] - [discount]", stored: true),
                    Paid = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Remain = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PayStatus = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IsPostpaid = table.Column<bool>(type: "bit", nullable: false),
                    PaymentDueDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceSaleHeaders", x => x.Id);
                    table.CheckConstraint("CK_InvoiceSaleHeader_InvType", "[InvType] IN ('SalesCash', 'SalesReturnCash', 'SalesCredit', 'SalesReturnCredit')");
                    table.CheckConstraint("CK_InvoiceSaleHeader_OrderStatus", "[OrderStatus] IN ('wait', 'sent', 'returned', 'returning')");
                    table.CheckConstraint("CK_InvoiceSaleHeader_PayStatus", "[PayStatus] IN ('open', 'closed')");
                    table.ForeignKey(
                        name: "FK_InvoiceSaleHeaders_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InvoiceSaleHeaders_Employees_AssignedBy",
                        column: x => x.AssignedBy,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InvoiceSaleHeaders_InvoiceSaleHeaders_RefInvId",
                        column: x => x.RefInvId,
                        principalTable: "InvoiceSaleHeaders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InvoiceSaleHeaders_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InvoiceSaleHeaders_delegate_DelegateId",
                        column: x => x.DelegateId,
                        principalTable: "delegate",
                        principalColumn: "EmpId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    IsActiveBuy = table.Column<bool>(type: "bit", nullable: true),
                    IsActiveSale = table.Column<bool>(type: "bit", nullable: true),
                    CompanyMade = table.Column<int>(type: "int", nullable: true),
                    DefaultStore = table.Column<int>(type: "int", nullable: true),
                    UnitNumber = table.Column<int>(type: "int", nullable: true),
                    MinimumQuantity = table.Column<int>(type: "int", nullable: true),
                    MinQuantitySale = table.Column<int>(type: "int", nullable: true),
                    PreventFraction = table.Column<bool>(type: "bit", nullable: false),
                    PreventDiscount = table.Column<bool>(type: "bit", nullable: false),
                    BuyPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SalePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Items_Companies_CompanyMade",
                        column: x => x.CompanyMade,
                        principalTable: "Companies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Items_Stores_DefaultStore",
                        column: x => x.DefaultStore,
                        principalTable: "Stores",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Items_Units_UnitNumber",
                        column: x => x.UnitNumber,
                        principalTable: "Units",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PurchasePayments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvoiceReturnId = table.Column<int>(type: "int", nullable: true),
                    InvPurchaseId = table.Column<int>(type: "int", nullable: false),
                    AmountPaid = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaymentMethod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Out = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchasePayments", x => x.Id);
                    table.CheckConstraint("CK_PurchasePayment_PaymentMethod", "[PaymentMethod] IN ('cash', 'visa', 'vCash', 'insta')");
                    table.ForeignKey(
                        name: "FK_PurchasePayments_InvoicePurchaseHeaders_InvPurchaseId",
                        column: x => x.InvPurchaseId,
                        principalTable: "InvoicePurchaseHeaders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PurchasePayments_InvoicePurchaseHeaders_InvoiceReturnId",
                        column: x => x.InvoiceReturnId,
                        principalTable: "InvoicePurchaseHeaders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "JournalEntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SourceType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InvPurId = table.Column<int>(type: "int", nullable: true),
                    InvSaleId = table.Column<int>(type: "int", nullable: true),
                    AssignedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JournalEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JournalEntries_Employees_AssignedBy",
                        column: x => x.AssignedBy,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_JournalEntries_InvoicePurchaseHeaders_InvPurId",
                        column: x => x.InvPurId,
                        principalTable: "InvoicePurchaseHeaders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_JournalEntries_InvoiceSaleHeaders_InvSaleId",
                        column: x => x.InvSaleId,
                        principalTable: "InvoiceSaleHeaders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SalePayments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvoiceReturnId = table.Column<int>(type: "int", nullable: true),
                    InvSaleId = table.Column<int>(type: "int", nullable: false),
                    AmountPaid = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaymentMethod = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    In = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalePayments", x => x.Id);
                    table.CheckConstraint("CK_SalePayment_PaymentMethod", "[PaymentMethod] IN ('cash', 'visa', 'vCash', 'insta')");
                    table.ForeignKey(
                        name: "FK_SalePayments_InvoiceSaleHeaders_InvSaleId",
                        column: x => x.InvSaleId,
                        principalTable: "InvoiceSaleHeaders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SalePayments_InvoiceSaleHeaders_InvoiceReturnId",
                        column: x => x.InvoiceReturnId,
                        principalTable: "InvoiceSaleHeaders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CustomerItems",
                columns: table => new
                {
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerItems", x => new { x.CustomerId, x.ItemId });
                    table.ForeignKey(
                        name: "FK_CustomerItems_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomerItems_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Inventories",
                columns: table => new
                {
                    StoreId = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    CurrentQuantity = table.Column<int>(type: "int", nullable: false),
                    OrderedQuantity = table.Column<int>(type: "int", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inventories", x => new { x.StoreId, x.ItemId });
                    table.ForeignKey(
                        name: "FK_Inventories_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Inventories_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InventoryTransactions",
                columns: table => new
                {
                    TransactionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    TransactionType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    StoreId = table.Column<int>(type: "int", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReferenceId = table.Column<int>(type: "int", nullable: true),
                    AssignedBy = table.Column<int>(type: "int", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryTransactions", x => x.TransactionId);
                    table.CheckConstraint("CK_InventoryTransaction_TransactionType", "[TransactionType] IN ('Purchase', 'PurchaseReturn', 'Sales', 'SalesReturn', 'Adjustment')");
                    table.ForeignKey(
                        name: "FK_InventoryTransactions_Employees_AssignedBy",
                        column: x => x.AssignedBy,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InventoryTransactions_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InventoryTransactions_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InvoicePurchaseDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvoiceId = table.Column<int>(type: "int", nullable: false),
                    InvReturnId = table.Column<int>(type: "int", nullable: true),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false, computedColumnSql: "[quantity] * [unitPrice]", stored: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoicePurchaseDetails", x => x.Id);
                    table.CheckConstraint("CK_InvoicePurchaseDetail_Status", "[Status] IN ('Purchased', 'Returned')");
                    table.ForeignKey(
                        name: "FK_InvoicePurchaseDetails_InvoicePurchaseHeaders_InvReturnId",
                        column: x => x.InvReturnId,
                        principalTable: "InvoicePurchaseHeaders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InvoicePurchaseDetails_InvoicePurchaseHeaders_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "InvoicePurchaseHeaders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InvoicePurchaseDetails_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InvoiceSaleDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvoiceId = table.Column<int>(type: "int", nullable: false),
                    InvReturnId = table.Column<int>(type: "int", nullable: true),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false, computedColumnSql: "[quantity] * [unitPrice]", stored: true),
                    Status = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceSaleDetails", x => x.Id);
                    table.CheckConstraint("CK_InvoiceSaleDetail_Status", "[Status] IN ('Purchased', 'Returned')");
                    table.ForeignKey(
                        name: "FK_InvoiceSaleDetails_InvoiceSaleHeaders_InvReturnId",
                        column: x => x.InvReturnId,
                        principalTable: "InvoiceSaleHeaders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InvoiceSaleDetails_InvoiceSaleHeaders_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "InvoiceSaleHeaders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InvoiceSaleDetails_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ItemCategories",
                columns: table => new
                {
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemCategories", x => new { x.ItemId, x.CategoryId });
                    table.ForeignKey(
                        name: "FK_ItemCategories_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItemCategories_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItemCodes",
                columns: table => new
                {
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    ItemCodeValue = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemCodes", x => new { x.ItemId, x.ItemCodeValue });
                    table.ForeignKey(
                        name: "FK_ItemCodes_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItemImages",
                columns: table => new
                {
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    ItemImageData = table.Column<byte[]>(type: "varbinary(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemImages", x => x.ItemId);
                    table.ForeignKey(
                        name: "FK_ItemImages_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JournalDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntryId = table.Column<int>(type: "int", nullable: false),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    Debit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Credit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JournalDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JournalDetails_ChartOfAccounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "ChartOfAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_JournalDetails_JournalEntries_EntryId",
                        column: x => x.EntryId,
                        principalTable: "JournalEntries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActivityLogs_Username",
                table: "ActivityLogs",
                column: "Username");

            migrationBuilder.CreateIndex(
                name: "IX_ChartOfAccounts_Code",
                table: "ChartOfAccounts",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChartOfAccounts_ParentCode",
                table: "ChartOfAccounts",
                column: "ParentCode");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerItems_ItemId",
                table: "CustomerItems",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_IdNumber",
                table: "Employees",
                column: "IdNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Inventories_ItemId",
                table: "Inventories",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryTransactions_AssignedBy",
                table: "InventoryTransactions",
                column: "AssignedBy");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryTransactions_ItemId",
                table: "InventoryTransactions",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryTransactions_StoreId",
                table: "InventoryTransactions",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoicePurchaseDetails_InvoiceId",
                table: "InvoicePurchaseDetails",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoicePurchaseDetails_InvReturnId",
                table: "InvoicePurchaseDetails",
                column: "InvReturnId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoicePurchaseDetails_ItemId",
                table: "InvoicePurchaseDetails",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoicePurchaseHeaders_AssignedByEmployeeId",
                table: "InvoicePurchaseHeaders",
                column: "AssignedByEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoicePurchaseHeaders_RefInvId",
                table: "InvoicePurchaseHeaders",
                column: "RefInvId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoicePurchaseHeaders_StoreId",
                table: "InvoicePurchaseHeaders",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoicePurchaseHeaders_SupplierId",
                table: "InvoicePurchaseHeaders",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceSaleDetails_InvoiceId",
                table: "InvoiceSaleDetails",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceSaleDetails_InvReturnId",
                table: "InvoiceSaleDetails",
                column: "InvReturnId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceSaleDetails_ItemId",
                table: "InvoiceSaleDetails",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceSaleHeaders_AssignedBy",
                table: "InvoiceSaleHeaders",
                column: "AssignedBy");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceSaleHeaders_CustomerId",
                table: "InvoiceSaleHeaders",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceSaleHeaders_DelegateId",
                table: "InvoiceSaleHeaders",
                column: "DelegateId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceSaleHeaders_RefInvId",
                table: "InvoiceSaleHeaders",
                column: "RefInvId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceSaleHeaders_StoreId",
                table: "InvoiceSaleHeaders",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemCategories_CategoryId",
                table: "ItemCategories",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemCodes_ItemCodeValue",
                table: "ItemCodes",
                column: "ItemCodeValue",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Items_CompanyMade",
                table: "Items",
                column: "CompanyMade");

            migrationBuilder.CreateIndex(
                name: "IX_Items_DefaultStore",
                table: "Items",
                column: "DefaultStore");

            migrationBuilder.CreateIndex(
                name: "IX_Items_UnitNumber",
                table: "Items",
                column: "UnitNumber");

            migrationBuilder.CreateIndex(
                name: "IX_JournalDetails_AccountId",
                table: "JournalDetails",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_JournalDetails_EntryId",
                table: "JournalDetails",
                column: "EntryId");

            migrationBuilder.CreateIndex(
                name: "IX_JournalEntries_AssignedBy",
                table: "JournalEntries",
                column: "AssignedBy");

            migrationBuilder.CreateIndex(
                name: "IX_JournalEntries_InvPurId",
                table: "JournalEntries",
                column: "InvPurId");

            migrationBuilder.CreateIndex(
                name: "IX_JournalEntries_InvSaleId",
                table: "JournalEntries",
                column: "InvSaleId");

            migrationBuilder.CreateIndex(
                name: "IX_Logins_EmpId",
                table: "Logins",
                column: "EmpId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchasePayments_InvoiceReturnId",
                table: "PurchasePayments",
                column: "InvoiceReturnId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchasePayments_InvPurchaseId",
                table: "PurchasePayments",
                column: "InvPurchaseId");

            migrationBuilder.CreateIndex(
                name: "IX_SalePayments_InvoiceReturnId",
                table: "SalePayments",
                column: "InvoiceReturnId");

            migrationBuilder.CreateIndex(
                name: "IX_SalePayments_InvSaleId",
                table: "SalePayments",
                column: "InvSaleId");

            migrationBuilder.CreateIndex(
                name: "IX_Stores_StoreManager",
                table: "Stores",
                column: "StoreManager");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActivityLogs");

            migrationBuilder.DropTable(
                name: "CompanyPhones");

            migrationBuilder.DropTable(
                name: "CustomerItems");

            migrationBuilder.DropTable(
                name: "CustomerPhones");

            migrationBuilder.DropTable(
                name: "EmpImages");

            migrationBuilder.DropTable(
                name: "EmpPhones");

            migrationBuilder.DropTable(
                name: "ExpenseNames");

            migrationBuilder.DropTable(
                name: "Inventories");

            migrationBuilder.DropTable(
                name: "InventoryTransactions");

            migrationBuilder.DropTable(
                name: "InvoicePurchaseDetails");

            migrationBuilder.DropTable(
                name: "InvoiceSaleDetails");

            migrationBuilder.DropTable(
                name: "ItemCategories");

            migrationBuilder.DropTable(
                name: "ItemCodes");

            migrationBuilder.DropTable(
                name: "ItemImages");

            migrationBuilder.DropTable(
                name: "JournalDetails");

            migrationBuilder.DropTable(
                name: "PurchasePayments");

            migrationBuilder.DropTable(
                name: "RevenueNames");

            migrationBuilder.DropTable(
                name: "SalePayments");

            migrationBuilder.DropTable(
                name: "SupplierPhones");

            migrationBuilder.DropTable(
                name: "Logins");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "ChartOfAccounts");

            migrationBuilder.DropTable(
                name: "JournalEntries");

            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.DropTable(
                name: "Units");

            migrationBuilder.DropTable(
                name: "InvoicePurchaseHeaders");

            migrationBuilder.DropTable(
                name: "InvoiceSaleHeaders");

            migrationBuilder.DropTable(
                name: "Suppliers");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Stores");

            migrationBuilder.DropTable(
                name: "delegate");

            migrationBuilder.DropTable(
                name: "Employees");
        }
    }
}
