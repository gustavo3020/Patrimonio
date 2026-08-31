using Patrimonio.Financas.Application.Contas.Queries.ReadModels;

namespace Patrimonio.Financas.Application.Contas.Queries.Abstractions;

/// <summary>
/// Define as operações de leitura necessárias para contas.
/// </summary>
public interface IContaQueryRepository
{
    /// <summary>
    /// Lista as contas disponíveis.
    /// </summary>
    /// <param name="cancellationToken">Token utilizado para cancelar a operação.</param>
    /// <returns>ReadModels contendo os dados das contas.</returns>
    Task<IReadOnlyCollection<ContaListaReadModel>> ListarAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Obtém uma conta pelo seu identificador.
    /// </summary>
    /// <param name="contaId">Identificador da conta.</param>
    /// <param name="cancellationToken">Token utilizado para cancelar a operação.</param>
    /// <returns>ReadModel contendo os dados detalhados da conta.</returns>
    Task<ContaDetalheReadModel?> ObterPorIdAsync(int contaId, CancellationToken cancellationToken);
}
