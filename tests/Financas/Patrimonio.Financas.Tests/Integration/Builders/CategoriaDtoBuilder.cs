using Patrimonio.Financas.Contracts.Categorias.Dtos;

namespace Patrimonio.Financas.Tests.Integration.Builders;

internal static class CategoriaDtoBuilder
{
    public static CategoriaCriacaoDto Criar(string? nome = null)
    {
        return new CategoriaCriacaoDto
        {
            Nome = nome ?? $"Categoria {Guid.NewGuid()}"
        };
    }

    public static CategoriaAlteracaoDto Alterar(string? nome = null)
    {
        return new CategoriaAlteracaoDto
        {
            Nome = nome ?? $"Categoria alterada {Guid.NewGuid()}"
        };
    }
}
