using FluentAssertions;
using Patrimonio.Financas.Contracts.Contas.Dtos;
using Patrimonio.Financas.Tests.Integration.Base;
using Patrimonio.Financas.Tests.Integration.Config;
using System.Net;
using System.Net.Http.Json;

namespace Patrimonio.Financas.Tests.Integration.Tests.Contas.Api;

public sealed class ContasControllerTests(IntegrationTestFactory factory)
    : ApiIntegrationTestBase(factory)
{
    private const string RotaBase = "/api/v1/financas/contas";

    // ============================================================================
    // DTOs
    // ============================================================================

    private ContaCriacaoDto CriarDto()
    {
        return new ContaCriacaoDto
        {
            Nome = $"Conta {Guid.NewGuid()}",
            InstituicaoId = Factory.BaseData.Instituicao.Id
        };
    }

    private ContaAlteracaoDto AlterarDto()
    {
        return new ContaAlteracaoDto
        {
            Nome = $"Conta alterada {Guid.NewGuid()}",
            InstituicaoId = Factory.BaseData.Instituicao.Id
        };
    }

    // ============================================================================
    // GET /api/v1/financas/contas
    // ============================================================================

    [Fact]
    public async Task Listar_DeveRetornarOkComContas()
    {
        var resposta = await Client.GetAsync(
            RotaBase,
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.OK);

        var contas = await resposta.Content
            .ReadFromJsonAsync<IReadOnlyCollection<ContaListaDto>>(TestContext.Current.CancellationToken);

        contas.Should().NotBeNull();
        contas.Should().Contain(c => c.Id == Factory.BaseData.Conta.Id);
    }

    // ============================================================================
    // GET /api/v1/financas/contas/{contaId}
    // ============================================================================

    [Fact]
    public async Task ObterPorId_DeveRetornarOkComConta()
    {
        var contaEsperada = Factory.BaseData.Conta;

        var resposta = await Client.GetAsync(
            $"{RotaBase}/{contaEsperada.Id}",
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.OK);

        var conta = await resposta.Content
            .ReadFromJsonAsync<ContaDetalheDto>(TestContext.Current.CancellationToken);

        conta.Should().NotBeNull();
        conta.Id.Should().Be(contaEsperada.Id);
        conta.Nome.Should().Be(contaEsperada.Nome);
    }

    [Fact]
    public async Task ObterPorId_DeveRetornarNotFoundParaContaInexistente()
    {
        var resposta = await Client.GetAsync(
            $"{RotaBase}/{int.MaxValue}",
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    // ============================================================================
    // POST /api/v1/financas/contas
    // ============================================================================

    [Fact]
    public async Task Criar_DeveRetornarCreatedComContaCriada()
    {
        var dto = CriarDto();

        var resposta = await Client.PostAsJsonAsync(
            RotaBase,
            dto,
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.Created);

        var conta = await resposta.Content
            .ReadFromJsonAsync<ContaDetalheDto>(
                TestContext.Current.CancellationToken);

        conta.Should().NotBeNull();
        conta.Id.Should().BeGreaterThan(0);
        conta.Nome.Should().Be(dto.Nome);

        resposta.Headers.Location.Should().NotBeNull();
    }

    [Fact]
    public async Task Criar_DeveRetornarBadRequestAoEnviarNomeVazio()
    {
        var dto = new ContaCriacaoDto
        {
            Nome = string.Empty,
            InstituicaoId = Factory.BaseData.Instituicao.Id
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
    // PUT /api/v1/financas/contas/{contaId}
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

        var conta = await criarResponse.Content
            .ReadFromJsonAsync<ContaDetalheDto>(
                TestContext.Current.CancellationToken);

        conta.Should().NotBeNull();

        var alterarDto = AlterarDto();

        var resposta = await Client.PutAsJsonAsync(
            $"{RotaBase}/{conta.Id}",
            alterarDto,
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Alterar_DeveRetornarBadRequestAoEnviarNomeVazio()
    {
        var alterarDto = new ContaAlteracaoDto
        {
            Nome = string.Empty,
            InstituicaoId = Factory.BaseData.Instituicao.Id
        };

        var resposta = await Client.PutAsJsonAsync(
            $"{RotaBase}/{Factory.BaseData.Conta.Id}",
            alterarDto,
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Alterar_DeveRetornarNotFoundParaContaInexistente()
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

        var conta = await criarResponse.Content
            .ReadFromJsonAsync<ContaDetalheDto>(
                TestContext.Current.CancellationToken);

        conta.Should().NotBeNull();

        var dtoAlteracao = new ContaAlteracaoDto
        {
            Nome = Factory.BaseData.Conta.Nome,
            InstituicaoId = Factory.BaseData.Instituicao.Id
        };

        var resposta = await Client.PutAsJsonAsync(
            $"{RotaBase}/{conta.Id}",
            dtoAlteracao,
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    // ============================================================================
    // DELETE /api/v1/financas/contas/{contaId}
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

        var conta = await criarResponse.Content
            .ReadFromJsonAsync<ContaDetalheDto>(
                TestContext.Current.CancellationToken);

        conta.Should().NotBeNull();

        var resposta = await Client.DeleteAsync(
            $"{RotaBase}/{conta.Id}",
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Excluir_DeveRetornarNotFoundParaContaInexistente()
    {
        var resposta = await Client.DeleteAsync(
            $"{RotaBase}/{int.MaxValue}",
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Excluir_DeveRetornarConflictAoExcluirContaBase()
    {
        var resposta = await Client.DeleteAsync(
            $"{RotaBase}/{Factory.BaseData.Conta.Id}",
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }
}
