using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Patrimonio.Financas.Contracts.Cartoes.Services;
using Patrimonio.Financas.Domain.Exceptions;
using Patrimonio.Financas.SharedKernel.Cartoes.Enums;
using Patrimonio.Financas.Tests.Integration.Base;
using Patrimonio.Financas.Tests.Integration.Builders;
using Patrimonio.Financas.Tests.Integration.Config;
using Patrimonio.Financas.Tests.Integration.Fixtures;

namespace Patrimonio.Financas.Tests.Integration.Tests.Cartoes.Contracts;

public sealed class FaturaCommandServiceTests(IntegrationTestFactory factory)
    : IntegrationTestBase(factory)
{
    // ============================================================================
    // CriarAsync
    // ============================================================================

    [Fact]
    public async Task CriarAsync_DeveCriarFatura_QuandoDadosValidos()
    {
        using var scope = CreateScope();
        var command = scope.ServiceProvider.GetRequiredService<IFaturaCommandService>();

        var cartao = await CartaoFixture.CriarAsync(Factory);
        var dto = FaturaDtoBuilder.Criar(cartao.Id);

        var fatura = await command.CriarAsync(dto, TestContext.Current.CancellationToken);

        fatura.Should().NotBeNull();
        fatura.DataFechamento.Should().Be(dto.DataFechamento);
        fatura.DataVencimento.Should().Be(dto.DataVencimento);
        fatura.Status.Should().Be(StatusFatura.Aberta);
        fatura.DataPagamento.Should().Be(null);
        fatura.CartaoId.Should().Be(dto.CartaoId);
    }

    [Fact]
    public async Task CriarAsync_DeveLancarExcecao_QuandoCartaoInexistente()
    {
        using var scope = CreateScope();
        var command = scope.ServiceProvider.GetRequiredService<IFaturaCommandService>();
        var dto = FaturaDtoBuilder.Criar(int.MaxValue);

        var acao = () => command.CriarAsync(dto, TestContext.Current.CancellationToken);
        await acao.Should().ThrowAsync<ConflitoException>();
    }

    // ============================================================================
    // AlterarAsync
    // ============================================================================

    [Fact]
    public async Task AlterarAsync_DeveAlterarFatura_QuandoDadosValidos()
    {
        using var scope = CreateScope();
        var command = scope.ServiceProvider.GetRequiredService<IFaturaCommandService>();
        var query = scope.ServiceProvider.GetRequiredService<IFaturaQueryService>();
        var dto = FaturaDtoBuilder.Alterar();

        var fatura = await FaturaFixture.CriarAsync(Factory);

        await command.AlterarAsync(fatura.Id, dto, TestContext.Current.CancellationToken);

        var alterado = await query.ObterPorIdAsync(fatura.Id, TestContext.Current.CancellationToken);

        alterado.Should().NotBeNull();
        alterado.DataFechamento.Should().Be(dto.DataFechamento);
        alterado.DataVencimento.Should().Be(dto.DataVencimento);
        alterado.Status.Should().Be(StatusFatura.Aberta);
    }

    [Fact]
    public async Task AlterarAsync_DeveLancarExcecao_QuandoFaturaInexistente()
    {
        using var scope = CreateScope();
        var command = scope.ServiceProvider.GetRequiredService<IFaturaCommandService>();

        var acao = () => command.AlterarAsync(
            int.MaxValue,
            FaturaDtoBuilder.Alterar(),
            TestContext.Current.CancellationToken);

        await acao.Should().ThrowAsync<RecursoNaoEncontradoException>();
    }

    // ============================================================================
    // ExcluirAsync
    // ============================================================================

    [Fact]
    public async Task ExcluirAsync_DeveExcluirFatura_QuandoDadosValidos()
    {
        using var scope = CreateScope();
        var command = scope.ServiceProvider.GetRequiredService<IFaturaCommandService>();
        var query = scope.ServiceProvider.GetRequiredService<IFaturaQueryService>();

        var fatura = await FaturaFixture.CriarAsync(Factory);

        await command.ExcluirAsync(fatura.Id, TestContext.Current.CancellationToken);

        var acao = () => query.ObterPorIdAsync(fatura.Id, TestContext.Current.CancellationToken);
        await acao.Should().ThrowAsync<RecursoNaoEncontradoException>();
    }

    [Fact]
    public async Task ExcluirAsync_DeveLancarExcecao_QuandoFaturaBase()
    {
        using var scope = CreateScope();
        var command = scope.ServiceProvider.GetRequiredService<IFaturaCommandService>();

        var acao = () => command.ExcluirAsync(
            Factory.BaseData.Fatura.Id,
            TestContext.Current.CancellationToken);

        await acao.Should().ThrowAsync<ConflitoException>();
    }

    [Fact]
    public async Task ExcluirAsync_DeveLancarExcecao_QuandoFaturaInexistente()
    {
        using var scope = CreateScope();
        var command = scope.ServiceProvider.GetRequiredService<IFaturaCommandService>();

        var acao = () => command.ExcluirAsync(
            int.MaxValue,
            TestContext.Current.CancellationToken);

        await acao.Should().ThrowAsync<RecursoNaoEncontradoException>();
    }

    [Fact]
    public async Task ExcluirAsync_DeveLancarExcecao_QuandoFaturaFechada()
    {
        using var scope = CreateScope();
        var command = scope.ServiceProvider.GetRequiredService<IFaturaCommandService>();

        var fatura = await FaturaFixture.CriarFechadaAsync(Factory);

        var acao = () => command.ExcluirAsync(fatura.Id, TestContext.Current.CancellationToken);
        await acao.Should().ThrowAsync<RegraDeNegocioException>();
    }

    // ============================================================================
    // FecharAsync
    // ============================================================================

    [Fact]
    public async Task FecharAsync_DeveFecharFatura_QuandoDadosValidos()
    {
        using var scope = CreateScope();
        var command = scope.ServiceProvider.GetRequiredService<IFaturaCommandService>();
        var query = scope.ServiceProvider.GetRequiredService<IFaturaQueryService>();

        var criado = await FaturaFixture.CriarAsync(Factory);

        await command.FecharAsync(criado.Id, TestContext.Current.CancellationToken);

        var fatura = await query.ObterPorIdAsync(
            criado.Id,
            TestContext.Current.CancellationToken);

        fatura.Should().NotBeNull();
        fatura.Status.Should().Be(StatusFatura.Fechada);
    }

    [Fact]
    public async Task FecharAsync_DeveLancarExcecao_QuandoFaturaInexistente()
    {
        using var scope = CreateScope();
        var command = scope.ServiceProvider.GetRequiredService<IFaturaCommandService>();

        var acao = () => command.FecharAsync(
            int.MaxValue,
            TestContext.Current.CancellationToken);

        await acao.Should().ThrowAsync<RecursoNaoEncontradoException>();
    }

    // ============================================================================
    // PagarAsync
    // ============================================================================

    [Fact]
    public async Task PagarAsync_DevePagarFatura_QuandoDadosValidos()
    {
        using var scope = CreateScope();
        var command = scope.ServiceProvider.GetRequiredService<IFaturaCommandService>();
        var query = scope.ServiceProvider.GetRequiredService<IFaturaQueryService>();

        var criado = await FaturaFixture.CriarFechadaComLancamentoAsync(Factory);

        var dto = FaturaDtoBuilder.Pagar(
            Factory.BaseData.Conta.Id,
            Factory.BaseData.Categoria.Id,
            dataPagamento: criado.DataFechamento.AddDays(5));

        await command.PagarAsync(criado.Id, dto, TestContext.Current.CancellationToken);

        var fatura = await query.ObterPorIdAsync(criado.Id, TestContext.Current.CancellationToken);
        fatura.Should().NotBeNull();
        fatura.Status.Should().Be(StatusFatura.Paga);
    }

    [Fact]
    public async Task PagarAsync_DeveLancarExcecao_QuandoFaturaInexistente()
    {
        using var scope = CreateScope();
        var command = scope.ServiceProvider.GetRequiredService<IFaturaCommandService>();
        var dto = FaturaDtoBuilder.Pagar(
            Factory.BaseData.Conta.Id,
            Factory.BaseData.Categoria.Id);

        var acao = () => command.PagarAsync(
            int.MaxValue,
            dto,
            TestContext.Current.CancellationToken);

        await acao.Should().ThrowAsync<RecursoNaoEncontradoException>();
    }
}
