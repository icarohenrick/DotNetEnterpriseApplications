using Microsoft.EntityFrameworkCore.Migrations;

namespace NSE.Carrinho.API.Migrations
{
    public partial class CarrinhoCascade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarrinhoItems_CarrinhoClientes_CarrinhoId",
                table: "CarrinhoItems");

            migrationBuilder.AddForeignKey(
                name: "FK_CarrinhoItems_CarrinhoClientes_CarrinhoId",
                table: "CarrinhoItems",
                column: "CarrinhoId",
                principalTable: "CarrinhoClientes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarrinhoItems_CarrinhoClientes_CarrinhoId",
                table: "CarrinhoItems");

            migrationBuilder.AddForeignKey(
                name: "FK_CarrinhoItems_CarrinhoClientes_CarrinhoId",
                table: "CarrinhoItems",
                column: "CarrinhoId",
                principalTable: "CarrinhoClientes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
