using Patrimonio.Financas.Contracts.Cartoes.Dtos;

namespace Patrimonio.Financas.Contracts.Cartoes.Services;

/// <summary>
/// Define as operações de escrita disponíveis para lançamentos.
/// </summary>
public interface ILancamentoCommandService
{
    /// <summary>
    /// Cria um novo lançamento.
    /// </summary>
    /// <param name="dto">Dados necessários para a criação do lançamento.</param>
    /// <param name="cancellationToken">Token utilizado para cancelar a operação.</param>
    /// <returns>Os dados detalhados do lançamento criado.</returns>
    Task<LancamentoDetalheDto> CriarAsync(LancamentoCriacaoDto dto, CancellationToken cancellationToken);

    /// <summary>
    /// Altera um lançamento existente.
    /// </summary>
    /// <param name="lancamentoId">Identificador do lançamento que será alterado.</param>
    /// <param name="dto">Dados da alteração.</param>
    /// <param name="cancellationToken">Token utilizado para cancelar a operação.</param>
    Task AlterarAsync(int lancamentoId, LancamentoAlteracaoDto dto, CancellationToken cancellationToken);

    /// <summary>
    /// Exclui um lançamento existente.
    /// </summary>
    /// <param name="lancamentoId">Identificador do lançamento que será excluído.</param>
    /// <param name="cancellationToken">Token utilizado para cancelar a operação.</param>
    Task ExcluirAsync(int lancamentoId, CancellationToken cancellationToken);
}
