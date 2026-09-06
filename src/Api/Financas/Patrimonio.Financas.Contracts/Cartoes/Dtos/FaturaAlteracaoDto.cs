namespace Patrimonio.Financas.Contracts.Cartoes.Dtos;

/// <summary>
/// Define os dados necessários para alterar uma fatura.
/// </summary>
public sealed record FaturaAlteracaoDto
{
    // Dados próprios.
    public required DateOnly DataFechamento { get; init; }
    public required DateOnly DataVencimento { get; init; }
}
