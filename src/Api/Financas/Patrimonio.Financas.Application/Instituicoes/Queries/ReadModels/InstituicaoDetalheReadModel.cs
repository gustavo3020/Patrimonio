using Patrimonio.Financas.Application.Common.ReadModels;

namespace Patrimonio.Financas.Application.Instituicoes.Queries.ReadModels;

/// <summary>
/// Representa os dados de uma instituição necessários para sua visualização detalhada.
/// </summary>
public sealed record InstituicaoDetalheReadModel : LeituraReadModelBase
{
    // Dados próprios.
    public required string Nome { get; init; }
}
