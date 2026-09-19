using Patrimonio.Financas.Application.Common.ReadModels;

namespace Patrimonio.Financas.Application.Cartoes.Queries.ReadModels;

/// <summary>
/// Representa os dados de um cartão necessários para uma listagem.
/// </summary>
public sealed record CartaoListaReadModel : LeituraReadModelBase
{
    // Dados próprios.
    public required string Nome { get; init; }
    public required decimal Limite { get; init; }
    public required int DiaFechamento { get; init; }
    public required int DiaVencimento { get; init; }

    // Relacionamentos.
    public required string InstituicaoNome { get; init; }
}
