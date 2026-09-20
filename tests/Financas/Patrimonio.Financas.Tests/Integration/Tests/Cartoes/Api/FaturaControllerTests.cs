using FluentAssertions;
using Patrimonio.Financas.Contracts.Cartoes.Dtos;
using Patrimonio.Financas.Tests.Integration.Base;
using Patrimonio.Financas.Tests.Integration.Builders;
using Patrimonio.Financas.Tests.Integration.Config;
using Patrimonio.Financas.Tests.Integration.Fixtures;
using System.Net;
using System.Net.Http.Json;

namespace Patrimonio.Financas.Tests.Integration.Tests.Cartoes.Api;

public sealed class FaturaControllerTests(IntegrationTestFactory factory)
    : ApiIntegrationTestBase(factory)
{
    private const string RotaBase = "/api/v1/financas/faturas";

    // ============================================================================
    // GET /api/v1/financas/faturas
    // ============================================================================

    [Fact]
    public async Task Listar_DeveRetornarOk_QuandoCartaoTemFaturas()
    {
        var cartaoId = Factory.BaseData.Cartao.Id;

        var resposta = await Client.GetAsync(
            $"{RotaBase}?cartaoId={cartaoId}",
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.OK);

        var faturas = await resposta.Content
            .ReadFromJsonAsync<IReadOnlyCollection<FaturaListaDto>>(TestContext.Current.CancellationToken);

        faturas.Should().NotBeNull();
        faturas.Should().Contain(f => f.Id == Factory.BaseData.Fatura.Id);
    }

    // ============================================================================
    // GET /api/v1/financas/faturas/{faturaId}
    // ============================================================================

    [Fact]
    public async Task ObterPorId_DeveRetornarOk_QuandoFaturaExiste()
    {
        var faturaEsperada = Factory.BaseData.Fatura;

        var resposta = await Client.GetAsync(
            $"{RotaBase}/{faturaEsperada.Id}",
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.OK);

        var fatura = await resposta.Content
            .ReadFromJsonAsync<FaturaDetalheDto>(TestContext.Current.CancellationToken);

        fatura.Should().NotBeNull();
        fatura.Id.Should().Be(faturaEsperada.Id);
        fatura.DataFechamento.Should().Be(faturaEsperada.DataFechamento);
        fatura.DataVencimento.Should().Be(faturaEsperada.DataVencimento);
    }

    [Fact]
    public async Task ObterPorId_DeveRetornarNotFound_QuandoFaturaInexistente()
    {
        var resposta = await Client.GetAsync(
            $"{RotaBase}/{int.MaxValue}",
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    // ============================================================================
    // POST /api/v1/financas/faturas
    // ============================================================================

    [Fact]
    public async Task Criar_DeveRetornarCreated_QuandoDadosValidos()
    {
        var cartao = await CartaoFixture.CriarAsync(Factory);

        var dto = FaturaDtoBuilder.Criar(cartao.Id);

        var resposta = await Client.PostAsJsonAsync(
            RotaBase,
            dto,
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.Created);

        var fatura = await resposta.Content
            .ReadFromJsonAsync<FaturaDetalheDto>(
                TestContext.Current.CancellationToken);

        fatura.Should().NotBeNull();
        fatura.Id.Should().BeGreaterThan(0);
        fatura.DataFechamento.Should().Be(dto.DataFechamento);
        fatura.DataVencimento.Should().Be(dto.DataVencimento);

        resposta.Headers.Location.Should().NotBeNull();
    }

    [Fact]
    public async Task Criar_DeveRetornarBadRequest_QuandoDataFechamentoMaiorQueDataVencimento()
    {
        var dto = FaturaDtoBuilder.Criar(
            Factory.BaseData.Cartao.Id,
            dataFechamento: new DateOnly(2024, 1, 11),
            dataVencimento: new DateOnly(2024, 1, 6));

        var resposta = await Client.PostAsJsonAsync(
            RotaBase,
            dto,
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Criar_DeveRetornarConflict_QuandoCartaoInexistente()
    {
        var dto = FaturaDtoBuilder.Criar(int.MaxValue);

        var resposta = await Client.PostAsJsonAsync(
            RotaBase,
            dto,
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    // ============================================================================
    // PUT /api/v1/financas/faturas/{faturaId}
    // ============================================================================

    [Fact]
    public async Task Alterar_DeveRetornarNoContent_QuandoDadosValidos()
    {
        var dto = FaturaDtoBuilder.Alterar();

        var fatura = await FaturaFixture.CriarAsync(Factory);

        var resposta = await Client.PutAsJsonAsync(
            $"{RotaBase}/{fatura.Id}",
            dto,
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Alterar_DeveRetornarBadRequest_QuandoDataFechamentoMaiorQueDataVencimento()
    {
        var dto = FaturaDtoBuilder.Alterar(
            dataFechamento: new DateOnly(2024, 1, 11),
            dataVencimento: new DateOnly(2024, 1, 6));

        var resposta = await Client.PutAsJsonAsync(
            $"{RotaBase}/{Factory.BaseData.Fatura.Id}",
            dto,
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Alterar_DeveRetornarNotFound_QuandoFaturaInexistente()
    {
        var dto = FaturaDtoBuilder.Alterar();

        var resposta = await Client.PutAsJsonAsync(
            $"{RotaBase}/{int.MaxValue}",
            dto,
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    // ============================================================================
    // DELETE /api/v1/financas/faturas/{faturaId}
    // ============================================================================

    [Fact]
    public async Task Excluir_DeveRetornarNoContent_QuandoDadosValidos()
    {
        var fatura = await FaturaFixture.CriarAsync(Factory);

        var resposta = await Client.DeleteAsync(
            $"{RotaBase}/{fatura.Id}",
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Excluir_DeveRetornarBadRequest_QuandoFechada()
    {
        var fatura = await FaturaFixture.CriarFechadaAsync(Factory);

        var resposta = await Client.DeleteAsync(
            $"{RotaBase}/{fatura.Id}",
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Excluir_DeveRetornarNotFound_QuandoFaturaInexistente()
    {
        var resposta = await Client.DeleteAsync(
            $"{RotaBase}/{int.MaxValue}",
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Excluir_DeveRetornarConflict_QuandoFaturaBase()
    {
        var resposta = await Client.DeleteAsync(
            $"{RotaBase}/{Factory.BaseData.Fatura.Id}",
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    // ============================================================================
    // PUT /api/v1/financas/faturas/{faturaId}/fechar
    // ============================================================================

    [Fact]
    public async Task Fechar_DeveRetornarNoContent_QuandoDadosValidos()
    {
        var fatura = await FaturaFixture.CriarAsync(Factory);

        var resposta = await Client.PutAsync(
            $"{RotaBase}/{fatura.Id}/fechar",
            null,
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Fechar_DeveRetornarBadRequest_QuandoFechada()
    {
        var fatura = await FaturaFixture.CriarFechadaAsync(Factory);

        var resposta = await Client.PutAsync(
            $"{RotaBase}/{fatura.Id}/fechar",
            null,
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Fechar_DeveRetornarNotFound_QuandoFaturaInexistente()
    {
        var resposta = await Client.PutAsync(
            $"{RotaBase}/{int.MaxValue}/fechar",
            null,
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    // ============================================================================
    // PUT /api/v1/financas/faturas/{faturaId}/pagar
    // ============================================================================

    [Fact]
    public async Task Pagar_DeveRetornarNoContent_QuandoDadosValidos()
    {
        var fatura = await FaturaFixture.CriarFechadaComLancamentoAsync(Factory);

        var dto = FaturaDtoBuilder.Pagar(
            Factory.BaseData.Conta.Id,
            Factory.BaseData.Categoria.Id,
            fatura.DataFechamento.AddDays(5));

        var resposta = await Client.PutAsJsonAsync(
            $"{RotaBase}/{fatura.Id}/pagar",
            dto,
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Pagar_DeveRetornarBadRequest_QuandoAberta()
    {
        var fatura = await FaturaFixture.CriarAsync(Factory);

        var dto = FaturaDtoBuilder.Pagar(
            Factory.BaseData.Conta.Id,
            Factory.BaseData.Categoria.Id,
            fatura.DataFechamento.AddDays(5));

        var resposta = await Client.PutAsJsonAsync(
            $"{RotaBase}/{fatura.Id}/pagar",
            dto,
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Pagar_DeveRetornarNotFound_QuandoFaturaInexistente()
    {
        var dto = FaturaDtoBuilder.Pagar(
            Factory.BaseData.Conta.Id,
            Factory.BaseData.Categoria.Id);

        var resposta = await Client.PutAsJsonAsync(
            $"{RotaBase}/{int.MaxValue}/pagar",
            dto,
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
