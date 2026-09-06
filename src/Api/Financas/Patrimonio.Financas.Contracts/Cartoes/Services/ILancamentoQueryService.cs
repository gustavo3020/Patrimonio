using Patrimonio.Financas.Contracts.Cartoes.Dtos;

namespace Patrimonio.Financas.Contracts.Cartoes.Services;

/// <summary>
/// Define as operações de leitura disponíveis para lançamentos.
/// </summary>
public interface ILancamentoQueryService
{
    /// <summary>
    /// Retorna todos os lançamentos.
    /// </summary>
    /// <param name="cancellationToken">Token utilizado para cancelar a operação.</param>
    /// <returns>Uma coleção com os lançamentos encontrados.</returns>
    Task<IReadOnlyCollection<LancamentoListaDto>> ListarAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Retorna um lançamento pelo seu identificador.
    /// </summary>
    /// <param name="lancamentoId">Identificador do lançamento.</param>
    /// <param name="cancellationToken">Token utilizado para cancelar a operação.</param>
    /// <returns>Os dados detalhados do lançamento.</returns>
    Task<LancamentoDetalheDto> ObterPorIdAsync(int lancamentoId, CancellationToken cancellationToken);
}
