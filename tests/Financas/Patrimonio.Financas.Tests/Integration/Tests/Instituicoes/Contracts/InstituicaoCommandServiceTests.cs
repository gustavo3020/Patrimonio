using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Patrimonio.Financas.Contracts.Instituicoes.Services;
using Patrimonio.Financas.Domain.Exceptions;
using Patrimonio.Financas.Tests.Integration.Base;
using Patrimonio.Financas.Tests.Integration.Builders;
using Patrimonio.Financas.Tests.Integration.Config;
using Patrimonio.Financas.Tests.Integration.Fixtures;

namespace Patrimonio.Financas.Tests.Integration.Tests.Instituicoes.Contracts;

public sealed class InstituicaoCommandServiceTests(IntegrationTestFactory factory) : IntegrationTestBase(factory)
{
    // ============================================================================
    // CriarAsync
    // ============================================================================

    [Fact]
    public async Task CriarAsync_DeveCriarInstituicao_QuandoDadosValidos()
    {
        using var scope = CreateScope();
        var command = scope.ServiceProvider.GetRequiredService<IInstituicaoCommandService>();
        var dto = InstituicaoDtoBuilder.Criar();

        var instituicao = await command.CriarAsync(dto, TestContext.Current.CancellationToken);

        instituicao.Should().NotBeNull();
        instituicao.Nome.Should().NotBeNullOrWhiteSpace();
        instituicao.Id.Should().BeGreaterThan(0);
        instituicao.Nome.Should().Be(dto.Nome);
    }

    [Fact]
    public async Task CriarAsync_DeveLancarExcecao_QuandoDuplicarNome()
    {
        using var scope = CreateScope();
        var command = scope.ServiceProvider.GetRequiredService<IInstituicaoCommandService>();
        var dto = InstituicaoDtoBuilder.Criar(Factory.BaseData.Instituicao.Nome);

        var acao = () => command.CriarAsync(dto, TestContext.Current.CancellationToken);

        await acao.Should().ThrowAsync<ConflitoException>();
    }

    // ============================================================================
    // AlterarAsync
    // ============================================================================

    [Fact]
    public async Task AlterarAsync_DeveAlterarInstituicao_QuandoDadosValidos()
    {
        using var scope = CreateScope();
        var command = scope.ServiceProvider.GetRequiredService<IInstituicaoCommandService>();
        var query = scope.ServiceProvider.GetRequiredService<IInstituicaoQueryService>();
        var dto = InstituicaoDtoBuilder.Alterar();

        var instituicao = await InstituicaoFixture.CriarAsync(Factory);

        await command.AlterarAsync(instituicao.Id, dto, TestContext.Current.CancellationToken);

        var alterado = await query.ObterPorIdAsync(instituicao.Id, TestContext.Current.CancellationToken);

        alterado.Should().NotBeNull();
        alterado.Nome.Should().Be(dto.Nome);
    }

    [Fact]
    public async Task AlterarAsync_DeveLancarExcecao_QuandoDuplicarNome()
    {
        using var scope = CreateScope();
        var command = scope.ServiceProvider.GetRequiredService<IInstituicaoCommandService>();
        var dto = InstituicaoDtoBuilder.Alterar(Factory.BaseData.Instituicao.Nome);

        var instituicao = await InstituicaoFixture.CriarAsync(Factory);

        var acao = () => command.AlterarAsync(
            instituicao.Id,
            dto,
            TestContext.Current.CancellationToken);

        await acao.Should().ThrowAsync<ConflitoException>();
    }

    [Fact]
    public async Task AlterarAsync_DeveLancarExcecao_QuandoInstituicaoInexistente()
    {
        using var scope = CreateScope();
        var command = scope.ServiceProvider.GetRequiredService<IInstituicaoCommandService>();
        var dto = InstituicaoDtoBuilder.Alterar();

        var acao = () => command.AlterarAsync(
            int.MaxValue,
            dto,
            TestContext.Current.CancellationToken);

        await acao.Should().ThrowAsync<RecursoNaoEncontradoException>();
    }

    // ============================================================================
    // ExcluirAsync
    // ============================================================================

    [Fact]
    public async Task ExcluirAsync_DeveExcluirInstituicao_QuandoDadosValidos()
    {
        using var scope = CreateScope();
        var command = scope.ServiceProvider.GetRequiredService<IInstituicaoCommandService>();
        var query = scope.ServiceProvider.GetRequiredService<IInstituicaoQueryService>();

        var instituicao = await InstituicaoFixture.CriarAsync(Factory);

        await command.ExcluirAsync(instituicao.Id, TestContext.Current.CancellationToken);

        var acao = () => query.ObterPorIdAsync(instituicao.Id, TestContext.Current.CancellationToken);
        await acao.Should().ThrowAsync<RecursoNaoEncontradoException>();
    }

    [Fact]
    public async Task ExcluirAsync_DeveLancarExcecao_QuandoInstituicaoBase()
    {
        using var scope = CreateScope();
        var command = scope.ServiceProvider.GetRequiredService<IInstituicaoCommandService>();

        var acao = () => command.ExcluirAsync(Factory.BaseData.Instituicao.Id, TestContext.Current.CancellationToken);

        await acao.Should().ThrowAsync<ConflitoException>();
    }

    [Fact]
    public async Task ExcluirAsync_DeveLancarExcecao_QuandoInstituicaoInexistente()
    {
        using var scope = CreateScope();
        var command = scope.ServiceProvider.GetRequiredService<IInstituicaoCommandService>();

        var acao = () => command.ExcluirAsync(
            int.MaxValue,
            TestContext.Current.CancellationToken);

        await acao.Should().ThrowAsync<RecursoNaoEncontradoException>();
    }
}
