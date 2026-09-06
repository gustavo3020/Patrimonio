using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Patrimonio.Financas.Contracts.Categorias.Services;
using Patrimonio.Financas.Domain.Exceptions;
using Patrimonio.Financas.Tests.Integration.Base;
using Patrimonio.Financas.Tests.Integration.Config;

namespace Patrimonio.Financas.Tests.Integration.Tests.Categorias.Contracts;

public sealed class CategoriaQueryServiceTests(IntegrationTestFactory factory) : IntegrationTestBase(factory)
{
    // ============================================================================
    // ListarAsync
    // ============================================================================

    [Fact]
    public async Task ListarAsync_DeveListarCategorias()
    {
        using var scope = CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<ICategoriaQueryService>();

        var categorias = await service.ListarAsync(CancellationToken.None);

        categorias.Should().NotBeNull();
        categorias.Should().Contain(c => c.Id == Factory.BaseData.Categoria.Id);
    }

    // ============================================================================
    // ObterPorIdAsync
    // ============================================================================

    [Fact]
    public async Task ObterPorIdAsync_DeveObterCategoriaPorId()
    {
        using var scope = CreateScope();

        var service = scope.ServiceProvider.GetRequiredService<ICategoriaQueryService>();

        var categoriaEsperada = Factory.BaseData.Categoria;

        var categoria = await service.ObterPorIdAsync(categoriaEsperada.Id, CancellationToken.None);

        categoria.Should().NotBeNull();
        categoria.Id.Should().Be(categoriaEsperada.Id);
        categoria.Nome.Should().Be(categoriaEsperada.Nome);
    }

    [Fact]
    public async Task ObterPorIdAsync_DeveLancarExcecaoAoObterCategoriaInexistente()
    {
        using var scope = CreateScope();

        var service = scope.ServiceProvider.GetRequiredService<ICategoriaQueryService>();

        var acao = () => service.ObterPorIdAsync(int.MaxValue, CancellationToken.None);

        await acao.Should().ThrowAsync<RecursoNaoEncontradoException>();
    }
}
