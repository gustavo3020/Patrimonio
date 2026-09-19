using Patrimonio.Financas.SharedKernel.Cartoes.Enums;

namespace Patrimonio.Financas.Contracts.Cartoes.Dtos;

/// <summary>
/// Define os dados necessários para alterar um cartão.
/// </summary>
public sealed record CartaoAlteracaoDto
{
    public required string Nome { get; init; }
    public required BandeiraCartao Bandeira { get; init; }
    public required decimal Limite { get; init; }
    public required int DiaFechamento { get; init; }
    public required int DiaVencimento { get; init; }
}
