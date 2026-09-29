using Patrimonio.Financas.Application.Cartoes.Lookups.ReadModels;

namespace Patrimonio.Financas.Application.Cartoes.Lookups.Abstractions;

/// <summary>
/// Define as operações de consultas de referência de faturas.
/// </summary>
public interface IFaturaLookupRepository
{
    /// <summary>
    /// Lista as faturas associadas a um cartão específico.
    /// </summary>
    /// <param name="cartaoId">Identificador do cartão.</param>
    /// <param name="cancellationToken">Token utilizado para cancelar a operação.</param>
    /// <returns>Uma coleção de faturas encontradas ou uma coleção vazia se nenhuma for encontrada.</returns>
    Task<IReadOnlyCollection<FaturaOpcaoReadModel>> ListarPorCartaoAsync(int cartaoId, CancellationToken cancellationToken);
}
