using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Patrimonio.Financas.Contracts.Instituicoes.Dtos;
using Patrimonio.Financas.Contracts.Instituicoes.Services;
using Patrimonio.Financas.Domain.Exceptions;
using Patrimonio.Financas.Tests.Integration.Base;
using Patrimonio.Financas.Tests.Integration.Config;

namespace Patrimonio.Financas.Tests.Integration.Tests.Instituicoes.Contracts;

public sealed class InstituicaoCommandServiceTests(IntegrationTestFactory factory) : IntegrationTestBase(factory)
{
    // ============================================================================
    // DTOs
    // ============================================================================

    private static InstituicaoCriacaoDto CriarDto()
    {
        return new InstituicaoCriacaoDto
        {
            Nome = $"Instituição {Guid.NewGuid()}"
        };
    }

    private static InstituicaoAlteracaoDto AlterarDto()
    {
        return new InstituicaoAlteracaoDto
        {
            Nome = $"Instituição alterada {Guid.NewGuid()}"
        };
    }

    // ============================================================================
    // CriarAsync
    // ============================================================================

    [Fact]
    public async Task CriarAsync_DeveCriarInstituicaoERetornarDto()
    {
        using var scope = CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<IInstituicaoCommandService>();

        var criado = await service.CriarAsync(CriarDto(), TestContext.Current.CancellationToken);

        criado.Should().NotBeNull();
        criado.Nome.Should().NotBeNullOrWhiteSpace();
        criado.Id.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task CriarAsync_DeveLancarExcecaoAoDuplicarNome()
    {
        using var scope = CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<IInstituicaoCommandService>();
        var dto = CriarDto();

        await service.CriarAsync(dto, TestContext.Current.CancellationToken);

        var action = () => service.CriarAsync(dto, TestContext.Current.CancellationToken);

        await action.Should().ThrowAsync<ConflitoException>();
    }

    // ============================================================================
    // AlterarAsync
    // ============================================================================

    [Fact]
    public async Task AlterarAsync_DeveAlterarInstituicao()
    {
        using var scope = CreateScope();
        var dtoAlterar = AlterarDto();
        var command = scope.ServiceProvider.GetRequiredService<IInstituicaoCommandService>();
        var query = scope.ServiceProvider.GetRequiredService<IInstituicaoQueryService>();

        var instituicao = await command.CriarAsync(CriarDto(), TestContext.Current.CancellationToken);

        await command.AlterarAsync(instituicao.Id, dtoAlterar, TestContext.Current.CancellationToken);

        var alterado = await query.ObterPorIdAsync(instituicao.Id, TestContext.Current.CancellationToken);

        alterado.Should().NotBeNull();
        alterado.Nome.Should().Be(dtoAlterar.Nome);
    }

    [Fact]
    public async Task AlterarAsync_DeveLancarExcecaoAoDuplicarNome()
    {
        using var scope = CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<IInstituicaoCommandService>();
        var dto = new InstituicaoAlteracaoDto
        {
            Nome = Factory.BaseData.Instituicao.Nome
        };

        var instituicao = await service.CriarAsync(CriarDto(), TestContext.Current.CancellationToken);

        var action = () => service.AlterarAsync(
            instituicao.Id,
            dto,
            TestContext.Current.CancellationToken);

        await action.Should().ThrowAsync<ConflitoException>();
    }

    [Fact]
    public async Task AlterarAsync_DeveLancarExcecaoAoAlterarInstituicaoInexistente()
    {
        using var scope = CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<IInstituicaoCommandService>();

        var action = () => service.AlterarAsync(
            int.MaxValue,
            AlterarDto(),
            TestContext.Current.CancellationToken);

        await action.Should().ThrowAsync<RecursoNaoEncontradoException>();
    }

    // ============================================================================
    // ExcluirAsync
    // ============================================================================

    [Fact]
    public async Task ExcluirAsync_DeveExcluirInstituicao()
    {
        using var scope = CreateScope();
        var command = scope.ServiceProvider.GetRequiredService<IInstituicaoCommandService>();
        var query = scope.ServiceProvider.GetRequiredService<IInstituicaoQueryService>();

        var criado = await command.CriarAsync(CriarDto(), TestContext.Current.CancellationToken);

        await command.ExcluirAsync(criado.Id, TestContext.Current.CancellationToken);

        var action = () => query.ObterPorIdAsync(criado.Id, TestContext.Current.CancellationToken);
        await action.Should().ThrowAsync<RecursoNaoEncontradoException>();
    }

    [Fact]
    public async Task ExcluirAsync_DeveLancarExcecaoAoExcluirInstituicaoBase()
    {
        using var scope = CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<IInstituicaoCommandService>();

        var action = () => service.ExcluirAsync(Factory.BaseData.Instituicao.Id, TestContext.Current.CancellationToken);

        await action.Should().ThrowAsync<ConflitoException>();
    }

    [Fact]
    public async Task ExcluirAsync_DeveLancarExcecaoAoExcluirInstituicaoInexistente()
    {
        using var scope = CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<IInstituicaoCommandService>();

        var action = () => service.ExcluirAsync(
            int.MaxValue,
            TestContext.Current.CancellationToken);

        await action.Should().ThrowAsync<RecursoNaoEncontradoException>();
    }
}
