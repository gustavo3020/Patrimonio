using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Patrimonio.Financas.Contracts.Movimentacoes.Services;
using Patrimonio.Financas.Domain.Exceptions;
using Patrimonio.Financas.Tests.Integration.Base;
using Patrimonio.Financas.Tests.Integration.Builders;
using Patrimonio.Financas.Tests.Integration.Config;
using Patrimonio.Financas.Tests.Integration.Fixtures;

namespace Patrimonio.Financas.Tests.Integration.Tests.Movimentacoes.Contracts;

public sealed class MovimentacaoCommandServiceTests(IntegrationTestFactory factory) : IntegrationTestBase(factory)
{
    // ============================================================================
    // CriarAsync
    // ============================================================================

    [Fact]
    public async Task CriarAsync_DeveCriarMovimentacao_QuandoDadosValidos()
    {
        using var scope = CreateScope();
        var command = scope.ServiceProvider.GetRequiredService<IMovimentacaoCommandService>();
        var dto = MovimentacaoDtoBuilder.Criar(
            Factory.BaseData.Categoria.Id,
            Factory.BaseData.Conta.Id);

        var movimentacao = await command.CriarAsync(dto, TestContext.Current.CancellationToken);

        movimentacao.Should().NotBeNull();
        movimentacao.Descricao.Should().NotBeNullOrWhiteSpace();
        movimentacao.Id.Should().BeGreaterThan(0);
        movimentacao.Data.Should().Be(dto.Data);
        movimentacao.Valor.Should().Be(dto.Valor);
        movimentacao.Natureza.Should().Be(dto.Natureza);
        movimentacao.Tipo.Should().Be(dto.Tipo);
        movimentacao.Descricao.Should().Be(dto.Descricao);
        movimentacao.CategoriaId.Should().Be(dto.CategoriaId);
        movimentacao.ContaId.Should().Be(dto.ContaId);
    }

    [Fact]
    public async Task CriarAsync_DeveCriarMovimentacao_QuandoDescricaoNula()
    {
        using var scope = CreateScope();
        var command = scope.ServiceProvider.GetRequiredService<IMovimentacaoCommandService>();
        var dto = MovimentacaoDtoBuilder.Criar(
            Factory.BaseData.Categoria.Id,
            Factory.BaseData.Conta.Id,
            descricao: string.Empty);

        var movimentacao = await command.CriarAsync(dto, TestContext.Current.CancellationToken);
        movimentacao.Descricao.Should().BeNullOrEmpty();
    }

    [Fact]
    public async Task CriarAsync_DeveLancarExcecao_QuandoCategoriaInexistente()
    {
        using var scope = CreateScope();
        var command = scope.ServiceProvider.GetRequiredService<IMovimentacaoCommandService>();
        var dto = MovimentacaoDtoBuilder.Criar(
            int.MaxValue,
            Factory.BaseData.Conta.Id);

        var acao = () => command.CriarAsync(dto, TestContext.Current.CancellationToken);

        await acao.Should().ThrowAsync<ConflitoException>();
    }

    [Fact]
    public async Task CriarAsync_DeveLancarExcecao_QuandoContaInexistente()
    {
        using var scope = CreateScope();
        var command = scope.ServiceProvider.GetRequiredService<IMovimentacaoCommandService>();
        var dto = MovimentacaoDtoBuilder.Criar(
            Factory.BaseData.Categoria.Id,
            int.MaxValue);

        var acao = () => command.CriarAsync(dto, TestContext.Current.CancellationToken);

        await acao.Should().ThrowAsync<ConflitoException>();
    }

    // ============================================================================
    // AlterarAsync
    // ============================================================================

    [Fact]
    public async Task AlterarAsync_DeveAlterarMovimentacao_QuandoDadosValidos()
    {
        using var scope = CreateScope();
        var command = scope.ServiceProvider.GetRequiredService<IMovimentacaoCommandService>();
        var query = scope.ServiceProvider.GetRequiredService<IMovimentacaoQueryService>();
        var dto = MovimentacaoDtoBuilder.Alterar(
            Factory.BaseData.Categoria.Id,
            Factory.BaseData.Conta.Id);

        var movimentacao = await MovimentacaoFixture.CriarAsync(Factory);

        await command.AlterarAsync(movimentacao.Id, dto, TestContext.Current.CancellationToken);

        var alterado = await query.ObterPorIdAsync(movimentacao.Id, TestContext.Current.CancellationToken);

        alterado.Should().NotBeNull();
        alterado.Data.Should().Be(dto.Data);
        alterado.Valor.Should().Be(dto.Valor);
        alterado.Natureza.Should().Be(dto.Natureza);
        alterado.Tipo.Should().Be(dto.Tipo);
        alterado.Descricao.Should().Be(dto.Descricao);
        alterado.CategoriaId.Should().Be(dto.CategoriaId);
        alterado.ContaId.Should().Be(dto.ContaId);
    }

