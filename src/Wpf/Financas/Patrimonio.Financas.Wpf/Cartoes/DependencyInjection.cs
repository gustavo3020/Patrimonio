using Microsoft.Extensions.DependencyInjection;
using Patrimonio.Financas.Contracts.Cartoes.Services;
using Patrimonio.Financas.Wpf.Cartoes.HttpClients;
using Patrimonio.Financas.Wpf.Common.Http;

namespace Patrimonio.Financas.Wpf.Cartoes;

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
    public static IServiceCollection AddCartoesWpf(this IServiceCollection services)
    {
        // HTTP Clients
        services.AddHttpClient<CartaoHttpClient>().AddHttpMessageHandler<ApiResponseHandler>();
        services.AddHttpClient<FaturaHttpClient>().AddHttpMessageHandler<ApiResponseHandler>();
        services.AddHttpClient<LancamentoHttpClient>().AddHttpMessageHandler<ApiResponseHandler>();

        // Services
        services.AddTransient<ICartaoCommandService, CartaoHttpClient>();
        services.AddTransient<ICartaoQueryService, CartaoHttpClient>();
        services.AddTransient<IFaturaCommandService, FaturaHttpClient>();
        services.AddTransient<IFaturaQueryService, FaturaHttpClient>();
        services.AddTransient<ILancamentoCommandService, LancamentoHttpClient>();
        services.AddTransient<ILancamentoQueryService, LancamentoHttpClient>();

        return services;
    }
}
