using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Patrimonio.Financas.Contracts.Categorias.Services;
using Patrimonio.Financas.Domain.Exceptions;
using Patrimonio.Financas.Tests.Integration.Base;
using Patrimonio.Financas.Tests.Integration.Builders;
using Patrimonio.Financas.Tests.Integration.Config;
using Patrimonio.Financas.Tests.Integration.Fixtures;

namespace Patrimonio.Financas.Tests.Integration.Tests.Categorias.Contracts;

public sealed class CategoriaCommandServiceTests(IntegrationTestFactory factory)
    : IntegrationTestBase(factory)
{
    // ============================================================================
    // CriarAsync
    // ============================================================================

    [Fact]
    public async Task CriarAsync_DeveCriarCategoria_QuandoDadosValidos()
    {
        using var scope = CreateScope();
        var command = scope.ServiceProvider.GetRequiredService<ICategoriaCommandService>();
        var dto = CategoriaDtoBuilder.Criar();

        var categoria = await command.CriarAsync(dto, TestContext.Current.CancellationToken);

        categoria.Should().NotBeNull();
        categoria.Nome.Should().NotBeNullOrWhiteSpace();
        categoria.Id.Should().BeGreaterThan(0);
        categoria.Nome.Should().Be(dto.Nome);
    }

    [Fact]
    public async Task CriarAsync_DeveLancarExcecao_QuandoDuplicarNome()
    {
        using var scope = CreateScope();
        var command = scope.ServiceProvider.GetRequiredService<ICategoriaCommandService>();
        var dto = CategoriaDtoBuilder.Criar(Factory.BaseData.Categoria.Nome);

        var acao = () => command.CriarAsync(dto, TestContext.Current.CancellationToken);

        await acao.Should().ThrowAsync<ConflitoException>();
    }

    // ============================================================================
    // AlterarAsync
    // ============================================================================

    [Fact]
    public async Task AlterarAsync_DeveAlterarCategoria_QuandoDadosValidos()
    {
        using var scope = CreateScope();
        var command = scope.ServiceProvider.GetRequiredService<ICategoriaCommandService>();
        var query = scope.ServiceProvider.GetRequiredService<ICategoriaQueryService>();
        var dto = CategoriaDtoBuilder.Alterar();

        var categoria = await CategoriaFixture.CriarAsync(Factory);

        await command.AlterarAsync(categoria.Id, dto, TestContext.Current.CancellationToken);

        var alterado = await query.ObterPorIdAsync(categoria.Id, TestContext.Current.CancellationToken);

        alterado.Should().NotBeNull();
        alterado.Nome.Should().Be(dto.Nome);
    }

    [Fact]
    public async Task AlterarAsync_DeveLancarExcecao_QuandoDuplicarNome()
    {
        using var scope = CreateScope();
        var command = scope.ServiceProvider.GetRequiredService<ICategoriaCommandService>();
        var dto = CategoriaDtoBuilder.Alterar(Factory.BaseData.Categoria.Nome);

        var categoria = await CategoriaFixture.CriarAsync(Factory);

        var acao = () => command.AlterarAsync(
            categoria.Id,
            dto,
            TestContext.Current.CancellationToken);

        await acao.Should().ThrowAsync<ConflitoException>();
    }

    [Fact]
    public async Task AlterarAsync_DeveLancarExcecao_QuandoCategoriaInexistente()
    {
        using var scope = CreateScope();
        var command = scope.ServiceProvider.GetRequiredService<ICategoriaCommandService>();

        var acao = () => command.AlterarAsync(
            int.MaxValue,
            CategoriaDtoBuilder.Alterar(),
            TestContext.Current.CancellationToken);

        await acao.Should().ThrowAsync<RecursoNaoEncontradoException>();
    }

    // ============================================================================
    // ExcluirAsync
    // ============================================================================

    [Fact]
    public async Task ExcluirAsync_DeveExcluirCategoria_QuandoDadosValidos()
    {
        using var scope = CreateScope();
        var command = scope.ServiceProvider.GetRequiredService<ICategoriaCommandService>();
        var query = scope.ServiceProvider.GetRequiredService<ICategoriaQueryService>();

        var categoria = await CategoriaFixture.CriarAsync(Factory);

        await command.ExcluirAsync(categoria.Id, TestContext.Current.CancellationToken);

        var acao = () => query.ObterPorIdAsync(categoria.Id, TestContext.Current.CancellationToken);
        await acao.Should().ThrowAsync<RecursoNaoEncontradoException>();
    }

    [Fact]
    public async Task ExcluirAsync_DeveLancarExcecao_QuandoCategoriaBase()
    {
        using var scope = CreateScope();
        var command = scope.ServiceProvider.GetRequiredService<ICategoriaCommandService>();

        var acao = () => command.ExcluirAsync(Factory.BaseData.Categoria.Id, TestContext.Current.CancellationToken);

        await acao.Should().ThrowAsync<ConflitoException>();
    }

    [Fact]
    public async Task ExcluirAsync_DeveLancarExcecao_QuandoCategoriaInexistente()
    {
        using var scope = CreateScope();
        var command = scope.ServiceProvider.GetRequiredService<ICategoriaCommandService>();

        var acao = () => command.ExcluirAsync(
            int.MaxValue,
            TestContext.Current.CancellationToken);

        await acao.Should().ThrowAsync<RecursoNaoEncontradoException>();
    }
}
