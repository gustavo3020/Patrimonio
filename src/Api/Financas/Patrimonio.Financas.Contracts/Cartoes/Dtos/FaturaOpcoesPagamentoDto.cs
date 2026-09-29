using Patrimonio.Financas.Contracts.Common.Dtos;

namespace Patrimonio.Financas.Contracts.Cartoes.Dtos;

/// <summary>
/// Representa as opções de referência disponíveis para o pagamento de uma fatura.
/// </summary>
public sealed record FaturaOpcoesPagamentoDto
{
    public required IReadOnlyCollection<OpcaoDto> Categorias { get; init; }
    public required IReadOnlyCollection<OpcaoDto> Contas { get; init; }
}
