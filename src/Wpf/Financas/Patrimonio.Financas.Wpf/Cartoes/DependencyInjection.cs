using Microsoft.Extensions.DependencyInjection;
using Patrimonio.Financas.Wpf.Cartoes.HttpClients;
using Patrimonio.Financas.Wpf.Cartoes.Navigation;
using Patrimonio.Financas.Wpf.Cartoes.ViewModels;
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

        // ViewModels
        services.AddTransient<CartaoAlteracaoViewModel>();
        services.AddTransient<CartaoCriacaoViewModel>();
        services.AddTransient<CartaoListaViewModel>();
        services.AddTransient<FaturaAlteracaoViewModel>();
        services.AddTransient<FaturaCriacaoViewModel>();
        services.AddTransient<FaturaListaViewModel>();
        services.AddTransient<FaturaPagamentoViewModel>();
        services.AddTransient<LancamentoAlteracaoViewModel>();
        services.AddTransient<LancamentoCriacaoViewModel>();
        services.AddTransient<LancamentoListaViewModel>();

        // Navigation
        services.AddSingleton<CartaoNavigation>();
        services.AddSingleton<FaturaNavigation>();
        services.AddSingleton<LancamentoNavigation>();

        return services;
    }
}
