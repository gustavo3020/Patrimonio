using Patrimonio.Financas.Contracts.Common.Dtos;

namespace Patrimonio.Financas.Contracts.Cartoes.Dtos;

/// <summary>
/// Representa as opções de referência disponíveis para a criação de um lançamento.
/// </summary>
public sealed record LancamentoOpcoesCriacaoDto
{
    public required IReadOnlyCollection<OpcaoDto> Categorias { get; init; }
}
