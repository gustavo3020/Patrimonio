using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Patrimonio.Financas.Contracts.Instituicoes.Dtos;
using Patrimonio.Financas.Contracts.Instituicoes.Services;
using Patrimonio.Financas.Domain.Exceptions;
using Patrimonio.Financas.Tests.Integration.Base;
using Patrimonio.Financas.Tests.Integration.Config;

namespace Patrimonio.Financas.Tests.Integration.Tests.Instituicoes.Contracts;

public sealed class InstituicaoCommandServiceTests(IntegrationTestFactory factory) : IntegrationTestBase(factory)
{
    // ============================================================================
    // DTOs
    // ============================================================================

    private static InstituicaoCriacaoDto CriarDto()
    {
        return new InstituicaoCriacaoDto
        {
            Nome = $"Instituição {Guid.NewGuid()}"
        };
    }

    private static InstituicaoAlteracaoDto AtualizarDto()
    {
        return new InstituicaoAlteracaoDto
        {
            Nome = $"Instituição alterada {Guid.NewGuid()}"
        };
    }

    // ============================================================================
    // CriarAsync
    // ============================================================================

    [Fact]
    public async Task CriarAsync_DeveCriarInstituicaoERetornarDto()
    {
        using var scope = CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<IInstituicaoCommandService>();

        var criado = await service.CriarAsync(CriarDto(), CancellationToken.None);

        criado.Should().NotBeNull();
        criado.Nome.Should().NotBeNullOrWhiteSpace();
        criado.Id.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task CriarAsync_DeveLancarExcecaoAoDuplicarNome()
    {
        using var scope = CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<IInstituicaoCommandService>();
        var dto = CriarDto();

        await service.CriarAsync(dto, CancellationToken.None);

        var action = () => service.CriarAsync(dto, CancellationToken.None);

        await action.Should().ThrowAsync<ConflitoException>();
    }

    // ============================================================================
    // AlterarAsync
    // ============================================================================

    [Fact]
    public async Task AlterarAsync_DeveAlterarInstituicao()
    {
        using var scope = CreateScope();
        var dtoAlterar = AtualizarDto();
        var command = scope.ServiceProvider.GetRequiredService<IInstituicaoCommandService>();
        var query = scope.ServiceProvider.GetRequiredService<IInstituicaoQueryService>();

        var instituicao = await command.CriarAsync(CriarDto(), CancellationToken.None);

        await command.AlterarAsync(instituicao.Id, dtoAlterar, CancellationToken.None);

        var alterado = await query.ObterPorIdAsync(instituicao.Id, CancellationToken.None);

        alterado.Should().NotBeNull();
        alterado.Nome.Should().Be(dtoAlterar.Nome);
    }

    [Fact]
    public async Task AlterarAsync_DeveLancarExcecaoAoDuplicarNome()
    {
        using var scope = CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<IInstituicaoCommandService>();

        var instituicao1 = await service.CriarAsync(CriarDto(), CancellationToken.None);

        var instituicao2 = await service.CriarAsync(CriarDto(), CancellationToken.None);

        var dto = new InstituicaoAlteracaoDto
        {
            Nome = instituicao1.Nome
        };

        var action = () => service.AlterarAsync(
            instituicao2.Id,
            dto,
            CancellationToken.None);

        await action.Should().ThrowAsync<ConflitoException>();
    }

    [Fact]
    public async Task AlterarAsync_DeveLancarExcecaoAoAlterarInstituicaoInexistente()
    {
        using var scope = CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<IInstituicaoCommandService>();

        var action = () => service.AlterarAsync(
            int.MaxValue,
            AtualizarDto(),
            CancellationToken.None);

        await action.Should().ThrowAsync<RecursoNaoEncontradoException>();
    }

    // ============================================================================
    // ExcluirAsync
    // ============================================================================

    [Fact]
    public async Task ExcluirAsync_DeveExcluirInstituicao()
    {
        using var scope = CreateScope();
        var command = scope.ServiceProvider.GetRequiredService<IInstituicaoCommandService>();
        var query = scope.ServiceProvider.GetRequiredService<IInstituicaoQueryService>();

        var criado = await command.CriarAsync(CriarDto(), CancellationToken.None);

        await command.ExcluirAsync(criado.Id, CancellationToken.None);

        var action = () => query.ObterPorIdAsync(criado.Id, CancellationToken.None);
        await action.Should().ThrowAsync<RecursoNaoEncontradoException>();
    }

    [Fact]
    public async Task ExcluirAsync_DeveLancarExcecaoAoExcluirInstituicaoBase()
    {
        using var scope = CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<IInstituicaoCommandService>();

        var action = () => service.ExcluirAsync(Factory.BaseData.Instituicao.Id, CancellationToken.None);

        await action.Should().ThrowAsync<ConflitoException>();
    }

    [Fact]
    public async Task ExcluirAsync_DeveLancarExcecaoAoExcluirInstituicaoInexistente()
    {
        using var scope = CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<IInstituicaoCommandService>();

        var action = () => service.ExcluirAsync(
            int.MaxValue,
            CancellationToken.None);

        await action.Should().ThrowAsync<RecursoNaoEncontradoException>();
    }
}
