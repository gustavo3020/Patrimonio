using Microsoft.Extensions.DependencyInjection;
using Patrimonio.Financas.Application.Contas.Commands.Services;
using Patrimonio.Financas.Application.Contas.Queries.Services;
using Patrimonio.Financas.Contracts.Contas.Services;

namespace Patrimonio.Financas.Application.Contas;

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
    internal static IServiceCollection AddContasApplication(this IServiceCollection services)
    {
        services.AddScoped<IContaCommandService, ContaCommandService>()
                .AddScoped<IContaQueryService, ContaQueryService>();

        return services;
    }
}
