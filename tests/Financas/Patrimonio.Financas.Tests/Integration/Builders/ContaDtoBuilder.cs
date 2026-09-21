using Patrimonio.Financas.Contracts.Contas.Dtos;

namespace Patrimonio.Financas.Tests.Integration.Builders;

internal static class ContaDtoBuilder
{
    public static ContaCriacaoDto Criar(int instituicaoId, string? nome = null)
    {
        return new ContaCriacaoDto
        {
            Nome = nome ?? $"Conta {Guid.NewGuid()}",
            InstituicaoId = instituicaoId
        };
    }

    public static ContaAlteracaoDto Alterar(int instituicaoId, string? nome = null)
    {
        return new ContaAlteracaoDto
        {
            Nome = nome ?? $"Conta alterada {Guid.NewGuid()}",
            InstituicaoId = instituicaoId
        };
    }
}
