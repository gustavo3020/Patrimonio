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

public sealed class CartaoCommandServiceTests(IntegrationTestFactory factory)
    : IntegrationTestBase(factory)
{
    // ============================================================================
    // CriarAsync
    // ============================================================================

    [Fact]
    public async Task CriarAsync_DeveCriarCartaoERetornarDto()
    {
        using var scope = CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<ICartaoCommandService>();
        var criarDto = CartaoDtoBuilder.Criar(Factory.BaseData.Instituicao.Id);

        var criado = await service.CriarAsync(criarDto, TestContext.Current.CancellationToken);

        criado.Should().NotBeNull();
        criado.Nome.Should().NotBeNullOrWhiteSpace();
        criado.Id.Should().BeGreaterThan(0);
        criado.Nome.Should().Be(criarDto.Nome);
        criado.Bandeira.Should().Be(criarDto.Bandeira);
        criado.Limite.Should().Be(criarDto.Limite);
        criado.DiaFechamento.Should().Be(criarDto.DiaFechamento);
        criado.DiaVencimento.Should().Be(criarDto.DiaVencimento);
        criado.InstituicaoId.Should().Be(criarDto.InstituicaoId);
    }

    [Fact]
    public async Task CriarAsync_DeveLancarExcecaoAoDuplicarNome()
    {
        using var scope = CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<ICartaoCommandService>();
        var criarDto = CartaoDtoBuilder.Criar(Factory.BaseData.Instituicao.Id);

        await service.CriarAsync(criarDto, TestContext.Current.CancellationToken);

        var action = () => service.CriarAsync(criarDto, TestContext.Current.CancellationToken);

        await action.Should().ThrowAsync<ConflitoException>();
    }

    // ============================================================================
    // AlterarAsync
    // ============================================================================

    [Fact]
    public async Task AlterarAsync_DeveAlterarCartao()
    {
        using var scope = CreateScope();
        var criarDto = CartaoDtoBuilder.Criar(Factory.BaseData.Instituicao.Id);
        var alterarDto = CartaoDtoBuilder.Alterar();
        var command = scope.ServiceProvider.GetRequiredService<ICartaoCommandService>();
        var query = scope.ServiceProvider.GetRequiredService<ICartaoQueryService>();

        var categoria = await command.CriarAsync(criarDto, TestContext.Current.CancellationToken);

        await command.AlterarAsync(categoria.Id, alterarDto, TestContext.Current.CancellationToken);

        var alterado = await query.ObterPorIdAsync(categoria.Id, TestContext.Current.CancellationToken);

        alterado.Should().NotBeNull();
        alterado.Nome.Should().Be(alterarDto.Nome);
        alterado.Bandeira.Should().Be(alterarDto.Bandeira);
        alterado.Limite.Should().Be(alterarDto.Limite);
        alterado.DiaFechamento.Should().Be(alterarDto.DiaFechamento);
        alterado.DiaVencimento.Should().Be(alterarDto.DiaVencimento);
        alterado.InstituicaoId.Should().Be(categoria.InstituicaoId);
    }

    [Fact]
    public async Task AlterarAsync_DeveLancarExcecaoAoDuplicarNome()
    {
        using var scope = CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<ICartaoCommandService>();
        var criarDto = CartaoDtoBuilder.Criar(Factory.BaseData.Instituicao.Id);
        var alterarDto = new CartaoAlteracaoDto
        {
            Nome = Factory.BaseData.Cartao.Nome,
            Bandeira = BandeiraCartao.Visa,
            Limite = 1000,
            DiaFechamento = 10,
            DiaVencimento = 20
        };

        var categoria = await service.CriarAsync(criarDto, TestContext.Current.CancellationToken);

        var action = () => service.AlterarAsync(
            categoria.Id,
            alterarDto,
            TestContext.Current.CancellationToken);

        await action.Should().ThrowAsync<ConflitoException>();
    }

    [Fact]
    public async Task AlterarAsync_DeveLancarExcecaoAoAlterarCartaoInexistente()
    {
        using var scope = CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<ICartaoCommandService>();

        var action = () => service.AlterarAsync(
            int.MaxValue,
            CartaoDtoBuilder.Alterar(),
            TestContext.Current.CancellationToken);

        await action.Should().ThrowAsync<RecursoNaoEncontradoException>();
    }

    // ============================================================================
    // ExcluirAsync
    // ============================================================================

    [Fact]
    public async Task ExcluirAsync_DeveExcluirCartao()
    {
        using var scope = CreateScope();
        var criarDto = CartaoDtoBuilder.Criar(Factory.BaseData.Instituicao.Id);
        var command = scope.ServiceProvider.GetRequiredService<ICartaoCommandService>();
        var query = scope.ServiceProvider.GetRequiredService<ICartaoQueryService>();

        var criado = await command.CriarAsync(criarDto, TestContext.Current.CancellationToken);

        await command.ExcluirAsync(criado.Id, TestContext.Current.CancellationToken);

        var action = () => query.ObterPorIdAsync(criado.Id, TestContext.Current.CancellationToken);
        await action.Should().ThrowAsync<RecursoNaoEncontradoException>();
    }

    [Fact]
    public async Task ExcluirAsync_DeveLancarExcecaoAoExcluirCartaoBase()
    {
        using var scope = CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<ICartaoCommandService>();

        var action = () => service.ExcluirAsync(Factory.BaseData.Cartao.Id, TestContext.Current.CancellationToken);

        await action.Should().ThrowAsync<ConflitoException>();
    }

    [Fact]
    public async Task ExcluirAsync_DeveLancarExcecaoAoExcluirCartaoInexistente()
    {
        using var scope = CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<ICartaoCommandService>();

        var action = () => service.ExcluirAsync(
            int.MaxValue,
            TestContext.Current.CancellationToken);

        await action.Should().ThrowAsync<RecursoNaoEncontradoException>();
    }
}
