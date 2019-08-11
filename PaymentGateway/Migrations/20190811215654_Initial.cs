using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace PaymentGateway.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Amount = table.Column<double>(nullable: false),
                    CardCVV = table.Column<string>(nullable: true),
                    CardExpiryDate = table.Column<DateTime>(nullable: false),
                    CardNumber = table.Column<string>(nullable: true),
                    Currency = table.Column<string>(nullable: true),
                    MaskedCardNumber = table.Column<string>(nullable: true),
                    PaymentDate = table.Column<DateTime>(nullable: false),
                    Result = table.Column<int>(nullable: false),
                    TransactionId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Payments");
        }
    }
}
