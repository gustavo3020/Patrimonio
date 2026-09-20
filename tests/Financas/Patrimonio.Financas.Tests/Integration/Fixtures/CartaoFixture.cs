using Microsoft.Extensions.DependencyInjection;
using Patrimonio.Financas.Contracts.Cartoes.Dtos;
using Patrimonio.Financas.Contracts.Cartoes.Services;
using Patrimonio.Financas.Tests.Integration.Builders;
using Patrimonio.Financas.Tests.Integration.Config;

namespace Patrimonio.Financas.Tests.Integration.Fixtures;

internal static class CartaoFixture
{
    public static async Task<CartaoDetalheDto> CriarAsync(IntegrationTestFactory factory)
    {
        using var scope = factory.Services.CreateScope();
        var command = scope.ServiceProvider.GetRequiredService<ICartaoCommandService>();
        var dto = CartaoDtoBuilder.Criar(factory.BaseData.Instituicao.Id);

        return await command.CriarAsync(dto, TestContext.Current.CancellationToken);
    }
}
