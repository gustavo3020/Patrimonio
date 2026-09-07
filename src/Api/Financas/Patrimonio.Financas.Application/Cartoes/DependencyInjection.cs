using Microsoft.Extensions.DependencyInjection;
using Patrimonio.Financas.Application.Cartoes.Commands.Services;
using Patrimonio.Financas.Application.Cartoes.Queries.Services;
using Patrimonio.Financas.Contracts.Cartoes.Services;

namespace Patrimonio.Financas.Application.Cartoes;

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
    internal static IServiceCollection AddCartoesApplication(this IServiceCollection services)
    {
        // Cartao
        services.AddScoped<ICartaoCommandService, CartaoCommandService>()
                .AddScoped<ICartaoOpcoesCriacaoService, CartaoOpcoesCriacaoService>()
                .AddScoped<ICartaoQueryService, CartaoQueryService>();

        // Fatura
        services.AddScoped<IFaturaCommandService, FaturaCommandService>()
                .AddScoped<IFaturaQueryService, FaturaQueryService>();

        // Lancamento
        services.AddScoped<ILancamentoCommandService, LancamentoCommandService>()
                .AddScoped<ILancamentoQueryService, LancamentoQueryService>();

        return services;
    }
}
