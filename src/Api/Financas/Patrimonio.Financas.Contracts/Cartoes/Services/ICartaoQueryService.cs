using Patrimonio.Financas.Contracts.Cartoes.Dtos;

namespace Patrimonio.Financas.Contracts.Cartoes.Services;

/// <summary>
/// Define as operações de leitura disponíveis para cartões.
/// </summary>
public interface ICartaoQueryService
{
    /// <summary>
    /// Retorna todos os cartões.
    /// </summary>
    /// <param name="cancellationToken">Token utilizado para cancelar a operação.</param>
    /// <returns>Uma coleção com os cartões encontrados.</returns>
    Task<IReadOnlyCollection<CartaoListaDto>> ListarAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Retorna um cartão pelo seu identificador.
    /// </summary>
    /// <param name="cartaoId">Identificador do cartão.</param>
    /// <param name="cancellationToken">Token utilizado para cancelar a operação.</param>
    /// <returns>Os dados detalhados do cartão.</returns>
    Task<CartaoDetalheDto> ObterPorIdAsync(int cartaoId, CancellationToken cancellationToken);
}
