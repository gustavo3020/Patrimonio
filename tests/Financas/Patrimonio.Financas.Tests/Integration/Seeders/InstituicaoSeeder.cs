using Patrimonio.Financas.Contracts.Instituicoes.Dtos;
using Patrimonio.Financas.Contracts.Instituicoes.Services;
using Patrimonio.Financas.Domain.Exceptions;
using Patrimonio.Financas.Tests.Integration.Base;

namespace Patrimonio.Financas.Tests.Integration.Seeders;

/// <summary>
/// Fornece os dados base de instituições necessários aos testes de integração.
/// </summary>
internal sealed class InstituicaoSeeder(
    IInstituicaoCommandService commandService,
    IInstituicaoQueryService queryService)
{
    public async Task SeedAsync(BaseData data)
    {
        InstituicaoDetalheDto instituicao;

        try
        {
            instituicao = await queryService.ObterPorIdAsync(1, TestContext.Current.CancellationToken);
        }
        catch (RecursoNaoEncontradoException)
        {
            var instituicaoDto = Criar();

            instituicao = await commandService.CriarAsync(instituicaoDto, TestContext.Current.CancellationToken);
        }

        data.Instituicao = instituicao;
    }

    private static InstituicaoCriacaoDto Criar()
    {
        return new InstituicaoCriacaoDto
        {
            Nome = "Instituição de teste",
        };
    }
}
