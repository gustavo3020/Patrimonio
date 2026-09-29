using Patrimonio.Financas.Contracts.Instituicoes.Dtos;

namespace Patrimonio.Financas.Tests.Integration.Builders;

internal static class InstituicaoDtoBuilder
{
    public static InstituicaoCriacaoDto Criar(string? nome = null)
    {
        return new InstituicaoCriacaoDto
        {
            Nome = nome ?? $"Instituição {Guid.NewGuid()}"
        };
    }

    public static InstituicaoAlteracaoDto Alterar(string? nome = null)
    {
        return new InstituicaoAlteracaoDto
        {
            Nome = nome ?? $"Instituição alterada {Guid.NewGuid()}"
        };
    }
}
