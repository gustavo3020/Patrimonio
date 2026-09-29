using Patrimonio.Financas.Contracts.Common.Dtos;

namespace Patrimonio.Financas.Contracts.Instituicoes.Dtos;

/// <summary>
/// Representa os dados detalhados de uma instituição.
/// </summary>
public sealed record InstituicaoDetalheDto : LeituraDtoBase
{
    public required string Nome { get; init; }
}
