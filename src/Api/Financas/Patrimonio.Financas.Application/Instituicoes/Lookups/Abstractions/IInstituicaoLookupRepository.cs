using Patrimonio.Financas.Application.Instituicoes.Lookups.ReadModels;

namespace Patrimonio.Financas.Application.Instituicoes.Lookups.Abstractions;

/// <summary>
/// Define as operações de consultas de referência de instituições.
/// </summary>
public interface IInstituicaoLookupRepository
{
    /// <summary>
    /// Lista as opções de referência disponíveis para instituições.
    /// </summary>
    /// <param name="cancellationToken">Token utilizado para cancelar a operação.</param>
    /// <returns>ReadModels contendo as opções de referência de instituições.</returns>
    Task<IReadOnlyCollection<InstituicaoOpcaoReadModel>> ListarOpcoesAsync(CancellationToken cancellationToken);
}
