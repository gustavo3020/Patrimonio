using Microsoft.Extensions.DependencyInjection;
using Patrimonio.Financas.Application.Categorias;
using Patrimonio.Financas.Application.Contas;
using Patrimonio.Financas.Application.Instituicoes;
using Patrimonio.Financas.Application.Movimentacoes;

namespace Patrimonio.Financas.Application;

/// <summary>
/// Contém os registros de injeção de dependência da camada Application.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Registra os serviços da camada Application no contêiner de injeção de dependência.
    /// </summary>
    /// <param name="services">Contêiner de serviços da aplicação.</param>
    /// <returns>O contêiner de serviços configurado.</returns>
    public static IServiceCollection AddFinancasApplication(this IServiceCollection services)
    {
        services.AddCategoriasApplication()
                .AddContasApplication()
                .AddInstituicoesApplication()
                .AddMovimentacoesApplication();

        return services;
    }
}
