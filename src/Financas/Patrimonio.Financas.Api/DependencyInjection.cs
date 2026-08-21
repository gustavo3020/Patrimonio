using Patrimonio.Financas.Application;
using Patrimonio.Financas.Infrastructure;

namespace Patrimonio.Financas.Api;

/// <summary>
/// Contém os registros de injeção de dependência do módulo.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Registra os serviços do módulo no contêiner de injeção de dependência.
    /// </summary>
    /// <param name="services">Contêiner de serviços da aplicação.</param>
    /// <param name="configuration">Configuração da aplicação.</param>
    /// <returns>O contêiner de serviços configurado.</returns>
    public static IServiceCollection AddFinancasModule(this IServiceCollection services, IConfiguration configuration)
    {
        // Application e Infrastructure
        services.AddFinancasApplication()
                .AddFinancasInfrastructure(configuration);

        // Controllers
        services.AddControllers()
                .AddApplicationPart(typeof(DependencyInjection).Assembly);

        return services;
    }
}
