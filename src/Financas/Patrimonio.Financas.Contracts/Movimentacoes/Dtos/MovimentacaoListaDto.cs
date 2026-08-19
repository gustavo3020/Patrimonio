using Patrimonio.Financas.SharedKernel.Movimentacoes.Enums;

namespace Patrimonio.Financas.Contracts.Movimentacoes.Dtos;

/// <summary>
/// Representa os dados de uma movimentação retornados em uma listagem.
/// </summary>
public sealed record MovimentacaoListaDto
{
    // Identificação.
    public required int Id { get; init; }

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
