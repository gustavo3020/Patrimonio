using Patrimonio.Financas.Application.Common.ReadModels;
using Patrimonio.Financas.SharedKernel.Movimentacoes.Enums;

namespace Patrimonio.Financas.Application.Movimentacoes.Queries.ReadModels;

/// <summary>
/// Representa os dados de uma movimentação necessários para sua visualização detalhada.
/// </summary>
public sealed record MovimentacaoDetalheReadModel : LeituraReadModelBase
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
