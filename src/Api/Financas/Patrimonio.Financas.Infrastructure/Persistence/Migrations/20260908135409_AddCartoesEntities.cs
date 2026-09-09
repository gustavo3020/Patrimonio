using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Patrimonio.Financas.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddCartoesEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FaturaId",
                schema: "financas",
                table: "Movimentacoes",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Cartoes",
                schema: "financas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NomeNormalizado = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Nome = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Bandeira = table.Column<int>(type: "integer", nullable: false),
                    InstituicaoId = table.Column<int>(type: "integer", nullable: false),
                    DiaFechamento = table.Column<int>(type: "integer", nullable: false),
                    DiaVencimento = table.Column<int>(type: "integer", nullable: false),
                    Limite = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    DataCriacao = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DataAlteracao = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cartoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cartoes_Instituicoes_InstituicaoId",
                        column: x => x.InstituicaoId,
                        principalSchema: "financas",
                        principalTable: "Instituicoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Faturas",
                schema: "financas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DataFechamento = table.Column<DateOnly>(type: "date", nullable: false),
                    DataVencimento = table.Column<DateOnly>(type: "date", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    DataPagamento = table.Column<DateOnly>(type: "date", nullable: true),
                    CartaoId = table.Column<int>(type: "integer", nullable: false),
                    DataCriacao = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DataAlteracao = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Faturas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Faturas_Cartoes_CartaoId",
                        column: x => x.CartaoId,
                        principalSchema: "financas",
                        principalTable: "Cartoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Lancamentos",
                schema: "financas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DataCompra = table.Column<DateOnly>(type: "date", nullable: false),
                    GrupoId = table.Column<Guid>(type: "uuid", nullable: false),
                    NumeroParcela = table.Column<int>(type: "integer", nullable: false),
                    TotalParcelas = table.Column<int>(type: "integer", nullable: false),
                    FaturaId = table.Column<int>(type: "integer", nullable: false),
                    CategoriaId = table.Column<int>(type: "integer", nullable: false),
                    Descricao = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Estabelecimento = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Responsavel = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Valor = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    DataCriacao = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DataAlteracao = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lancamentos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lancamentos_Categorias_CategoriaId",
                        column: x => x.CategoriaId,
                        principalSchema: "financas",
                        principalTable: "Categorias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Lancamentos_Faturas_FaturaId",
                        column: x => x.FaturaId,
                        principalSchema: "financas",
                        principalTable: "Faturas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Movimentacoes_FaturaId",
                schema: "financas",
                table: "Movimentacoes",
                column: "FaturaId");

            migrationBuilder.CreateIndex(
                name: "IX_Cartoes_InstituicaoId",
                schema: "financas",
                table: "Cartoes",
                column: "InstituicaoId");

            migrationBuilder.CreateIndex(
                name: "IX_Cartoes_NomeNormalizado",
                schema: "financas",
                table: "Cartoes",
                column: "NomeNormalizado",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Faturas_CartaoId",
                schema: "financas",
                table: "Faturas",
                column: "CartaoId");

            migrationBuilder.CreateIndex(
                name: "IX_Lancamentos_CategoriaId",
                schema: "financas",
                table: "Lancamentos",
                column: "CategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_Lancamentos_FaturaId",
                schema: "financas",
                table: "Lancamentos",
                column: "FaturaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Movimentacoes_Faturas_FaturaId",
                schema: "financas",
                table: "Movimentacoes",
                column: "FaturaId",
                principalSchema: "financas",
                principalTable: "Faturas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movimentacoes_Faturas_FaturaId",
                schema: "financas",
                table: "Movimentacoes");

            migrationBuilder.DropTable(
                name: "Lancamentos",
                schema: "financas");

            migrationBuilder.DropTable(
                name: "Faturas",
                schema: "financas");

            migrationBuilder.DropTable(
                name: "Cartoes",
                schema: "financas");

            migrationBuilder.DropIndex(
                name: "IX_Movimentacoes_FaturaId",
                schema: "financas",
                table: "Movimentacoes");

            migrationBuilder.DropColumn(
                name: "FaturaId",
                schema: "financas",
                table: "Movimentacoes");
        }
    }
}
