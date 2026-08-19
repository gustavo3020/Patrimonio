using Patrimonio.Financas.Contracts.Categorias.Dtos;

namespace Patrimonio.Financas.Contracts.Categorias.Services;

/// <summary>
/// Define as operações de escrita disponíveis para categorias.
/// </summary>
public interface ICategoriaCommandService
{
    /// <summary>
    /// Cria uma nova categoria.
    /// </summary>
    /// <param name="dto">Dados necessários para a criação da categoria.</param>
    /// <param name="cancellationToken">Token utilizado para cancelar a operação.</param>
    /// <returns>Os dados detalhados da categoria criada.</returns>
    Task<CategoriaDetalheDto> CriarAsync(CategoriaCriacaoDto dto, CancellationToken cancellationToken);

    /// <summary>
    /// Altera uma categoria existente.
    /// </summary>
    /// <param name="categoriaId">Identificador da categoria que será alterada.</param>
    /// <param name="dto">Dados da alteração.</param>
    /// <param name="cancellationToken">Token utilizado para cancelar a operação.</param>
    Task AlterarAsync(int categoriaId, CategoriaAlteracaoDto dto, CancellationToken cancellationToken);

    /// <summary>
    /// Exclui uma categoria existente.
    /// </summary>
    /// <param name="categoriaId">Identificador da categoria que será excluída.</param>
    /// <param name="cancellationToken">Token utilizado para cancelar a operação.</param>
    Task ExcluirAsync(int categoriaId, CancellationToken cancellationToken);
}
