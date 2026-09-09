using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Patrimonio.Financas.Contracts.Cartoes.Dtos;
using Patrimonio.Financas.Contracts.Cartoes.Services;
using Patrimonio.Financas.Domain.Exceptions;
using Patrimonio.Financas.Tests.Integration.Base;
using Patrimonio.Financas.Tests.Integration.Config;

namespace Patrimonio.Financas.Tests.Integration.Tests.Cartoes.Contracts;

public sealed class LancamentoCommandServiceTests(IntegrationTestFactory factory)
    : IntegrationTestBase(factory)
{
    // ============================================================================
    // DTOs
    // ============================================================================

    private LancamentoCriacaoDto CriarDto(int totalParcelas, int faturaId)
    {
        return new LancamentoCriacaoDto
        {
            Descricao = $"Lancamento {Guid.NewGuid()}",
            Valor = 100.00m,
            DataCompra = new DateOnly(2026, 9, 1),
            Estabelecimento = "Estabelecimento Teste",
            Responsavel = "Responsavel Teste",
            TotalParcelas = totalParcelas,
            FaturaId = faturaId,
            CategoriaId = Factory.BaseData.Categoria.Id
        };
    }

    private FaturaCriacaoDto CriarFaturaDto()
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

    private LancamentoAlteracaoDto AlterarDto(int categoriaId)
    {
        return new LancamentoAlteracaoDto
        {
            Descricao = $"Lancamento alterado {Guid.NewGuid()}",
            Valor = 200.00m,
            DataCompra = new DateOnly(2026, 9, 6),
            Estabelecimento = "Estabelecimento Alterado",
            Responsavel = "Responsavel Alterado",
            CategoriaId = categoriaId
        };
    }

    // ============================================================================
    // CriarAsync
    // ============================================================================

    [Fact]
    public async Task CriarAsync_DeveCriarLancamentoERetornarDto()
    {
        using var scope = CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<ILancamentoCommandService>();
        var dto = CriarDto(1, Factory.BaseData.Fatura.Id);

        var criado = await service.CriarAsync(dto, CancellationToken.None);

        criado.Should().NotBeNull();
        criado.Descricao.Should().NotBeNullOrWhiteSpace();
        criado.Id.Should().BeGreaterThan(0);
        criado.Descricao.Should().Be(dto.Descricao);
        criado.Valor.Should().Be(dto.Valor);
        criado.DataCompra.Should().Be(dto.DataCompra);
        criado.Estabelecimento.Should().Be(dto.Estabelecimento);
        criado.Responsavel.Should().Be(dto.Responsavel);
        criado.NumeroParcela.Should().Be(1);
        criado.TotalParcelas.Should().Be(dto.TotalParcelas);
        criado.FaturaId.Should().Be(dto.FaturaId);
        criado.CategoriaId.Should().Be(dto.CategoriaId);
    }

    [Fact]
    public async Task CriarAsync_DeveCriarLancamentoParceladoERetornarDto()
    {
        using var scope = CreateScope();
        var faturaCommand = scope.ServiceProvider.GetRequiredService<IFaturaCommandService>();
        var lancamentoCommand = scope.ServiceProvider.GetRequiredService<ILancamentoCommandService>();

        var rnd = new Random();

        var ano = rnd.Next(1, 9998);
        var mes = rnd.Next(1, 13);
        var diaFechamento = rnd.Next(1, 29);

        var dataFechamento = new DateOnly(ano, mes, diaFechamento);
        var dataVencimento = dataFechamento.AddDays(5);

        // 1. Criar fatura inicial
        var faturaInicial = await faturaCommand.CriarAsync(new FaturaCriacaoDto
        {
            DataFechamento = dataFechamento,
            DataVencimento = dataVencimento,
            CartaoId = Factory.BaseData.Cartao.Id
        }, CancellationToken.None);

        // 2. Criar subsequentes (+1 mês e +2 meses)
        await faturaCommand.CriarAsync(new FaturaCriacaoDto
        {
            DataFechamento = faturaInicial.DataFechamento.AddMonths(1),
            DataVencimento = faturaInicial.DataVencimento.AddMonths(1),
            CartaoId = faturaInicial.CartaoId
        }, CancellationToken.None);

        await faturaCommand.CriarAsync(new FaturaCriacaoDto
        {
            DataFechamento = faturaInicial.DataFechamento.AddMonths(2),
            DataVencimento = faturaInicial.DataVencimento.AddMonths(2),
            CartaoId = faturaInicial.CartaoId
        }, CancellationToken.None);

        // 3. Criar lançamento parcelado em 3x
        var dto = new LancamentoCriacaoDto
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

        var criado = await lancamentoCommand.CriarAsync(dto, CancellationToken.None);

        // 4. Validações
        criado.Should().NotBeNull();
        criado.Descricao.Should().Be(dto.Descricao);
        criado.Valor.Should().Be(33.34m);
        criado.DataCompra.Should().Be(dto.DataCompra);
        criado.Estabelecimento.Should().Be(dto.Estabelecimento);
        criado.Responsavel.Should().Be(dto.Responsavel);
        criado.NumeroParcela.Should().Be(1);
        criado.TotalParcelas.Should().Be(3);
        criado.FaturaId.Should().Be(dto.FaturaId);
        criado.CategoriaId.Should().Be(dto.CategoriaId);
    }

    [Fact]
    public async Task CriarAsync_DeveLancarExcecaoAoInformarFaturaInexistente()
    {
        using var scope = CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<ILancamentoCommandService>();
        var dto = CriarDto(1, int.MaxValue);

        var action = () => service.CriarAsync(dto, CancellationToken.None);

        await action.Should().ThrowAsync<RecursoNaoEncontradoException>();
    }

    [Fact]
    public async Task CriarAsync_DeveLancarExcecaoAoInformarCategoriaInexistente()
    {
        using var scope = CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<ILancamentoCommandService>();
        var dto = new LancamentoCriacaoDto
        {
            Descricao = "Lancamento Teste",
            Valor = 100.00m,
            DataCompra = new DateOnly(2026, 1, 1),
            Estabelecimento = "Estabelecimento Teste",
            Responsavel = "Responsavel Teste",
            TotalParcelas = 1,
            FaturaId = Factory.BaseData.Fatura.Id,
            CategoriaId = int.MaxValue
        };

        var action = () => service.CriarAsync(dto, CancellationToken.None);

        await action.Should().ThrowAsync<ConflitoException>();
    }

    [Fact]
    public async Task CriarAsync_DeveLancarExcecaoAoCriarLancamentoEmFaturaFechada()
    {
        using var scope = CreateScope();
        var faturaCommand = scope.ServiceProvider.GetRequiredService<IFaturaCommandService>();
        var service = scope.ServiceProvider.GetRequiredService<ILancamentoCommandService>();

        var fatura = await faturaCommand.CriarAsync(CriarFaturaDto(), CancellationToken.None);
        await faturaCommand.FecharAsync(fatura.Id, CancellationToken.None);

        var dto = CriarDto(1, fatura.Id);

        var action = () => service.CriarAsync(dto, CancellationToken.None);

        await action.Should().ThrowAsync<RegraDeNegocioException>();
    }

    [Fact]
    public async Task CriarAsync_DeveLancarExcecaoAoCriarLancamentoEmFaturaPaga()
    {
        using var scope = CreateScope();
        var faturaCommand = scope.ServiceProvider.GetRequiredService<IFaturaCommandService>();
        var service = scope.ServiceProvider.GetRequiredService<ILancamentoCommandService>();

        var fatura = await faturaCommand.CriarAsync(CriarFaturaDto(), CancellationToken.None);

        var dto = CriarDto(1, fatura.Id);

        var pagamentoDto = new FaturaPagamentoDto
        {
            DataPagamento = fatura.DataVencimento,
            ContaId = Factory.BaseData.Conta.Id,
            CategoriaId = Factory.BaseData.Categoria.Id
        };

        var lancamento = await service.CriarAsync(dto, CancellationToken.None);
        await faturaCommand.FecharAsync(lancamento.FaturaId, CancellationToken.None);
        await faturaCommand.PagarAsync(lancamento.FaturaId, pagamentoDto, CancellationToken.None);

        var action = () => service.CriarAsync(dto, CancellationToken.None);

        await action.Should().ThrowAsync<RegraDeNegocioException>();
    }

    // ============================================================================
    // AlterarAsync
    // ============================================================================

    [Fact]
    public async Task AlterarAsync_DeveAlterarLancamento()
    {
        using var scope = CreateScope();
        var dtoAlterar = AlterarDto(Factory.BaseData.Categoria.Id);
        var command = scope.ServiceProvider.GetRequiredService<ILancamentoCommandService>();
        var query = scope.ServiceProvider.GetRequiredService<ILancamentoQueryService>();

        var lancamento = await command.CriarAsync(CriarDto(1, Factory.BaseData.Fatura.Id), CancellationToken.None);

        await command.AlterarAsync(lancamento.Id, dtoAlterar, CancellationToken.None);

        var alterado = await query.ObterPorIdAsync(lancamento.Id, CancellationToken.None);

        alterado.Should().NotBeNull();
        alterado.Descricao.Should().Be(dtoAlterar.Descricao);
        alterado.Valor.Should().Be(dtoAlterar.Valor);
        alterado.DataCompra.Should().Be(dtoAlterar.DataCompra);
        alterado.Estabelecimento.Should().Be(dtoAlterar.Estabelecimento);
        alterado.Responsavel.Should().Be(dtoAlterar.Responsavel);
        alterado.NumeroParcela.Should().Be(lancamento.NumeroParcela);
        alterado.TotalParcelas.Should().Be(lancamento.TotalParcelas);
        alterado.FaturaId.Should().Be(lancamento.FaturaId);
        alterado.CategoriaId.Should().Be(dtoAlterar.CategoriaId);
    }

    [Fact]
    public async Task AlterarAsync_DeveLancarExcecaoAoInformarCategoriaInexistente()
    {
        using var scope = CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<ILancamentoCommandService>();
        var dto = AlterarDto(int.MaxValue);

        var lancamento = await service.CriarAsync(CriarDto(1, Factory.BaseData.Fatura.Id), CancellationToken.None);

        var action = () => service.AlterarAsync(
            lancamento.Id,
            dto,
            CancellationToken.None);

        await action.Should().ThrowAsync<ConflitoException>();
    }

    [Fact]
    public async Task AlterarAsync_DeveLancarExcecaoAoAlterarLancamentoInexistente()
    {
        using var scope = CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<ILancamentoCommandService>();

        var action = () => service.AlterarAsync(
            int.MaxValue,
            AlterarDto(Factory.BaseData.Categoria.Id),
            CancellationToken.None);

        await action.Should().ThrowAsync<RecursoNaoEncontradoException>();
    }

    [Fact]
    public async Task AlterarAsync_DeveLancarExcecaoAoAlterarLancamentoDeFaturaFechada()
    {
        using var scope = CreateScope();
        var faturaCommand = scope.ServiceProvider.GetRequiredService<IFaturaCommandService>();
        var service = scope.ServiceProvider.GetRequiredService<ILancamentoCommandService>();

        var fatura = await faturaCommand.CriarAsync(CriarFaturaDto(), CancellationToken.None);

        var dto = CriarDto(1, fatura.Id);

        var lancamento = await service.CriarAsync(dto, CancellationToken.None);
        await faturaCommand.FecharAsync(lancamento.FaturaId, CancellationToken.None);

        var action = () => service.AlterarAsync(
            lancamento.Id,
            AlterarDto(Factory.BaseData.Categoria.Id),
            CancellationToken.None);

        await action.Should().ThrowAsync<RegraDeNegocioException>();
    }

    [Fact]
    public async Task AlterarAsync_DeveLancarExcecaoAoAlterarLancamentoDeFaturaPaga()
    {
        using var scope = CreateScope();
        var faturaCommand = scope.ServiceProvider.GetRequiredService<IFaturaCommandService>();
        var service = scope.ServiceProvider.GetRequiredService<ILancamentoCommandService>();

        var fatura = await faturaCommand.CriarAsync(CriarFaturaDto(), CancellationToken.None);

        var dto = CriarDto(1, fatura.Id);

        var pagamentoDto = new FaturaPagamentoDto
        {
            DataPagamento = fatura.DataVencimento,
            ContaId = Factory.BaseData.Conta.Id,
            CategoriaId = Factory.BaseData.Categoria.Id
        };

        var lancamento = await service.CriarAsync(dto, CancellationToken.None);
        await faturaCommand.FecharAsync(lancamento.FaturaId, CancellationToken.None);
        await faturaCommand.PagarAsync(lancamento.FaturaId, pagamentoDto, CancellationToken.None);

        var action = () => service.AlterarAsync(
            lancamento.Id,
            AlterarDto(Factory.BaseData.Categoria.Id),
            CancellationToken.None);

        await action.Should().ThrowAsync<RegraDeNegocioException>();
    }

    // ============================================================================
    // ExcluirAsync
    // ============================================================================

    [Fact]
    public async Task ExcluirAsync_DeveExcluirLancamento()
    {
        using var scope = CreateScope();
        var command = scope.ServiceProvider.GetRequiredService<ILancamentoCommandService>();
        var query = scope.ServiceProvider.GetRequiredService<ILancamentoQueryService>();

        var criado = await command.CriarAsync(CriarDto(1, Factory.BaseData.Fatura.Id), CancellationToken.None);

        await command.ExcluirAsync(criado.Id, CancellationToken.None);

        var action = () => query.ObterPorIdAsync(criado.Id, CancellationToken.None);
        await action.Should().ThrowAsync<RecursoNaoEncontradoException>();
    }

    [Fact]
    public async Task ExcluirAsync_DeveLancarExcecaoAoExcluirLancamentoInexistente()
    {
        using var scope = CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<ILancamentoCommandService>();

        var action = () => service.ExcluirAsync(
            int.MaxValue,
            CancellationToken.None);

        await action.Should().ThrowAsync<RecursoNaoEncontradoException>();
    }

    [Fact]
    public async Task ExcluirAsync_DeveLancarExcecaoAoExcluirLancamentoEmFaturaFechada()
    {
        using var scope = CreateScope();
        var faturaCommand = scope.ServiceProvider.GetRequiredService<IFaturaCommandService>();
        var service = scope.ServiceProvider.GetRequiredService<ILancamentoCommandService>();

        var fatura = await faturaCommand.CriarAsync(CriarFaturaDto(), CancellationToken.None);

        var dto = CriarDto(1, fatura.Id);

        var lancamento = await service.CriarAsync(dto, CancellationToken.None);
        await faturaCommand.FecharAsync(lancamento.FaturaId, CancellationToken.None);

        var action = () => service.ExcluirAsync(
            lancamento.Id,
            CancellationToken.None);

        await action.Should().ThrowAsync<RegraDeNegocioException>();
    }

    [Fact]
    public async Task ExcluirAsync_DeveLancarExcecaoAoExcluirLancamentoEmFaturaPaga()
    {
        using var scope = CreateScope();
        var faturaCommand = scope.ServiceProvider.GetRequiredService<IFaturaCommandService>();
        var service = scope.ServiceProvider.GetRequiredService<ILancamentoCommandService>();

        var fatura = await faturaCommand.CriarAsync(CriarFaturaDto(), CancellationToken.None);

        var dto = CriarDto(1, fatura.Id);

        var pagamentoDto = new FaturaPagamentoDto
        {
            DataPagamento = fatura.DataVencimento,
            ContaId = Factory.BaseData.Conta.Id,
            CategoriaId = Factory.BaseData.Categoria.Id
        };

        var lancamento = await service.CriarAsync(dto, CancellationToken.None);
        await faturaCommand.FecharAsync(lancamento.FaturaId, CancellationToken.None);
        await faturaCommand.PagarAsync(lancamento.FaturaId, pagamentoDto, CancellationToken.None);

        var action = () => service.ExcluirAsync(
            lancamento.Id,
            CancellationToken.None);

        await action.Should().ThrowAsync<RegraDeNegocioException>();
    }
}
