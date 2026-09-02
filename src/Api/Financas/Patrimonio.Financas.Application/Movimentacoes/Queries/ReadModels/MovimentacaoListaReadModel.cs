using Patrimonio.Financas.Application.Common.ReadModels;
using Patrimonio.Financas.SharedKernel.Movimentacoes.Enums;

namespace Patrimonio.Financas.Application.Movimentacoes.Queries.ReadModels;

/// <summary>
/// Representa os dados de uma movimentação necessários para uma listagem.
/// </summary>
public sealed record MovimentacaoListaReadModel : LeituraReadModelBase
{
    // Dados próprios.
    public required DateOnly Data { get; init; }
    public required decimal Valor { get; init; }
    public required Natureza Natureza { get; init; }
    public required TipoMovimentacao Tipo { get; init; }
    public required string? Descricao { get; init; }

    // Relacionamentos.
    public required string ContaNome { get; init; }
    public required string CategoriaNome { get; init; }
}
