using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Patrimonio.Financas.Contracts.Cartoes.Services;
using Patrimonio.Financas.Domain.Exceptions;
using Patrimonio.Financas.Tests.Integration.Base;
using Patrimonio.Financas.Tests.Integration.Config;

namespace Patrimonio.Financas.Tests.Integration.Tests.Cartoes.Contracts;

public sealed class CartaoQueryServiceTests(IntegrationTestFactory factory) : IntegrationTestBase(factory)
{
    // ============================================================================
    // ListarAsync
    // ============================================================================

    [Fact]
    public async Task ListarAsync_DeveListarCartoes_QuandoDadosValidos()
    {
        using var scope = CreateScope();
        var query = scope.ServiceProvider.GetRequiredService<ICartaoQueryService>();

        var cartoes = await query.ListarAsync(TestContext.Current.CancellationToken);

        cartoes.Should().NotBeNull();
        cartoes.Should().Contain(c => c.Id == Factory.BaseData.Cartao.Id);
    }

    // ============================================================================
    // ObterPorIdAsync
    // ============================================================================

    [Fact]
    public async Task ObterPorIdAsync_DeveObterCartaoPorId_QuandoCartaoExiste()
    {
        using var scope = CreateScope();
        var query = scope.ServiceProvider.GetRequiredService<ICartaoQueryService>();
        var cartaoEsperado = Factory.BaseData.Cartao;

        var cartao = await query.ObterPorIdAsync(
            cartaoEsperado.Id,
            TestContext.Current.CancellationToken);

        cartao.Should().NotBeNull();
        cartao.Id.Should().Be(cartaoEsperado.Id);
        cartao.Nome.Should().Be(cartaoEsperado.Nome);
    }

    [Fact]
    public async Task ObterPorIdAsync_DeveLancarExcecao_QuandoCartaoInexistente()
    {
        using var scope = CreateScope();
        var query = scope.ServiceProvider.GetRequiredService<ICartaoQueryService>();

        var acao = () => query.ObterPorIdAsync(
            int.MaxValue,
            TestContext.Current.CancellationToken);

        await acao.Should().ThrowAsync<RecursoNaoEncontradoException>();
    }
}
