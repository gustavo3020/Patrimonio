using Microsoft.Extensions.DependencyInjection;
using Patrimonio.Financas.Application.Categorias.Commands.Abstractions;
using Patrimonio.Financas.Application.Categorias.Queries.Abstractions;
using Patrimonio.Financas.Infrastructure.Categorias.Commands;
using Patrimonio.Financas.Infrastructure.Categorias.Exceptions;
using Patrimonio.Financas.Infrastructure.Categorias.Queries;
using Patrimonio.Financas.Infrastructure.Common.Exceptions;

namespace Patrimonio.Financas.Infrastructure.Categorias;

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
    public static IServiceCollection AddCategoriasInfrastructure(this IServiceCollection services)
    {
        // Repositórios
        services.AddScoped<ICategoriaCommandRepository, CategoriaCommandRepository>()
                .AddScoped<ICategoriaQueryRepository, CategoriaQueryRepository>();

        // Tradutor
        services.AddScoped<IConstraintTranslator, CategoriaConstraintTranslator>();

        return services;
    }
}
