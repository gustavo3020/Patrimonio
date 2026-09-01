using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Patrimonio.Financas.Contracts.Categorias.Dtos;
using Patrimonio.Financas.Contracts.Categorias.Services;
using Patrimonio.Financas.Domain.Exceptions;
using Patrimonio.Financas.Tests.Integration.Base;
using Patrimonio.Financas.Tests.Integration.Config;

namespace Patrimonio.Financas.Tests.Integration.Tests.Categorias.Contracts;

public sealed class CategoriaCommandServiceTests(IntegrationTestFactory factory) : IntegrationTestBase(factory)
{
    // ============================================================================
    // DTOs
    // ============================================================================

    private static CategoriaCriacaoDto CriarDto()
    {
        return new CategoriaCriacaoDto
        {
            Nome = $"Categoria {Guid.NewGuid()}"
        };
    }

    private static CategoriaAlteracaoDto AlterarDto()
    {
        return new CategoriaAlteracaoDto
        {
            Nome = $"Categoria alterada {Guid.NewGuid()}"
        };
    }

    // ============================================================================
    // CriarAsync
    // ============================================================================

    [Fact]
    public async Task CriarAsync_DeveCriarCategoriaERetornarDto()
    {
        using var scope = CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<ICategoriaCommandService>();
        var dto = CriarDto();

        var criado = await service.CriarAsync(dto, CancellationToken.None);

        criado.Should().NotBeNull();
        criado.Nome.Should().NotBeNullOrWhiteSpace();
        criado.Id.Should().BeGreaterThan(0);
        criado.Nome.Should().Be(dto.Nome);
    }

    [Fact]
    public async Task CriarAsync_DeveLancarExcecaoAoDuplicarNome()
    {
        using var scope = CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<ICategoriaCommandService>();
        var dto = CriarDto();

        await service.CriarAsync(dto, CancellationToken.None);

        var action = () => service.CriarAsync(dto, CancellationToken.None);

        await action.Should().ThrowAsync<ConflitoException>();
    }

    // ============================================================================
    // AlterarAsync
    // ============================================================================

    [Fact]
    public async Task AlterarAsync_DeveAlterarCategoria()
    {
        using var scope = CreateScope();
        var dtoAlterar = AlterarDto();
        var command = scope.ServiceProvider.GetRequiredService<ICategoriaCommandService>();
        var query = scope.ServiceProvider.GetRequiredService<ICategoriaQueryService>();

        var categoria = await command.CriarAsync(CriarDto(), CancellationToken.None);

        await command.AlterarAsync(categoria.Id, dtoAlterar, CancellationToken.None);

        var alterado = await query.ObterPorIdAsync(categoria.Id, CancellationToken.None);

        alterado.Should().NotBeNull();
        alterado.Nome.Should().Be(dtoAlterar.Nome);
    }

    [Fact]
    public async Task AlterarAsync_DeveLancarExcecaoAoDuplicarNome()
    {
        using var scope = CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<ICategoriaCommandService>();
        var dto = new CategoriaAlteracaoDto
        {
            Nome = Factory.BaseData.Categoria.Nome
        };

        var categoria = await service.CriarAsync(CriarDto(), CancellationToken.None);

        var action = () => service.AlterarAsync(
            categoria.Id,
            dto,
            CancellationToken.None);

        await action.Should().ThrowAsync<ConflitoException>();
    }

    [Fact]
    public async Task AlterarAsync_DeveLancarExcecaoAoAlterarCategoriaInexistente()
    {
        using var scope = CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<ICategoriaCommandService>();

        var action = () => service.AlterarAsync(
            int.MaxValue,
            AlterarDto(),
            CancellationToken.None);

        await action.Should().ThrowAsync<RecursoNaoEncontradoException>();
    }

    // ============================================================================
    // ExcluirAsync
    // ============================================================================

    [Fact]
    public async Task ExcluirAsync_DeveExcluirCategoria()
    {
        using var scope = CreateScope();
        var command = scope.ServiceProvider.GetRequiredService<ICategoriaCommandService>();
        var query = scope.ServiceProvider.GetRequiredService<ICategoriaQueryService>();

        var criado = await command.CriarAsync(CriarDto(), CancellationToken.None);

        await command.ExcluirAsync(criado.Id, CancellationToken.None);

        var action = () => query.ObterPorIdAsync(criado.Id, CancellationToken.None);
        await action.Should().ThrowAsync<RecursoNaoEncontradoException>();
    }

    [Fact]
    public async Task ExcluirAsync_DeveLancarExcecaoAoExcluirCategoriaBase()
    {
        using var scope = CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<ICategoriaCommandService>();

        var action = () => service.ExcluirAsync(Factory.BaseData.Categoria.Id, CancellationToken.None);

        await action.Should().ThrowAsync<ConflitoException>();
    }

    [Fact]
    public async Task ExcluirAsync_DeveLancarExcecaoAoExcluirCategoriaInexistente()
    {
        using var scope = CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<ICategoriaCommandService>();

        var action = () => service.ExcluirAsync(
            int.MaxValue,
            CancellationToken.None);

        await action.Should().ThrowAsync<RecursoNaoEncontradoException>();
    }
}
