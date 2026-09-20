using FluentAssertions;
using Patrimonio.Financas.Contracts.Movimentacoes.Dtos;
using Patrimonio.Financas.Tests.Integration.Base;
using Patrimonio.Financas.Tests.Integration.Builders;
using Patrimonio.Financas.Tests.Integration.Config;
using Patrimonio.Financas.Tests.Integration.Fixtures;
using System.Net;
using System.Net.Http.Json;

namespace Patrimonio.Financas.Tests.Integration.Tests.Movimentacoes.Api;

public sealed class MovimentacoesControllerTests(IntegrationTestFactory factory)
    : ApiIntegrationTestBase(factory)
{
    private const string RotaBase = "/api/v1/financas/movimentacoes";

    // ============================================================================
    // GET /api/v1/financas/movimentacoes
    // ============================================================================

    [Fact]
    public async Task Listar_DeveRetornarOk_QuandoDadosValidos()
    {
        var resposta = await Client.GetAsync(
            RotaBase,
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.OK);

        var movimentacoes = await resposta.Content
            .ReadFromJsonAsync<IReadOnlyCollection<MovimentacaoListaDto>>(TestContext.Current.CancellationToken);

        movimentacoes.Should().NotBeNull();
        movimentacoes.Should().Contain(m => m.Id == Factory.BaseData.Movimentacao.Id);
    }

    // ============================================================================
    // GET /api/v1/financas/movimentacoes/{movimentacaoId}
    // ============================================================================

    [Fact]
    public async Task ObterPorId_DeveRetornarOk_QuandoMovimentacaoExiste()
    {
        var movimentacaoEsperada = Factory.BaseData.Movimentacao;

        var resposta = await Client.GetAsync(
            $"{RotaBase}/{movimentacaoEsperada.Id}",
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.OK);

        var movimentacao = await resposta.Content
            .ReadFromJsonAsync<MovimentacaoDetalheDto>(TestContext.Current.CancellationToken);

        movimentacao.Should().NotBeNull();
        movimentacao.Id.Should().Be(movimentacaoEsperada.Id);
        movimentacao.Descricao.Should().Be(movimentacaoEsperada.Descricao);
    }

    [Fact]
    public async Task ObterPorId_DeveRetornarNotFound_QuandoMovimentacaoInexistente()
    {
        var resposta = await Client.GetAsync(
            $"{RotaBase}/{int.MaxValue}",
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    // ============================================================================
    // POST /api/v1/financas/movimentacoes
    // ============================================================================

    [Fact]
    public async Task Criar_DeveRetornarCreated_QuandoDadosValidos()
    {
        var dto = MovimentacaoDtoBuilder.Criar(
            Factory.BaseData.Categoria.Id,
            Factory.BaseData.Conta.Id);

        var resposta = await Client.PostAsJsonAsync(
            RotaBase,
            dto,
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.Created);

        var movimentacao = await resposta.Content
            .ReadFromJsonAsync<MovimentacaoDetalheDto>(
                TestContext.Current.CancellationToken);

        movimentacao.Should().NotBeNull();
        movimentacao.Id.Should().BeGreaterThan(0);
        movimentacao.Descricao.Should().Be(dto.Descricao);

        resposta.Headers.Location.Should().NotBeNull();
    }

    [Fact]
    public async Task Criar_DeveRetornarBadRequest_QuandoValorNegativo()
    {
        var dto = MovimentacaoDtoBuilder.Criar(
            Factory.BaseData.Categoria.Id,
            Factory.BaseData.Conta.Id,
            valor: -500.00m);

        var resposta = await Client.PostAsJsonAsync(
            RotaBase,
            dto,
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Criar_DeveRetornarConflict_QuandoCategoriaInexistente()
    {
        var dto = MovimentacaoDtoBuilder.Criar(
            int.MaxValue,
            Factory.BaseData.Conta.Id);

        var resposta = await Client.PostAsJsonAsync(
            RotaBase,
            dto,
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    // ============================================================================
    // PUT /api/v1/financas/movimentacoes/{movimentacaoId}
    // ============================================================================

    [Fact]
    public async Task Alterar_DeveRetornarNoContent_QuandoDadosValidos()
    {
        var dto = MovimentacaoDtoBuilder.Alterar(
            Factory.BaseData.Categoria.Id,
            Factory.BaseData.Conta.Id);

        var movimentacao = await MovimentacaoFixture.CriarAsync(Factory);

        var resposta = await Client.PutAsJsonAsync(
            $"{RotaBase}/{movimentacao.Id}",
            dto,
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Alterar_DeveRetornarBadRequest_QuandoValorNegativo()
    {
        var dto = MovimentacaoDtoBuilder.Alterar(
            Factory.BaseData.Categoria.Id,
            Factory.BaseData.Conta.Id,
            valor: -500.00m);

        var resposta = await Client.PutAsJsonAsync(
            $"{RotaBase}/{Factory.BaseData.Movimentacao.Id}",
            dto,
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Alterar_DeveRetornarNotFound_QuandoMovimentacaoInexistente()
    {
        var dto = MovimentacaoDtoBuilder.Alterar(
            Factory.BaseData.Categoria.Id,
            Factory.BaseData.Conta.Id);

        var resposta = await Client.PutAsJsonAsync(
            $"{RotaBase}/{int.MaxValue}",
            dto,
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Alterar_DeveRetornarConflict_QuandoCategoriaInexistente()
    {
        var dto = MovimentacaoDtoBuilder.Alterar(
            int.MaxValue,
            Factory.BaseData.Conta.Id);

        var resposta = await Client.PutAsJsonAsync(
            $"{RotaBase}/{Factory.BaseData.Movimentacao.Id}",
            dto,
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    // ============================================================================
    // DELETE /api/v1/financas/movimentacoes/{movimentacaoId}
    // ============================================================================

    [Fact]
    public async Task Excluir_DeveRetornarNoContent_QuandoDadosValidos()
    {
        var movimentacao = await MovimentacaoFixture.CriarAsync(Factory);

        var resposta = await Client.DeleteAsync(
            $"{RotaBase}/{movimentacao.Id}",
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Excluir_DeveRetornarNotFound_QuandoMovimentacaoInexistente()
    {
        var resposta = await Client.DeleteAsync(
            $"{RotaBase}/{int.MaxValue}",
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
