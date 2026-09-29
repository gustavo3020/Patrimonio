using Microsoft.Extensions.DependencyInjection;
using Patrimonio.Financas.Contracts.Contas.Dtos;
using Patrimonio.Financas.Contracts.Contas.Services;
using Patrimonio.Financas.Tests.Integration.Builders;
using Patrimonio.Financas.Tests.Integration.Config;

namespace Patrimonio.Financas.Tests.Integration.Fixtures;

internal static class ContaFixture
{
    public static async Task<ContaDetalheDto> CriarAsync(IntegrationTestFactory factory)
    {
        using var scope = factory.Services.CreateScope();
        var command = scope.ServiceProvider.GetRequiredService<IContaCommandService>();
        var dto = ContaDtoBuilder.Criar(factory.BaseData.Instituicao.Id);

        return await command.CriarAsync(dto, TestContext.Current.CancellationToken);
    }
}
