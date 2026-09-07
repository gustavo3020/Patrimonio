using Patrimonio.Financas.Application.Cartoes.Queries.ReadModels;

namespace Patrimonio.Financas.Application.Cartoes.Queries.Abstractions;

/// <summary>
/// Define as operações de leitura necessárias para cartões.
/// </summary>
public interface ICartaoQueryRepository
{
    /// <summary>
    /// Lista os cartões disponíveis.
    /// </summary>
    /// <param name="cancellationToken">Token utilizado para cancelar a operação.</param>
    /// <returns>ReadModels contendo os dados dos cartões.</returns>
    Task<IReadOnlyCollection<CartaoListaReadModel>> ListarAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Obtém um cartão pelo seu identificador.
    /// </summary>
    /// <param name="cartaoId">Identificador do cartão.</param>
    /// <param name="cancellationToken">Token utilizado para cancelar a operação.</param>
    /// <returns>ReadModel contendo os dados detalhados do cartão.</returns>
    Task<CartaoDetalheReadModel?> ObterPorIdAsync(int cartaoId, CancellationToken cancellationToken);
}
