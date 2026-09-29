using Patrimonio.Financas.Contracts.Movimentacoes.Dtos;

namespace Patrimonio.Financas.Contracts.Movimentacoes.Services;

/// <summary>
/// Define as operações de leitura disponíveis para movimentações.
/// </summary>
public interface IMovimentacaoQueryService
{
    /// <summary>
    /// Retorna todas as movimentações.
    /// </summary>
    /// <param name="cancellationToken">Token utilizado para cancelar a operação.</param>
    /// <returns>Uma coleção com as movimentações encontradas.</returns>
    Task<IReadOnlyCollection<MovimentacaoListaDto>> ListarAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Retorna uma movimentação pelo seu identificador.
    /// </summary>
    /// <param name="movimentacaoId">Identificador da movimentação.</param>
    /// <param name="cancellationToken">Token utilizado para cancelar a operação.</param>
    /// <returns>Os dados detalhados da movimentação.</returns>
    Task<MovimentacaoDetalheDto> ObterPorIdAsync(int movimentacaoId, CancellationToken cancellationToken);
}
