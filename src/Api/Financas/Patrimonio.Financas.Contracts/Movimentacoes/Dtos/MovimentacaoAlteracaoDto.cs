using Patrimonio.Financas.SharedKernel.Movimentacoes.Enums;

namespace Patrimonio.Financas.Contracts.Movimentacoes.Dtos;

/// <summary>
/// Define os dados necessários para alterar uma movimentação.
/// </summary>
public sealed record MovimentacaoAlteracaoDto
{
    public required DateOnly Data { get; init; }
    public required decimal Valor { get; init; }
    public required Natureza Natureza { get; init; }
    public required TipoMovimentacao Tipo { get; init; }
    public required string? Descricao { get; init; }

    // Relacionamentos.
    public required int ContaId { get; init; }
    public required int CategoriaId { get; init; }
}
