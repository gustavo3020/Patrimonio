using Microsoft.Extensions.DependencyInjection;
using Patrimonio.Financas.Contracts.Categorias.Dtos;
using Patrimonio.Financas.Contracts.Categorias.Services;
using Patrimonio.Financas.Tests.Integration.Builders;
using Patrimonio.Financas.Tests.Integration.Config;

namespace Patrimonio.Financas.Tests.Integration.Fixtures;

internal static class CategoriaFixture
{
    public static async Task<CategoriaDetalheDto> CriarAsync(IntegrationTestFactory factory)
    {
        using var scope = factory.Services.CreateScope();
        var command = scope.ServiceProvider.GetRequiredService<ICategoriaCommandService>();
        var dto = CategoriaDtoBuilder.Criar();

        return await command.CriarAsync(dto, TestContext.Current.CancellationToken);
    }
}
