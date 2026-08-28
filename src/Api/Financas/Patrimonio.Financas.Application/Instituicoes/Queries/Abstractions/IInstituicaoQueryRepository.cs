using Patrimonio.Financas.Application.Instituicoes.Queries.ReadModels;

namespace Patrimonio.Financas.Application.Instituicoes.Queries.Abstractions;

/// <summary>
/// Define as operações de leitura necessárias para instituições.
/// </summary>
public interface IInstituicaoQueryRepository
{
    /// <summary>
    /// Lista as instituições disponíveis.
    /// </summary>
    /// <param name="cancellationToken">Token utilizado para cancelar a operação.</param>
    /// <returns>ReadModels contendo os dados das instituições.</returns>
    Task<IReadOnlyCollection<InstituicaoListaReadModel>> ListarAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Obtém uma instituição pelo seu identificador.
    /// </summary>
    /// <param name="instituicaoId">Identificador da instituição.</param>
    /// <param name="cancellationToken">Token utilizado para cancelar a operação.</param>
    /// <returns>ReadModel contendo os dados detalhados da instituição.</returns>
    Task<InstituicaoDetalheReadModel> ObterPorIdAsync(int instituicaoId, CancellationToken cancellationToken);
}
