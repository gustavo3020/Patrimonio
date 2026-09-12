using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Patrimonio.Financas.Contracts.Cartoes.Dtos;
using Patrimonio.Financas.Contracts.Cartoes.Services;
using Patrimonio.Financas.Domain.Exceptions;
using Patrimonio.Financas.Tests.Integration.Base;
using Patrimonio.Financas.Tests.Integration.Builders;
using Patrimonio.Financas.Tests.Integration.Config;

namespace Patrimonio.Financas.Tests.Integration.Tests.Cartoes.Contracts;

public sealed class LancamentoCommandServiceTests(IntegrationTestFactory factory)
    : IntegrationTestBase(factory)
{
    // ============================================================================
    // CriarAsync
    // ============================================================================

    [Fact]
    public async Task CriarAsync_DeveCriarLancamentoERetornarDto()
    {
        using var scope = CreateScope();
        var lancamentoCommand = scope.ServiceProvider.GetRequiredService<ILancamentoCommandService>();

        var lancamentoDto = LancamentoDtoBuilder.Criar(Factory.BaseData.Fatura.Id, Factory.BaseData.Categoria.Id);
        var criado = await lancamentoCommand.CriarAsync(lancamentoDto, TestContext.Current.CancellationToken);

        criado.Should().NotBeNull();
        criado.Descricao.Should().NotBeNullOrWhiteSpace();
        criado.Id.Should().BeGreaterThan(0);
        criado.Descricao.Should().Be(lancamentoDto.Descricao);
        criado.Valor.Should().Be(lancamentoDto.Valor);
        criado.DataCompra.Should().Be(lancamentoDto.DataCompra);
        criado.Estabelecimento.Should().Be(lancamentoDto.Estabelecimento);
        criado.Responsavel.Should().Be(lancamentoDto.Responsavel);
        criado.NumeroParcela.Should().Be(1);
        criado.TotalParcelas.Should().Be(lancamentoDto.TotalParcelas);
        criado.FaturaId.Should().Be(lancamentoDto.FaturaId);
        criado.CategoriaId.Should().Be(lancamentoDto.CategoriaId);
    }

    [Fact]
    public async Task CriarAsync_DeveCriarLancamentoParceladoERetornarDto()
    {
        using var scope = CreateScope();
        var cartaoCommand = scope.ServiceProvider.GetRequiredService<ICartaoCommandService>();
        var faturaCommand = scope.ServiceProvider.GetRequiredService<IFaturaCommandService>();
        var lancamentoCommand = scope.ServiceProvider.GetRequiredService<ILancamentoCommandService>();

        var cartaoDto = CartaoDtoBuilder.Criar(Factory.BaseData.Instituicao.Id);
        var cartao = await cartaoCommand.CriarAsync(cartaoDto, TestContext.Current.CancellationToken);

        var faturaDto = FaturaDtoBuilder.Criar(cartao.Id);

        // 1. Criar fatura inicial
        var faturaInicial = await faturaCommand.CriarAsync(faturaDto, TestContext.Current.CancellationToken);

        // 2. Criar subsequentes (+1 mês e +2 meses)
        await faturaCommand.CriarAsync(new FaturaCriacaoDto
        {
            DataFechamento = faturaInicial.DataFechamento.AddMonths(1),
            DataVencimento = faturaInicial.DataVencimento.AddMonths(1),
            CartaoId = faturaInicial.CartaoId
        }, TestContext.Current.CancellationToken);

        await faturaCommand.CriarAsync(new FaturaCriacaoDto
        {
            DataFechamento = faturaInicial.DataFechamento.AddMonths(2),
            DataVencimento = faturaInicial.DataVencimento.AddMonths(2),
            CartaoId = faturaInicial.CartaoId
        }, TestContext.Current.CancellationToken);

        // 3. Criar lançamento parcelado em 3x
        var criarDto = new LancamentoCriacaoDto
        {
            Descricao = "Compra parcelada",
            Valor = 100,
            DataCompra = new DateOnly(2026, 9, 10),
            Estabelecimento = "Loja Teste",
            Responsavel = "Usuário Teste",
            TotalParcelas = 3,
            FaturaId = faturaInicial.Id,
            CategoriaId = Factory.BaseData.Categoria.Id
        };

        var criado = await lancamentoCommand.CriarAsync(criarDto, TestContext.Current.CancellationToken);

        // 4. Validações
        criado.Should().NotBeNull();
        criado.Descricao.Should().Be(criarDto.Descricao);
        criado.Valor.Should().Be(33.34m);
        criado.DataCompra.Should().Be(criarDto.DataCompra);
        criado.Estabelecimento.Should().Be(criarDto.Estabelecimento);
        criado.Responsavel.Should().Be(criarDto.Responsavel);
        criado.NumeroParcela.Should().Be(1);
        criado.TotalParcelas.Should().Be(3);
        criado.FaturaId.Should().Be(criarDto.FaturaId);
        criado.CategoriaId.Should().Be(criarDto.CategoriaId);
    }

    [Fact]
    public async Task CriarAsync_DeveLancarExcecaoAoInformarFaturaInexistente()
    {
        using var scope = CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<ILancamentoCommandService>();
        var criarDto = LancamentoDtoBuilder.Criar(int.MaxValue, Factory.BaseData.Categoria.Id);

        var action = () => service.CriarAsync(criarDto, TestContext.Current.CancellationToken);

        await action.Should().ThrowAsync<RecursoNaoEncontradoException>();
    }

    [Fact]
    public async Task CriarAsync_DeveLancarExcecaoAoInformarCategoriaInexistente()
    {
        using var scope = CreateScope();
        var lancamentoCommand = scope.ServiceProvider.GetRequiredService<ILancamentoCommandService>();

        var criarDto = LancamentoDtoBuilder.Criar(Factory.BaseData.Fatura.Id, int.MaxValue);
        var action = () => lancamentoCommand.CriarAsync(criarDto, TestContext.Current.CancellationToken);

        await action.Should().ThrowAsync<ConflitoException>();
    }

    [Fact]
    public async Task CriarAsync_DeveLancarExcecaoAoCriarLancamentoEmFaturaFechada()
    {
        using var scope = CreateScope();
        var cartaoCommand = scope.ServiceProvider.GetRequiredService<ICartaoCommandService>();
        var faturaCommand = scope.ServiceProvider.GetRequiredService<IFaturaCommandService>();
        var lancamentoCommand = scope.ServiceProvider.GetRequiredService<ILancamentoCommandService>();

        var fatura = await CriarFaturaFechadaAsync(cartaoCommand, faturaCommand);

        var criarDto = LancamentoDtoBuilder.Criar(fatura.Id, Factory.BaseData.Categoria.Id);
        var action = () => lancamentoCommand.CriarAsync(criarDto, TestContext.Current.CancellationToken);

        await action.Should().ThrowAsync<RegraDeNegocioException>();
    }

    [Fact]
    public async Task CriarAsync_DeveLancarExcecaoAoCriarLancamentoEmFaturaPaga()
    {
        using var scope = CreateScope();
        var cartaoCommand = scope.ServiceProvider.GetRequiredService<ICartaoCommandService>();
        var faturaCommand = scope.ServiceProvider.GetRequiredService<IFaturaCommandService>();
        var lancamentoCommand = scope.ServiceProvider.GetRequiredService<ILancamentoCommandService>();

        var fatura = await CriarFaturaPagaAsync(cartaoCommand, faturaCommand, lancamentoCommand);

        var criarDto = LancamentoDtoBuilder.Criar(fatura.Id, Factory.BaseData.Categoria.Id);
        var action = () => lancamentoCommand.CriarAsync(criarDto, TestContext.Current.CancellationToken);

        await action.Should().ThrowAsync<RegraDeNegocioException>();
    }

    // ============================================================================
    // AlterarAsync
    // ============================================================================

    [Fact]
    public async Task AlterarAsync_DeveAlterarLancamento()
    {
        using var scope = CreateScope();
        var lancamentoCommand = scope.ServiceProvider.GetRequiredService<ILancamentoCommandService>();
        var lancamentoQuery = scope.ServiceProvider.GetRequiredService<ILancamentoQueryService>();

        var criarLancamentoDto = LancamentoDtoBuilder.Criar(Factory.BaseData.Fatura.Id, Factory.BaseData.Categoria.Id);
        var lancamento = await lancamentoCommand.CriarAsync(criarLancamentoDto, TestContext.Current.CancellationToken);

        var alterarLancamentoDto = LancamentoDtoBuilder.Alterar(Factory.BaseData.Categoria.Id);
        await lancamentoCommand.AlterarAsync(lancamento.Id, alterarLancamentoDto, TestContext.Current.CancellationToken);

        var alterado = await lancamentoQuery.ObterPorIdAsync(lancamento.Id, TestContext.Current.CancellationToken);

        alterado.Should().NotBeNull();
        alterado.Descricao.Should().Be(alterarLancamentoDto.Descricao);
        alterado.Valor.Should().Be(alterarLancamentoDto.Valor);
        alterado.DataCompra.Should().Be(alterarLancamentoDto.DataCompra);
        alterado.Estabelecimento.Should().Be(alterarLancamentoDto.Estabelecimento);
        alterado.Responsavel.Should().Be(alterarLancamentoDto.Responsavel);
        alterado.NumeroParcela.Should().Be(lancamento.NumeroParcela);
        alterado.TotalParcelas.Should().Be(lancamento.TotalParcelas);
        alterado.FaturaId.Should().Be(lancamento.FaturaId);
        alterado.CategoriaId.Should().Be(alterarLancamentoDto.CategoriaId);
    }

    [Fact]
    public async Task AlterarAsync_DeveLancarExcecaoAoInformarCategoriaInexistente()
    {
        using var scope = CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<ILancamentoCommandService>();

        var criarLancamentoDto = LancamentoDtoBuilder.Criar(Factory.BaseData.Fatura.Id, Factory.BaseData.Categoria.Id);
        var lancamento = await service.CriarAsync(criarLancamentoDto, TestContext.Current.CancellationToken);

        var alterarLancamentoDto = LancamentoDtoBuilder.Alterar(int.MaxValue);
        var action = () => service.AlterarAsync(
            lancamento.Id,
            alterarLancamentoDto,
            TestContext.Current.CancellationToken);

        await action.Should().ThrowAsync<ConflitoException>();
    }

    [Fact]
    public async Task AlterarAsync_DeveLancarExcecaoAoAlterarLancamentoInexistente()
    {
        using var scope = CreateScope();
        var lancamentoCommand = scope.ServiceProvider.GetRequiredService<ILancamentoCommandService>();

        var action = () => lancamentoCommand.AlterarAsync(
            int.MaxValue,
            LancamentoDtoBuilder.Alterar(Factory.BaseData.Categoria.Id),
            TestContext.Current.CancellationToken);

        await action.Should().ThrowAsync<RecursoNaoEncontradoException>();
    }

    [Fact]
    public async Task AlterarAsync_DeveLancarExcecaoAoAlterarLancamentoDeFaturaFechada()
    {
        using var scope = CreateScope();
        var cartaoCommand = scope.ServiceProvider.GetRequiredService<ICartaoCommandService>();
        var faturaCommand = scope.ServiceProvider.GetRequiredService<IFaturaCommandService>();
        var lancamentoCommand = scope.ServiceProvider.GetRequiredService<ILancamentoCommandService>();

        var lancamento = await CriarLancamentoEmFaturaFechadaAsync(cartaoCommand, faturaCommand, lancamentoCommand);

        var action = () => lancamentoCommand.AlterarAsync(
            lancamento.Id,
            LancamentoDtoBuilder.Alterar(Factory.BaseData.Categoria.Id),
            TestContext.Current.CancellationToken);

        await action.Should().ThrowAsync<RegraDeNegocioException>();
    }

    [Fact]
    public async Task AlterarAsync_DeveLancarExcecaoAoAlterarLancamentoDeFaturaPaga()
    {
        using var scope = CreateScope();
        var cartaoCommand = scope.ServiceProvider.GetRequiredService<ICartaoCommandService>();
        var faturaCommand = scope.ServiceProvider.GetRequiredService<IFaturaCommandService>();
        var lancamentoCommand = scope.ServiceProvider.GetRequiredService<ILancamentoCommandService>();

        var lancamento = await CriarLancamentoEmFaturaPagaAsync(cartaoCommand, faturaCommand, lancamentoCommand);

        var action = () => lancamentoCommand.AlterarAsync(
            lancamento.Id,
            LancamentoDtoBuilder.Alterar(Factory.BaseData.Categoria.Id),
            TestContext.Current.CancellationToken);

        await action.Should().ThrowAsync<RegraDeNegocioException>();
    }

    // ============================================================================
    // ExcluirAsync
    // ============================================================================

    [Fact]
    public async Task ExcluirAsync_DeveExcluirLancamento()
    {
        using var scope = CreateScope();
        var criarDto = LancamentoDtoBuilder.Criar(Factory.BaseData.Fatura.Id, Factory.BaseData.Categoria.Id);
        var lancamentoCommand = scope.ServiceProvider.GetRequiredService<ILancamentoCommandService>();
        var lancamentoQuery = scope.ServiceProvider.GetRequiredService<ILancamentoQueryService>();

        var criado = await lancamentoCommand.CriarAsync(criarDto, TestContext.Current.CancellationToken);

        await lancamentoCommand.ExcluirAsync(criado.Id, TestContext.Current.CancellationToken);

        var action = () => lancamentoQuery.ObterPorIdAsync(criado.Id, TestContext.Current.CancellationToken);
        await action.Should().ThrowAsync<RecursoNaoEncontradoException>();
    }

    [Fact]
    public async Task ExcluirAsync_DeveLancarExcecaoAoExcluirLancamentoInexistente()
    {
        using var scope = CreateScope();
        var lancamentoCommand = scope.ServiceProvider.GetRequiredService<ILancamentoCommandService>();

        var action = () => lancamentoCommand.ExcluirAsync(
            int.MaxValue,
            TestContext.Current.CancellationToken);

        await action.Should().ThrowAsync<RecursoNaoEncontradoException>();
    }

    [Fact]
    public async Task ExcluirAsync_DeveLancarExcecaoAoExcluirLancamentoEmFaturaFechada()
    {
        using var scope = CreateScope();
        var cartaoCommand = scope.ServiceProvider.GetRequiredService<ICartaoCommandService>();
        var faturaCommand = scope.ServiceProvider.GetRequiredService<IFaturaCommandService>();
        var lancamentoCommand = scope.ServiceProvider.GetRequiredService<ILancamentoCommandService>();

        var lancamento = await CriarLancamentoEmFaturaFechadaAsync(cartaoCommand, faturaCommand, lancamentoCommand);

        var action = () => lancamentoCommand.ExcluirAsync(
            lancamento.Id,
            TestContext.Current.CancellationToken);

        await action.Should().ThrowAsync<RegraDeNegocioException>();
    }

    [Fact]
    public async Task ExcluirAsync_DeveLancarExcecaoAoExcluirLancamentoEmFaturaPaga()
    {
        using var scope = CreateScope();
        var cartaoCommand = scope.ServiceProvider.GetRequiredService<ICartaoCommandService>();
        var faturaCommand = scope.ServiceProvider.GetRequiredService<IFaturaCommandService>();
        var lancamentoCommand = scope.ServiceProvider.GetRequiredService<ILancamentoCommandService>();

        var lancamento = await CriarLancamentoEmFaturaPagaAsync(cartaoCommand, faturaCommand, lancamentoCommand);

        var action = () => lancamentoCommand.ExcluirAsync(
            lancamento.Id,
            TestContext.Current.CancellationToken);

        await action.Should().ThrowAsync<RegraDeNegocioException>();
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

    private async Task<FaturaDetalheDto> CriarFaturaPagaAsync(
        ICartaoCommandService cartaoCommand,
        IFaturaCommandService faturaCommand,
        ILancamentoCommandService lancamentoCommand)
    {
        var fatura = await CriarCartaoEFaturaAsync(cartaoCommand, faturaCommand);

        var lancamentoDto = LancamentoDtoBuilder.Criar(fatura.Id, Factory.BaseData.Categoria.Id);
        await lancamentoCommand.CriarAsync(lancamentoDto, TestContext.Current.CancellationToken);

        await faturaCommand.FecharAsync(fatura.Id, TestContext.Current.CancellationToken);

        var pagamentoDto = new FaturaPagamentoDto
        {
            DataPagamento = fatura.DataVencimento.AddDays(5),
            ContaId = Factory.BaseData.Conta.Id,
            CategoriaId = Factory.BaseData.Categoria.Id
        };
        await faturaCommand.PagarAsync(fatura.Id, pagamentoDto, TestContext.Current.CancellationToken);

        return fatura;
    }

    private async Task<LancamentoDetalheDto> CriarLancamentoEmFaturaFechadaAsync(
        ICartaoCommandService cartaoCommand,
        IFaturaCommandService faturaCommand,
        ILancamentoCommandService lancamentoCommand)
    {
        var fatura = await CriarCartaoEFaturaAsync(cartaoCommand, faturaCommand);

        var criarLancamentoDto = LancamentoDtoBuilder.Criar(fatura.Id, Factory.BaseData.Categoria.Id);
        var lancamento = await lancamentoCommand.CriarAsync(criarLancamentoDto, TestContext.Current.CancellationToken);

        await faturaCommand.FecharAsync(fatura.Id, TestContext.Current.CancellationToken);

        return lancamento;
    }

    private async Task<LancamentoDetalheDto> CriarLancamentoEmFaturaPagaAsync(
        ICartaoCommandService cartaoCommand,
        IFaturaCommandService faturaCommand,
        ILancamentoCommandService lancamentoCommand)
    {
        var fatura = await CriarCartaoEFaturaAsync(cartaoCommand, faturaCommand);

        var criarLancamentoDto = LancamentoDtoBuilder.Criar(fatura.Id, Factory.BaseData.Categoria.Id);
        var lancamento = await lancamentoCommand.CriarAsync(criarLancamentoDto, TestContext.Current.CancellationToken);

        await faturaCommand.FecharAsync(fatura.Id, TestContext.Current.CancellationToken);

        var pagarDto = new FaturaPagamentoDto
        {
            DataPagamento = fatura.DataVencimento.AddDays(5),
            ContaId = Factory.BaseData.Conta.Id,
            CategoriaId = Factory.BaseData.Categoria.Id
        };
        await faturaCommand.PagarAsync(fatura.Id, pagarDto, TestContext.Current.CancellationToken);

        return lancamento;
    }
}
