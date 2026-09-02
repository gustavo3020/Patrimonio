using Patrimonio.Financas.Contracts.Common.Dtos;

namespace Patrimonio.Financas.Contracts.Movimentacoes.Dtos;

/// <summary>
/// Representa as opções de referência disponíveis para a criação de uma movimentação.
/// </summary>
public sealed record MovimentacaoOpcoesCriacaoDto
{
    public required IReadOnlyCollection<OpcaoDto> Categorias { get; init; }
    public required IReadOnlyCollection<OpcaoDto> Contas { get; init; }
}
