using Patrimonio.Financas.Contracts.Contas.Dtos;

namespace Patrimonio.Financas.Contracts.Contas.Services;

/// <summary>
/// Define as operações de escrita disponíveis para contas.
/// </summary>
public interface IContaCommandService
{
    /// <summary>
    /// Cria uma nova conta.
    /// </summary>
    /// <param name="dto">Dados necessários para a criação da conta.</param>
    /// <param name="cancellationToken">Token utilizado para cancelar a operação.</param>
    /// <returns>Os dados detalhados da conta criada.</returns>
    Task<ContaDetalheDto> CriarAsync(ContaCriacaoDto dto, CancellationToken cancellationToken);

    /// <summary>
    /// Altera uma conta existente.
    /// </summary>
    /// <param name="contaId">Identificador da conta que será alterada.</param>
    /// <param name="dto">Dados da alteração.</param>
    /// <param name="cancellationToken">Token utilizado para cancelar a operação.</param>
    Task AlterarAsync(int contaId, ContaAlteracaoDto dto, CancellationToken cancellationToken);

    /// <summary>
    /// Exclui uma conta existente.
    /// </summary>
    /// <param name="contaId">Identificador da conta que será excluída.</param>
    /// <param name="cancellationToken">Token utilizado para cancelar a operação.</param>
    Task ExcluirAsync(int contaId, CancellationToken cancellationToken);
}
