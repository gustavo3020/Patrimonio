using FluentAssertions;
using Patrimonio.Financas.Contracts.Cartoes.Dtos;
using Patrimonio.Financas.Tests.Integration.Base;
using Patrimonio.Financas.Tests.Integration.Builders;
using Patrimonio.Financas.Tests.Integration.Config;
using Patrimonio.Financas.Tests.Integration.Fixtures;
using System.Net;
using System.Net.Http.Json;

namespace Patrimonio.Financas.Tests.Integration.Tests.Cartoes.Api;

public sealed class CartaoControllerTests(IntegrationTestFactory factory)
    : ApiIntegrationTestBase(factory)
{
    private const string RotaBase = "/api/v1/financas/cartoes";

    // ============================================================================
    // GET /api/v1/financas/cartoes
    // ============================================================================

    [Fact]
    public async Task Listar_DeveRetornarOk_QuandoDadosValidos()
    {
        var resposta = await Client.GetAsync(
            RotaBase,
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.OK);

        var cartoes = await resposta.Content
            .ReadFromJsonAsync<IReadOnlyCollection<CartaoListaDto>>(TestContext.Current.CancellationToken);

        cartoes.Should().NotBeNull();
        cartoes.Should().Contain(c => c.Id == Factory.BaseData.Cartao.Id);
    }

    // ============================================================================
    // GET /api/v1/financas/cartoes/{cartaoId}
    // ============================================================================

    [Fact]
    public async Task ObterPorId_DeveRetornarOk_QuandoCartaoExiste()
    {
        var cartaoEsperado = Factory.BaseData.Cartao;

        var resposta = await Client.GetAsync(
            $"{RotaBase}/{cartaoEsperado.Id}",
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.OK);

        var cartao = await resposta.Content
            .ReadFromJsonAsync<CartaoDetalheDto>(TestContext.Current.CancellationToken);

        cartao.Should().NotBeNull();
        cartao.Id.Should().Be(cartaoEsperado.Id);
        cartao.Nome.Should().Be(cartaoEsperado.Nome);
        cartao.Bandeira.Should().Be(cartaoEsperado.Bandeira);
        cartao.Limite.Should().Be(cartaoEsperado.Limite);
        cartao.DiaFechamento.Should().Be(cartaoEsperado.DiaFechamento);
        cartao.DiaVencimento.Should().Be(cartaoEsperado.DiaVencimento);
        cartao.InstituicaoId.Should().Be(cartaoEsperado.InstituicaoId);
    }

    [Fact]
    public async Task ObterPorId_DeveRetornarNotFound_QuandoCartaoInexistente()
    {
        var resposta = await Client.GetAsync(
            $"{RotaBase}/{int.MaxValue}",
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    // ============================================================================
    // POST /api/v1/financas/cartoes
    // ============================================================================

    [Fact]
    public async Task Criar_DeveRetornarCreated_QuandoDadosValidos()
    {
        var dto = CartaoDtoBuilder.Criar(Factory.BaseData.Instituicao.Id);

        var resposta = await Client.PostAsJsonAsync(
            RotaBase,
            dto,
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.Created);

        var cartao = await resposta.Content
            .ReadFromJsonAsync<CartaoDetalheDto>(
                TestContext.Current.CancellationToken);

        cartao.Should().NotBeNull();
        cartao.Id.Should().BeGreaterThan(0);
        cartao.Nome.Should().Be(dto.Nome);

        resposta.Headers.Location.Should().NotBeNull();
    }

    [Fact]
    public async Task Criar_DeveRetornarBadRequest_QuandoNomeVazio()
    {
        var dto = CartaoDtoBuilder.Criar(
            Factory.BaseData.Instituicao.Id,
            nome: String.Empty);

        var resposta = await Client.PostAsJsonAsync(
            RotaBase,
            dto,
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Criar_DeveRetornarConflict_QuandoDuplicarNome()
    {
        var dto = CartaoDtoBuilder.Criar(
            Factory.BaseData.Instituicao.Id,
            nome: Factory.BaseData.Cartao.Nome);

        var resposta = await Client.PostAsJsonAsync(
            RotaBase,
            dto,
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    // ============================================================================
    // PUT /api/v1/financas/cartoes/{cartaoId}
    // ============================================================================

    [Fact]
    public async Task Alterar_DeveRetornarNoContent_QuandoDadosValidos()
    {
        var dto = CartaoDtoBuilder.Alterar();

        var cartao = await CartaoFixture.CriarAsync(Factory);

        var resposta = await Client.PutAsJsonAsync(
            $"{RotaBase}/{cartao.Id}",
            dto,
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Alterar_DeveRetornarBadRequest_QuandoNomeVazio()
    {
        var dto = CartaoDtoBuilder.Alterar(nome: String.Empty);

        var resposta = await Client.PutAsJsonAsync(
            $"{RotaBase}/{Factory.BaseData.Cartao.Id}",
            dto,
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Alterar_DeveRetornarNotFound_QuandoCartaoInexistente()
    {
        var dto = CartaoDtoBuilder.Alterar();

        var resposta = await Client.PutAsJsonAsync(
            $"{RotaBase}/{int.MaxValue}",
            dto,
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Alterar_DeveRetornarConflict_QuandoDuplicarNome()
    {
        var cartao = await CartaoFixture.CriarAsync(Factory);

        var dto = CartaoDtoBuilder.Alterar(nome: Factory.BaseData.Cartao.Nome);

        var resposta = await Client.PutAsJsonAsync(
            $"{RotaBase}/{cartao.Id}",
            dto,
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    // ============================================================================
    // DELETE /api/v1/financas/cartoes/{cartaoId}
    // ============================================================================

    [Fact]
    public async Task Excluir_DeveRetornarNoContent_QuandoDadosValidos()
    {
        var cartao = await CartaoFixture.CriarAsync(Factory);

        var resposta = await Client.DeleteAsync(
            $"{RotaBase}/{cartao.Id}",
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Excluir_DeveRetornarNotFound_QuandoCartaoInexistente()
    {
        var resposta = await Client.DeleteAsync(
            $"{RotaBase}/{int.MaxValue}",
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Excluir_DeveRetornarConflict_QuandoCartaoBase()
    {
        var resposta = await Client.DeleteAsync(
            $"{RotaBase}/{Factory.BaseData.Cartao.Id}",
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }
}
