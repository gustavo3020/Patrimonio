using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Patrimonio.Financas.Contracts.Instituicoes.Services;
using Patrimonio.Financas.Domain.Exceptions;
using Patrimonio.Financas.Tests.Integration.Base;
using Patrimonio.Financas.Tests.Integration.Config;

namespace Patrimonio.Financas.Tests.Integration.Tests.Instituicoes.Contracts;

public sealed class InstituicaoQueryServiceTests(IntegrationTestFactory factory) : IntegrationTestBase(factory)
{
    // ============================================================================
    // ListarAsync
    // ============================================================================

    [Fact]
    public async Task ListarAsync_DeveListarInstituicoes_QuandoDadosValidos()
    {
        using var scope = CreateScope();
        var query = scope.ServiceProvider.GetRequiredService<IInstituicaoQueryService>();

        var instituicoes = await query.ListarAsync(TestContext.Current.CancellationToken);

        instituicoes.Should().NotBeNull();
        instituicoes.Should().Contain(i => i.Id == Factory.BaseData.Instituicao.Id);
    }

    // ============================================================================
    // ObterPorIdAsync
    // ============================================================================

    [Fact]
    public async Task ObterPorIdAsync_DeveObterInstituicaoPorId_QuandoInstituicaoExiste()
    {
        using var scope = CreateScope();
        var query = scope.ServiceProvider.GetRequiredService<IInstituicaoQueryService>();
        var instituicaoEsperada = Factory.BaseData.Instituicao;

        var instituicao = await query.ObterPorIdAsync(
            instituicaoEsperada.Id,
            TestContext.Current.CancellationToken);

        instituicao.Should().NotBeNull();
        instituicao.Id.Should().Be(instituicaoEsperada.Id);
        instituicao.Nome.Should().Be(instituicaoEsperada.Nome);
    }

    [Fact]
    public async Task ObterPorIdAsync_DeveLancarExcecao_QuandoInstituicaoInexistente()
    {
        using var scope = CreateScope();
        var query = scope.ServiceProvider.GetRequiredService<IInstituicaoQueryService>();

        var acao = () => query.ObterPorIdAsync(int.MaxValue, TestContext.Current.CancellationToken);

        await acao.Should().ThrowAsync<RecursoNaoEncontradoException>();
    }
}
