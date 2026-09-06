using Patrimonio.Financas.Contracts.Cartoes.Dtos;

namespace Patrimonio.Financas.Contracts.Cartoes.Services;

/// <summary>
/// Define as operações de leitura disponíveis para faturas.
/// </summary>
public interface IFaturaQueryService
{
    /// <summary>
    /// Retorna todas as faturas.
    /// </summary>
    /// <param name="cancellationToken">Token utilizado para cancelar a operação.</param>
    /// <returns>Uma coleção com as faturas encontradas.</returns>
    Task<IReadOnlyCollection<FaturaListaDto>> ListarAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Retorna uma fatura pelo seu identificador.
    /// </summary>
    /// <param name="faturaId">Identificador da fatura.</param>
    /// <param name="cancellationToken">Token utilizado para cancelar a operação.</param>
    /// <returns>Os dados detalhados da fatura.</returns>
    Task<FaturaDetalheDto> ObterPorIdAsync(int faturaId, CancellationToken cancellationToken);
}
