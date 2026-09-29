using Patrimonio.Financas.Contracts.Contas.Dtos;

namespace Patrimonio.Financas.Contracts.Contas.Services;

/// <summary>
/// Define as operações de leitura disponíveis para contas.
/// </summary>
public interface IContaQueryService
{
    /// <summary>
    /// Retorna todas as contas.
    /// </summary>
    /// <param name="cancellationToken">Token utilizado para cancelar a operação.</param>
    /// <returns>Uma coleção com as contas encontradas.</returns>
    Task<IReadOnlyCollection<ContaListaDto>> ListarAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Retorna uma conta pelo seu identificador.
    /// </summary>
    /// <param name="contaId">Identificador da conta.</param>
    /// <param name="cancellationToken">Token utilizado para cancelar a operação.</param>
    /// <returns>Os dados detalhados da conta.</returns>
    Task<ContaDetalheDto> ObterPorIdAsync(int contaId, CancellationToken cancellationToken);
}
