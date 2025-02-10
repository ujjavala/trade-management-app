﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TradeManagementApp.Persistence.Migrations
{
    public partial class SeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FirstName = table.Column<string>(type: "TEXT", nullable: false),
                    LastName = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Trades",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AccountId = table.Column<int>(type: "INTEGER", nullable: false),
                    SecurityCode = table.Column<string>(type: "TEXT", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Amount = table.Column<decimal>(type: "TEXT", nullable: false),
                    BuyOrSell = table.Column<string>(type: "TEXT", nullable: false),
                    Status = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trades", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Trades_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "Id", "FirstName", "LastName" },
                values: new object[] { 1, "John", "Doe" });

            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "Id", "FirstName", "LastName" },
                values: new object[] { 2, "Jane", "Smith" });

            migrationBuilder.InsertData(
                table: "Trades",
                columns: new[] { "Id", "AccountId", "Amount", "BuyOrSell", "SecurityCode", "Status", "Timestamp" },
                values: new object[] { 1, 1, 100m, "Buy", "AAPL", "Completed", new DateTime(2025, 2, 10, 12, 46, 11, 574, DateTimeKind.Local).AddTicks(1360) });

            migrationBuilder.InsertData(
                table: "Trades",
                columns: new[] { "Id", "AccountId", "Amount", "BuyOrSell", "SecurityCode", "Status", "Timestamp" },
                values: new object[] { 2, 2, 200m, "Sell", "GOOGL", "Completed", new DateTime(2025, 2, 10, 12, 46, 11, 574, DateTimeKind.Local).AddTicks(1390) });

            migrationBuilder.CreateIndex(
                name: "IX_Trades_AccountId",
                table: "Trades",
                column: "AccountId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Trades");

            migrationBuilder.DropTable(
                name: "Accounts");
        }
    }
}
