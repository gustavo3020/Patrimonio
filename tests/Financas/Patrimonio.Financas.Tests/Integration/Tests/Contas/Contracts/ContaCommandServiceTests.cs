using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Patrimonio.Financas.Contracts.Contas.Dtos;
using Patrimonio.Financas.Contracts.Contas.Services;
using Patrimonio.Financas.Domain.Exceptions;
using Patrimonio.Financas.Tests.Integration.Base;
using Patrimonio.Financas.Tests.Integration.Config;

namespace Patrimonio.Financas.Tests.Integration.Tests.Contas.Contracts;

public sealed class ContaCommandServiceTests(IntegrationTestFactory factory) : IntegrationTestBase(factory)
{
    // ============================================================================
    // DTOs
    // ============================================================================

    private ContaCriacaoDto CriarDto()
    {
        return new ContaCriacaoDto
        {
            Nome = $"Conta {Guid.NewGuid()}",
            InstituicaoId = Factory.BaseData.Instituicao.Id
        };
    }

    private ContaAlteracaoDto AlterarDto()
    {
        return new ContaAlteracaoDto
        {
            Nome = $"Conta alterada {Guid.NewGuid()}",
            InstituicaoId = Factory.BaseData.Instituicao.Id
        };
    }

    // ============================================================================
    // CriarAsync
    // ============================================================================

    [Fact]
    public async Task CriarAsync_DeveCriarContaERetornarDto()
    {
        using var scope = CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<IContaCommandService>();

        var criado = await service.CriarAsync(CriarDto(), TestContext.Current.CancellationToken);

        criado.Should().NotBeNull();
        criado.Nome.Should().NotBeNullOrWhiteSpace();
        criado.Id.Should().BeGreaterThan(0);
        criado.InstituicaoId.Should().Be(Factory.BaseData.Instituicao.Id);
    }

    [Fact]
    public async Task CriarAsync_DeveLancarExcecaoAoDuplicarNome()
    {
        using var scope = CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<IContaCommandService>();
        var dto = CriarDto();

        await service.CriarAsync(dto, TestContext.Current.CancellationToken);

        var action = () => service.CriarAsync(dto, TestContext.Current.CancellationToken);

        await action.Should().ThrowAsync<ConflitoException>();
    }

    [Fact]
    public async Task CriarAsync_DeveLancarExcecaoAoInformarInstituicaoInexistente()
    {
        using var scope = CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<IContaCommandService>();
        var dto = new ContaCriacaoDto
        {
            Nome = $"Conta {Guid.NewGuid()}",
            InstituicaoId = int.MaxValue
        };

        var action = () => service.CriarAsync(dto, TestContext.Current.CancellationToken);

        await action.Should().ThrowAsync<ConflitoException>();
    }

    // ============================================================================
    // AlterarAsync
    // ============================================================================

    [Fact]
    public async Task AlterarAsync_DeveAlterarConta()
    {
        using var scope = CreateScope();
        var dtoAlterar = AlterarDto();
        var command = scope.ServiceProvider.GetRequiredService<IContaCommandService>();
        var query = scope.ServiceProvider.GetRequiredService<IContaQueryService>();

        var conta = await command.CriarAsync(CriarDto(), TestContext.Current.CancellationToken);

        await command.AlterarAsync(conta.Id, dtoAlterar, TestContext.Current.CancellationToken);

        var alterado = await query.ObterPorIdAsync(conta.Id, TestContext.Current.CancellationToken);

        alterado.Should().NotBeNull();
        alterado.Nome.Should().Be(dtoAlterar.Nome);
        alterado.InstituicaoId.Should().Be(dtoAlterar.InstituicaoId);
    }

    [Fact]
    public async Task AlterarAsync_DeveLancarExcecaoAoDuplicarNome()
    {
        using var scope = CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<IContaCommandService>();
        var dto = new ContaAlteracaoDto
        {
            Nome = Factory.BaseData.Conta.Nome,
            InstituicaoId = Factory.BaseData.Instituicao.Id
        };

        var conta = await service.CriarAsync(CriarDto(), TestContext.Current.CancellationToken);

        var action = () => service.AlterarAsync(
            conta.Id,
            dto,
            TestContext.Current.CancellationToken);

        await action.Should().ThrowAsync<ConflitoException>();
    }

    [Fact]
    public async Task AlterarAsync_DeveLancarExcecaoAoAlterarContaInexistente()
    {
        using var scope = CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<IContaCommandService>();

        var action = () => service.AlterarAsync(
            int.MaxValue,
            AlterarDto(),
            TestContext.Current.CancellationToken);

        await action.Should().ThrowAsync<RecursoNaoEncontradoException>();
    }

    [Fact]
    public async Task AlterarAsync_DeveLancarExcecaoAoInformarInstituicaoInexistente()
    {
        using var scope = CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<IContaCommandService>();
        var dto = new ContaAlteracaoDto
        {
            Nome = $"Conta {Guid.NewGuid()}",
            InstituicaoId = int.MaxValue
        };

        var conta = await service.CriarAsync(CriarDto(), TestContext.Current.CancellationToken);

        var action = () => service.AlterarAsync(
            conta.Id,
            dto,
            TestContext.Current.CancellationToken);

        await action.Should().ThrowAsync<ConflitoException>();
    }

    // ============================================================================
    // ExcluirAsync
    // ============================================================================

    [Fact]
    public async Task ExcluirAsync_DeveExcluirConta()
    {
        using var scope = CreateScope();
        var command = scope.ServiceProvider.GetRequiredService<IContaCommandService>();
        var query = scope.ServiceProvider.GetRequiredService<IContaQueryService>();

        var criado = await command.CriarAsync(CriarDto(), TestContext.Current.CancellationToken);

        await command.ExcluirAsync(criado.Id, TestContext.Current.CancellationToken);

        var action = () => query.ObterPorIdAsync(criado.Id, TestContext.Current.CancellationToken);
        await action.Should().ThrowAsync<RecursoNaoEncontradoException>();
    }

    [Fact]
    public async Task ExcluirAsync_DeveLancarExcecaoAoExcluirContaBase()
    {
        using var scope = CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<IContaCommandService>();

        var action = () => service.ExcluirAsync(Factory.BaseData.Conta.Id, TestContext.Current.CancellationToken);

        await action.Should().ThrowAsync<ConflitoException>();
    }

    [Fact]
    public async Task ExcluirAsync_DeveLancarExcecaoAoExcluirContaInexistente()
    {
        using var scope = CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<IContaCommandService>();

        var action = () => service.ExcluirAsync(
            int.MaxValue,
            TestContext.Current.CancellationToken);

        await action.Should().ThrowAsync<RecursoNaoEncontradoException>();
    }
}
