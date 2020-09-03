using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PaymentGateway.Infrastructure.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "checkout");

            migrationBuilder.CreateTable(
                name: "Payments",
                schema: "checkout",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    CardNumber = table.Column<string>(nullable: false),
                    Expiry = table.Column<string>(nullable: false),
                    FullName = table.Column<string>(nullable: false),
                    Amount = table.Column<float>(nullable: false),
                    Currency = table.Column<string>(nullable: false),
                    BankPaymentId = table.Column<string>(nullable: false),
                    BankPaymentStatus = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Payments",
                schema: "checkout");
        }
    }
}
