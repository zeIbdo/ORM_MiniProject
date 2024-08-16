using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ORM_MiniProject.Migrations
{
    public partial class _111 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Payments_Amount_Positive",
                table: "Payment");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Payments_Amount_Positive",
                table: "Payment",
                sql: "[Amount]>=0");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Payments_Amount_Positive",
                table: "Payment");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Payments_Amount_Positive",
                table: "Payment",
                sql: "[Amount]>0");
        }
    }
}
