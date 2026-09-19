using FluentAssertions;
using Patrimonio.Financas.Contracts.Instituicoes.Dtos;
using Patrimonio.Financas.Tests.Integration.Base;
using Patrimonio.Financas.Tests.Integration.Config;
using System.Net;
using System.Net.Http.Json;

namespace Patrimonio.Financas.Tests.Integration.Tests.Instituicoes.Api;

public sealed class InstituicoesControllerTests(IntegrationTestFactory factory)
    : ApiIntegrationTestBase(factory)
{
    private const string RotaBase = "/api/v1/financas/instituicoes";

    // ============================================================================
    // DTOs
    // ============================================================================

    private static InstituicaoCriacaoDto CriarDto()
    {
        return new InstituicaoCriacaoDto
        {
            Nome = $"Instituição {Guid.NewGuid()}"
        };
    }

    private static InstituicaoAlteracaoDto AlterarDto()
    {
        return new InstituicaoAlteracaoDto
        {
            Nome = $"Instituição alterada {Guid.NewGuid()}"
        };
    }

    // ============================================================================
    // GET /api/v1/financas/instituicoes
    // ============================================================================

    [Fact]
    public async Task Listar_DeveRetornarOkComInstituicoes()
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
    public async Task ObterPorId_DeveRetornarOkComInstituicao()
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
    public async Task ObterPorId_DeveRetornarNotFoundParaInstituicaoInexistente()
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
    public async Task Criar_DeveRetornarCreatedComInstituicaoCriada()
    {
        var dto = CriarDto();

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
    public async Task Criar_DeveRetornarBadRequestAoEnviarNomeVazio()
    {
        var dto = new InstituicaoCriacaoDto
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
    // PUT /api/v1/financas/instituicoes/{instituicaoId}
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

        var instituicao = await criarResponse.Content
            .ReadFromJsonAsync<InstituicaoDetalheDto>(
                TestContext.Current.CancellationToken);

        instituicao.Should().NotBeNull();

        var alterarDto = AlterarDto();

        var resposta = await Client.PutAsJsonAsync(
            $"{RotaBase}/{instituicao.Id}",
            alterarDto,
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Alterar_DeveRetornarBadRequestAoEnviarNomeVazio()
    {
        var alterarDto = new InstituicaoAlteracaoDto
        {
            Nome = string.Empty
        };

        var resposta = await Client.PutAsJsonAsync(
            $"{RotaBase}/{Factory.BaseData.Instituicao.Id}",
            alterarDto,
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Alterar_DeveRetornarNotFoundParaInstituicaoInexistente()
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

        var instituicao = await criarResponse.Content
            .ReadFromJsonAsync<InstituicaoDetalheDto>(
                TestContext.Current.CancellationToken);

        instituicao.Should().NotBeNull();

        var dtoAlteracao = new InstituicaoAlteracaoDto
        {
            Nome = Factory.BaseData.Instituicao.Nome
        };

        var resposta = await Client.PutAsJsonAsync(
            $"{RotaBase}/{instituicao.Id}",
            dtoAlteracao,
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    // ============================================================================
    // DELETE /api/v1/financas/instituicoes/{instituicaoId}
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

        var instituicao = await criarResponse.Content
            .ReadFromJsonAsync<InstituicaoDetalheDto>(
                TestContext.Current.CancellationToken);

        instituicao.Should().NotBeNull();

        var resposta = await Client.DeleteAsync(
            $"{RotaBase}/{instituicao.Id}",
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Excluir_DeveRetornarNotFoundParaInstituicaoInexistente()
    {
        var resposta = await Client.DeleteAsync(
            $"{RotaBase}/{int.MaxValue}",
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Excluir_DeveRetornarConflictAoExcluirInstituicaoBase()
    {
        var resposta = await Client.DeleteAsync(
            $"{RotaBase}/{Factory.BaseData.Instituicao.Id}",
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }
}
