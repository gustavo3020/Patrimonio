using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Patrimonio.Financas.Infrastructure.Persistence.Migrations;

/// <inheritdoc />
public partial class AddNaturezaEmLancamentosEIndiceUnicoEmDataVencimentoFatura : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropIndex(
            name: "IX_Faturas_CartaoId",
            schema: "financas",
            table: "Faturas");

        migrationBuilder.AddColumn<int>(
            name: "Natureza",
            schema: "financas",
            table: "Lancamentos",
            type: "integer",
            nullable: false,
            defaultValue: 0);

        migrationBuilder.CreateIndex(
            name: "IX_Faturas_CartaoId_DataVencimento",
            schema: "financas",
            table: "Faturas",
            columns: new[] { "CartaoId", "DataVencimento" },
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropIndex(
            name: "IX_Faturas_CartaoId_DataVencimento",
            schema: "financas",
            table: "Faturas");

        migrationBuilder.DropColumn(
            name: "Natureza",
            schema: "financas",
            table: "Lancamentos");

        migrationBuilder.CreateIndex(
            name: "IX_Faturas_CartaoId",
            schema: "financas",
            table: "Faturas",
            column: "CartaoId");
    }
}
