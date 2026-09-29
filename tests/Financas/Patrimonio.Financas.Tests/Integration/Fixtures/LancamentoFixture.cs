using Microsoft.Extensions.DependencyInjection;
using Patrimonio.Financas.Contracts.Cartoes.Dtos;
using Patrimonio.Financas.Contracts.Cartoes.Services;
using Patrimonio.Financas.Tests.Integration.Builders;
using Patrimonio.Financas.Tests.Integration.Config;

namespace Patrimonio.Financas.Tests.Integration.Fixtures;

internal static class LancamentoFixture
{
    public static async Task<LancamentoDetalheDto> CriarAsync(IntegrationTestFactory factory)
    {
        using var scope = factory.Services.CreateScope();
        var command = scope.ServiceProvider.GetRequiredService<ILancamentoCommandService>();

        var fatura = await FaturaFixture.CriarAsync(factory);
        var dto = LancamentoDtoBuilder.Criar(fatura.Id, factory.BaseData.Categoria.Id);

        return await command.CriarAsync(dto, TestContext.Current.CancellationToken);
    }

    public static async Task<LancamentoDetalheDto> CriarEmFaturaFechadaAsync(IntegrationTestFactory factory)
    {
        using var scope = factory.Services.CreateScope();
        var faturaCommand = scope.ServiceProvider.GetRequiredService<IFaturaCommandService>();
        var lancamentoCommand = scope.ServiceProvider.GetRequiredService<ILancamentoCommandService>();

        var fatura = await FaturaFixture.CriarAsync(factory);

        var dto = LancamentoDtoBuilder.Criar(fatura.Id, factory.BaseData.Categoria.Id);
        var lancamento = await lancamentoCommand.CriarAsync(dto, TestContext.Current.CancellationToken);

        await faturaCommand.FecharAsync(fatura.Id, TestContext.Current.CancellationToken);

        return lancamento;
    }

    public static async Task<LancamentoDetalheDto> CriarEmFaturaPagaAsync(IntegrationTestFactory factory)
    {
        using var scope = factory.Services.CreateScope();
        var faturaCommand = scope.ServiceProvider.GetRequiredService<IFaturaCommandService>();
        var lancamentoCommand = scope.ServiceProvider.GetRequiredService<ILancamentoCommandService>();

        var fatura = await FaturaFixture.CriarAsync(factory);

        var dto = LancamentoDtoBuilder.Criar(fatura.Id, factory.BaseData.Categoria.Id);
        var lancamento = await lancamentoCommand.CriarAsync(dto, TestContext.Current.CancellationToken);

        await faturaCommand.FecharAsync(fatura.Id, TestContext.Current.CancellationToken);

        var pagamentoDto = FaturaDtoBuilder.Pagar(
            factory.BaseData.Conta.Id,
            factory.BaseData.Categoria.Id,
            fatura.DataVencimento.AddDays(5));

        await faturaCommand.PagarAsync(fatura.Id, pagamentoDto, TestContext.Current.CancellationToken);

        return lancamento;
    }
}
