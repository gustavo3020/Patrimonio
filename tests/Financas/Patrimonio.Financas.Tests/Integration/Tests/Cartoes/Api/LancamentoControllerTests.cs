using FluentAssertions;
using Patrimonio.Financas.Contracts.Cartoes.Dtos;
using Patrimonio.Financas.Tests.Integration.Base;
using Patrimonio.Financas.Tests.Integration.Builders;
using Patrimonio.Financas.Tests.Integration.Config;
using Patrimonio.Financas.Tests.Integration.Fixtures;
using System.Net;
using System.Net.Http.Json;

namespace Patrimonio.Financas.Tests.Integration.Tests.Cartoes.Api;

public sealed class LancamentoControllerTests(IntegrationTestFactory factory)
    : ApiIntegrationTestBase(factory)
{
    private const string RotaBase = "/api/v1/financas/lancamentos";

    // ============================================================================
    // GET /api/v1/financas/lancamentos
    // ============================================================================

    [Fact]
    public async Task Listar_DeveRetornarOk_QuandoFaturaTemLancamentos()
    {
        var faturaId = Factory.BaseData.Fatura.Id;

        var resposta = await Client.GetAsync(
            $"{RotaBase}?faturaId={faturaId}",
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.OK);

        var lancamentos = await resposta.Content
            .ReadFromJsonAsync<IReadOnlyCollection<LancamentoListaDto>>(TestContext.Current.CancellationToken);

        lancamentos.Should().NotBeNull();
        lancamentos.Should().Contain(m => m.Id == Factory.BaseData.Lancamento.Id);
    }

    // ============================================================================
    // GET /api/v1/financas/lancamentos/{lancamentoId}
    // ============================================================================

    [Fact]
    public async Task ObterPorId_DeveRetornarOk_QuandoLancamentoExiste()
    {
        var lancamentoEsperado = Factory.BaseData.Lancamento;

        var resposta = await Client.GetAsync(
            $"{RotaBase}/{lancamentoEsperado.Id}",
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.OK);

        var lancamento = await resposta.Content
            .ReadFromJsonAsync<LancamentoDetalheDto>(TestContext.Current.CancellationToken);

        lancamento.Should().NotBeNull();
        lancamento.Id.Should().Be(lancamentoEsperado.Id);
        lancamento.Descricao.Should().Be(lancamentoEsperado.Descricao);
    }

    [Fact]
    public async Task ObterPorId_DeveRetornarNotFound_QuandoLancamentoInexistente()
    {
        var resposta = await Client.GetAsync(
            $"{RotaBase}/{int.MaxValue}",
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    // ============================================================================
    // POST /api/v1/financas/lancamentos
    // ============================================================================

    [Fact]
    public async Task Criar_DeveRetornarCreated_QuandoDadosValidos()
    {
        var dto = LancamentoDtoBuilder.Criar(
            Factory.BaseData.Fatura.Id,
            Factory.BaseData.Categoria.Id);

        var resposta = await Client.PostAsJsonAsync(
            RotaBase,
            dto,
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.Created);

        var lancamento = await resposta.Content
            .ReadFromJsonAsync<LancamentoDetalheDto>(
                TestContext.Current.CancellationToken);

        lancamento.Should().NotBeNull();
        lancamento.Id.Should().BeGreaterThan(0);
        lancamento.Descricao.Should().Be(dto.Descricao);

        resposta.Headers.Location.Should().NotBeNull();
    }

    [Fact]
    public async Task Criar_DeveRetornarBadRequest_QuandoDescricaoVazia()
    {
        var dto = LancamentoDtoBuilder.Criar(
            Factory.BaseData.Fatura.Id,
            Factory.BaseData.Categoria.Id,
            descricao: string.Empty);

        var resposta = await Client.PostAsJsonAsync(
            RotaBase,
            dto,
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Criar_DeveRetornarConflict_QuandoCategoriaInexistente()
    {
        var dto = LancamentoDtoBuilder.Criar(
            Factory.BaseData.Fatura.Id,
            int.MaxValue);

        var resposta = await Client.PostAsJsonAsync(
            RotaBase,
            dto,
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    // ============================================================================
    // PUT /api/v1/financas/lancamentos/{lancamentoId}
    // ============================================================================

    [Fact]
    public async Task Alterar_DeveRetornarNoContent_QuandoDadosValidos()
    {
        var dto = LancamentoDtoBuilder.Alterar(Factory.BaseData.Categoria.Id);

        var lancamento = await LancamentoFixture.CriarAsync(Factory);

        var resposta = await Client.PutAsJsonAsync(
            $"{RotaBase}/{lancamento.Id}",
            dto,
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Alterar_DeveRetornarBadRequest_QuandoDescricaoVazia()
    {
        var dto = LancamentoDtoBuilder.Alterar(
            Factory.BaseData.Categoria.Id,
            descricao: string.Empty);

        var resposta = await Client.PutAsJsonAsync(
            $"{RotaBase}/{Factory.BaseData.Lancamento.Id}",
            dto,
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Alterar_DeveRetornarNotFound_QuandoLancamentoInexistente()
    {
        var dto = LancamentoDtoBuilder.Alterar(Factory.BaseData.Categoria.Id);

        var resposta = await Client.PutAsJsonAsync(
            $"{RotaBase}/{int.MaxValue}",
            dto,
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Alterar_DeveRetornarConflict_QuandoCategoriaInexistente()
    {
        var dto = LancamentoDtoBuilder.Alterar(int.MaxValue);

        var resposta = await Client.PutAsJsonAsync(
            $"{RotaBase}/{Factory.BaseData.Lancamento.Id}",
            dto,
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    // ============================================================================
    // DELETE /api/v1/financas/lancamentos/{lancamentoId}
    // ============================================================================

    [Fact]
    public async Task Excluir_DeveRetornarNoContent_QuandoDadosValidos()
    {
        var lancamento = await LancamentoFixture.CriarAsync(Factory);

        var resposta = await Client.DeleteAsync(
            $"{RotaBase}/{lancamento.Id}",
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Excluir_DeveRetornarBadRequest_QuandoExcluirLancamentoEmFaturaFechada()
    {
        var lancamento = await LancamentoFixture.CriarEmFaturaFechadaAsync(Factory);

        var resposta = await Client.DeleteAsync(
            $"{RotaBase}/{lancamento.Id}",
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Excluir_DeveRetornarNotFound_QuandoLancamentoInexistente()
    {
        var resposta = await Client.DeleteAsync(
            $"{RotaBase}/{int.MaxValue}",
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
