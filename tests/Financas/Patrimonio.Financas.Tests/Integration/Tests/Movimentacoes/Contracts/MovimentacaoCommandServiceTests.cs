using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Patrimonio.Financas.Contracts.Movimentacoes.Dtos;
using Patrimonio.Financas.Contracts.Movimentacoes.Services;
using Patrimonio.Financas.Domain.Exceptions;
using Patrimonio.Financas.SharedKernel.Movimentacoes.Enums;
using Patrimonio.Financas.Tests.Integration.Base;
using Patrimonio.Financas.Tests.Integration.Config;

namespace Patrimonio.Financas.Tests.Integration.Tests.Movimentacoes.Contracts;

public sealed class MovimentacaoCommandServiceTests(IntegrationTestFactory factory) : IntegrationTestBase(factory)
{
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
    // CriarAsync
    // ============================================================================

    [Fact]
    public async Task CriarAsync_DeveCriarMovimentacaoERetornarDto()
    {
        using var scope = CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<IMovimentacaoCommandService>();

        var criado = await service.CriarAsync(CriarDto(), TestContext.Current.CancellationToken);

        criado.Should().NotBeNull();
        criado.Descricao.Should().NotBeNullOrWhiteSpace();
        criado.Id.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task CriarAsync_DeveAceitarDescricaoNula()
    {
        using var scope = CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<IMovimentacaoCommandService>();
        var dto = new MovimentacaoCriacaoDto
        {
            Data = DateOnly.FromDateTime(DateTime.Now),
            Valor = 500.00m,
            Natureza = Natureza.Saida,
            Tipo = TipoMovimentacao.Pix,
            Descricao = string.Empty,
            CategoriaId = Factory.BaseData.Categoria.Id,
            ContaId = Factory.BaseData.Conta.Id
        };

        var criado = await service.CriarAsync(dto, TestContext.Current.CancellationToken);
        criado.Descricao.Should().BeNullOrEmpty();
    }

    [Fact]
    public async Task CriarAsync_DeveLancarExcecaoAoInformarCategoriaInexistente()
    {
        using var scope = CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<IMovimentacaoCommandService>();
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

        var action = () => service.CriarAsync(dto, TestContext.Current.CancellationToken);

        await action.Should().ThrowAsync<ConflitoException>();
    }

    [Fact]
    public async Task CriarAsync_DeveLancarExcecaoAoInformarContaInexistente()
    {
        using var scope = CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<IMovimentacaoCommandService>();
        var dto = new MovimentacaoCriacaoDto
        {
            Data = DateOnly.FromDateTime(DateTime.Now),
            Valor = 500.00m,
            Natureza = Natureza.Saida,
            Tipo = TipoMovimentacao.Pix,
            Descricao = $"Movimentação {Guid.NewGuid()}",
            CategoriaId = Factory.BaseData.Categoria.Id,
            ContaId = int.MaxValue
        };

        var action = () => service.CriarAsync(dto, TestContext.Current.CancellationToken);

        await action.Should().ThrowAsync<ConflitoException>();
    }

    // ============================================================================
    // AlterarAsync
    // ============================================================================

    [Fact]
    public async Task AlterarAsync_DeveAlterarMovimentacao()
    {
        using var scope = CreateScope();
        var dtoAlterar = AlterarDto();
        var command = scope.ServiceProvider.GetRequiredService<IMovimentacaoCommandService>();
        var query = scope.ServiceProvider.GetRequiredService<IMovimentacaoQueryService>();

        var movimentacao = await command.CriarAsync(CriarDto(), TestContext.Current.CancellationToken);

        await command.AlterarAsync(movimentacao.Id, dtoAlterar, TestContext.Current.CancellationToken);

        var alterado = await query.ObterPorIdAsync(movimentacao.Id, TestContext.Current.CancellationToken);

        alterado.Should().NotBeNull();
        alterado.Data.Should().Be(dtoAlterar.Data);
        alterado.Valor.Should().Be(dtoAlterar.Valor);
        alterado.Natureza.Should().Be(dtoAlterar.Natureza);
        alterado.Tipo.Should().Be(dtoAlterar.Tipo);
        alterado.Descricao.Should().Be(dtoAlterar.Descricao);
        alterado.CategoriaId.Should().Be(dtoAlterar.CategoriaId);
        alterado.ContaId.Should().Be(dtoAlterar.ContaId);
    }

    [Fact]
    public async Task AlterarAsync_DeveAceitarDescricaoNula()
    {
        using var scope = CreateScope();
        var command = scope.ServiceProvider.GetRequiredService<IMovimentacaoCommandService>();
        var query = scope.ServiceProvider.GetRequiredService<IMovimentacaoQueryService>();
        var dto = new MovimentacaoAlteracaoDto
        {
            Data = DateOnly.FromDateTime(DateTime.Now),
            Valor = 500.00m,
            Natureza = Natureza.Saida,
            Tipo = TipoMovimentacao.Pix,
            Descricao = string.Empty,
            CategoriaId = Factory.BaseData.Categoria.Id,
            ContaId = Factory.BaseData.Conta.Id
        };

        var movimentacao = await command.CriarAsync(CriarDto(), TestContext.Current.CancellationToken);

        await command.AlterarAsync(movimentacao.Id, dto, TestContext.Current.CancellationToken);

        var alterado = await query.ObterPorIdAsync(movimentacao.Id, TestContext.Current.CancellationToken);
        alterado.Descricao.Should().BeNullOrEmpty();
    }

    [Fact]
    public async Task AlterarAsync_DeveLancarExcecaoAoAlterarMovimentacaoInexistente()
    {
        using var scope = CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<IMovimentacaoCommandService>();

        var action = () => service.AlterarAsync(
            int.MaxValue,
            AlterarDto(),
            TestContext.Current.CancellationToken);

        await action.Should().ThrowAsync<RecursoNaoEncontradoException>();
    }

    [Fact]
    public async Task AlterarAsync_DeveLancarExcecaoAoInformarCategoriaInexistente()
    {
        using var scope = CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<IMovimentacaoCommandService>();
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

        var action = () => service.AlterarAsync(
            Factory.BaseData.Movimentacao.Id,
            dto,
            TestContext.Current.CancellationToken);

        await action.Should().ThrowAsync<ConflitoException>();
    }

    [Fact]
    public async Task AlterarAsync_DeveLancarExcecaoAoInformarContaInexistente()
    {
        using var scope = CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<IMovimentacaoCommandService>();
        var dto = new MovimentacaoAlteracaoDto
        {
            Data = DateOnly.FromDateTime(DateTime.Now),
            Valor = 500.00m,
            Natureza = Natureza.Saida,
            Tipo = TipoMovimentacao.Pix,
            Descricao = $"Movimentação {Guid.NewGuid()}",
            CategoriaId = Factory.BaseData.Categoria.Id,
            ContaId = int.MaxValue
        };

        var action = () => service.AlterarAsync(
            Factory.BaseData.Movimentacao.Id,
            dto,
            TestContext.Current.CancellationToken);

        await action.Should().ThrowAsync<ConflitoException>();
    }

    // ============================================================================
    // ExcluirAsync
    // ============================================================================

    [Fact]
    public async Task ExcluirAsync_DeveExcluirMovimentacao()
    {
        using var scope = CreateScope();
        var command = scope.ServiceProvider.GetRequiredService<IMovimentacaoCommandService>();
        var query = scope.ServiceProvider.GetRequiredService<IMovimentacaoQueryService>();

        var criado = await command.CriarAsync(CriarDto(), TestContext.Current.CancellationToken);

        await command.ExcluirAsync(criado.Id, TestContext.Current.CancellationToken);

        var action = () => query.ObterPorIdAsync(criado.Id, TestContext.Current.CancellationToken);
        await action.Should().ThrowAsync<RecursoNaoEncontradoException>();
    }

    [Fact]
    public async Task ExcluirAsync_DeveLancarExcecaoAoExcluirMovimentacaoInexistente()
    {
        using var scope = CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<IMovimentacaoCommandService>();

        var action = () => service.ExcluirAsync(
            int.MaxValue,
            TestContext.Current.CancellationToken);

        await action.Should().ThrowAsync<RecursoNaoEncontradoException>();
    }
}
