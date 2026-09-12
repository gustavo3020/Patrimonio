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
    public async Task ListarAsync_DeveListarCartoes()
    {
        using var scope = CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<ICartaoQueryService>();

        var cartoes = await service.ListarAsync(TestContext.Current.CancellationToken);

        cartoes.Should().NotBeNull();
        cartoes.Should().Contain(c => c.Id == Factory.BaseData.Cartao.Id);
    }

    // ============================================================================
    // ObterPorIdAsync
    // ============================================================================

    [Fact]
    public async Task ObterPorIdAsync_DeveObterCartaoPorId()
    {
        using var scope = CreateScope();

        var service = scope.ServiceProvider.GetRequiredService<ICartaoQueryService>();

        var cartaoEsperada = Factory.BaseData.Cartao;

        var cartao = await service.ObterPorIdAsync(cartaoEsperada.Id, TestContext.Current.CancellationToken);

        cartao.Should().NotBeNull();
        cartao.Id.Should().Be(cartaoEsperada.Id);
        cartao.Nome.Should().Be(cartaoEsperada.Nome);
    }

    [Fact]
    public async Task ObterPorIdAsync_DeveLancarExcecaoAoObterCartaoInexistente()
    {
        using var scope = CreateScope();

        var service = scope.ServiceProvider.GetRequiredService<ICartaoQueryService>();

        var acao = () => service.ObterPorIdAsync(int.MaxValue, TestContext.Current.CancellationToken);

        await acao.Should().ThrowAsync<RecursoNaoEncontradoException>();
    }
}
