using Microsoft.Extensions.DependencyInjection;
using Patrimonio.Financas.Wpf.Categorias;
using Patrimonio.Financas.Wpf.Common.Http;
using Patrimonio.Financas.Wpf.Contas;
using Patrimonio.Financas.Wpf.Instituicoes;
using Patrimonio.Financas.Wpf.Movimentacoes;

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
        services.AddTransient<ApiResponseHandler>();

        services.AddCategoriasWpf()
                .AddContasWpf()
                .AddInstituicoesWpf()
                .AddMovimentacoesWpf();

        return services;
    }
}
