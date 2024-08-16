using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ORM_MiniProject.Migrations
{
    public partial class initt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Orders_TotalAmount_Positive",
                table: "Orders");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Orders_TotalAmount_Positive",
                table: "Orders",
                sql: "[TotalAmount] >= 0");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Orders_TotalAmount_Positive",
                table: "Orders");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Orders_TotalAmount_Positive",
                table: "Orders",
                sql: "[TotalAmount] > 0");
        }
    }
}
