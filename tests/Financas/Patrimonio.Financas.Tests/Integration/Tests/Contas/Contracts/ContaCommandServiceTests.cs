using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Patrimonio.Financas.Contracts.Contas.Services;
using Patrimonio.Financas.Domain.Exceptions;
using Patrimonio.Financas.Tests.Integration.Base;
using Patrimonio.Financas.Tests.Integration.Builders;
using Patrimonio.Financas.Tests.Integration.Config;
using Patrimonio.Financas.Tests.Integration.Fixtures;

namespace Patrimonio.Financas.Tests.Integration.Tests.Contas.Contracts;

public sealed class ContaCommandServiceTests(IntegrationTestFactory factory) : IntegrationTestBase(factory)
{
    // ============================================================================
    // CriarAsync
    // ============================================================================

    [Fact]
    public async Task CriarAsync_DeveCriarConta_QuandoDadosValidos()
    {
        using var scope = CreateScope();
        var command = scope.ServiceProvider.GetRequiredService<IContaCommandService>();
        var dto = ContaDtoBuilder.Criar(Factory.BaseData.Instituicao.Id);

        var conta = await command.CriarAsync(dto, TestContext.Current.CancellationToken);

        conta.Should().NotBeNull();
        conta.Nome.Should().NotBeNullOrWhiteSpace();
        conta.Id.Should().BeGreaterThan(0);
        conta.Nome.Should().Be(dto.Nome);
        conta.InstituicaoId.Should().Be(Factory.BaseData.Instituicao.Id);
        conta.InstituicaoNome.Should().Be(Factory.BaseData.Instituicao.Nome);
    }

    [Fact]
    public async Task CriarAsync_DeveLancarExcecao_QuandoDuplicarNome()
    {
        using var scope = CreateScope();
        var command = scope.ServiceProvider.GetRequiredService<IContaCommandService>();
        var dto = ContaDtoBuilder.Criar(
            Factory.BaseData.Instituicao.Id,
            Factory.BaseData.Conta.Nome);

        var acao = () => command.CriarAsync(dto, TestContext.Current.CancellationToken);

        await acao.Should().ThrowAsync<ConflitoException>();
    }

    [Fact]
    public async Task CriarAsync_DeveLancarExcecao_QuandoInstituicaoInexistente()
    {
        using var scope = CreateScope();
        var command = scope.ServiceProvider.GetRequiredService<IContaCommandService>();
        var dto = ContaDtoBuilder.Criar(int.MaxValue);

        var acao = () => command.CriarAsync(dto, TestContext.Current.CancellationToken);

        await acao.Should().ThrowAsync<ConflitoException>();
    }

    // ============================================================================
    // AlterarAsync
    // ============================================================================

    [Fact]
    public async Task AlterarAsync_DeveAlterarConta_QuandoDadosValidos()
    {
        using var scope = CreateScope();
        var command = scope.ServiceProvider.GetRequiredService<IContaCommandService>();
        var query = scope.ServiceProvider.GetRequiredService<IContaQueryService>();
        var dto = ContaDtoBuilder.Alterar(Factory.BaseData.Instituicao.Id);

        var conta = await ContaFixture.CriarAsync(Factory);

        await command.AlterarAsync(conta.Id, dto, TestContext.Current.CancellationToken);

        var alterado = await query.ObterPorIdAsync(conta.Id, TestContext.Current.CancellationToken);

        alterado.Should().NotBeNull();
        alterado.Nome.Should().Be(dto.Nome);
        alterado.InstituicaoId.Should().Be(dto.InstituicaoId);
    }

    [Fact]
    public async Task AlterarAsync_DeveLancarExcecao_QuandoDuplicarNome()
    {
        using var scope = CreateScope();
        var command = scope.ServiceProvider.GetRequiredService<IContaCommandService>();
        var dto = ContaDtoBuilder.Alterar(
            Factory.BaseData.Instituicao.Id,
            Factory.BaseData.Conta.Nome);

        var conta = await ContaFixture.CriarAsync(Factory);

        var acao = () => command.AlterarAsync(
            conta.Id,
            dto,
            TestContext.Current.CancellationToken);

        await acao.Should().ThrowAsync<ConflitoException>();
    }

    [Fact]
    public async Task AlterarAsync_DeveLancarExcecao_QuandoContaInexistente()
    {
        using var scope = CreateScope();
        var command = scope.ServiceProvider.GetRequiredService<IContaCommandService>();
        var dto = ContaDtoBuilder.Alterar(Factory.BaseData.Instituicao.Id);

        var acao = () => command.AlterarAsync(
            int.MaxValue,
            dto,
            TestContext.Current.CancellationToken);

        await acao.Should().ThrowAsync<RecursoNaoEncontradoException>();
    }

    [Fact]
    public async Task AlterarAsync_DeveLancarExcecao_QuandoInstituicaoInexistente()
    {
        using var scope = CreateScope();
        var command = scope.ServiceProvider.GetRequiredService<IContaCommandService>();
        var dto = ContaDtoBuilder.Alterar(int.MaxValue);

        var acao = () => command.AlterarAsync(
            Factory.BaseData.Conta.Id,
            dto,
            TestContext.Current.CancellationToken);

        await acao.Should().ThrowAsync<ConflitoException>();
    }

    // ============================================================================
    // ExcluirAsync
    // ============================================================================

    [Fact]
    public async Task ExcluirAsync_DeveExcluirConta_QuandoDadosValidos()
    {
        using var scope = CreateScope();
        var command = scope.ServiceProvider.GetRequiredService<IContaCommandService>();
        var query = scope.ServiceProvider.GetRequiredService<IContaQueryService>();

        var conta = await ContaFixture.CriarAsync(Factory);

        await command.ExcluirAsync(conta.Id, TestContext.Current.CancellationToken);

        var acao = () => query.ObterPorIdAsync(conta.Id, TestContext.Current.CancellationToken);
        await acao.Should().ThrowAsync<RecursoNaoEncontradoException>();
    }

    [Fact]
    public async Task ExcluirAsync_DeveLancarExcecao_QuandoContaBase()
    {
        using var scope = CreateScope();
        var command = scope.ServiceProvider.GetRequiredService<IContaCommandService>();

        var acao = () => command.ExcluirAsync(Factory.BaseData.Conta.Id, TestContext.Current.CancellationToken);

        await acao.Should().ThrowAsync<ConflitoException>();
    }

    [Fact]
    public async Task ExcluirAsync_DeveLancarExcecao_QuandoContaInexistente()
    {
        using var scope = CreateScope();
        var command = scope.ServiceProvider.GetRequiredService<IContaCommandService>();

        var acao = () => command.ExcluirAsync(
            int.MaxValue,
            TestContext.Current.CancellationToken);

        await acao.Should().ThrowAsync<RecursoNaoEncontradoException>();
    }
}
