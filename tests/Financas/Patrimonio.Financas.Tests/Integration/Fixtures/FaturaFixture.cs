using Microsoft.Extensions.DependencyInjection;
using Patrimonio.Financas.Contracts.Cartoes.Dtos;
using Patrimonio.Financas.Contracts.Cartoes.Services;
using Patrimonio.Financas.Tests.Integration.Builders;
using Patrimonio.Financas.Tests.Integration.Config;

namespace Patrimonio.Financas.Tests.Integration.Fixtures;

internal static class FaturaFixture
{
    public static async Task<FaturaDetalheDto> CriarAsync(IntegrationTestFactory factory)
    {
        using var scope = factory.Services.CreateScope();
        var command = scope.ServiceProvider.GetRequiredService<IFaturaCommandService>();

        var cartao = await CartaoFixture.CriarAsync(factory);
        var dto = FaturaDtoBuilder.Criar(cartao.Id);

        return await command.CriarAsync(dto, TestContext.Current.CancellationToken);
    }

    public static async Task<FaturaDetalheDto> CriarFechadaAsync(IntegrationTestFactory factory)
    {
        using var scope = factory.Services.CreateScope();
        var command = scope.ServiceProvider.GetRequiredService<IFaturaCommandService>();

        var fatura = await CriarAsync(factory);

        await command.FecharAsync(fatura.Id, TestContext.Current.CancellationToken);

        return fatura;
    }

    public static async Task<FaturaDetalheDto> CriarFechadaComLancamentoAsync(IntegrationTestFactory factory)
    {
        using var scope = factory.Services.CreateScope();
        var lancamentoCommand = scope.ServiceProvider.GetRequiredService<ILancamentoCommandService>();

        var fatura = await CriarAsync(factory);

        var dto = LancamentoDtoBuilder.Criar(fatura.Id, factory.BaseData.Categoria.Id);
        await lancamentoCommand.CriarAsync(dto, TestContext.Current.CancellationToken);

        var faturaCommand = scope.ServiceProvider.GetRequiredService<IFaturaCommandService>();
        await faturaCommand.FecharAsync(fatura.Id, TestContext.Current.CancellationToken);

        return fatura;
    }

    public static async Task<FaturaDetalheDto> CriarPagaAsync(IntegrationTestFactory factory)
    {
        using var scope = factory.Services.CreateScope();
        var command = scope.ServiceProvider.GetRequiredService<IFaturaCommandService>();

        var fatura = await CriarFechadaComLancamentoAsync(factory);

        var dto = FaturaDtoBuilder.Pagar(
            factory.BaseData.Conta.Id,
            factory.BaseData.Categoria.Id,
            fatura.DataVencimento.AddDays(5));

        await command.PagarAsync(fatura.Id, dto, TestContext.Current.CancellationToken);

        return fatura;
    }
}
