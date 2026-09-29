using Patrimonio.Financas.Contracts.Movimentacoes.Dtos;

namespace Patrimonio.Financas.Contracts.Movimentacoes.Services;

/// <summary>
/// Define as operações de escrita disponíveis para movimentações.
/// </summary>
public interface IMovimentacaoCommandService
{
    /// <summary>
    /// Cria uma nova movimentação.
    /// </summary>
    /// <param name="dto">Dados necessários para a criação da movimentação.</param>
    /// <param name="cancellationToken">Token utilizado para cancelar a operação.</param>
    /// <returns>Os dados detalhados da movimentação criada.</returns>
    Task<MovimentacaoDetalheDto> CriarAsync(MovimentacaoCriacaoDto dto, CancellationToken cancellationToken);

    /// <summary>
    /// Altera uma movimentação existente.
    /// </summary>
    /// <param name="movimentacaoId">Identificador da movimentação que será alterada.</param>
    /// <param name="dto">Dados da alteração.</param>
    /// <param name="cancellationToken">Token utilizado para cancelar a operação.</param>
    Task AlterarAsync(int movimentacaoId, MovimentacaoAlteracaoDto dto, CancellationToken cancellationToken);

    /// <summary>
    /// Exclui uma movimentação existente.
    /// </summary>
    /// <param name="movimentacaoId">Identificador da movimentação que será excluída.</param>
    /// <param name="cancellationToken">Token utilizado para cancelar a operação.</param>
    Task ExcluirAsync(int movimentacaoId, CancellationToken cancellationToken);
}
