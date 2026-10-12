using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GapBi.Api.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarIdsExternos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdExterno",
                table: "Produtos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdExterno",
                table: "Pedidos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdExterno",
                table: "Clientes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Produtos_IdExterno",
                table: "Produtos",
                column: "IdExterno",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_IdExterno",
                table: "Pedidos",
                column: "IdExterno",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Clientes_IdExterno",
                table: "Clientes",
                column: "IdExterno",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Produtos_IdExterno",
                table: "Produtos");

            migrationBuilder.DropIndex(
                name: "IX_Pedidos_IdExterno",
                table: "Pedidos");

            migrationBuilder.DropIndex(
                name: "IX_Clientes_IdExterno",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "IdExterno",
                table: "Produtos");

            migrationBuilder.DropColumn(
                name: "IdExterno",
                table: "Pedidos");

            migrationBuilder.DropColumn(
                name: "IdExterno",
                table: "Clientes");
        }
    }
}
