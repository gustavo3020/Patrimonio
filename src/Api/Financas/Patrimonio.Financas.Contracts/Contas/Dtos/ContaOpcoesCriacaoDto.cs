using Patrimonio.Financas.Contracts.Common.Dtos;

namespace Patrimonio.Financas.Contracts.Contas.Dtos;

/// <summary>
/// Representa as opções de referência disponíveis para a criação de uma conta.
/// </summary>
public sealed record ContaOpcoesCriacaoDto
{
    public required IReadOnlyCollection<OpcaoDto> Instituicoes { get; init; }
}
