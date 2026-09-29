using Patrimonio.Financas.Contracts.Categorias.Dtos;

namespace Patrimonio.Financas.Contracts.Categorias.Services;

/// <summary>
/// Define as operações de leitura disponíveis para categorias.
/// </summary>
public interface ICategoriaQueryService
{
    /// <summary>
    /// Retorna todas as categorias.
    /// </summary>
    /// <param name="cancellationToken">Token utilizado para cancelar a operação.</param>
    /// <returns>Uma coleção com as categorias encontradas.</returns>
    Task<IReadOnlyCollection<CategoriaListaDto>> ListarAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Retorna uma categoria pelo seu identificador.
    /// </summary>
    /// <param name="categoriaId">Identificador da categoria.</param>
    /// <param name="cancellationToken">Token utilizado para cancelar a operação.</param>
    /// <returns>Os dados detalhados da categoria.</returns>
    Task<CategoriaDetalheDto> ObterPorIdAsync(int categoriaId, CancellationToken cancellationToken);
}
