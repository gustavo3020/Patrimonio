using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Patrimonio.Financas.Contracts.Movimentacoes.Services;
using Patrimonio.Financas.Domain.Exceptions;
using Patrimonio.Financas.Tests.Integration.Base;
using Patrimonio.Financas.Tests.Integration.Config;

namespace Patrimonio.Financas.Tests.Integration.Tests.Movimentacoes.Contracts;

public sealed class MovimentacaoQueryServiceTests(IntegrationTestFactory factory) : IntegrationTestBase(factory)
{
    // ============================================================================
    // ListarAsync
    // ============================================================================

    [Fact]
    public async Task ListarAsync_DeveListarMovimentacoes_QuandoDadosValidos()
    {
        using var scope = CreateScope();
        var query = scope.ServiceProvider.GetRequiredService<IMovimentacaoQueryService>();

        var movimentacoes = await query.ListarAsync(TestContext.Current.CancellationToken);

        movimentacoes.Should().NotBeNull();
        movimentacoes.Should().Contain(m => m.Id == Factory.BaseData.Movimentacao.Id);
    }

    // ============================================================================
    // ObterPorIdAsync
    // ============================================================================

    [Fact]
    public async Task ObterPorIdAsync_DeveObterMovimentacaoPorId_QuandoMovimentacaoExiste()
    {
        using var scope = CreateScope();
        var query = scope.ServiceProvider.GetRequiredService<IMovimentacaoQueryService>();
        var movimentacaoEsperada = Factory.BaseData.Movimentacao;

        var movimentacao = await query.ObterPorIdAsync(
            movimentacaoEsperada.Id,
            TestContext.Current.CancellationToken);

        movimentacao.Should().NotBeNull();
        movimentacao.Id.Should().Be(movimentacaoEsperada.Id);
        movimentacao.Descricao.Should().Be(movimentacaoEsperada.Descricao);
        movimentacao.CategoriaId.Should().Be(movimentacaoEsperada.CategoriaId);
        movimentacao.ContaId.Should().Be(movimentacaoEsperada.ContaId);
    }

    [Fact]
    public async Task ObterPorIdAsync_DeveLancarExcecao_QuandoMovimentacaoInexistente()
    {
        using var scope = CreateScope();
        var query = scope.ServiceProvider.GetRequiredService<IMovimentacaoQueryService>();

        var acao = () => query.ObterPorIdAsync(
            int.MaxValue,
            TestContext.Current.CancellationToken);

        await acao.Should().ThrowAsync<RecursoNaoEncontradoException>();
    }
}
