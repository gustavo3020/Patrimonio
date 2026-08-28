using Patrimonio.Financas.Application.Categorias.Queries.ReadModels;

namespace Patrimonio.Financas.Application.Categorias.Queries.Abstractions;

/// <summary>
/// Define as operações de leitura necessárias para categorias.
/// </summary>
public interface ICategoriaQueryRepository
{
    /// <summary>
    /// Lista as categorias disponíveis.
    /// </summary>
    /// <param name="cancellationToken">Token utilizado para cancelar a operação.</param>
    /// <returns>ReadModels contendo os dados das categorias.</returns>
    Task<IReadOnlyCollection<CategoriaListaReadModel>> ListarAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Obtém uma categoria pelo seu identificador.
    /// </summary>
    /// <param name="categoriaId">Identificador da categoria.</param>
    /// <param name="cancellationToken">Token utilizado para cancelar a operação.</param>
    /// <returns>ReadModel contendo os dados detalhados da categoria.</returns>
    Task<CategoriaDetalheReadModel> ObterPorIdAsync(int categoriaId, CancellationToken cancellationToken);
}
