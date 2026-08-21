using Microsoft.Extensions.DependencyInjection;
using Patrimonio.Financas.Application.Contas.Commands.Abstractions;
using Patrimonio.Financas.Application.Contas.Queries.Abstractions;
using Patrimonio.Financas.Infrastructure.Common.Exceptions;
using Patrimonio.Financas.Infrastructure.Contas.Commands;
using Patrimonio.Financas.Infrastructure.Contas.Exceptions;
using Patrimonio.Financas.Infrastructure.Contas.Queries;

namespace Patrimonio.Financas.Infrastructure.Contas;

/// <summary>
/// Contém os registros de injeção de dependência da funcionalidade de contas.
/// </summary>
internal static class DependencyInjection
{
    /// <summary>
    /// Registra os serviços da funcionalidade de contas no contêiner de injeção de dependência.
    /// </summary>
    /// <param name="services">Contêiner de serviços da aplicação.</param>
    /// <returns>O contêiner de serviços configurado.</returns>
    public static IServiceCollection AddContasInfrastructure(this IServiceCollection services)
    {
        // Repositórios
        services.AddScoped<IContaCommandRepository, ContaCommandRepository>()
                .AddScoped<IContaQueryRepository, ContaQueryRepository>();

        // Tradutor
        services.AddScoped<IConstraintTranslator, ContaConstraintTranslator>();

        return services;
    }
}
