using Patrimonio.Financas.Contracts.Contas.Dtos;
using Patrimonio.Financas.Contracts.Contas.Services;
using Patrimonio.Financas.Domain.Exceptions;
using Patrimonio.Financas.Tests.Integration.Base;

namespace Patrimonio.Financas.Tests.Integration.Seeders;

/// <summary>
/// Fornece os dados base de contas necessários aos testes de integração.
/// </summary>
internal sealed class ContaSeeder(
    IContaCommandService commandService,
    IContaQueryService queryService)
{
    public async Task SeedAsync(BaseData data)
    {
        ContaDetalheDto conta;

        try
        {
            conta = await queryService.ObterPorIdAsync(1, TestContext.Current.CancellationToken);
        }
        catch (RecursoNaoEncontradoException)
        {
            var contaDto = Criar(data.Instituicao.Id);

            conta = await commandService.CriarAsync(contaDto, TestContext.Current.CancellationToken);
        }

        data.Conta = conta;
    }

    private static ContaCriacaoDto Criar(int instituicaoId)
    {
        return new ContaCriacaoDto
        {
            Nome = "Conta de teste",
            InstituicaoId = instituicaoId
        };
    }
}
