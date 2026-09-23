using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Patrimonio.Financas.Application.Cartoes.Lookups.Abstractions;
using Patrimonio.Financas.Contracts.Cartoes.Services;
using Patrimonio.Financas.Domain.Exceptions;
using Patrimonio.Financas.Tests.Integration.Base;
using Patrimonio.Financas.Tests.Integration.Builders;
using Patrimonio.Financas.Tests.Integration.Config;
using Patrimonio.Financas.Tests.Integration.Fixtures;

namespace Patrimonio.Financas.Tests.Integration.Tests.Cartoes.Contracts;

public sealed class LancamentoCommandServiceTests(IntegrationTestFactory factory)
    : IntegrationTestBase(factory)
{
    // ============================================================================
    // CriarAsync
    // ============================================================================

    [Fact]
    public async Task CriarAsync_DeveCriarLancamento_QuandoDadosValidos()
    {
        using var scope = CreateScope();
        var command = scope.ServiceProvider.GetRequiredService<ILancamentoCommandService>();
        var dto = LancamentoDtoBuilder.Criar(Factory.BaseData.Fatura.Id, Factory.BaseData.Categoria.Id);

        var lancamento = await command.CriarAsync(dto, TestContext.Current.CancellationToken);

        lancamento.Should().NotBeNull();
        lancamento.Descricao.Should().NotBeNullOrWhiteSpace();
        lancamento.Id.Should().BeGreaterThan(0);
        lancamento.Descricao.Should().Be(dto.Descricao);
        lancamento.Valor.Should().Be(dto.Valor);
        lancamento.DataCompra.Should().Be(dto.DataCompra);
        lancamento.Estabelecimento.Should().Be(dto.Estabelecimento);
        lancamento.Responsavel.Should().Be(dto.Responsavel);
        lancamento.NumeroParcela.Should().Be(1);
        lancamento.TotalParcelas.Should().Be(dto.TotalParcelas);
        lancamento.Natureza.Should().Be(dto.Natureza);
        lancamento.FaturaId.Should().Be(dto.FaturaId);
        lancamento.CategoriaId.Should().Be(dto.CategoriaId);
    }

    [Fact]
    public async Task CriarAsync_DeveCriarLancamentoParcelado_QuandoDadosValidos()
    {
        using var scope = CreateScope();
        var command = scope.ServiceProvider.GetRequiredService<ILancamentoCommandService>();
        var faturaLookup = scope.ServiceProvider.GetRequiredService<IFaturaLookupRepository>();

        var fatura = await FaturaFixture.CriarAsync(Factory);

        var dto = LancamentoDtoBuilder.Criar(
            fatura.Id,
            Factory.BaseData.Categoria.Id,
            descricao: "Compra parcelada",
            totalParcelas: 3);

        var lancamento = await command.CriarAsync(dto, TestContext.Current.CancellationToken);
        var faturas = await faturaLookup.ListarPorCartaoAsync(
            fatura.CartaoId,
            TestContext.Current.CancellationToken);

        lancamento.Should().NotBeNull();
        lancamento.Descricao.Should().Be(dto.Descricao);
        lancamento.Valor.Should().Be(33.34m);
        lancamento.DataCompra.Should().Be(dto.DataCompra);
        lancamento.Estabelecimento.Should().Be(dto.Estabelecimento);
        lancamento.Responsavel.Should().Be(dto.Responsavel);
        lancamento.NumeroParcela.Should().Be(1);
        lancamento.TotalParcelas.Should().Be(3);
        lancamento.Natureza.Should().Be(dto.Natureza);
        lancamento.FaturaId.Should().Be(dto.FaturaId);
        lancamento.CategoriaId.Should().Be(dto.CategoriaId);
        faturas.Count.Should().Be(3);
    }

    [Fact]
    public async Task CriarAsync_DeveLancarExcecao_QuandoFaturaInexistente()
    {
        using var scope = CreateScope();
        var command = scope.ServiceProvider.GetRequiredService<ILancamentoCommandService>();
        var dto = LancamentoDtoBuilder.Criar(int.MaxValue, Factory.BaseData.Categoria.Id);

        var acao = () => command.CriarAsync(dto, TestContext.Current.CancellationToken);

        await acao.Should().ThrowAsync<RecursoNaoEncontradoException>();
    }

    [Fact]
    public async Task CriarAsync_DeveLancarExcecao_QuandoCategoriaInexistente()
    {
        using var scope = CreateScope();
        var command = scope.ServiceProvider.GetRequiredService<ILancamentoCommandService>();
        var dto = LancamentoDtoBuilder.Criar(Factory.BaseData.Fatura.Id, int.MaxValue);

        var acao = () => command.CriarAsync(dto, TestContext.Current.CancellationToken);

        await acao.Should().ThrowAsync<ConflitoException>();
    }

    [Fact]
    public async Task CriarAsync_DeveLancarExcecao_QuandoFaturaFechada()
    {
        using var scope = CreateScope();
        var command = scope.ServiceProvider.GetRequiredService<ILancamentoCommandService>();

        var fatura = await FaturaFixture.CriarFechadaAsync(Factory);
        var dto = LancamentoDtoBuilder.Criar(fatura.Id, Factory.BaseData.Categoria.Id);

        var acao = () => command.CriarAsync(dto, TestContext.Current.CancellationToken);

        await acao.Should().ThrowAsync<RegraDeNegocioException>();
    }

    [Fact]
    public async Task CriarAsync_DeveLancarExcecao_QuandoFaturaPaga()
    {
        using var scope = CreateScope();
        var command = scope.ServiceProvider.GetRequiredService<ILancamentoCommandService>();

        var fatura = await FaturaFixture.CriarPagaAsync(Factory);
        var dto = LancamentoDtoBuilder.Criar(fatura.Id, Factory.BaseData.Categoria.Id);

        var acao = () => command.CriarAsync(dto, TestContext.Current.CancellationToken);

        await acao.Should().ThrowAsync<RegraDeNegocioException>();
    }

    // ============================================================================
    // AlterarAsync
    // ============================================================================

    [Fact]
    public async Task AlterarAsync_DeveAlterarLancamento_QuandoDadosValidos()
    {
        using var scope = CreateScope();
        var command = scope.ServiceProvider.GetRequiredService<ILancamentoCommandService>();
        var query = scope.ServiceProvider.GetRequiredService<ILancamentoQueryService>();

        var lancamento = await LancamentoFixture.CriarAsync(Factory);
        var dto = LancamentoDtoBuilder.Alterar(Factory.BaseData.Categoria.Id);

        await command.AlterarAsync(lancamento.Id, dto, TestContext.Current.CancellationToken);

        var alterado = await query.ObterPorIdAsync(lancamento.Id, TestContext.Current.CancellationToken);

        alterado.Should().NotBeNull();
        alterado.Descricao.Should().Be(dto.Descricao);
        alterado.Valor.Should().Be(dto.Valor);
        alterado.DataCompra.Should().Be(dto.DataCompra);
        alterado.Estabelecimento.Should().Be(dto.Estabelecimento);
        alterado.Responsavel.Should().Be(dto.Responsavel);
        alterado.NumeroParcela.Should().Be(lancamento.NumeroParcela);
        alterado.TotalParcelas.Should().Be(lancamento.TotalParcelas);
        alterado.Natureza.Should().Be(dto.Natureza);
        alterado.FaturaId.Should().Be(lancamento.FaturaId);
        alterado.CategoriaId.Should().Be(dto.CategoriaId);
    }

    [Fact]
    public async Task AlterarAsync_DeveLancarExcecao_QuandoCategoriaInexistente()
    {
        using var scope = CreateScope();
        var command = scope.ServiceProvider.GetRequiredService<ILancamentoCommandService>();

        var dto = LancamentoDtoBuilder.Alterar(int.MaxValue);

        var acao = () => command.AlterarAsync(
            Factory.BaseData.Lancamento.Id,
            dto,
            TestContext.Current.CancellationToken);

        await acao.Should().ThrowAsync<ConflitoException>();
    }

    [Fact]
    public async Task AlterarAsync_DeveLancarExcecao_QuandoLancamentoInexistente()
    {
        using var scope = CreateScope();
        var command = scope.ServiceProvider.GetRequiredService<ILancamentoCommandService>();
        var dto = LancamentoDtoBuilder.Alterar(Factory.BaseData.Categoria.Id);

        var acao = () => command.AlterarAsync(
            int.MaxValue,
            dto,
            TestContext.Current.CancellationToken);

        await acao.Should().ThrowAsync<RecursoNaoEncontradoException>();
    }

    [Fact]
    public async Task AlterarAsync_DeveLancarExcecao_QuandoAlterarLancamentoDeFaturaFechada()
    {
        using var scope = CreateScope();
        var command = scope.ServiceProvider.GetRequiredService<ILancamentoCommandService>();
        var dto = LancamentoDtoBuilder.Alterar(Factory.BaseData.Categoria.Id);

        var lancamento = await LancamentoFixture.CriarEmFaturaFechadaAsync(Factory);

        var acao = () => command.AlterarAsync(
            lancamento.Id,
            dto,
            TestContext.Current.CancellationToken);

        await acao.Should().ThrowAsync<RegraDeNegocioException>();
    }

    [Fact]
    public async Task AlterarAsync_DeveLancarExcecao_QuandoAlterarLancamentoDeFaturaPaga()
    {
        using var scope = CreateScope();
        var command = scope.ServiceProvider.GetRequiredService<ILancamentoCommandService>();
        var dto = LancamentoDtoBuilder.Alterar(Factory.BaseData.Categoria.Id);

        var lancamento = await LancamentoFixture.CriarEmFaturaPagaAsync(Factory);

        var acao = () => command.AlterarAsync(
            lancamento.Id,
            dto,
            TestContext.Current.CancellationToken);

        await acao.Should().ThrowAsync<RegraDeNegocioException>();
    }

    // ============================================================================
    // ExcluirAsync
    // ============================================================================

    [Fact]
    public async Task ExcluirAsync_DeveExcluirLancamento_QuandoDadosValidos()
    {
        using var scope = CreateScope();
        var command = scope.ServiceProvider.GetRequiredService<ILancamentoCommandService>();
        var query = scope.ServiceProvider.GetRequiredService<ILancamentoQueryService>();
        var dto = LancamentoDtoBuilder.Criar(Factory.BaseData.Fatura.Id, Factory.BaseData.Categoria.Id);

        var lancamento = await command.CriarAsync(dto, TestContext.Current.CancellationToken);

        await command.ExcluirAsync(lancamento.Id, TestContext.Current.CancellationToken);

        var acao = () => query.ObterPorIdAsync(lancamento.Id, TestContext.Current.CancellationToken);

        await acao.Should().ThrowAsync<RecursoNaoEncontradoException>();
    }

    [Fact]
    public async Task ExcluirAsync_DeveLancarExcecao_QuandoLancamentoInexistente()
    {
        using var scope = CreateScope();
        var command = scope.ServiceProvider.GetRequiredService<ILancamentoCommandService>();

        var acao = () => command.ExcluirAsync(
            int.MaxValue,
            TestContext.Current.CancellationToken);

        await acao.Should().ThrowAsync<RecursoNaoEncontradoException>();
    }

    [Fact]
    public async Task ExcluirAsync_DeveLancarExcecao_QuandoLancamentoDeFaturaFechada()
    {
        using var scope = CreateScope();
        var command = scope.ServiceProvider.GetRequiredService<ILancamentoCommandService>();

        var lancamento = await LancamentoFixture.CriarEmFaturaFechadaAsync(Factory);

        var acao = () => command.ExcluirAsync(
            lancamento.Id,
            TestContext.Current.CancellationToken);

        await acao.Should().ThrowAsync<RegraDeNegocioException>();
    }

    [Fact]
    public async Task ExcluirAsync_DeveLancarExcecao_QuandoLancamentoDeFaturaPaga()
    {
        using var scope = CreateScope();
        var command = scope.ServiceProvider.GetRequiredService<ILancamentoCommandService>();

        var lancamento = await LancamentoFixture.CriarEmFaturaPagaAsync(Factory);

        var acao = () => command.ExcluirAsync(
            lancamento.Id,
            TestContext.Current.CancellationToken);

        await acao.Should().ThrowAsync<RegraDeNegocioException>();
    }
}
