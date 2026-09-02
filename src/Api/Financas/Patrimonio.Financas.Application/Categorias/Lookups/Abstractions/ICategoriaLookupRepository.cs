using Patrimonio.Financas.Application.Categorias.Lookups.ReadModels;

namespace Patrimonio.Financas.Application.Categorias.Lookups.Abstractions;

/// <summary>
/// Define as operações de consultas de referência de categorias.
/// </summary>
public interface ICategoriaLookupRepository
{
    /// <summary>
    /// Lista as opções de referência disponíveis para categorias.
    /// </summary>
    /// <param name="cancellationToken">Token utilizado para cancelar a operação.</param>
    /// <returns>ReadModels contendo as opções de referência de categorias.</returns>
    Task<IReadOnlyCollection<CategoriaOpcaoReadModel>> ListarOpcoesAsync(CancellationToken cancellationToken);
}
