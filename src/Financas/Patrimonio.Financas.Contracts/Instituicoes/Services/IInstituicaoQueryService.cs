using Patrimonio.Financas.Contracts.Instituicoes.Dtos;

namespace Patrimonio.Financas.Contracts.Instituicoes.Services;

/// <summary>
/// Define as operações de leitura disponíveis para instituições.
/// </summary>
public interface IInstituicaoQueryService
{
    /// <summary>
    /// Retorna todas as instituições.
    /// </summary>
    /// <param name="cancellationToken">Token utilizado para cancelar a operação.</param>
    /// <returns>Uma coleção com as instituições encontradas.</returns>
    Task<IReadOnlyCollection<InstituicaoListaDto>> ListarAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Retorna uma instituição pelo seu identificador.
    /// </summary>
    /// <param name="instituicaoId">Identificador da instituição.</param>
    /// <param name="cancellationToken">Token utilizado para cancelar a operação.</param>
    /// <returns>Os dados detalhados da instituição.</returns>
    Task<InstituicaoDetalheDto> ObterPorIdAsync(int instituicaoId, CancellationToken cancellationToken);
}
