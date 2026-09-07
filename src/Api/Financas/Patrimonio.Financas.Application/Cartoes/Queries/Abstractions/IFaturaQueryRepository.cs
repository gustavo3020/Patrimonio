using Patrimonio.Financas.Application.Cartoes.Queries.ReadModels;

namespace Patrimonio.Financas.Application.Cartoes.Queries.Abstractions;

/// <summary>
/// Define as operações de leitura necessárias para faturas.
/// </summary>
public interface IFaturaQueryRepository
{
    /// <summary>
    /// Lista as faturas disponíveis.
    /// </summary>
    /// <param name="cancellationToken">Token utilizado para cancelar a operação.</param>
    /// <returns>ReadModels contendo os dados das faturas.</returns>
    Task<IReadOnlyCollection<FaturaListaReadModel>> ListarAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Obtém uma fatura pelo seu identificador.
    /// </summary>
    /// <param name="faturaId">Identificador da fatura.</param>
    /// <param name="cancellationToken">Token utilizado para cancelar a operação.</param>
    /// <returns>ReadModel contendo os dados detalhados da fatura.</returns>
    Task<FaturaDetalheReadModel?> ObterPorIdAsync(int faturaId, CancellationToken cancellationToken);
}
