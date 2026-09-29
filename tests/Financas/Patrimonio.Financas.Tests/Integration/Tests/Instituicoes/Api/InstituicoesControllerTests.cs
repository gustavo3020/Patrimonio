using FluentAssertions;
using Patrimonio.Financas.Contracts.Instituicoes.Dtos;
using Patrimonio.Financas.Tests.Integration.Base;
using Patrimonio.Financas.Tests.Integration.Builders;
using Patrimonio.Financas.Tests.Integration.Config;
using Patrimonio.Financas.Tests.Integration.Fixtures;
using System.Net;
using System.Net.Http.Json;

namespace Patrimonio.Financas.Tests.Integration.Tests.Instituicoes.Api;

public sealed class InstituicoesControllerTests(IntegrationTestFactory factory)
    : ApiIntegrationTestBase(factory)
{
    private const string RotaBase = "/api/v1/financas/instituicoes";

    // ============================================================================
    // GET /api/v1/financas/instituicoes
    // ============================================================================

    [Fact]
    public async Task Listar_DeveRetornarOk_QuandoDadosValidos()
    {
        var resposta = await Client.GetAsync(
            RotaBase,
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.OK);

        var instituicoes = await resposta.Content
            .ReadFromJsonAsync<IReadOnlyCollection<InstituicaoListaDto>>(TestContext.Current.CancellationToken);

        instituicoes.Should().NotBeNull();
        instituicoes.Should().Contain(i => i.Id == Factory.BaseData.Instituicao.Id);
    }

    // ============================================================================
    // GET /api/v1/financas/instituicoes/{instituicaoId}
    // ============================================================================

    [Fact]
    public async Task ObterPorId_DeveRetornarOk_QuandoInstituicaoExiste()
    {
        var instituicaoEsperada = Factory.BaseData.Instituicao;

        var resposta = await Client.GetAsync(
            $"{RotaBase}/{instituicaoEsperada.Id}",
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.OK);

        var instituicao = await resposta.Content
            .ReadFromJsonAsync<InstituicaoDetalheDto>(TestContext.Current.CancellationToken);

        instituicao.Should().NotBeNull();
        instituicao.Id.Should().Be(instituicaoEsperada.Id);
        instituicao.Nome.Should().Be(instituicaoEsperada.Nome);
    }

    [Fact]
    public async Task ObterPorId_DeveRetornarNotFound_QuandoInstituicaoInexistente()
    {
        var resposta = await Client.GetAsync(
            $"{RotaBase}/{int.MaxValue}",
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    // ============================================================================
    // POST /api/v1/financas/instituicoes
    // ============================================================================

    [Fact]
    public async Task Criar_DeveRetornarCreated_QuandoDadosValidos()
    {
        var dto = InstituicaoDtoBuilder.Criar();

        var resposta = await Client.PostAsJsonAsync(
            RotaBase,
            dto,
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.Created);

        var instituicao = await resposta.Content
            .ReadFromJsonAsync<InstituicaoDetalheDto>(
                TestContext.Current.CancellationToken);

        instituicao.Should().NotBeNull();
        instituicao.Id.Should().BeGreaterThan(0);
        instituicao.Nome.Should().Be(dto.Nome);

        resposta.Headers.Location.Should().NotBeNull();
    }

    [Fact]
    public async Task Criar_DeveRetornarBadRequest_QuandoNomeVazio()
    {
        var dto = InstituicaoDtoBuilder.Criar(string.Empty);

        var resposta = await Client.PostAsJsonAsync(
            RotaBase,
            dto,
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Criar_DeveRetornarConflict_QuandoDuplicarNome()
    {
        var dto = InstituicaoDtoBuilder.Criar(Factory.BaseData.Instituicao.Nome);

        var resposta = await Client.PostAsJsonAsync(
            RotaBase,
            dto,
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    // ============================================================================
    // PUT /api/v1/financas/instituicoes/{instituicaoId}
    // ============================================================================

    [Fact]
    public async Task Alterar_DeveRetornarNoContent_QuandoDadosValidos()
    {
        var dto = InstituicaoDtoBuilder.Alterar();

        var instituicao = await InstituicaoFixture.CriarAsync(Factory);

        var resposta = await Client.PutAsJsonAsync(
            $"{RotaBase}/{instituicao.Id}",
            dto,
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Alterar_DeveRetornarBadRequest_QuandoNomeVazio()
    {
        var dto = InstituicaoDtoBuilder.Alterar(string.Empty);

        var resposta = await Client.PutAsJsonAsync(
            $"{RotaBase}/{Factory.BaseData.Instituicao.Id}",
            dto,
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Alterar_DeveRetornarNotFound_QuandoInstituicaoInexistente()
    {
        var dto = InstituicaoDtoBuilder.Alterar();

        var resposta = await Client.PutAsJsonAsync(
            $"{RotaBase}/{int.MaxValue}",
            dto,
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Alterar_DeveRetornarConflict_QuandoDuplicarNome()
    {
        var instituicao = await InstituicaoFixture.CriarAsync(Factory);

        var dto = InstituicaoDtoBuilder.Alterar(Factory.BaseData.Instituicao.Nome);

        var resposta = await Client.PutAsJsonAsync(
            $"{RotaBase}/{instituicao.Id}",
            dto,
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    // ============================================================================
    // DELETE /api/v1/financas/instituicoes/{instituicaoId}
    // ============================================================================

    [Fact]
    public async Task Excluir_DeveRetornarNoContent_QuandoDadosValidos()
    {
        var instituicao = await InstituicaoFixture.CriarAsync(Factory);

        var resposta = await Client.DeleteAsync(
            $"{RotaBase}/{instituicao.Id}",
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Excluir_DeveRetornarNotFound_QuandoInstituicaoInexistente()
    {
        var resposta = await Client.DeleteAsync(
            $"{RotaBase}/{int.MaxValue}",
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Excluir_DeveRetornarConflict_QuandoInstituicaoBase()
    {
        var resposta = await Client.DeleteAsync(
            $"{RotaBase}/{Factory.BaseData.Instituicao.Id}",
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }
}
