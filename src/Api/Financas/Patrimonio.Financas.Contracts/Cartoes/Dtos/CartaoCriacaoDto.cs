using Patrimonio.Financas.SharedKernel.Cartoes.Enums;

namespace Patrimonio.Financas.Contracts.Cartoes.Dtos;

/// <summary>
/// Define os dados necessários para criar um cartão.
/// </summary>
public sealed record CartaoCriacaoDto
{
    // Dados próprios.
    public required string Nome { get; init; }
    public required BandeiraCartao Bandeira { get; init; }
    public required decimal Limite { get; init; }
    public required int DiaFechamento { get; init; }
    public required int DiaVencimento { get; init; }

    // Relacionamentos.
    public required int InstituicaoId { get; init; }
}
