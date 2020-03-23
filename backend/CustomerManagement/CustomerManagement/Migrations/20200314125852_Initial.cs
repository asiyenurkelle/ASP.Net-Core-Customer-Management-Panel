using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace CustomerManagement.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                 name: "States",
                 columns: table => new
                 {
                     Id = table.Column<int>(nullable: false)
                         .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                     Abbreviation = table.Column<string>(maxLength: 2, nullable: true),
                     Name = table.Column<string>(maxLength: 25, nullable: true)
                 },
                 constraints: table =>
                 {
                     table.PrimaryKey("PK_States", x => x.Id);
                 });
            migrationBuilder.CreateTable(
                           name: "Customers",
                           columns: table => new
                           {
                               Id = table.Column<int>(nullable: false)
                                   .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                               Address = table.Column<string>(maxLength: 1000, nullable: true),
                               City = table.Column<string>(maxLength: 50, nullable: true),
                               Email = table.Column<string>(maxLength: 100, nullable: true),
                               FirstName = table.Column<string>(maxLength: 50, nullable: true),
                               Gender = table.Column<int>(nullable: false),
                               LastName = table.Column<string>(maxLength: 50, nullable: true),
                               OrderCount = table.Column<int>(nullable: false),
                               StateId = table.Column<int>(nullable: false),
                               Zip = table.Column<int>(nullable: false)
                           },
                           constraints: table =>
                           {
                               table.PrimaryKey("PK_Customers", x => x.Id);
                               table.ForeignKey(
                                   name: "FK_Customers_States_StateId",
                                   column: x => x.StateId,
                                   principalTable: "States",
                                   principalColumn: "Id",
                                   onDelete: ReferentialAction.Cascade);
                           });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CustomerId = table.Column<int>(nullable: true),
                    Price = table.Column<decimal>(nullable: false),
                    Product = table.Column<string>(nullable: true),
                    Quantity = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Customers_StateId",
                table: "Customers",
                column: "StateId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomerId",
                table: "Orders",
                column: "CustomerId");



        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                          name: "Customers");

            migrationBuilder.DropTable(
                          name: "States");


        }
    }
}
