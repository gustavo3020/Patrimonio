using Patrimonio.Financas.Contracts.Common.Dtos;

namespace Patrimonio.Financas.Contracts.Cartoes.Dtos;

/// <summary>
/// Representa as opções de referência disponíveis para a criação de um cartão.
/// </summary>
public sealed record CartaoOpcoesCriacaoDto
{
    public required IReadOnlyCollection<OpcaoDto> Instituicoes { get; init; }
}
