using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Patrimonio.Financas.Infrastructure.Persistence.Migrations;

/// <inheritdoc />
public partial class MigracaoInicial : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.EnsureSchema(
            name: "financas");

        migrationBuilder.CreateTable(
            name: "Categorias",
            schema: "financas",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                Nome = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                NomeNormalizado = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                DataCriacao = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                DataAlteracao = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Categorias", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Instituicoes",
            schema: "financas",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                Nome = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                NomeNormalizado = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                DataCriacao = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                DataAlteracao = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Instituicoes", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Contas",
            schema: "financas",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                Nome = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                NomeNormalizado = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                InstituicaoId = table.Column<int>(type: "integer", nullable: false),
                DataCriacao = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                DataAlteracao = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Contas", x => x.Id);
                table.ForeignKey(
                    name: "FK_Contas_Instituicoes_InstituicaoId",
                    column: x => x.InstituicaoId,
                    principalSchema: "financas",
                    principalTable: "Instituicoes",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "Movimentacoes",
            schema: "financas",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                Data = table.Column<DateOnly>(type: "date", nullable: false),
                Natureza = table.Column<int>(type: "integer", nullable: false),
                Tipo = table.Column<int>(type: "integer", nullable: false),
                Descricao = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                ContaId = table.Column<int>(type: "integer", nullable: false),
                CategoriaId = table.Column<int>(type: "integer", nullable: false),
                Valor = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                DataCriacao = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                DataAlteracao = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Movimentacoes", x => x.Id);
                table.ForeignKey(
                    name: "FK_Movimentacoes_Categorias_CategoriaId",
                    column: x => x.CategoriaId,
                    principalSchema: "financas",
                    principalTable: "Categorias",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_Movimentacoes_Contas_ContaId",
                    column: x => x.ContaId,
                    principalSchema: "financas",
                    principalTable: "Contas",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateIndex(
            name: "IX_Categorias_NomeNormalizado",
            schema: "financas",
            table: "Categorias",
            column: "NomeNormalizado",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Contas_InstituicaoId",
            schema: "financas",
            table: "Contas",
            column: "InstituicaoId");

        migrationBuilder.CreateIndex(
            name: "IX_Contas_NomeNormalizado",
            schema: "financas",
            table: "Contas",
            column: "NomeNormalizado",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Instituicoes_NomeNormalizado",
            schema: "financas",
            table: "Instituicoes",
            column: "NomeNormalizado",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Movimentacoes_CategoriaId",
            schema: "financas",
            table: "Movimentacoes",
            column: "CategoriaId");

        migrationBuilder.CreateIndex(
            name: "IX_Movimentacoes_ContaId",
            schema: "financas",
            table: "Movimentacoes",
            column: "ContaId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Movimentacoes",
            schema: "financas");

        migrationBuilder.DropTable(
            name: "Categorias",
            schema: "financas");

        migrationBuilder.DropTable(
            name: "Contas",
            schema: "financas");

        migrationBuilder.DropTable(
            name: "Instituicoes",
            schema: "financas");
    }
}