    [Fact]
    public async Task AlterarAsync_DeveAlterarMovimentacao_QuandoDescricaoNula()
    {
        using var scope = CreateScope();
        var command = scope.ServiceProvider.GetRequiredService<IMovimentacaoCommandService>();
        var query = scope.ServiceProvider.GetRequiredService<IMovimentacaoQueryService>();
        var dto = MovimentacaoDtoBuilder.Alterar(
            Factory.BaseData.Categoria.Id,
            Factory.BaseData.Conta.Id,
            descricao: string.Empty);

        var movimentacao = await MovimentacaoFixture.CriarAsync(Factory);

        await command.AlterarAsync(movimentacao.Id, dto, TestContext.Current.CancellationToken);

        var alterado = await query.ObterPorIdAsync(movimentacao.Id, TestContext.Current.CancellationToken);
        alterado.Descricao.Should().BeNullOrEmpty();
    }

    [Fact]
    public async Task AlterarAsync_DeveLancarExcecao_QuandoMovimentacaoInexistente()
    {
        using var scope = CreateScope();
        var command = scope.ServiceProvider.GetRequiredService<IMovimentacaoCommandService>();
        var dto = MovimentacaoDtoBuilder.Alterar(
            Factory.BaseData.Categoria.Id,
            Factory.BaseData.Conta.Id);

        var acao = () => command.AlterarAsync(
            int.MaxValue,
            dto,
            TestContext.Current.CancellationToken);

        await acao.Should().ThrowAsync<RecursoNaoEncontradoException>();
    }

    [Fact]
    public async Task AlterarAsync_DeveLancarExcecao_QuandoCategoriaInexistente()
    {
        using var scope = CreateScope();
        var command = scope.ServiceProvider.GetRequiredService<IMovimentacaoCommandService>();
        var dto = MovimentacaoDtoBuilder.Alterar(
            int.MaxValue,
            Factory.BaseData.Conta.Id);

        var acao = () => command.AlterarAsync(
            Factory.BaseData.Movimentacao.Id,
            dto,
            TestContext.Current.CancellationToken);

        await acao.Should().ThrowAsync<ConflitoException>();
    }

    [Fact]
    public async Task AlterarAsync_DeveLancarExcecao_QuandoContaInexistente()
    {
        using var scope = CreateScope();
        var command = scope.ServiceProvider.GetRequiredService<IMovimentacaoCommandService>();
        var dto = MovimentacaoDtoBuilder.Alterar(
            Factory.BaseData.Categoria.Id,
            int.MaxValue);

        var acao = () => command.AlterarAsync(
            Factory.BaseData.Movimentacao.Id,
            dto,
            TestContext.Current.CancellationToken);

        await acao.Should().ThrowAsync<ConflitoException>();
    }

    // ============================================================================
    // ExcluirAsync
    // ============================================================================

    [Fact]
    public async Task ExcluirAsync_DeveExcluirMovimentacao_QuandoDadosValidos()
    {
        using var scope = CreateScope();
        var command = scope.ServiceProvider.GetRequiredService<IMovimentacaoCommandService>();
        var query = scope.ServiceProvider.GetRequiredService<IMovimentacaoQueryService>();

        var movimentacao = await MovimentacaoFixture.CriarAsync(Factory);

        await command.ExcluirAsync(movimentacao.Id, TestContext.Current.CancellationToken);

        var acao = () => query.ObterPorIdAsync(movimentacao.Id, TestContext.Current.CancellationToken);
        await acao.Should().ThrowAsync<RecursoNaoEncontradoException>();
    }

    [Fact]
    public async Task ExcluirAsync_DeveLancarExcecao_QuandoMovimentacaoInexistente()
    {
        using var scope = CreateScope();
        var command = scope.ServiceProvider.GetRequiredService<IMovimentacaoCommandService>();

        var acao = () => command.ExcluirAsync(
            int.MaxValue,
            TestContext.Current.CancellationToken);

        await acao.Should().ThrowAsync<RecursoNaoEncontradoException>();
    }
}
