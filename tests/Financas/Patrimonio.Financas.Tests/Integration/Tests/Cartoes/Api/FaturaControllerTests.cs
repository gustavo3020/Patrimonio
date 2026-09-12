using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Patrimonio.Financas.Contracts.Cartoes.Dtos;
using Patrimonio.Financas.Contracts.Cartoes.Services;
using Patrimonio.Financas.Tests.Integration.Base;
using Patrimonio.Financas.Tests.Integration.Builders;
using Patrimonio.Financas.Tests.Integration.Config;
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
    public async Task Listar_DeveRetornarOkComFaturas()
    {
        var resposta = await Client.GetAsync(
            RotaBase,
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
    public async Task ObterPorId_DeveRetornarOkComFatura()
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
    public async Task ObterPorId_DeveRetornarNotFoundParaFaturaInexistente()
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
    public async Task Criar_DeveRetornarCreatedComFaturaCriada()
    {
        var cartao = await CriarCartaoAsync();

        var criarDto = FaturaDtoBuilder.Criar(cartao.Id);

        var resposta = await Client.PostAsJsonAsync(
            RotaBase,
            criarDto,
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.Created);

        var fatura = await resposta.Content
            .ReadFromJsonAsync<FaturaDetalheDto>(
                TestContext.Current.CancellationToken);

        fatura.Should().NotBeNull();
        fatura.Id.Should().BeGreaterThan(0);
        fatura.DataFechamento.Should().Be(criarDto.DataFechamento);
        fatura.DataVencimento.Should().Be(criarDto.DataVencimento);

        resposta.Headers.Location.Should().NotBeNull();
    }

    [Fact]
    public async Task CriarAsync_DeveRetornarBadRequestQuandoDataFechamentoMaiorQueDataVencimento()
    {
        var criarDto = new FaturaCriacaoDto
        {
            DataFechamento = new DateOnly(2024, 1, 11),
            DataVencimento = new DateOnly(2024, 1, 6),
            CartaoId = Factory.BaseData.Cartao.Id
        };

        var resposta = await Client.PostAsJsonAsync(
            RotaBase,
            criarDto,
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Criar_DeveRetornarConflictAoEnviarCartaoInexistente()
    {
        var criarDto = FaturaDtoBuilder.Criar(int.MaxValue);

        var resposta = await Client.PostAsJsonAsync(
            RotaBase,
            criarDto,
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    // ============================================================================
    // PUT /api/v1/financas/faturas/{faturaId}
    // ============================================================================

    [Fact]
    public async Task Alterar_DeveRetornarNoContent()
    {
        var fatura = await CriarCartaoEFaturaAsync();

        var alterarDto = FaturaDtoBuilder.Alterar();

        var resposta = await Client.PutAsJsonAsync(
            $"{RotaBase}/{fatura.Id}",
            alterarDto,
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Alterar_DeveRetornarBadRequestQuandoDataFechamentoMaiorQueDataVencimento()
    {
        var alterarDto = new FaturaAlteracaoDto
        {
            DataFechamento = new DateOnly(2024, 1, 11),
            DataVencimento = new DateOnly(2024, 1, 6)
        };

        var resposta = await Client.PutAsJsonAsync(
            $"{RotaBase}/{Factory.BaseData.Fatura.Id}",
            alterarDto,
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Alterar_DeveRetornarNotFoundParaFaturaInexistente()
    {
        var alterarDto = FaturaDtoBuilder.Alterar();

        var resposta = await Client.PutAsJsonAsync(
            $"{RotaBase}/{int.MaxValue}",
            alterarDto,
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    // ============================================================================
    // DELETE /api/v1/financas/faturas/{faturaId}
    // ============================================================================

    [Fact]
    public async Task Excluir_DeveRetornarNoContent()
    {
        var fatura = await CriarCartaoEFaturaAsync();

        var resposta = await Client.DeleteAsync(
            $"{RotaBase}/{fatura.Id}",
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Excluir_DeveRetornarBadRequestParaFaturaFechada()
    {
        var fatura = await CriarFaturaFechadaAsync();

        var resposta = await Client.DeleteAsync(
            $"{RotaBase}/{fatura.Id}",
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Excluir_DeveRetornarNotFoundParaFaturaInexistente()
    {
        var resposta = await Client.DeleteAsync(
            $"{RotaBase}/{int.MaxValue}",
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Excluir_DeveRetornarConflictAoExcluirFaturaBase()
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
    public async Task Fechar_DeveRetornarNoContent()
    {
        var fatura = await CriarCartaoEFaturaAsync();

        var resposta = await Client.PutAsync(
            $"{RotaBase}/{fatura.Id}/fechar",
            null,
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Fechar_DeveRetornarBadRequestParaFaturaFechada()
    {
        var fatura = await CriarFaturaFechadaAsync();

        var resposta = await Client.PutAsync(
            $"{RotaBase}/{fatura.Id}/fechar",
            null,
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Fechar_DeveRetornarNotFoundParaFaturaInexistente()
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
    public async Task Pagar_DeveRetornarNoContent()
    {
        var fatura = await CriarFaturaFechadaComLancamentoAsync();

        var pagarDto = new FaturaPagamentoDto
        {
            DataPagamento = fatura.DataFechamento.AddDays(5),
            ContaId = Factory.BaseData.Conta.Id,
            CategoriaId = Factory.BaseData.Categoria.Id
        };

        var resposta = await Client.PutAsJsonAsync(
            $"{RotaBase}/{fatura.Id}/pagar",
            pagarDto,
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Pagar_DeveRetornarBadRequestParaFaturaAberta()
    {
        var fatura = await CriarCartaoEFaturaAsync();

        var pagarDto = new FaturaPagamentoDto
        {
            DataPagamento = fatura.DataFechamento.AddDays(5),
            ContaId = Factory.BaseData.Conta.Id,
            CategoriaId = Factory.BaseData.Categoria.Id
        };

        var resposta = await Client.PutAsJsonAsync(
            $"{RotaBase}/{fatura.Id}/pagar",
            pagarDto,
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Pagar_DeveRetornarNotFoundParaFaturaInexistente()
    {
        var pagarDto = new FaturaPagamentoDto
        {
            DataPagamento = new DateOnly(2026, 9, 10),
            ContaId = Factory.BaseData.Conta.Id,
            CategoriaId = Factory.BaseData.Categoria.Id
        };

        var resposta = await Client.PutAsJsonAsync(
            $"{RotaBase}/{int.MaxValue}/pagar",
            pagarDto,
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    // ============================================================================
    // MÉTODOS PRIVADOS
    // ============================================================================

    private async Task<CartaoDetalheDto> CriarCartaoAsync()
    {
        using var scope = CreateScope();
        var cartaoCommand = scope.ServiceProvider.GetRequiredService<ICartaoCommandService>();

        var criarCartaoDto = CartaoDtoBuilder.Criar(Factory.BaseData.Instituicao.Id);
        return await cartaoCommand.CriarAsync(criarCartaoDto, TestContext.Current.CancellationToken);
    }

    private async Task<FaturaDetalheDto> CriarCartaoEFaturaAsync()
    {
        using var scope = CreateScope();
        var cartaoCommand = scope.ServiceProvider.GetRequiredService<ICartaoCommandService>();
        var faturaCommand = scope.ServiceProvider.GetRequiredService<IFaturaCommandService>();

        var criarCartaoDto = CartaoDtoBuilder.Criar(Factory.BaseData.Instituicao.Id);
        var cartao = await cartaoCommand.CriarAsync(criarCartaoDto, TestContext.Current.CancellationToken);

        var criarFaturaDto = FaturaDtoBuilder.Criar(cartao.Id);
        return await faturaCommand.CriarAsync(criarFaturaDto, TestContext.Current.CancellationToken);
    }

    private async Task<FaturaDetalheDto> CriarFaturaFechadaAsync()
    {
        using var scope = CreateScope();
        var cartaoCommand = scope.ServiceProvider.GetRequiredService<ICartaoCommandService>();
        var faturaCommand = scope.ServiceProvider.GetRequiredService<IFaturaCommandService>();

        var criarCartaoDto = CartaoDtoBuilder.Criar(Factory.BaseData.Instituicao.Id);
        var cartao = await cartaoCommand.CriarAsync(criarCartaoDto, TestContext.Current.CancellationToken);

        var criarFaturaDto = FaturaDtoBuilder.Criar(cartao.Id);
        var fatura = await faturaCommand.CriarAsync(criarFaturaDto, TestContext.Current.CancellationToken);

        await faturaCommand.FecharAsync(fatura.Id, TestContext.Current.CancellationToken);

        return fatura;
    }

    private async Task<FaturaDetalheDto> CriarFaturaFechadaComLancamentoAsync()
    {
        using var scope = CreateScope();
        var cartaoCommand = scope.ServiceProvider.GetRequiredService<ICartaoCommandService>();
        var faturaCommand = scope.ServiceProvider.GetRequiredService<IFaturaCommandService>();
        var lancamentoCommand = scope.ServiceProvider.GetRequiredService<ILancamentoCommandService>();

        var criarCartaoDto = CartaoDtoBuilder.Criar(Factory.BaseData.Instituicao.Id);
        var cartao = await cartaoCommand.CriarAsync(criarCartaoDto, TestContext.Current.CancellationToken);

        var criarFaturaDto = FaturaDtoBuilder.Criar(cartao.Id);
        var fatura = await faturaCommand.CriarAsync(criarFaturaDto, TestContext.Current.CancellationToken);

        var dtoLancamento = new LancamentoCriacaoDto
        {
            Descricao = $"Lancamento {Guid.NewGuid()}",
            Valor = 100.00m,
            DataCompra = new DateOnly(2026, 9, 1),
            Estabelecimento = "Estabelecimento Teste",
            Responsavel = "Responsavel Teste",
            TotalParcelas = 1,
            FaturaId = fatura.Id,
            CategoriaId = Factory.BaseData.Categoria.Id
        };

        await lancamentoCommand.CriarAsync(dtoLancamento, TestContext.Current.CancellationToken);

        await faturaCommand.FecharAsync(fatura.Id, TestContext.Current.CancellationToken);

        return fatura;
    }
}
