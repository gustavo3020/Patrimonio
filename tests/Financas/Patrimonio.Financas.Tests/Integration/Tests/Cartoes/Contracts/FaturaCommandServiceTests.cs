using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Patrimonio.Financas.Contracts.Cartoes.Dtos;
using Patrimonio.Financas.Contracts.Cartoes.Services;
using Patrimonio.Financas.Domain.Exceptions;
using Patrimonio.Financas.SharedKernel.Cartoes.Enums;
using Patrimonio.Financas.Tests.Integration.Base;
using Patrimonio.Financas.Tests.Integration.Builders;
using Patrimonio.Financas.Tests.Integration.Config;

namespace Patrimonio.Financas.Tests.Integration.Tests.Cartoes.Contracts;

public sealed class FaturaCommandServiceTests(IntegrationTestFactory factory)
    : IntegrationTestBase(factory)
{
    // ============================================================================
    // CriarAsync
    // ============================================================================

    [Fact]
    public async Task CriarAsync_DeveCriarFaturaERetornarDto()
    {
        using var scope = CreateScope();
        var cartaoCommand = scope.ServiceProvider.GetRequiredService<ICartaoCommandService>();
        var faturaCommand = scope.ServiceProvider.GetRequiredService<IFaturaCommandService>();

        var cartao = await CriarCartaoAsync(cartaoCommand);

        var criarDto = FaturaDtoBuilder.Criar(cartao.Id);
        var fatura = await faturaCommand.CriarAsync(criarDto, TestContext.Current.CancellationToken);

        fatura.Should().NotBeNull();
        fatura.DataFechamento.Should().Be(criarDto.DataFechamento);
        fatura.DataVencimento.Should().Be(criarDto.DataVencimento);
        fatura.Status.Should().Be(StatusFatura.Aberta);
        fatura.DataPagamento.Should().Be(null);
        fatura.CartaoId.Should().Be(criarDto.CartaoId);
    }

    [Fact]
    public async Task CriarAsync_DeveLancarExcecaoQuandoCartaoNaoExiste()
    {
        using var scope = CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<IFaturaCommandService>();
        var criarDto = new FaturaCriacaoDto
        {
            DataFechamento = new DateOnly(2023, 1, 1),
            DataVencimento = new DateOnly(2023, 1, 6),
            CartaoId = int.MaxValue
        };

        var action = () => service.CriarAsync(criarDto, TestContext.Current.CancellationToken);
        await action.Should().ThrowAsync<ConflitoException>();
    }

    // ============================================================================
    // AlterarAsync
    // ============================================================================

    [Fact]
    public async Task AlterarAsync_DeveAlterarFatura()
    {
        using var scope = CreateScope();
        var cartaoCommand = scope.ServiceProvider.GetRequiredService<ICartaoCommandService>();
        var faturaCommand = scope.ServiceProvider.GetRequiredService<IFaturaCommandService>();
        var faturaQuery = scope.ServiceProvider.GetRequiredService<IFaturaQueryService>();

        var fatura = await CriarCartaoEFaturaAsync(cartaoCommand, faturaCommand);

        var dtoAlterar = FaturaDtoBuilder.Alterar();
        await faturaCommand.AlterarAsync(fatura.Id, dtoAlterar, TestContext.Current.CancellationToken);

        var alterado = await faturaQuery.ObterPorIdAsync(fatura.Id, TestContext.Current.CancellationToken);

        alterado.Should().NotBeNull();
        alterado.DataFechamento.Should().Be(dtoAlterar.DataFechamento);
        alterado.DataVencimento.Should().Be(dtoAlterar.DataVencimento);
        alterado.Status.Should().Be(StatusFatura.Aberta);
    }

    [Fact]
    public async Task AlterarAsync_DeveLancarExcecaoAoAlterarFaturaInexistente()
    {
        using var scope = CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<IFaturaCommandService>();

        var action = () => service.AlterarAsync(
            int.MaxValue,
            FaturaDtoBuilder.Alterar(),
            TestContext.Current.CancellationToken);

        await action.Should().ThrowAsync<RecursoNaoEncontradoException>();
    }

    // ============================================================================
    // ExcluirAsync
    // ============================================================================

    [Fact]
    public async Task ExcluirAsync_DeveExcluirFatura()
    {
        using var scope = CreateScope();
        var cartaoCommand = scope.ServiceProvider.GetRequiredService<ICartaoCommandService>();
        var faturaCommand = scope.ServiceProvider.GetRequiredService<IFaturaCommandService>();
        var faturaQuery = scope.ServiceProvider.GetRequiredService<IFaturaQueryService>();

        var fatura = await CriarCartaoEFaturaAsync(cartaoCommand, faturaCommand);

        await faturaCommand.ExcluirAsync(fatura.Id, TestContext.Current.CancellationToken);

        var action = () => faturaQuery.ObterPorIdAsync(fatura.Id, TestContext.Current.CancellationToken);
        await action.Should().ThrowAsync<RecursoNaoEncontradoException>();
    }

    [Fact]
    public async Task ExcluirAsync_DeveLancarExcecaoAoExcluirFaturaBase()
    {
        using var scope = CreateScope();
        var faturaCommand = scope.ServiceProvider.GetRequiredService<IFaturaCommandService>();

        var action = () => faturaCommand.ExcluirAsync(Factory.BaseData.Fatura.Id, TestContext.Current.CancellationToken);

        await action.Should().ThrowAsync<ConflitoException>();
    }

    [Fact]
    public async Task ExcluirAsync_DeveLancarExcecaoAoExcluirFaturaInexistente()
    {
        using var scope = CreateScope();
        var faturaCommand = scope.ServiceProvider.GetRequiredService<IFaturaCommandService>();

        var action = () => faturaCommand.ExcluirAsync(
            int.MaxValue,
            TestContext.Current.CancellationToken);

        await action.Should().ThrowAsync<RecursoNaoEncontradoException>();
    }

    [Fact]
    public async Task ExcluirAsync_DeveLancarExcecaoAoExcluirFaturaFechada()
    {
        using var scope = CreateScope();
        var cartaoCommand = scope.ServiceProvider.GetRequiredService<ICartaoCommandService>();
        var faturaCommand = scope.ServiceProvider.GetRequiredService<IFaturaCommandService>();

        var fatura = await CriarFaturaFechadaAsync(cartaoCommand, faturaCommand);

        var action = () => faturaCommand.ExcluirAsync(fatura.Id, TestContext.Current.CancellationToken);
        await action.Should().ThrowAsync<RegraDeNegocioException>();
    }

    // ============================================================================
    // FecharAsync
    // ============================================================================

    [Fact]
    public async Task FecharAsync_DeveFecharFatura()
    {
        using var scope = CreateScope();
        var cartaoCommand = scope.ServiceProvider.GetRequiredService<ICartaoCommandService>();
        var faturaCommand = scope.ServiceProvider.GetRequiredService<IFaturaCommandService>();
        var query = scope.ServiceProvider.GetRequiredService<IFaturaQueryService>();

        var criado = await CriarCartaoEFaturaAsync(cartaoCommand, faturaCommand);

        await faturaCommand.FecharAsync(criado.Id, TestContext.Current.CancellationToken);

        var fatura = await query.ObterPorIdAsync(criado.Id, TestContext.Current.CancellationToken);
        fatura.Should().NotBeNull();
        fatura.Status.Should().Be(StatusFatura.Fechada);
    }

    [Fact]
    public async Task FecharAsync_DeveLancarExcecaoAoFecharFaturaInexistente()
    {
        using var scope = CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<IFaturaCommandService>();

        var action = () => service.FecharAsync(
            int.MaxValue,
            TestContext.Current.CancellationToken);

        await action.Should().ThrowAsync<RecursoNaoEncontradoException>();
    }

    // ============================================================================
    // PagarAsync
    // ============================================================================

    [Fact]
    public async Task PagarAsync_DevePagarFatura()
    {
        using var scope = CreateScope();
        var cartaoCommand = scope.ServiceProvider.GetRequiredService<ICartaoCommandService>();
        var faturaCommand = scope.ServiceProvider.GetRequiredService<IFaturaCommandService>();
        var lancamentoCommand = scope.ServiceProvider.GetRequiredService<ILancamentoCommandService>();
        var faturaQuery = scope.ServiceProvider.GetRequiredService<IFaturaQueryService>();

        var criado = await CriarFaturaFechadaComLancamentoAsync(cartaoCommand, faturaCommand, lancamentoCommand);

        var pagarDto = new FaturaPagamentoDto
        {
            DataPagamento = criado.DataFechamento.AddDays(5),
            ContaId = Factory.BaseData.Conta.Id,
            CategoriaId = Factory.BaseData.Categoria.Id
        };
        await faturaCommand.PagarAsync(criado.Id, pagarDto, TestContext.Current.CancellationToken);

        var fatura = await faturaQuery.ObterPorIdAsync(criado.Id, TestContext.Current.CancellationToken);
        fatura.Should().NotBeNull();
        fatura.Status.Should().Be(StatusFatura.Paga);
    }

    [Fact]
    public async Task PagarAsync_DeveLancarExcecaoAoFecharFaturaInexistente()
    {
        using var scope = CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<IFaturaCommandService>();
        var pagarDto = new FaturaPagamentoDto
        {
            DataPagamento = new DateOnly(2023, 1, 1),
            ContaId = Factory.BaseData.Conta.Id,
            CategoriaId = Factory.BaseData.Categoria.Id
        };

        var action = () => service.PagarAsync(
            int.MaxValue,
            pagarDto,
            TestContext.Current.CancellationToken);

        await action.Should().ThrowAsync<RecursoNaoEncontradoException>();
    }

    // ============================================================================
    // MÉTODOS PRIVADOS
    // ============================================================================

    private async Task<CartaoDetalheDto> CriarCartaoAsync(
        ICartaoCommandService cartaoCommand)
    {
        var cartaoDto = CartaoDtoBuilder.Criar(Factory.BaseData.Instituicao.Id);
        return await cartaoCommand.CriarAsync(cartaoDto, TestContext.Current.CancellationToken);
    }

    private async Task<FaturaDetalheDto> CriarCartaoEFaturaAsync(
        ICartaoCommandService cartaoCommand,
        IFaturaCommandService faturaCommand)
    {
        var cartao = await CriarCartaoAsync(cartaoCommand);

        var faturaDto = FaturaDtoBuilder.Criar(cartao.Id);
        return await faturaCommand.CriarAsync(faturaDto, TestContext.Current.CancellationToken);
    }

    private async Task<FaturaDetalheDto> CriarFaturaFechadaAsync(
        ICartaoCommandService cartaoCommand,
        IFaturaCommandService faturaCommand)
    {
        var fatura = await CriarCartaoEFaturaAsync(cartaoCommand, faturaCommand);

        await faturaCommand.FecharAsync(fatura.Id, TestContext.Current.CancellationToken);

        return fatura;
    }

    private async Task<FaturaDetalheDto> CriarFaturaFechadaComLancamentoAsync(
        ICartaoCommandService cartaoCommand,
        IFaturaCommandService faturaCommand,
        ILancamentoCommandService lancamentoCommand)
    {
        var fatura = await CriarCartaoEFaturaAsync(cartaoCommand, faturaCommand);

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
