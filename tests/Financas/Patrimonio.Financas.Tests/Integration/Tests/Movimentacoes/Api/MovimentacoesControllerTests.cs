using FluentAssertions;
using Patrimonio.Financas.Contracts.Movimentacoes.Dtos;
using Patrimonio.Financas.SharedKernel.Movimentacoes.Enums;
using Patrimonio.Financas.Tests.Integration.Base;
using Patrimonio.Financas.Tests.Integration.Config;
using System.Net;
using System.Net.Http.Json;

namespace Patrimonio.Financas.Tests.Integration.Tests.Movimentacoes.Api;

public sealed class MovimentacoesControllerTests(IntegrationTestFactory factory)
    : ApiIntegrationTestBase(factory)
{
    private const string RotaBase = "/api/v1/financas/movimentacoes";

    // ============================================================================
    // DTOs
    // ============================================================================

    private MovimentacaoCriacaoDto CriarDto()
    {
        return new MovimentacaoCriacaoDto
        {
            Data = DateOnly.FromDateTime(DateTime.Now),
            Valor = 500.00m,
            Natureza = Natureza.Saida,
            Tipo = TipoMovimentacao.Pix,
            Descricao = $"Movimentação {Guid.NewGuid()}",
            CategoriaId = Factory.BaseData.Categoria.Id,
            ContaId = Factory.BaseData.Conta.Id
        };
    }

    private MovimentacaoAlteracaoDto AlterarDto()
    {
        return new MovimentacaoAlteracaoDto
        {
            Data = DateOnly.FromDateTime(DateTime.Now).AddDays(5),
            Valor = 1000.00m,
            Natureza = Natureza.Entrada,
            Tipo = TipoMovimentacao.Credito,
            Descricao = $"Movimentação alterada {Guid.NewGuid()}",
            CategoriaId = Factory.BaseData.Categoria.Id,
            ContaId = Factory.BaseData.Conta.Id
        };
    }

    // ============================================================================
    // GET /api/v1/financas/movimentacoes
    // ============================================================================

    [Fact]
    public async Task Listar_DeveRetornarOkComMovimentacoes()
    {
        var resposta = await Client.GetAsync(
            RotaBase,
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.OK);

        var movimentacoes = await resposta.Content
            .ReadFromJsonAsync<IReadOnlyCollection<MovimentacaoListaDto>>(TestContext.Current.CancellationToken);

        movimentacoes.Should().NotBeNull();
        movimentacoes.Should().Contain(m => m.Id == Factory.BaseData.Movimentacao.Id);
    }

    // ============================================================================
    // GET /api/v1/financas/movimentacoes/{movimentacaoId}
    // ============================================================================

    [Fact]
    public async Task ObterPorId_DeveRetornarOkComMovimentacao()
    {
        var movimentacaoEsperada = Factory.BaseData.Movimentacao;

        var resposta = await Client.GetAsync(
            $"{RotaBase}/{movimentacaoEsperada.Id}",
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.OK);

        var movimentacao = await resposta.Content
            .ReadFromJsonAsync<MovimentacaoDetalheDto>(TestContext.Current.CancellationToken);

        movimentacao.Should().NotBeNull();
        movimentacao.Id.Should().Be(movimentacaoEsperada.Id);
        movimentacao.Descricao.Should().Be(movimentacaoEsperada.Descricao);
    }

    [Fact]
    public async Task ObterPorId_DeveRetornarNotFoundParaMovimentacaoInexistente()
    {
        var resposta = await Client.GetAsync(
            $"{RotaBase}/{int.MaxValue}",
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    // ============================================================================
    // POST /api/v1/financas/movimentacoes
    // ============================================================================

    [Fact]
    public async Task Criar_DeveRetornarCreatedComMovimentacaoCriada()
    {
        var dto = CriarDto();

        var resposta = await Client.PostAsJsonAsync(
            RotaBase,
            dto,
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.Created);

        var movimentacao = await resposta.Content
            .ReadFromJsonAsync<MovimentacaoDetalheDto>(
                TestContext.Current.CancellationToken);

        movimentacao.Should().NotBeNull();
        movimentacao.Id.Should().BeGreaterThan(0);
        movimentacao.Descricao.Should().Be(dto.Descricao);

        resposta.Headers.Location.Should().NotBeNull();
    }

    [Fact]
    public async Task Criar_DeveRetornarBadRequestAoEnviarValorNegativo()
    {
        var dto = new MovimentacaoCriacaoDto
        {
            Data = DateOnly.FromDateTime(DateTime.Now),
            Valor = -500.00m,
            Natureza = Natureza.Saida,
            Tipo = TipoMovimentacao.Pix,
            Descricao = "Dto inválido",
            CategoriaId = Factory.BaseData.Categoria.Id,
            ContaId = Factory.BaseData.Conta.Id
        };

        var resposta = await Client.PostAsJsonAsync(
            RotaBase,
            dto,
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Criar_DeveRetornarConflictAoInformarCategoriaInexistente()
    {
        var dto = new MovimentacaoCriacaoDto
        {
            Data = DateOnly.FromDateTime(DateTime.Now),
            Valor = 500.00m,
            Natureza = Natureza.Saida,
            Tipo = TipoMovimentacao.Pix,
            Descricao = $"Movimentação {Guid.NewGuid()}",
            CategoriaId = int.MaxValue,
            ContaId = Factory.BaseData.Conta.Id
        };

        var resposta = await Client.PostAsJsonAsync(
            RotaBase,
            dto,
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    // ============================================================================
    // PUT /api/v1/financas/movimentacoes/{movimentacaoId}
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

        var movimentacao = await criarResponse.Content
            .ReadFromJsonAsync<MovimentacaoDetalheDto>(
                TestContext.Current.CancellationToken);

        movimentacao.Should().NotBeNull();

        var alterarDto = AlterarDto();

        var resposta = await Client.PutAsJsonAsync(
            $"{RotaBase}/{movimentacao.Id}",
            alterarDto,
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Alterar_DeveRetornarBadRequestAoEnviarValorNegativo()
    {
        var alterarDto = new MovimentacaoAlteracaoDto
        {
            Data = DateOnly.FromDateTime(DateTime.Now),
            Valor = -500.00m,
            Natureza = Natureza.Saida,
            Tipo = TipoMovimentacao.Pix,
            Descricao = "Dto inválido",
            CategoriaId = Factory.BaseData.Categoria.Id,
            ContaId = Factory.BaseData.Conta.Id
        };

        var resposta = await Client.PutAsJsonAsync(
            $"{RotaBase}/{Factory.BaseData.Movimentacao.Id}",
            alterarDto,
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Alterar_DeveRetornarNotFoundParaMovimentacaoInexistente()
    {
        var dto = AlterarDto();

        var resposta = await Client.PutAsJsonAsync(
            $"{RotaBase}/{int.MaxValue}",
            dto,
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Alterar_DeveRetornarConflictAoInformarCategoriaInexistente()
    {
        var dto = new MovimentacaoAlteracaoDto
        {
            Data = DateOnly.FromDateTime(DateTime.Now),
            Valor = 500.00m,
            Natureza = Natureza.Saida,
            Tipo = TipoMovimentacao.Pix,
            Descricao = $"Movimentação {Guid.NewGuid()}",
            CategoriaId = int.MaxValue,
            ContaId = Factory.BaseData.Conta.Id
        };

        var resposta = await Client.PutAsJsonAsync(
            $"{RotaBase}/{Factory.BaseData.Movimentacao.Id}",
            dto,
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    // ============================================================================
    // DELETE /api/v1/financas/movimentacoes/{movimentacaoId}
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

        var movimentacao = await criarResponse.Content
            .ReadFromJsonAsync<MovimentacaoDetalheDto>(
                TestContext.Current.CancellationToken);

        movimentacao.Should().NotBeNull();

        var resposta = await Client.DeleteAsync(
            $"{RotaBase}/{movimentacao.Id}",
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Excluir_DeveRetornarNotFoundParaMovimentacaoInexistente()
    {
        var resposta = await Client.DeleteAsync(
            $"{RotaBase}/{int.MaxValue}",
            TestContext.Current.CancellationToken);

        resposta.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
