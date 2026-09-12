using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Patrimonio.Financas.Contracts.Cartoes.Services;
using Patrimonio.Financas.Domain.Exceptions;
using Patrimonio.Financas.Tests.Integration.Base;
using Patrimonio.Financas.Tests.Integration.Config;

namespace Patrimonio.Financas.Tests.Integration.Tests.Cartoes.Contracts;

public sealed class FaturaQueryServiceTests(IntegrationTestFactory factory) : IntegrationTestBase(factory)
{
    // ============================================================================
    // ListarAsync
    // ============================================================================

    [Fact]
    public async Task ListarAsync_DeveListarFaturas()
    {
        using var scope = CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<IFaturaQueryService>();

        var faturas = await service.ListarAsync(TestContext.Current.CancellationToken);

        faturas.Should().NotBeNull();
        faturas.Should().Contain(c => c.Id == Factory.BaseData.Fatura.Id);
    }

    // ============================================================================
    // ObterPorIdAsync
    // ============================================================================

    [Fact]
    public async Task ObterPorIdAsync_DeveObterFaturaPorId()
    {
        using var scope = CreateScope();

        var service = scope.ServiceProvider.GetRequiredService<IFaturaQueryService>();

        var faturaEsperada = Factory.BaseData.Fatura;

        var fatura = await service.ObterPorIdAsync(faturaEsperada.Id, TestContext.Current.CancellationToken);

        fatura.Should().NotBeNull();
        fatura.Id.Should().Be(faturaEsperada.Id);
        fatura.DataFechamento.Should().Be(faturaEsperada.DataFechamento);
    }

    [Fact]
    public async Task ObterPorIdAsync_DeveLancarExcecaoAoObterFaturaInexistente()
    {
        using var scope = CreateScope();

        var service = scope.ServiceProvider.GetRequiredService<IFaturaQueryService>();

        var acao = () => service.ObterPorIdAsync(int.MaxValue, TestContext.Current.CancellationToken);

        await acao.Should().ThrowAsync<RecursoNaoEncontradoException>();
    }
}
