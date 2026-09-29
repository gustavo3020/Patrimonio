using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Patrimonio.Financas.Contracts.Cartoes.Services;
using Patrimonio.Financas.Domain.Exceptions;
using Patrimonio.Financas.Tests.Integration.Base;
using Patrimonio.Financas.Tests.Integration.Builders;
using Patrimonio.Financas.Tests.Integration.Config;
using Patrimonio.Financas.Tests.Integration.Fixtures;

namespace Patrimonio.Financas.Tests.Integration.Tests.Cartoes.Contracts;

public sealed class CartaoCommandServiceTests(IntegrationTestFactory factory)
    : IntegrationTestBase(factory)
{
    // ============================================================================
    // CriarAsync
    // ============================================================================

    [Fact]
    public async Task CriarAsync_DeveCriarCartao_QuandoDadosValidos()
    {
        using var scope = CreateScope();
        var command = scope.ServiceProvider.GetRequiredService<ICartaoCommandService>();
        var dto = CartaoDtoBuilder.Criar(Factory.BaseData.Instituicao.Id);

        var cartao = await command.CriarAsync(dto, TestContext.Current.CancellationToken);

        cartao.Should().NotBeNull();
        cartao.Nome.Should().NotBeNullOrWhiteSpace();
        cartao.Id.Should().BeGreaterThan(0);
        cartao.Nome.Should().Be(dto.Nome);
        cartao.Bandeira.Should().Be(dto.Bandeira);
        cartao.Limite.Should().Be(dto.Limite);
        cartao.DiaFechamento.Should().Be(dto.DiaFechamento);
        cartao.DiaVencimento.Should().Be(dto.DiaVencimento);
        cartao.InstituicaoId.Should().Be(dto.InstituicaoId);
    }

    [Fact]
    public async Task CriarAsync_DeveLancarExcecao_QuandoDuplicarNome()
    {
        using var scope = CreateScope();
        var command = scope.ServiceProvider.GetRequiredService<ICartaoCommandService>();
        var dto = CartaoDtoBuilder.Criar(Factory.BaseData.Instituicao.Id);

        await command.CriarAsync(dto, TestContext.Current.CancellationToken);

        var acao = () => command.CriarAsync(dto, TestContext.Current.CancellationToken);

        await acao.Should().ThrowAsync<ConflitoException>();
    }

    // ============================================================================
    // AlterarAsync
    // ============================================================================

    [Fact]
    public async Task AlterarAsync_DeveAlterarCartao_QuandoDadosValidos()
    {
        using var scope = CreateScope();
        var command = scope.ServiceProvider.GetRequiredService<ICartaoCommandService>();
        var query = scope.ServiceProvider.GetRequiredService<ICartaoQueryService>();
        var dto = CartaoDtoBuilder.Alterar();

        var cartao = await CartaoFixture.CriarAsync(Factory);

        await command.AlterarAsync(cartao.Id, dto, TestContext.Current.CancellationToken);

        var alterado = await query.ObterPorIdAsync(cartao.Id, TestContext.Current.CancellationToken);

        alterado.Should().NotBeNull();
        alterado.Nome.Should().Be(dto.Nome);
        alterado.Bandeira.Should().Be(dto.Bandeira);
        alterado.Limite.Should().Be(dto.Limite);
        alterado.DiaFechamento.Should().Be(dto.DiaFechamento);
        alterado.DiaVencimento.Should().Be(dto.DiaVencimento);
        alterado.InstituicaoId.Should().Be(cartao.InstituicaoId);
    }

    [Fact]
    public async Task AlterarAsync_DeveLancarExcecao_QuandoDuplicarNome()
    {
        using var scope = CreateScope();
        var command = scope.ServiceProvider.GetRequiredService<ICartaoCommandService>();
        var dto = CartaoDtoBuilder.Alterar(nome: Factory.BaseData.Cartao.Nome);

        var cartao = await CartaoFixture.CriarAsync(Factory);

        var acao = () => command.AlterarAsync(
            cartao.Id,
            dto,
            TestContext.Current.CancellationToken);

        await acao.Should().ThrowAsync<ConflitoException>();
    }

    [Fact]
    public async Task AlterarAsync_DeveLancarExcecao_QuandoCartaoInexistente()
    {
        using var scope = CreateScope();
        var command = scope.ServiceProvider.GetRequiredService<ICartaoCommandService>();

        var acao = () => command.AlterarAsync(
            int.MaxValue,
            CartaoDtoBuilder.Alterar(),
            TestContext.Current.CancellationToken);

        await acao.Should().ThrowAsync<RecursoNaoEncontradoException>();
    }

    // ============================================================================
    // ExcluirAsync
    // ============================================================================

    [Fact]
    public async Task ExcluirAsync_DeveExcluirCartao_QuandoDadosValidos()
    {
        using var scope = CreateScope();
        var command = scope.ServiceProvider.GetRequiredService<ICartaoCommandService>();
        var query = scope.ServiceProvider.GetRequiredService<ICartaoQueryService>();

        var cartao = await CartaoFixture.CriarAsync(Factory);

        await command.ExcluirAsync(cartao.Id, TestContext.Current.CancellationToken);

        var acao = () => query.ObterPorIdAsync(cartao.Id, TestContext.Current.CancellationToken);
        await acao.Should().ThrowAsync<RecursoNaoEncontradoException>();
    }

    [Fact]
    public async Task ExcluirAsync_DeveLancarExcecao_QuandoCartaoBase()
    {
        using var scope = CreateScope();
        var command = scope.ServiceProvider.GetRequiredService<ICartaoCommandService>();

        var acao = () => command.ExcluirAsync(Factory.BaseData.Cartao.Id, TestContext.Current.CancellationToken);

        await acao.Should().ThrowAsync<ConflitoException>();
    }

    [Fact]
    public async Task ExcluirAsync_DeveLancarExcecao_QuandoCartaoInexistente()
    {
        using var scope = CreateScope();
        var command = scope.ServiceProvider.GetRequiredService<ICartaoCommandService>();

        var acao = () => command.ExcluirAsync(
            int.MaxValue,
            TestContext.Current.CancellationToken);

        await acao.Should().ThrowAsync<RecursoNaoEncontradoException>();
    }
}
