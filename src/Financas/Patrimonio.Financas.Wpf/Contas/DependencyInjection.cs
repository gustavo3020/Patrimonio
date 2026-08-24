using Microsoft.Extensions.DependencyInjection;
using Patrimonio.Financas.Contracts.Contas.Services;
using Patrimonio.Financas.Wpf.Common.Http;
using Patrimonio.Financas.Wpf.Contas.HttpClients;
using Patrimonio.Financas.Wpf.Contas.ViewModels;

namespace Patrimonio.Financas.Wpf.Contas;

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
    public static IServiceCollection AddContasWpf(this IServiceCollection services)
    {
        // HTTP Clients
        services.AddHttpClient<ContaHttpClient>()
            .AddHttpMessageHandler<ApiResponseHandler>();

        // Services
        services.AddTransient<IContaCommandService>(
            serviceProvider => serviceProvider.GetRequiredService<ContaHttpClient>());

        services.AddTransient<IContaQueryService>(
            serviceProvider => serviceProvider.GetRequiredService<ContaHttpClient>());

        // ViewModels
        services.AddTransient<ContaAlteracaoViewModel>();
        services.AddTransient<ContaCriacaoViewModel>();
        services.AddTransient<ContaListaViewModel>();

        return services;
    }
}
