namespace Patrimonio.Financas.Contracts.Cartoes.Dtos;

/// <summary>
/// Define os dados necessários para criar uma fatura.
/// </summary>
public sealed record FaturaCriacaoDto
{
    // Dados próprios.
    public required DateOnly DataFechamento { get; init; }
    public required DateOnly DataVencimento { get; init; }

    // Relacionamentos.
    public required int CartaoId { get; init; }
}
