using Microsoft.Extensions.DependencyInjection;
using Patrimonio.Financas.Wpf.Categorias;
using Patrimonio.Financas.Wpf.Common.Dialogs;
using Patrimonio.Financas.Wpf.Common.Exceptions;
using Patrimonio.Financas.Wpf.Common.Http;
using Patrimonio.Financas.Wpf.Contas;
using Patrimonio.Financas.Wpf.Instituicoes;
using Patrimonio.Financas.Wpf.Movimentacoes;
using Patrimonio.Financas.Wpf.Navigation;

namespace Patrimonio.Financas.Wpf;

/// <summary>
/// Contém os registros de injeção de dependência da camada WPF.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Registra os serviços da camada WPF no contêiner de injeção de dependência.
    /// </summary>
    /// <param name="services">Contêiner de serviços da aplicação.</param>
    /// <returns>O contêiner de serviços configurado.</returns>
    public static IServiceCollection AddFinancasModule(this IServiceCollection services)
    {
        // Infraestrutura comum
        services.AddTransient<ApiResponseHandler>()
                .AddSingleton<ExceptionHandler>()
                .AddSingleton<IDialogService, DialogService>();

        // Funcionalidades
        services.AddCategoriasWpf()
                .AddContasWpf()
                .AddInstituicoesWpf()
                .AddMovimentacoesWpf();

        // Navegação
        services.AddSingleton<FinancasNavigation>();

        return services;
    }
}
