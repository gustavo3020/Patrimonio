using Patrimonio.Financas.Contracts.Categorias.Dtos;
using Patrimonio.Financas.Contracts.Categorias.Services;
using Patrimonio.Financas.Domain.Exceptions;
using Patrimonio.Financas.Tests.Integration.Base;

namespace Patrimonio.Financas.Tests.Integration.Seeders;

/// <summary>
/// Fornece os dados base de categorias necessários aos testes de integração.
/// </summary>
internal sealed class CategoriaSeeder(
    ICategoriaCommandService commandService,
    ICategoriaQueryService queryService)
{
    public async Task SeedAsync(BaseData data)
    {
        CategoriaDetalheDto categoria;

        try
        {
            categoria = await queryService.ObterPorIdAsync(1, TestContext.Current.CancellationToken);
        }
        catch (RecursoNaoEncontradoException)
        {
            var categoriaDto = Criar();

            categoria = await commandService.CriarAsync(categoriaDto, TestContext.Current.CancellationToken);
        }

        data.Categoria = categoria;
    }

    private static CategoriaCriacaoDto Criar()
    {
        return new CategoriaCriacaoDto
        {
            Nome = "Categoria de teste",
        };
    }
}
