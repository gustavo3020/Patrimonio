using Patrimonio.Financas.Contracts.Cartoes.Dtos;
using Patrimonio.Financas.Contracts.Cartoes.Services;
using Patrimonio.Financas.Domain.Exceptions;
using Patrimonio.Financas.Tests.Integration.Base;

namespace Patrimonio.Financas.Tests.Integration.Seeders;

/// <summary>
/// Fornece os dados base de faturas necessários aos testes de integração.
/// </summary>
internal sealed class FaturaSeeder(
    IFaturaCommandService commandService,
    IFaturaQueryService queryService)
{
    public async Task SeedAsync(BaseData data)
    {
        FaturaDetalheDto fatura;

        try
        {
            fatura = await queryService.ObterPorIdAsync(1, TestContext.Current.CancellationToken);
        }
        catch (RecursoNaoEncontradoException)
        {
            var faturaDto = Criar(data.Cartao.Id);

            fatura = await commandService.CriarAsync(faturaDto, TestContext.Current.CancellationToken);
        }

        data.Fatura = fatura;
    }

    private static FaturaCriacaoDto Criar(int cartaoId)
    {
        return new FaturaCriacaoDto
        {
            DataFechamento = new DateOnly(2026, 08, 11),
            DataVencimento = new DateOnly(2026, 08, 15),
            CartaoId = cartaoId
        };
    }
}
