using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Patrimonio.Financas.Contracts.Cartoes.Services;
using Patrimonio.Financas.Domain.Exceptions;
using Patrimonio.Financas.Tests.Integration.Base;
using Patrimonio.Financas.Tests.Integration.Config;

namespace Patrimonio.Financas.Tests.Integration.Tests.Cartoes.Contracts;

public sealed class LancamentoQueryServiceTests(IntegrationTestFactory factory) : IntegrationTestBase(factory)
{
    // ============================================================================
    // ListarAsync
    // ============================================================================

    [Fact]
    public async Task ListarAsync_DeveRetornarLancamentosDaFaturaBase_QuandoFaturaTemLancamentos()
    {
        using var scope = CreateScope();
        var query = scope.ServiceProvider.GetRequiredService<ILancamentoQueryService>();

        var lancamentos = await query.ListarAsync(
            Factory.BaseData.Fatura.Id,
            TestContext.Current.CancellationToken);

        lancamentos.Should().NotBeNull();
        lancamentos.Should().Contain(c => c.Id == Factory.BaseData.Lancamento.Id);
    }

    [Fact]
    public async Task ListarAsync_DeveRetornarListaVazia_QuandoFaturaNaoPossuiLancamentos()
    {
        using var scope = CreateScope();
        var query = scope.ServiceProvider.GetRequiredService<ILancamentoQueryService>();

        var faturas = await query.ListarAsync(0, TestContext.Current.CancellationToken);

        faturas.Should().NotBeNull();
        faturas.Should().BeEmpty();
    }

    // ============================================================================
    // ObterPorIdAsync
    // ============================================================================

    [Fact]
    public async Task ObterPorIdAsync_DeveObterLancamentoPorId_QuandoLancamentoExiste()
    {
        using var scope = CreateScope();
        var query = scope.ServiceProvider.GetRequiredService<ILancamentoQueryService>();
        var lancamentoEsperado = Factory.BaseData.Lancamento;

        var lancamento = await query.ObterPorIdAsync(
            lancamentoEsperado.Id,
            TestContext.Current.CancellationToken);

        lancamento.Should().NotBeNull();
        lancamento.Id.Should().Be(lancamentoEsperado.Id);
        lancamento.Descricao.Should().Be(lancamentoEsperado.Descricao);
    }

    [Fact]
    public async Task ObterPorIdAsync_DeveLancarExcecao_QuandoLancamentoInexistente()
    {
        using var scope = CreateScope();
        var query = scope.ServiceProvider.GetRequiredService<ILancamentoQueryService>();

        var acao = () => query.ObterPorIdAsync(
            int.MaxValue,
            TestContext.Current.CancellationToken);

        await acao.Should().ThrowAsync<RecursoNaoEncontradoException>();
    }
}
