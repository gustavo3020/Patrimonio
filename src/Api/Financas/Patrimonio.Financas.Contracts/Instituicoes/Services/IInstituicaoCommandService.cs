using Patrimonio.Financas.Contracts.Instituicoes.Dtos;

namespace Patrimonio.Financas.Contracts.Instituicoes.Services;

/// <summary>
/// Define as operações de escrita disponíveis para instituições.
/// </summary>
public interface IInstituicaoCommandService
{
    /// <summary>
    /// Cria uma nova instituição.
    /// </summary>
    /// <param name="dto">Dados necessários para a criação da instituição.</param>
    /// <param name="cancellationToken">Token utilizado para cancelar a operação.</param>
    /// <returns>Os dados detalhados da instituição criada.</returns>
    Task<InstituicaoDetalheDto> CriarAsync(InstituicaoCriacaoDto dto, CancellationToken cancellationToken);

    /// <summary>
    /// Altera uma instituição existente.
    /// </summary>
    /// <param name="instituicaoId">Identificador da instituição que será alterada.</param>
    /// <param name="dto">Dados da alteração.</param>
    /// <param name="cancellationToken">Token utilizado para cancelar a operação.</param>
    Task AlterarAsync(int instituicaoId, InstituicaoAlteracaoDto dto, CancellationToken cancellationToken);

    /// <summary>
    /// Exclui uma instituição existente.
    /// </summary>
    /// <param name="instituicaoId">Identificador da instituição que será excluída.</param>
    /// <param name="cancellationToken">Token utilizado para cancelar a operação.</param>
    Task ExcluirAsync(int instituicaoId, CancellationToken cancellationToken);
}
