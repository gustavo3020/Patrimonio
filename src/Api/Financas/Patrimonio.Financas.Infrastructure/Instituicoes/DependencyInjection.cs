using Microsoft.Extensions.DependencyInjection;
using Patrimonio.Financas.Application.Instituicoes.Commands.Abstractions;
using Patrimonio.Financas.Application.Instituicoes.Lookups.Abstractions;
using Patrimonio.Financas.Application.Instituicoes.Queries.Abstractions;
using Patrimonio.Financas.Infrastructure.Common.Exceptions;
using Patrimonio.Financas.Infrastructure.Instituicoes.Commands;
using Patrimonio.Financas.Infrastructure.Instituicoes.Exceptions;
using Patrimonio.Financas.Infrastructure.Instituicoes.Lookups;
using Patrimonio.Financas.Infrastructure.Instituicoes.Queries;

namespace Patrimonio.Financas.Infrastructure.Instituicoes;

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
    public static IServiceCollection AddInstituicoesInfrastructure(this IServiceCollection services)
    {
        // Repositórios
        services.AddScoped<IInstituicaoCommandRepository, InstituicaoCommandRepository>()
                .AddScoped<IInstituicaoLookupRepository, InstituicaoLookupRepository>()
                .AddScoped<IInstituicaoQueryRepository, InstituicaoQueryRepository>();

        // Tradutor
        services.AddScoped<IConstraintTranslator, InstituicaoConstraintTranslator>();

        return services;
    }
}
