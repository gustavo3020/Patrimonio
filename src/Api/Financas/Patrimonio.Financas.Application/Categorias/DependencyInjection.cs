using Microsoft.Extensions.DependencyInjection;
using Patrimonio.Financas.Application.Categorias.Commands.Services;
using Patrimonio.Financas.Application.Categorias.Queries.Services;
using Patrimonio.Financas.Contracts.Categorias.Services;

namespace Patrimonio.Financas.Application.Categorias;

/// <summary>
/// Contém os registros de injeção de dependência da funcionalidade de categorias.
/// </summary>
internal static class DependencyInjection
{
    /// <summary>
    /// Registra os serviços da funcionalidade de categorias no contêiner de injeção de dependência.
    /// </summary>
    /// <param name="services">Contêiner de serviços da aplicação.</param>
    /// <returns>O contêiner de serviços configurado.</returns>
    internal static IServiceCollection AddCategoriasApplication(this IServiceCollection services)
    {
        services.AddScoped<ICategoriaCommandService, CategoriaCommandService>()
                .AddScoped<ICategoriaQueryService, CategoriaQueryService>();

        return services;
    }
}
