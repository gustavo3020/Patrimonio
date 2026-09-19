using FluentAssertions;
using Patrimonio.Financas.Contracts.Categorias.Dtos;
using Patrimonio.Financas.Tests.Integration.Base;
using Patrimonio.Financas.Tests.Integration.Config;
using System.Net;
using System.Net.Http.Json;

namespace Patrimonio.Financas.Tests.Integration.Tests.Categorias.Api;

public sealed class CategoriasControllerTests(IntegrationTestFactory factory)
    : ApiIntegrationTestBase(factory)
{
    private const string RotaBase = "/api/v1/financas/categorias";

    // ============================================================================
    // DTOs
    // ============================================================================

    private static CategoriaCriacaoDto CriarDto()
    {
        return new CategoriaCriacaoDto
        {
            Nome = $"Categoria {Guid.NewGuid()}"
        };
    }

    private static CategoriaAlteracaoDto AlterarDto()
    {
        return new CategoriaAlteracaoDto
        {
            Nome = $"Categoria alterada {Guid.NewGuid()}"
        };
    }

    // ============================================================================
    // GET /api/v1/financas/categorias
    // ============================================================================

    [Fact]
    public async Task Listar_DeveRetornarOkComCategorias()
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
    public async Task ObterPorId_DeveRetornarOkComCategoria()
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
    public async Task ObterPorId_DeveRetornarNotFoundParaCategoriaInexistente()
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
    public async Task Criar_DeveRetornarCreatedComCategoriaCriada()
    {
        var dto = CriarDto();

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
    public async Task Criar_DeveRetornarBadRequestAoEnviarNomeVazio()
    {
        var dto = new CategoriaCriacaoDto
        {
            Nome = string.Empty
        };

        var resposta = await Client.PostAsJsonAsync(
            RotaBase,
            dto,
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Criar_DeveRetornarConflictAoDuplicarNome()
    {
        var dto = CriarDto();

        var primeiraResposta = await Client.PostAsJsonAsync(
            RotaBase,
            dto,
            TestContext.Current.CancellationToken);

        primeiraResposta.StatusCode.Should().Be(HttpStatusCode.Created);

        var segundaResposta = await Client.PostAsJsonAsync(
            RotaBase,
            dto,
            TestContext.Current.CancellationToken);

        segundaResposta.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    // ============================================================================
    // PUT /api/v1/financas/categorias/{categoriaId}
    // ============================================================================

    [Fact]
    public async Task Alterar_DeveRetornarNoContent()
    {
        var criarDto = CriarDto();

        var criarResponse = await Client.PostAsJsonAsync(
            RotaBase,
            criarDto,
            TestContext.Current.CancellationToken);

        criarResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        var categoria = await criarResponse.Content
            .ReadFromJsonAsync<CategoriaDetalheDto>(
                TestContext.Current.CancellationToken);

        categoria.Should().NotBeNull();

        var alterarDto = AlterarDto();

        var resposta = await Client.PutAsJsonAsync(
            $"{RotaBase}/{categoria.Id}",
            alterarDto,
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Alterar_DeveRetornarBadRequestAoEnviarNomeVazio()
    {
        var alterarDto = new CategoriaAlteracaoDto
        {
            Nome = string.Empty
        };

        var resposta = await Client.PutAsJsonAsync(
            $"{RotaBase}/{Factory.BaseData.Categoria.Id}",
            alterarDto,
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Alterar_DeveRetornarNotFoundParaCategoriaInexistente()
    {
        var dto = AlterarDto();

        var resposta = await Client.PutAsJsonAsync(
            $"{RotaBase}/{int.MaxValue}",
            dto,
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Alterar_DeveRetornarConflictAoDuplicarNome()
    {
        var dtoCriacao = CriarDto();

        var criarResponse = await Client.PostAsJsonAsync(
            RotaBase,
            dtoCriacao,
            TestContext.Current.CancellationToken);

        criarResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        var categoria = await criarResponse.Content
            .ReadFromJsonAsync<CategoriaDetalheDto>(
                TestContext.Current.CancellationToken);

        categoria.Should().NotBeNull();

        var dtoAlteracao = new CategoriaAlteracaoDto
        {
            Nome = Factory.BaseData.Categoria.Nome
        };

        var resposta = await Client.PutAsJsonAsync(
            $"{RotaBase}/{categoria.Id}",
            dtoAlteracao,
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    // ============================================================================
    // DELETE /api/v1/financas/categorias/{categoriaId}
    // ============================================================================

    [Fact]
    public async Task Excluir_DeveRetornarNoContent()
    {
        var dto = CriarDto();

        var criarResponse = await Client.PostAsJsonAsync(
            RotaBase,
            dto,
            TestContext.Current.CancellationToken);

        criarResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        var categoria = await criarResponse.Content
            .ReadFromJsonAsync<CategoriaDetalheDto>(
                TestContext.Current.CancellationToken);

        categoria.Should().NotBeNull();

        var resposta = await Client.DeleteAsync(
            $"{RotaBase}/{categoria.Id}",
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Excluir_DeveRetornarNotFoundParaCategoriaInexistente()
    {
        var resposta = await Client.DeleteAsync(
            $"{RotaBase}/{int.MaxValue}",
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Excluir_DeveRetornarConflictAoExcluirCategoriaBase()
    {
        var resposta = await Client.DeleteAsync(
            $"{RotaBase}/{Factory.BaseData.Categoria.Id}",
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }
}
