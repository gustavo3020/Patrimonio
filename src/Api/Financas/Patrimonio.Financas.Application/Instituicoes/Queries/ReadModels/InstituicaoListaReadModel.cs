using Patrimonio.Financas.Application.Common.ReadModels;

namespace Patrimonio.Financas.Application.Instituicoes.Queries.ReadModels;

/// <summary>
/// Representa os dados de uma instituição necessários para uma listagem.
/// </summary>
public sealed record InstituicaoListaReadModel : LeituraReadModelBase
{
    // Dados próprios.
    public required string Nome { get; init; }
}
