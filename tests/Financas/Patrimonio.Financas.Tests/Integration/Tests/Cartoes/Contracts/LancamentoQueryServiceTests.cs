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
    public async Task ListarAsync_DeveListarLancamentos()
    {
        using var scope = CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<ILancamentoQueryService>();

        var lancamentos = await service.ListarAsync(CancellationToken.None);

        lancamentos.Should().NotBeNull();
        lancamentos.Should().Contain(c => c.Id == Factory.BaseData.Lancamento.Id);
    }

    // ============================================================================
    // ObterPorIdAsync
    // ============================================================================

    [Fact]
    public async Task ObterPorIdAsync_DeveObterLancamentoPorId()
    {
        using var scope = CreateScope();

        var service = scope.ServiceProvider.GetRequiredService<ILancamentoQueryService>();

        var lancamentoEsperada = Factory.BaseData.Lancamento;

        var lancamento = await service.ObterPorIdAsync(lancamentoEsperada.Id, CancellationToken.None);

        lancamento.Should().NotBeNull();
        lancamento.Id.Should().Be(lancamentoEsperada.Id);
        lancamento.Descricao.Should().Be(lancamentoEsperada.Descricao);
    }

    [Fact]
    public async Task ObterPorIdAsync_DeveLancarExcecaoAoObterLancamentoInexistente()
    {
        using var scope = CreateScope();

        var service = scope.ServiceProvider.GetRequiredService<ILancamentoQueryService>();

        var acao = () => service.ObterPorIdAsync(int.MaxValue, CancellationToken.None);

        await acao.Should().ThrowAsync<RecursoNaoEncontradoException>();
    }
}
