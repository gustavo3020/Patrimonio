using Patrimonio.Financas.Application.Movimentacoes.Queries.ReadModels;

namespace Patrimonio.Financas.Application.Movimentacoes.Queries.Abstractions;

/// <summary>
/// Define as operações de leitura necessárias para movimentações.
/// </summary>
public interface IMovimentacaoQueryRepository
{
    /// <summary>
    /// Lista as movimentações disponíveis.
    /// </summary>
    /// <param name="cancellationToken">Token utilizado para cancelar a operação.</param>
    /// <returns>ReadModels contendo os dados das movimentações.</returns>
    Task<IReadOnlyCollection<MovimentacaoListaReadModel>> ListarAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Obtém uma movimentação pelo seu identificador.
    /// </summary>
    /// <param name="movimentacaoId">Identificador da movimentação.</param>
    /// <param name="cancellationToken">Token utilizado para cancelar a operação.</param>
    /// <returns>ReadModel contendo os dados detalhados da movimentação.</returns>
    Task<MovimentacaoDetalheReadModel?> ObterPorIdAsync(int movimentacaoId, CancellationToken cancellationToken);
}
