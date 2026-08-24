using Microsoft.Extensions.DependencyInjection;
using Patrimonio.Financas.Contracts.Categorias.Services;
using Patrimonio.Financas.Wpf.Categorias.HttpClients;
using Patrimonio.Financas.Wpf.Categorias.ViewModels;
using Patrimonio.Financas.Wpf.Common.Http;

namespace Patrimonio.Financas.Wpf.Categorias;

/// <summary>
/// Contém os registros de injeção de dependência da funcionalidade de categorias.
/// </summary>
internal static class DependencyInjection
{
    /// <summary>
    /// Registra os serviços da funcionalidade de categorias no contêiner de injeção de dependência.
    /// </summary>
    /// <param name="services">Contêiner de serviços da aplicação.</param>
    /// <returns>O contêiner de serviços configurado.</returns>
    public static IServiceCollection AddCategoriasWpf(this IServiceCollection services)
    {
        // HTTP Clients
        services.AddHttpClient<CategoriaHttpClient>()
            .AddHttpMessageHandler<ApiResponseHandler>();

        // Services
        services.AddTransient<ICategoriaCommandService>(
            serviceProvider => serviceProvider.GetRequiredService<CategoriaHttpClient>());

        services.AddTransient<ICategoriaQueryService>(
            serviceProvider => serviceProvider.GetRequiredService<CategoriaHttpClient>());

        // ViewModels
        services.AddTransient<CategoriaAlteracaoViewModel>();
        services.AddTransient<CategoriaCriacaoViewModel>();
        services.AddTransient<CategoriaListaViewModel>();

        return services;
    }
}
