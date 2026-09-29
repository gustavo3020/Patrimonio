using Patrimonio.Financas.Contracts.Common.Dtos;
using Patrimonio.Financas.SharedKernel.Movimentacoes.Enums;

namespace Patrimonio.Financas.Contracts.Movimentacoes.Dtos;

/// <summary>
/// Representa os dados detalhados de uma movimentação.
/// </summary>
public sealed record MovimentacaoDetalheDto : LeituraDtoBase
{
    // Dados próprios.
    public required DateOnly Data { get; init; }
    public required decimal Valor { get; init; }
    public required Natureza Natureza { get; init; }
    public required TipoMovimentacao Tipo { get; init; }
    public required string? Descricao { get; init; }

    // Relacionamentos.
    public required int ContaId { get; init; }
    public required string ContaNome { get; init; }

    public required int CategoriaId { get; init; }
    public required string CategoriaNome { get; init; }
}
