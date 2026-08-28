using Microsoft.Extensions.DependencyInjection;
using Patrimonio.Financas.Contracts.Instituicoes.Services;
using Patrimonio.Financas.Wpf.Common.Http;
using Patrimonio.Financas.Wpf.Instituicoes.HttpClients;
using Patrimonio.Financas.Wpf.Instituicoes.Navigation;
using Patrimonio.Financas.Wpf.Instituicoes.ViewModels;

namespace Patrimonio.Financas.Wpf.Instituicoes;

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
    public static IServiceCollection AddInstituicoesWpf(this IServiceCollection services)
    {
        // HTTP Clients
        services.AddHttpClient<InstituicaoHttpClient>()
            .AddHttpMessageHandler<ApiResponseHandler>();

        // Services
        services.AddTransient<IInstituicaoCommandService>(
            serviceProvider => serviceProvider.GetRequiredService<InstituicaoHttpClient>());

        services.AddTransient<IInstituicaoQueryService>(
            serviceProvider => serviceProvider.GetRequiredService<InstituicaoHttpClient>());

        // ViewModels
        services.AddTransient<InstituicaoAlteracaoViewModel>();
        services.AddTransient<InstituicaoCriacaoViewModel>();
        services.AddTransient<InstituicaoListaViewModel>();

        // Navigation
        services.AddSingleton<InstituicaoNavigation>();

        return services;
    }
}
