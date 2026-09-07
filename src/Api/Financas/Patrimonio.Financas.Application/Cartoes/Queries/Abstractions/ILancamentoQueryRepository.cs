using Patrimonio.Financas.Application.Cartoes.Queries.ReadModels;

namespace Patrimonio.Financas.Application.Cartoes.Queries.Abstractions;

/// <summary>
/// Define as operações de leitura necessárias para lançamentos.
/// </summary>
public interface ILancamentoQueryRepository
{
    /// <summary>
    /// Lista os lançamentos disponíveis.
    /// </summary>
    /// <param name="cancellationToken">Token utilizado para cancelar a operação.</param>
    /// <returns>ReadModels contendo os dados dos lançamentos.</returns>
    Task<IReadOnlyCollection<LancamentoListaReadModel>> ListarAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Obtém um lançamento pelo seu identificador.
    /// </summary>
    /// <param name="lancamentoId">Identificador do lançamento.</param>
    /// <param name="cancellationToken">Token utilizado para cancelar a operação.</param>
    /// <returns>ReadModel contendo os dados detalhados do lançamento.</returns>
    Task<LancamentoDetalheReadModel?> ObterPorIdAsync(int lancamentoId, CancellationToken cancellationToken);

    /// <summary>
    /// Obtém o valor total dos lançamentos associados a uma fatura específica.
    /// </summary>
    /// <param name="faturaId">Identificador da fatura.</param>
    /// <param name="cancellationToken">Token utilizado para cancelar a operação.</param>
    /// <returns>Valor total dos lançamentos associados à fatura.</returns>
    Task<decimal> ObterValorTotalPorFaturaAsync(int faturaId, CancellationToken cancellationToken);
}
