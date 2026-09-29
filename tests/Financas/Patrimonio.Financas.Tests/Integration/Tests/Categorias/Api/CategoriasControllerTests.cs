using FluentAssertions;
using Patrimonio.Financas.Contracts.Categorias.Dtos;
using Patrimonio.Financas.Tests.Integration.Base;
using Patrimonio.Financas.Tests.Integration.Builders;
using Patrimonio.Financas.Tests.Integration.Config;
using Patrimonio.Financas.Tests.Integration.Fixtures;
using System.Net;
using System.Net.Http.Json;

namespace Patrimonio.Financas.Tests.Integration.Tests.Categorias.Api;

public sealed class CategoriasControllerTests(IntegrationTestFactory factory)
    : ApiIntegrationTestBase(factory)
{
    private const string RotaBase = "/api/v1/financas/categorias";

    // ============================================================================
    // GET /api/v1/financas/categorias
    // ============================================================================

    [Fact]
    public async Task Listar_DeveRetornarOkComCategorias_QuandoDadosValidos()
    {
        var resposta = await Client.GetAsync(
            RotaBase,
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.OK);

        var categorias = await resposta.Content
            .ReadFromJsonAsync<IReadOnlyCollection<CategoriaListaDto>>(TestContext.Current.CancellationToken);

        categorias.Should().NotBeNull();
        categorias.Should().Contain(c => c.Id == Factory.BaseData.Categoria.Id);
    }

    // ============================================================================
    // GET /api/v1/financas/categorias/{categoriaId}
    // ============================================================================

    [Fact]
    public async Task ObterPorId_DeveRetornarOkComCategoria_QuandoDadosValidos()
    {
        var categoriaEsperada = Factory.BaseData.Categoria;

        var resposta = await Client.GetAsync(
            $"{RotaBase}/{categoriaEsperada.Id}",
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.OK);

        var categoria = await resposta.Content
            .ReadFromJsonAsync<CategoriaDetalheDto>(TestContext.Current.CancellationToken);

        categoria.Should().NotBeNull();
        categoria.Id.Should().Be(categoriaEsperada.Id);
        categoria.Nome.Should().Be(categoriaEsperada.Nome);
    }

    [Fact]
    public async Task ObterPorId_DeveRetornarNotFound_QuandoCategoriaInexistente()
    {
        var resposta = await Client.GetAsync(
            $"{RotaBase}/{int.MaxValue}",
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    // ============================================================================
    // POST /api/v1/financas/categorias
    // ============================================================================

    [Fact]
    public async Task Criar_DeveRetornarCreated_QuandoDadosValidos()
    {
        var dto = CategoriaDtoBuilder.Criar();

        var resposta = await Client.PostAsJsonAsync(
            RotaBase,
            dto,
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.Created);

        var categoria = await resposta.Content
            .ReadFromJsonAsync<CategoriaDetalheDto>(
                TestContext.Current.CancellationToken);

        categoria.Should().NotBeNull();
        categoria.Id.Should().BeGreaterThan(0);
        categoria.Nome.Should().Be(dto.Nome);

        resposta.Headers.Location.Should().NotBeNull();
    }

    [Fact]
    public async Task Criar_DeveRetornarBadRequest_QuandoNomeVazio()
    {
        var dto = CategoriaDtoBuilder.Criar(string.Empty);

        var resposta = await Client.PostAsJsonAsync(
            RotaBase,
            dto,
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Criar_DeveRetornarConflict_QuandoDuplicarNome()
    {
        var dto = CategoriaDtoBuilder.Criar(Factory.BaseData.Categoria.Nome);

        var resposta = await Client.PostAsJsonAsync(
            RotaBase,
            dto,
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    // ============================================================================
    // PUT /api/v1/financas/categorias/{categoriaId}
    // ============================================================================

    [Fact]
    public async Task Alterar_DeveRetornarNoContent_QuandoDadosValidos()
    {
        var dto = CategoriaDtoBuilder.Alterar();

        var categoria = await CategoriaFixture.CriarAsync(Factory);

        var resposta = await Client.PutAsJsonAsync(
            $"{RotaBase}/{categoria.Id}",
            dto,
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Alterar_DeveRetornarBadRequest_QuandoNomeVazio()
    {
        var dto = CategoriaDtoBuilder.Alterar(string.Empty);

        var resposta = await Client.PutAsJsonAsync(
            $"{RotaBase}/{Factory.BaseData.Categoria.Id}",
            dto,
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Alterar_DeveRetornarNotFound_QuandoCategoriaInexistente()
    {
        var dto = CategoriaDtoBuilder.Alterar();

        var resposta = await Client.PutAsJsonAsync(
            $"{RotaBase}/{int.MaxValue}",
            dto,
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Alterar_DeveRetornarConflict_QuandoDuplicarNome()
    {
        var categoria = await CategoriaFixture.CriarAsync(Factory);

        var dto = CategoriaDtoBuilder.Alterar(Factory.BaseData.Categoria.Nome);

        var resposta = await Client.PutAsJsonAsync(
            $"{RotaBase}/{categoria.Id}",
            dto,
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    // ============================================================================
    // DELETE /api/v1/financas/categorias/{categoriaId}
    // ============================================================================

    [Fact]
    public async Task Excluir_DeveRetornarNoContent_QuandoDadosValidos()
    {
        var categoria = await CategoriaFixture.CriarAsync(Factory);

        var resposta = await Client.DeleteAsync(
            $"{RotaBase}/{categoria.Id}",
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Excluir_DeveRetornarNotFound_QuandoCategoriaInexistente()
    {
        var resposta = await Client.DeleteAsync(
            $"{RotaBase}/{int.MaxValue}",
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Excluir_DeveRetornarConflict_QuandoCategoriaBase()
    {
        var resposta = await Client.DeleteAsync(
            $"{RotaBase}/{Factory.BaseData.Categoria.Id}",
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }
}
