using Patrimonio.Financas.Contracts.Common.Dtos;
using Patrimonio.Financas.SharedKernel.Cartoes.Enums;

namespace Patrimonio.Financas.Contracts.Cartoes.Dtos;

/// <summary>
/// Representa os dados detalhados de uma fatura.
/// </summary>
public sealed record FaturaDetalheDto : LeituraDtoBase
{
    // Dados próprios.
    public required DateOnly DataFechamento { get; init; }
    public required DateOnly DataVencimento { get; init; }
    public required StatusFatura Status { get; init; }
    public required DateOnly? DataPagamento { get; init; }

    // Relacionamentos.
    public required int CartaoId { get; init; }
    public required string CartaoNome { get; init; }
}
