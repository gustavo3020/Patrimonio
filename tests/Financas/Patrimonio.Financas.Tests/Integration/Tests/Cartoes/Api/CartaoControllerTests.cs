using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Patrimonio.Financas.Contracts.Cartoes.Dtos;
using Patrimonio.Financas.Contracts.Cartoes.Services;
using Patrimonio.Financas.SharedKernel.Cartoes.Enums;
using Patrimonio.Financas.Tests.Integration.Base;
using Patrimonio.Financas.Tests.Integration.Builders;
using Patrimonio.Financas.Tests.Integration.Config;
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
    public async Task Listar_DeveRetornarOkComCartoes()
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
    public async Task ObterPorId_DeveRetornarOkComCartao()
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
    public async Task ObterPorId_DeveRetornarNotFoundParaCartaoInexistente()
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
    public async Task Criar_DeveRetornarCreatedComCartaoCriado()
    {
        var criarDto = CartaoDtoBuilder.Criar(Factory.BaseData.Instituicao.Id);

        var resposta = await Client.PostAsJsonAsync(
            RotaBase,
            criarDto,
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.Created);

        var cartao = await resposta.Content
            .ReadFromJsonAsync<CartaoDetalheDto>(
                TestContext.Current.CancellationToken);

        cartao.Should().NotBeNull();
        cartao.Id.Should().BeGreaterThan(0);
        cartao.Nome.Should().Be(criarDto.Nome);

        resposta.Headers.Location.Should().NotBeNull();
    }

    [Fact]
    public async Task Criar_DeveRetornarBadRequestAoEnviarNomeVazio()
    {
        var criarDto = new CartaoCriacaoDto
        {
            Nome = string.Empty,
            Bandeira = BandeiraCartao.Mastercard,
            Limite = 5000,
            DiaFechamento = 15,
            DiaVencimento = 20,
            InstituicaoId = Factory.BaseData.Instituicao.Id
        };

        var resposta = await Client.PostAsJsonAsync(
            RotaBase,
            criarDto,
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Criar_DeveRetornarConflictAoDuplicarNome()
    {
        var criarDto = new CartaoCriacaoDto
        {
            Nome = Factory.BaseData.Cartao.Nome,
            Bandeira = BandeiraCartao.Mastercard,
            Limite = 5000,
            DiaFechamento = 15,
            DiaVencimento = 20,
            InstituicaoId = Factory.BaseData.Instituicao.Id
        };

        var resposta = await Client.PostAsJsonAsync(
            RotaBase,
            criarDto,
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    // ============================================================================
    // PUT /api/v1/financas/cartoes/{cartaoId}
    // ============================================================================

    [Fact]
    public async Task Alterar_DeveRetornarNoContent()
    {
        var cartao = await CriarAsync();

        var alterarDto = CartaoDtoBuilder.Alterar();

        var resposta = await Client.PutAsJsonAsync(
            $"{RotaBase}/{cartao.Id}",
            alterarDto,
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Alterar_DeveRetornarBadRequestAoEnviarNomeVazio()
    {
        var alterarDto = new CartaoAlteracaoDto
        {
            Nome = string.Empty,
            Bandeira = BandeiraCartao.Mastercard,
            Limite = 5000,
            DiaFechamento = 15,
            DiaVencimento = 20
        };

        var resposta = await Client.PutAsJsonAsync(
            $"{RotaBase}/{Factory.BaseData.Cartao.Id}",
            alterarDto,
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Alterar_DeveRetornarNotFoundParaCartaoInexistente()
    {
        var dto = CartaoDtoBuilder.Alterar();

        var resposta = await Client.PutAsJsonAsync(
            $"{RotaBase}/{int.MaxValue}",
            dto,
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Alterar_DeveRetornarConflictAoDuplicarNome()
    {
        var cartao = await CriarAsync();

        var dtoAlteracao = new CartaoAlteracaoDto
        {
            Nome = Factory.BaseData.Cartao.Nome,
            Bandeira = BandeiraCartao.Mastercard,
            Limite = 5000,
            DiaFechamento = 15,
            DiaVencimento = 20
        };

        var resposta = await Client.PutAsJsonAsync(
            $"{RotaBase}/{cartao.Id}",
            dtoAlteracao,
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    // ============================================================================
    // DELETE /api/v1/financas/cartoes/{cartaoId}
    // ============================================================================

    [Fact]
    public async Task Excluir_DeveRetornarNoContent()
    {
        var cartao = await CriarAsync();

        var resposta = await Client.DeleteAsync(
            $"{RotaBase}/{cartao.Id}",
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Excluir_DeveRetornarNotFoundParaCartaoInexistente()
    {
        var resposta = await Client.DeleteAsync(
            $"{RotaBase}/{int.MaxValue}",
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Excluir_DeveRetornarConflictAoExcluirCartaoBase()
    {
        var resposta = await Client.DeleteAsync(
            $"{RotaBase}/{Factory.BaseData.Cartao.Id}",
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    // ============================================================================
    // MÉTODOS PRIVADOS
    // ============================================================================

    private async Task<CartaoDetalheDto> CriarAsync()
    {
        using var scope = CreateScope();
        var cartaoCommand = scope.ServiceProvider.GetRequiredService<ICartaoCommandService>();

        var criarDto = CartaoDtoBuilder.Criar(Factory.BaseData.Instituicao.Id);
        return await cartaoCommand.CriarAsync(criarDto, TestContext.Current.CancellationToken);
    }
}
