using Patrimonio.Financas.Contracts.Cartoes.Dtos;

namespace Patrimonio.Financas.Contracts.Cartoes.Services;

/// <summary>
/// Define as operações de escrita disponíveis para cartões.
/// </summary>
public interface ICartaoCommandService
{
    /// <summary>
    /// Cria um novo cartão.
    /// </summary>
    /// <param name="dto">Dados necessários para a criação do cartão.</param>
    /// <param name="cancellationToken">Token utilizado para cancelar a operação.</param>
    /// <returns>Os dados detalhados do cartão criado.</returns>
    Task<CartaoDetalheDto> CriarAsync(CartaoCriacaoDto dto, CancellationToken cancellationToken);

    /// <summary>
    /// Altera um cartão existente.
    /// </summary>
    /// <param name="cartaoId">Identificador do cartão que será alterado.</param>
    /// <param name="dto">Dados da alteração.</param>
    /// <param name="cancellationToken">Token utilizado para cancelar a operação.</param>
    Task AlterarAsync(int cartaoId, CartaoAlteracaoDto dto, CancellationToken cancellationToken);

    /// <summary>
    /// Exclui um cartão existente.
    /// </summary>
    /// <param name="cartaoId">Identificador do cartão que será excluído.</param>
    /// <param name="cancellationToken">Token utilizado para cancelar a operação.</param>
    Task ExcluirAsync(int cartaoId, CancellationToken cancellationToken);
}
