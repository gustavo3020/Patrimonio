using Microsoft.Extensions.DependencyInjection;
using Patrimonio.Financas.Application.Cartoes.Commands.Abstractions;
using Patrimonio.Financas.Application.Cartoes.Lookups.Abstractions;
using Patrimonio.Financas.Application.Cartoes.Queries.Abstractions;
using Patrimonio.Financas.Infrastructure.Cartoes.Commands;
using Patrimonio.Financas.Infrastructure.Cartoes.Exceptions;
using Patrimonio.Financas.Infrastructure.Cartoes.Lookups;
using Patrimonio.Financas.Infrastructure.Cartoes.Queries;
using Patrimonio.Financas.Infrastructure.Common.Exceptions;

namespace Patrimonio.Financas.Infrastructure.Cartoes;

/// <summary>
/// Contém os registros de injeção de dependência da funcionalidade de cartões.
/// </summary>
internal static class DependencyInjection
{
    /// <summary>
    /// Registra os serviços da funcionalidade de cartões no contêiner de injeção de dependência.
    /// </summary>
    /// <param name="services">Contêiner de serviços da aplicação.</param>
    /// <returns>O contêiner de serviços configurado.</returns>
    public static IServiceCollection AddCartoesInfrastructure(this IServiceCollection services)
    {
        // Repositórios
        services.AddScoped<ICartaoCommandRepository, CartaoCommandRepository>()
                .AddScoped<ICartaoQueryRepository, CartaoQueryRepository>()
                .AddScoped<IFaturaCommandRepository, FaturaCommandRepository>()
                .AddScoped<IFaturaLookupRepository, FaturaLookupRepository>()
                .AddScoped<IFaturaQueryRepository, FaturaQueryRepository>()
                .AddScoped<ILancamentoCommandRepository, LancamentoCommandRepository>()
                .AddScoped<ILancamentoQueryRepository, LancamentoQueryRepository>();

        // Tradutor
        services.AddScoped<IConstraintTranslator, CartaoConstraintTranslator>()
                .AddScoped<IConstraintTranslator, FaturaConstraintTranslator>()
                .AddScoped<IConstraintTranslator, LancamentoConstraintTranslator>();

        return services;
    }
}
