using Patrimonio.Financas.Application.Contas.Lookups.ReadModels;

namespace Patrimonio.Financas.Application.Contas.Lookups.Abstractions;

/// <summary>
/// Define as operações de consultas de referência de contas.
/// </summary>
public interface IContaLookupRepository
{
    /// <summary>
    /// Lista as opções de referência disponíveis para contas.
    /// </summary>
    /// <param name="cancellationToken">Token utilizado para cancelar a operação.</param>
    /// <returns>ReadModels contendo as opções de referência de contas.</returns>
    Task<IReadOnlyCollection<ContaOpcaoReadModel>> ListarOpcoesAsync(CancellationToken cancellationToken);
}
