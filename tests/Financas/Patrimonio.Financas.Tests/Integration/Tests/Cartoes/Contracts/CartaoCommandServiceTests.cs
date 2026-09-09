using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Patrimonio.Financas.Contracts.Cartoes.Dtos;
using Patrimonio.Financas.Contracts.Cartoes.Services;
using Patrimonio.Financas.Domain.Exceptions;
using Patrimonio.Financas.SharedKernel.Cartoes.Enums;
using Patrimonio.Financas.Tests.Integration.Base;
using Patrimonio.Financas.Tests.Integration.Config;

namespace Patrimonio.Financas.Tests.Integration.Tests.Cartoes.Contracts;

public sealed class CartaoCommandServiceTests(IntegrationTestFactory factory)
    : IntegrationTestBase(factory)
{
    // ============================================================================
    // DTOs
    // ============================================================================

    private CartaoCriacaoDto CriarDto()
    {
        return new CartaoCriacaoDto
        {
            Nome = $"Cartão {Guid.NewGuid()}",
            Bandeira = BandeiraCartao.Mastercard,
            Limite = 3000,
            DiaFechamento = 25,
            DiaVencimento = 30,
            InstituicaoId = Factory.BaseData.Instituicao.Id
        };
    }

    private static CartaoAlteracaoDto AlterarDto()
    {
        return new CartaoAlteracaoDto
        {
            Nome = $"Cartão alterado {Guid.NewGuid()}",
            Bandeira = BandeiraCartao.Visa,
            Limite = 1000,
            DiaFechamento = 10,
            DiaVencimento = 20
        };
    }

    // ============================================================================
    // CriarAsync
    // ============================================================================

    [Fact]
    public async Task CriarAsync_DeveCriarCartaoERetornarDto()
    {
        using var scope = CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<ICartaoCommandService>();
        var dto = CriarDto();

        var criado = await service.CriarAsync(dto, CancellationToken.None);

        criado.Should().NotBeNull();
        criado.Nome.Should().NotBeNullOrWhiteSpace();
        criado.Id.Should().BeGreaterThan(0);
        criado.Nome.Should().Be(dto.Nome);
        criado.Bandeira.Should().Be(dto.Bandeira);
        criado.Limite.Should().Be(dto.Limite);
        criado.DiaFechamento.Should().Be(dto.DiaFechamento);
        criado.DiaVencimento.Should().Be(dto.DiaVencimento);
        criado.InstituicaoId.Should().Be(dto.InstituicaoId);
    }

    [Fact]
    public async Task CriarAsync_DeveLancarExcecaoAoDuplicarNome()
    {
        using var scope = CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<ICartaoCommandService>();
        var dto = CriarDto();

        await service.CriarAsync(dto, CancellationToken.None);

        var action = () => service.CriarAsync(dto, CancellationToken.None);

        await action.Should().ThrowAsync<ConflitoException>();
    }

    // ============================================================================
    // AlterarAsync
    // ============================================================================

    [Fact]
    public async Task AlterarAsync_DeveAlterarCartao()
    {
        using var scope = CreateScope();
        var dtoAlterar = AlterarDto();
        var command = scope.ServiceProvider.GetRequiredService<ICartaoCommandService>();
        var query = scope.ServiceProvider.GetRequiredService<ICartaoQueryService>();

        var categoria = await command.CriarAsync(CriarDto(), CancellationToken.None);

        await command.AlterarAsync(categoria.Id, dtoAlterar, CancellationToken.None);

        var alterado = await query.ObterPorIdAsync(categoria.Id, CancellationToken.None);

        alterado.Should().NotBeNull();
        alterado.Nome.Should().Be(dtoAlterar.Nome);
        alterado.Bandeira.Should().Be(dtoAlterar.Bandeira);
        alterado.Limite.Should().Be(dtoAlterar.Limite);
        alterado.DiaFechamento.Should().Be(dtoAlterar.DiaFechamento);
        alterado.DiaVencimento.Should().Be(dtoAlterar.DiaVencimento);
        alterado.InstituicaoId.Should().Be(categoria.InstituicaoId);
    }

    [Fact]
    public async Task AlterarAsync_DeveLancarExcecaoAoDuplicarNome()
    {
        using var scope = CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<ICartaoCommandService>();
        var dto = new CartaoAlteracaoDto
        {
            Nome = Factory.BaseData.Cartao.Nome,
            Bandeira = BandeiraCartao.Visa,
            Limite = 1000,
            DiaFechamento = 10,
            DiaVencimento = 20
        };

        var categoria = await service.CriarAsync(CriarDto(), CancellationToken.None);

        var action = () => service.AlterarAsync(
            categoria.Id,
            dto,
            CancellationToken.None);

        await action.Should().ThrowAsync<ConflitoException>();
    }

    [Fact]
    public async Task AlterarAsync_DeveLancarExcecaoAoAlterarCartaoInexistente()
    {
        using var scope = CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<ICartaoCommandService>();

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
    public async Task ExcluirAsync_DeveExcluirCartao()
    {
        using var scope = CreateScope();
        var command = scope.ServiceProvider.GetRequiredService<ICartaoCommandService>();
        var query = scope.ServiceProvider.GetRequiredService<ICartaoQueryService>();

        var criado = await command.CriarAsync(CriarDto(), CancellationToken.None);

        await command.ExcluirAsync(criado.Id, CancellationToken.None);

        var action = () => query.ObterPorIdAsync(criado.Id, CancellationToken.None);
        await action.Should().ThrowAsync<RecursoNaoEncontradoException>();
    }

    [Fact]
    public async Task ExcluirAsync_DeveLancarExcecaoAoExcluirCartaoBase()
    {
        using var scope = CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<ICartaoCommandService>();

        var action = () => service.ExcluirAsync(Factory.BaseData.Cartao.Id, CancellationToken.None);

        await action.Should().ThrowAsync<ConflitoException>();
    }

    [Fact]
    public async Task ExcluirAsync_DeveLancarExcecaoAoExcluirCartaoInexistente()
    {
        using var scope = CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<ICartaoCommandService>();

        var action = () => service.ExcluirAsync(
            int.MaxValue,
            CancellationToken.None);

        await action.Should().ThrowAsync<RecursoNaoEncontradoException>();
    }
}
