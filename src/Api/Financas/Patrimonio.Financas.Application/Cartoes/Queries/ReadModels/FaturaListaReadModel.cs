using Patrimonio.Financas.Application.Common.ReadModels;
using Patrimonio.Financas.SharedKernel.Cartoes.Enums;

namespace Patrimonio.Financas.Application.Cartoes.Queries.ReadModels;

/// <summary>
/// Representa os dados de uma fatura necessários para uma listagem.
/// </summary>
public sealed record FaturaListaReadModel : LeituraReadModelBase
{
    // Dados próprios.
    public required DateOnly DataFechamento { get; init; }
    public required DateOnly DataVencimento { get; init; }
    public required StatusFatura Status { get; init; }
    public required DateOnly? DataPagamento { get; init; }

    // Relacionamentos.
    public required string CartaoNome { get; init; }
}
