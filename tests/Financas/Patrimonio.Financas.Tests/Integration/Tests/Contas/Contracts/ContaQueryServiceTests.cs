using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Patrimonio.Financas.Contracts.Contas.Services;
using Patrimonio.Financas.Domain.Exceptions;
using Patrimonio.Financas.Tests.Integration.Base;
using Patrimonio.Financas.Tests.Integration.Config;

namespace Patrimonio.Financas.Tests.Integration.Tests.Contas.Contracts;

public sealed class ContaQueryServiceTests(IntegrationTestFactory factory) : IntegrationTestBase(factory)
{
    // ============================================================================
    // ListarAsync
    // ============================================================================

    [Fact]
    public async Task ListarAsync_DeveListarContas_QuandoDadosValidos()
    {
        using var scope = CreateScope();
        var query = scope.ServiceProvider.GetRequiredService<IContaQueryService>();

        var contas = await query.ListarAsync(TestContext.Current.CancellationToken);

        contas.Should().NotBeNull();
        contas.Should().Contain(c => c.Id == Factory.BaseData.Conta.Id);
    }

    // ============================================================================
    // ObterPorIdAsync
    // ============================================================================

    [Fact]
    public async Task ObterPorIdAsync_DeveObterContaPorId_QuandoContaExiste()
    {
        using var scope = CreateScope();
        var query = scope.ServiceProvider.GetRequiredService<IContaQueryService>();
        var contaEsperada = Factory.BaseData.Conta;

        var conta = await query.ObterPorIdAsync(
            contaEsperada.Id,
            TestContext.Current.CancellationToken);

        conta.Should().NotBeNull();
        conta.Id.Should().Be(contaEsperada.Id);
        conta.Nome.Should().Be(contaEsperada.Nome);
        conta.InstituicaoId.Should().Be(contaEsperada.InstituicaoId);
    }

    [Fact]
    public async Task ObterPorIdAsync_DeveLancarExcecao_QuandoContaInexistente()
    {
        using var scope = CreateScope();
        var query = scope.ServiceProvider.GetRequiredService<IContaQueryService>();

        var acao = () => query.ObterPorIdAsync(
            int.MaxValue,
            TestContext.Current.CancellationToken);

        await acao.Should().ThrowAsync<RecursoNaoEncontradoException>();
    }
}
