using Microsoft.Extensions.DependencyInjection;
using Patrimonio.Financas.Contracts.Movimentacoes.Dtos;
using Patrimonio.Financas.Contracts.Movimentacoes.Services;
using Patrimonio.Financas.Tests.Integration.Builders;
using Patrimonio.Financas.Tests.Integration.Config;

namespace Patrimonio.Financas.Tests.Integration.Fixtures;

internal static class MovimentacaoFixture
{
    public static async Task<MovimentacaoDetalheDto> CriarAsync(IntegrationTestFactory factory)
    {
        using var scope = factory.Services.CreateScope();
        var command = scope.ServiceProvider.GetRequiredService<IMovimentacaoCommandService>();
        var dto = MovimentacaoDtoBuilder.Criar(
            factory.BaseData.Categoria.Id,
            factory.BaseData.Conta.Id);

        return await command.CriarAsync(dto, TestContext.Current.CancellationToken);
    }
}
