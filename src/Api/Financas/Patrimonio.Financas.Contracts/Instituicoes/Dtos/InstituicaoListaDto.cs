using Patrimonio.Financas.Contracts.Common.Dtos;

namespace Patrimonio.Financas.Contracts.Instituicoes.Dtos;

/// <summary>
/// Representa os dados de uma instituição retornados em uma listagem.
/// </summary>
public sealed record InstituicaoListaDto : LeituraDtoBase
{
    public required string Nome { get; init; }
}
