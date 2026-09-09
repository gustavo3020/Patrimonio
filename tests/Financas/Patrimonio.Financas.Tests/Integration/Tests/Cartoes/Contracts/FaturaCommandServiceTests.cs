using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Patrimonio.Financas.Contracts.Cartoes.Dtos;
using Patrimonio.Financas.Contracts.Cartoes.Services;
using Patrimonio.Financas.Domain.Exceptions;
using Patrimonio.Financas.SharedKernel.Cartoes.Enums;
using Patrimonio.Financas.Tests.Integration.Base;
using Patrimonio.Financas.Tests.Integration.Config;

namespace Patrimonio.Financas.Tests.Integration.Tests.Cartoes.Contracts;

public sealed class FaturaCommandServiceTests(IntegrationTestFactory factory)
    : IntegrationTestBase(factory)
{
    // ============================================================================
    // DTOs
    // ============================================================================

    private FaturaCriacaoDto CriarDto()
    {
        var rnd = new Random();

        var ano = rnd.Next(1, 9998);
        var mes = rnd.Next(1, 13);
        var diaFechamento = rnd.Next(1, 29);

        var dataFechamento = new DateOnly(ano, mes, diaFechamento);
        var dataVencimento = dataFechamento.AddDays(5);

        return new FaturaCriacaoDto
        {
            DataFechamento = dataFechamento,
            DataVencimento = dataVencimento,
            CartaoId = Factory.BaseData.Cartao.Id
        };
    }

    private static FaturaAlteracaoDto AlterarDto()
    {
        var rnd = new Random();

        var ano = rnd.Next(1, 9999);
        var mes = rnd.Next(1, 13);
        var diaFechamento = rnd.Next(1, 29);

        var dataFechamento = new DateOnly(ano, mes, diaFechamento);
        var dataVencimento = dataFechamento.AddDays(5);

        return new FaturaAlteracaoDto
        {
            DataFechamento = dataFechamento,
            DataVencimento = dataVencimento
        };
    }

    private FaturaPagamentoDto PagarDto(DateOnly dataPagamento)
    {
        return new FaturaPagamentoDto
        {
            DataPagamento = dataPagamento,
            ContaId = Factory.BaseData.Conta.Id,
            CategoriaId = Factory.BaseData.Categoria.Id
        };
    }

    // ============================================================================
    // CriarAsync
    // ============================================================================

    [Fact]
    public async Task CriarAsync_DeveCriarFaturaERetornarDto()
    {
        using var scope = CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<IFaturaCommandService>();
        var dto = CriarDto();

        var criado = await service.CriarAsync(dto, CancellationToken.None);

        criado.Should().NotBeNull();
        criado.DataFechamento.Should().Be(dto.DataFechamento);
        criado.DataVencimento.Should().Be(dto.DataVencimento);
        criado.Status.Should().Be(StatusFatura.Aberta);
        criado.DataPagamento.Should().Be(null);
        criado.CartaoId.Should().Be(dto.CartaoId);
    }

    [Fact]
    public async Task CriarAsync_DeveLancarExcecaoQuandoCartaoNaoExiste()
    {
        using var scope = CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<IFaturaCommandService>();
        var dto = new FaturaCriacaoDto
        {
            DataFechamento = new DateOnly(2023, 1, 1),
            DataVencimento = new DateOnly(2023, 1, 6),
            CartaoId = int.MaxValue
        };

        var action = () => service.CriarAsync(dto, CancellationToken.None);
        await action.Should().ThrowAsync<ConflitoException>();
    }

    // ============================================================================
    // AlterarAsync
    // ============================================================================

    [Fact]
    public async Task AlterarAsync_DeveAlterarFatura()
    {
        using var scope = CreateScope();
        var dtoAlterar = AlterarDto();
        var command = scope.ServiceProvider.GetRequiredService<IFaturaCommandService>();
        var query = scope.ServiceProvider.GetRequiredService<IFaturaQueryService>();

        var fatura = await command.CriarAsync(CriarDto(), CancellationToken.None);

        await command.AlterarAsync(fatura.Id, dtoAlterar, CancellationToken.None);

        var alterado = await query.ObterPorIdAsync(fatura.Id, CancellationToken.None);

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
            AlterarDto(),
            CancellationToken.None);

        await action.Should().ThrowAsync<RecursoNaoEncontradoException>();
    }

    // ============================================================================
    // ExcluirAsync
    // ============================================================================

    [Fact]
    public async Task ExcluirAsync_DeveExcluirFatura()
    {
        using var scope = CreateScope();
        var command = scope.ServiceProvider.GetRequiredService<IFaturaCommandService>();
        var query = scope.ServiceProvider.GetRequiredService<IFaturaQueryService>();

        var criado = await command.CriarAsync(CriarDto(), CancellationToken.None);

        await command.ExcluirAsync(criado.Id, CancellationToken.None);

        var action = () => query.ObterPorIdAsync(criado.Id, CancellationToken.None);
        await action.Should().ThrowAsync<RecursoNaoEncontradoException>();
    }

    [Fact]
    public async Task ExcluirAsync_DeveLancarExcecaoAoExcluirFaturaBase()
    {
        using var scope = CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<IFaturaCommandService>();

        var action = () => service.ExcluirAsync(Factory.BaseData.Fatura.Id, CancellationToken.None);

        await action.Should().ThrowAsync<ConflitoException>();
    }

    [Fact]
    public async Task ExcluirAsync_DeveLancarExcecaoAoExcluirFaturaInexistente()
    {
        using var scope = CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<IFaturaCommandService>();

        var action = () => service.ExcluirAsync(
            int.MaxValue,
            CancellationToken.None);

        await action.Should().ThrowAsync<RecursoNaoEncontradoException>();
    }

    [Fact]
    public async Task ExcluirAsync_DeveLancarExcecaoAoExcluirFaturaFechada()
    {
        using var scope = CreateScope();
        var command = scope.ServiceProvider.GetRequiredService<IFaturaCommandService>();

        var criado = await command.CriarAsync(CriarDto(), CancellationToken.None);
        await command.FecharAsync(criado.Id, CancellationToken.None);

        var action = () => command.ExcluirAsync(criado.Id, CancellationToken.None);
        await action.Should().ThrowAsync<RegraDeNegocioException>();
    }

    // ============================================================================
    // FecharAsync
    // ============================================================================

    [Fact]
    public async Task FecharAsync_DeveFecharFatura()
    {
        using var scope = CreateScope();
        var command = scope.ServiceProvider.GetRequiredService<IFaturaCommandService>();
        var query = scope.ServiceProvider.GetRequiredService<IFaturaQueryService>();

        var criado = await command.CriarAsync(CriarDto(), CancellationToken.None);

        await command.FecharAsync(criado.Id, CancellationToken.None);

        var fatura = await query.ObterPorIdAsync(criado.Id, CancellationToken.None);
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
            CancellationToken.None);

        await action.Should().ThrowAsync<RecursoNaoEncontradoException>();
    }

    // ============================================================================
    // PagarAsync
    // ============================================================================

    [Fact]
    public async Task PagarAsync_DevePagarFatura()
    {
        using var scope = CreateScope();
        var command = scope.ServiceProvider.GetRequiredService<IFaturaCommandService>();
        var query = scope.ServiceProvider.GetRequiredService<IFaturaQueryService>();
        var lancamentoCommand = scope.ServiceProvider.GetRequiredService<ILancamentoCommandService>();
        var dto = CriarDto();

        var criado = await command.CriarAsync(dto, CancellationToken.None);

        var lancamentoDto = new LancamentoCriacaoDto
        {
            Descricao = "Teste de Lancamento para pagar fatura",
            Valor = 100,
            DataCompra = criado.DataFechamento,
            Estabelecimento = "Loja Teste",
            Responsavel = "Usuário Teste",
            TotalParcelas = 1,
            FaturaId = criado.Id,
            CategoriaId = Factory.BaseData.Categoria.Id
        };

        await lancamentoCommand.CriarAsync(lancamentoDto, CancellationToken.None);

        await command.FecharAsync(criado.Id, CancellationToken.None);
        await command.PagarAsync(criado.Id, PagarDto(dto.DataVencimento.AddDays(5)), CancellationToken.None);

        var fatura = await query.ObterPorIdAsync(criado.Id, CancellationToken.None);
        fatura.Should().NotBeNull();
        fatura.Status.Should().Be(StatusFatura.Paga);
    }

    [Fact]
    public async Task PagarAsync_DeveLancarExcecaoAoFecharFaturaInexistente()
    {
        using var scope = CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<IFaturaCommandService>();

        var action = () => service.PagarAsync(
            int.MaxValue,
            PagarDto(new DateOnly(2023, 1, 1)),
            CancellationToken.None);

        await action.Should().ThrowAsync<RecursoNaoEncontradoException>();
    }
}
