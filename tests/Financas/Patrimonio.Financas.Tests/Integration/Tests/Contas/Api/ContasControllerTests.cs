using FluentAssertions;
using Patrimonio.Financas.Contracts.Contas.Dtos;
using Patrimonio.Financas.Tests.Integration.Base;
using Patrimonio.Financas.Tests.Integration.Builders;
using Patrimonio.Financas.Tests.Integration.Config;
using Patrimonio.Financas.Tests.Integration.Fixtures;
using System.Net;
using System.Net.Http.Json;

namespace Patrimonio.Financas.Tests.Integration.Tests.Contas.Api;

public sealed class ContasControllerTests(IntegrationTestFactory factory)
    : ApiIntegrationTestBase(factory)
{
    private const string RotaBase = "/api/v1/financas/contas";

    // ============================================================================
    // GET /api/v1/financas/contas
    // ============================================================================

    [Fact]
    public async Task Listar_DeveRetornarOk_QuandoDadosValidos()
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
    public async Task ObterPorId_DeveRetornarOk_QuandoContaExiste()
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
    public async Task ObterPorId_DeveRetornarNotFound_QuandoContaInexistente()
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
    public async Task Criar_DeveRetornarCreated_QuandoDadosValidos()
    {
        var dto = ContaDtoBuilder.Criar(Factory.BaseData.Instituicao.Id);

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
    public async Task Criar_DeveRetornarBadRequest_QuandoNomeVazio()
    {
        var dto = ContaDtoBuilder.Criar(
            Factory.BaseData.Instituicao.Id,
            nome: string.Empty);

        var resposta = await Client.PostAsJsonAsync(
            RotaBase,
            dto,
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Criar_DeveRetornarConflict_QuandoDuplicarNome()
    {
        var dto = ContaDtoBuilder.Criar(
            Factory.BaseData.Instituicao.Id,
            nome: Factory.BaseData.Conta.Nome);

        var resposta = await Client.PostAsJsonAsync(
            RotaBase,
            dto,
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    // ============================================================================
    // PUT /api/v1/financas/contas/{contaId}
    // ============================================================================

    [Fact]
    public async Task Alterar_DeveRetornarNoContent_QuandoDadosValidos()
    {
        var dto = ContaDtoBuilder.Alterar(Factory.BaseData.Instituicao.Id);

        var conta = await ContaFixture.CriarAsync(Factory);

        var resposta = await Client.PutAsJsonAsync(
            $"{RotaBase}/{conta.Id}",
            dto,
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Alterar_DeveRetornarBadRequest_QuandoNomeVazio()
    {
        var dto = ContaDtoBuilder.Alterar(
            Factory.BaseData.Instituicao.Id,
            nome: string.Empty);

        var resposta = await Client.PutAsJsonAsync(
            $"{RotaBase}/{Factory.BaseData.Conta.Id}",
            dto,
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Alterar_DeveRetornarNotFound_QuandoContaInexistente()
    {
        var dto = ContaDtoBuilder.Alterar(Factory.BaseData.Instituicao.Id);

        var resposta = await Client.PutAsJsonAsync(
            $"{RotaBase}/{int.MaxValue}",
            dto,
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Alterar_DeveRetornarConflict_QuandoDuplicarNome()
    {
        var conta = await ContaFixture.CriarAsync(Factory);

        var dto = ContaDtoBuilder.Alterar(
            Factory.BaseData.Instituicao.Id,
            nome: Factory.BaseData.Conta.Nome);

        var resposta = await Client.PutAsJsonAsync(
            $"{RotaBase}/{conta.Id}",
            dto,
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    // ============================================================================
    // DELETE /api/v1/financas/contas/{contaId}
    // ============================================================================

    [Fact]
    public async Task Excluir_DeveRetornarNoContent_QuandoDadosValidos()
    {
        var conta = await ContaFixture.CriarAsync(Factory);

        var resposta = await Client.DeleteAsync(
            $"{RotaBase}/{conta.Id}",
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Excluir_DeveRetornarNotFound_QuandoContaInexistente()
    {
        var resposta = await Client.DeleteAsync(
            $"{RotaBase}/{int.MaxValue}",
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Excluir_DeveRetornarConflict_QuandoContaBase()
    {
        var resposta = await Client.DeleteAsync(
            $"{RotaBase}/{Factory.BaseData.Conta.Id}",
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }
}
