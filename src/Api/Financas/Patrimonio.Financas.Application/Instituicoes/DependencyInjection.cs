using Microsoft.Extensions.DependencyInjection;
using Patrimonio.Financas.Application.Instituicoes.Commands.Services;
using Patrimonio.Financas.Application.Instituicoes.Queries.Services;
using Patrimonio.Financas.Contracts.Instituicoes.Services;

namespace Patrimonio.Financas.Application.Instituicoes;

/// <summary>
/// Contém os registros de injeção de dependência da funcionalidade de instituições.
/// </summary>
internal static class DependencyInjection
{
    /// <summary>
    /// Registra os serviços da funcionalidade de instituições no contêiner de injeção de dependência.
    /// </summary>
    /// <param name="services">Contêiner de serviços da aplicação.</param>
    /// <returns>O contêiner de serviços configurado.</returns>
    internal static IServiceCollection AddInstituicoesApplication(this IServiceCollection services)
    {
        services.AddScoped<IInstituicaoCommandService, InstituicaoCommandService>()
                .AddScoped<IInstituicaoQueryService, InstituicaoQueryService>();

        return services;
    }
}
