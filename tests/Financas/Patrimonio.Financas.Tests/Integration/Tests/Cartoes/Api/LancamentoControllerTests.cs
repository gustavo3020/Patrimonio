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

public sealed class LancamentoControllerTests(IntegrationTestFactory factory)
    : ApiIntegrationTestBase(factory)
{
    private const string RotaBase = "/api/v1/financas/lancamentos";

    // ============================================================================
    // GET /api/v1/financas/lancamentos
    // ============================================================================

    [Fact]
    public async Task Listar_DeveRetornarOkComLancamentos()
    {
        var resposta = await Client.GetAsync(
            RotaBase,
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
    public async Task ObterPorId_DeveRetornarOkComLancamento()
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
    public async Task ObterPorId_DeveRetornarNotFoundParaLancamentoInexistente()
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
    public async Task Criar_DeveRetornarCreatedComLancamentoCriado()
    {
        var criarDto = LancamentoDtoBuilder.Criar(Factory.BaseData.Fatura.Id, Factory.BaseData.Categoria.Id);

        var resposta = await Client.PostAsJsonAsync(
            RotaBase,
            criarDto,
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.Created);

        var lancamento = await resposta.Content
            .ReadFromJsonAsync<LancamentoDetalheDto>(
                TestContext.Current.CancellationToken);

        lancamento.Should().NotBeNull();
        lancamento.Id.Should().BeGreaterThan(0);
        lancamento.Descricao.Should().Be(criarDto.Descricao);

        resposta.Headers.Location.Should().NotBeNull();
    }

    [Fact]
    public async Task Criar_DeveRetornarBadRequestAoEnviarDescricaoVazia()
    {
        var criarDto = new LancamentoCriacaoDto
        {
            Descricao = string.Empty,
            Valor = 100.00m,
            DataCompra = new DateOnly(2026, 9, 1),
            Estabelecimento = "Estabelecimento Teste",
            Responsavel = "Responsavel Teste",
            TotalParcelas = 1,
            FaturaId = Factory.BaseData.Fatura.Id,
            CategoriaId = Factory.BaseData.Categoria.Id
        };

        var resposta = await Client.PostAsJsonAsync(
            RotaBase,
            criarDto,
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Criar_DeveRetornarConflictAoInformarCategoriaInexistente()
    {
        var criarDto = LancamentoDtoBuilder.Criar(Factory.BaseData.Fatura.Id, int.MaxValue);

        var resposta = await Client.PostAsJsonAsync(
            RotaBase,
            criarDto,
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    // ============================================================================
    // PUT /api/v1/financas/lancamentos/{lancamentoId}
    // ============================================================================

    [Fact]
    public async Task Alterar_DeveRetornarNoContent()
    {
        var lancamento = await CriarAsync();

        var alterarDto = LancamentoDtoBuilder.Alterar(Factory.BaseData.Categoria.Id);

        var resposta = await Client.PutAsJsonAsync(
            $"{RotaBase}/{lancamento.Id}",
            alterarDto,
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Alterar_DeveRetornarBadRequestAoEnviarDescricaoVazia()
    {
        var alterarDto = new LancamentoAlteracaoDto
        {
            Descricao = string.Empty,
            Valor = 100.00m,
            DataCompra = new DateOnly(2026, 9, 1),
            Estabelecimento = "Estabelecimento Teste",
            Responsavel = "Responsavel Teste",
            CategoriaId = Factory.BaseData.Categoria.Id
        };

        var resposta = await Client.PutAsJsonAsync(
            $"{RotaBase}/{Factory.BaseData.Lancamento.Id}",
            alterarDto,
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Alterar_DeveRetornarNotFoundParaLancamentoInexistente()
    {
        var alterarDto = LancamentoDtoBuilder.Alterar(Factory.BaseData.Categoria.Id);

        var resposta = await Client.PutAsJsonAsync(
            $"{RotaBase}/{int.MaxValue}",
            alterarDto,
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Alterar_DeveRetornarConflictAoInformarCategoriaInexistente()
    {
        var alterarDto = LancamentoDtoBuilder.Alterar(int.MaxValue);

        var resposta = await Client.PutAsJsonAsync(
            $"{RotaBase}/{Factory.BaseData.Lancamento.Id}",
            alterarDto,
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    // ============================================================================
    // DELETE /api/v1/financas/lancamentos/{lancamentoId}
    // ============================================================================

    [Fact]
    public async Task Excluir_DeveRetornarNoContent()
    {
        var lancamento = await CriarAsync();

        var resposta = await Client.DeleteAsync(
            $"{RotaBase}/{lancamento.Id}",
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Excluir_DeveRetornarBadRequestAoExcluirLancamentoEmFaturaFechada()
    {
        var lancamento = await CriarLancamentoEmFaturaFechadaAsync();

        var resposta = await Client.DeleteAsync(
            $"{RotaBase}/{lancamento.Id}",
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Excluir_DeveRetornarNotFoundParaLancamentoInexistente()
    {
        var resposta = await Client.DeleteAsync(
            $"{RotaBase}/{int.MaxValue}",
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    // ============================================================================
    // MÉTODOS PRIVADOS
    // ============================================================================
    private async Task<FaturaDetalheDto> CriarCartaoEFaturaAsync(
        ICartaoCommandService cartaoCommand,
        IFaturaCommandService faturaCommand)
    {
        var cartaoDto = CartaoDtoBuilder.Criar(Factory.BaseData.Instituicao.Id);
        var cartao = await cartaoCommand.CriarAsync(cartaoDto, TestContext.Current.CancellationToken);

        var faturaDto = FaturaDtoBuilder.Criar(cartao.Id);
        var fatura = await faturaCommand.CriarAsync(faturaDto, TestContext.Current.CancellationToken);

        return fatura;
    }

    private async Task<LancamentoDetalheDto> CriarAsync()
    {
        using var scope = CreateScope();
        var cartaoCommand = scope.ServiceProvider.GetRequiredService<ICartaoCommandService>();
        var faturaCommand = scope.ServiceProvider.GetRequiredService<IFaturaCommandService>();
        var lancamentoCommand = scope.ServiceProvider.GetRequiredService<ILancamentoCommandService>();

        var fatura = await CriarCartaoEFaturaAsync(cartaoCommand, faturaCommand);

        var criarLancamentoDto = LancamentoDtoBuilder.Criar(fatura.Id, Factory.BaseData.Categoria.Id);
        return await lancamentoCommand.CriarAsync(criarLancamentoDto, TestContext.Current.CancellationToken);
    }

    private async Task<LancamentoDetalheDto> CriarLancamentoEmFaturaFechadaAsync()
    {
        using var scope = CreateScope();
        var cartaoCommand = scope.ServiceProvider.GetRequiredService<ICartaoCommandService>();
        var faturaCommand = scope.ServiceProvider.GetRequiredService<IFaturaCommandService>();
        var lancamentoCommand = scope.ServiceProvider.GetRequiredService<ILancamentoCommandService>();

        var fatura = await CriarCartaoEFaturaAsync(cartaoCommand, faturaCommand);

        var criarLancamentoDto = LancamentoDtoBuilder.Criar(fatura.Id, Factory.BaseData.Categoria.Id);
        var lancamento = await lancamentoCommand.CriarAsync(criarLancamentoDto, TestContext.Current.CancellationToken);

        await faturaCommand.FecharAsync(fatura.Id, TestContext.Current.CancellationToken);

        return lancamento;
    }
}
