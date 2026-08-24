using Microsoft.Extensions.DependencyInjection;
using Patrimonio.Financas.Contracts.Movimentacoes.Services;
using Patrimonio.Financas.Wpf.Common.Http;
using Patrimonio.Financas.Wpf.Movimentacoes.HttpClients;
using Patrimonio.Financas.Wpf.Movimentacoes.ViewModels;

namespace Patrimonio.Financas.Wpf.Movimentacoes;

/// <summary>
/// Contém os registros de injeção de dependência da funcionalidade de movimentações.
/// </summary>
internal static class DependencyInjection
{
    /// <summary>
    /// Registra os serviços da funcionalidade de movimentações no contêiner de injeção de dependência.
    /// </summary>
    /// <param name="services">Contêiner de serviços da aplicação.</param>
    /// <returns>O contêiner de serviços configurado.</returns>
    public static IServiceCollection AddMovimentacoesWpf(this IServiceCollection services)
    {
        // HTTP Clients
        services.AddHttpClient<MovimentacaoHttpClient>()
            .AddHttpMessageHandler<ApiResponseHandler>();

        // Services
        services.AddTransient<IMovimentacaoCommandService>(
            serviceProvider => serviceProvider.GetRequiredService<MovimentacaoHttpClient>());

        services.AddTransient<IMovimentacaoQueryService>(
            serviceProvider => serviceProvider.GetRequiredService<MovimentacaoHttpClient>());

        // ViewModels
        services.AddTransient<MovimentacaoAlteracaoViewModel>();
        services.AddTransient<MovimentacaoCriacaoViewModel>();
        services.AddTransient<MovimentacaoListaViewModel>();

        return services;
    }
}
