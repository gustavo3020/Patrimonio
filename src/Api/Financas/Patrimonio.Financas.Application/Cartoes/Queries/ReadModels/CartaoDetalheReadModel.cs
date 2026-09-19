using Patrimonio.Financas.Application.Common.ReadModels;
using Patrimonio.Financas.SharedKernel.Cartoes.Enums;

namespace Patrimonio.Financas.Application.Cartoes.Queries.ReadModels;

/// <summary>
/// Representa os dados de um cartão necessários para sua visualização detalhada.
/// </summary>
public sealed record CartaoDetalheReadModel : LeituraReadModelBase
{
    // Dados próprios.
    public required string Nome { get; init; }
    public required BandeiraCartao Bandeira { get; init; }
    public required decimal Limite { get; init; }
    public required int DiaFechamento { get; init; }
    public required int DiaVencimento { get; init; }

    // Relacionamentos.
    public required int InstituicaoId { get; init; }
    public required string InstituicaoNome { get; init; }
}
