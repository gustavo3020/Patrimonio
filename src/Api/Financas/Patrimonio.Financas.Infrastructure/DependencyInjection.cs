using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Patrimonio.Financas.Infrastructure.Categorias;
using Patrimonio.Financas.Infrastructure.Common.Exceptions;
using Patrimonio.Financas.Infrastructure.Contas;
using Patrimonio.Financas.Infrastructure.Instituicoes;
using Patrimonio.Financas.Infrastructure.Movimentacoes;
using Patrimonio.Financas.Infrastructure.Persistence;
using Patrimonio.Financas.Infrastructure.Persistence.Interceptors;

namespace Patrimonio.Financas.Infrastructure;

/// <summary>
/// Contém os registros de injeção de dependência da camada Infrastructure.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Registra os serviços da camada Infrastructure no contêiner de injeção de dependência.
    /// </summary>
    /// <param name="services">Contêiner de serviços da aplicação.</param>
    /// <param name="configuration">Configuração da aplicação.</param>
    /// <returns>O contêiner de serviços configurado.</returns>
    public static IServiceCollection AddFinancasInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // String de conexão
        var connectionString = configuration.GetConnectionString("DatabaseConnectionString");

        if (string.IsNullOrWhiteSpace(connectionString))
            throw new ConfiguracaoInvalidaException("Connection string 'DatabaseConnectionString' não configurada.");

        // Serviços compartilhados
        services.AddScoped<DatabaseExceptionTranslator>();
        services.AddScoped<DatabaseExceptionInterceptor>();

        // DbContext
        services.AddDbContext<FinancasDbContext>((serviceProvider, options) =>
        {
            options.UseNpgsql(connectionString, b =>
            {
                b.MigrationsHistoryTable("__EFMigrationsHistory", "financas");
            });

            options.AddInterceptors(
                serviceProvider.GetRequiredService<DatabaseExceptionInterceptor>());
        });

        // Funcionalidades
        services.AddCategoriasInfrastructure()
                .AddContasInfrastructure()
                .AddInstituicoesInfrastructure()
                .AddMovimentacoesInfrastructure();

        return services;
    }
}
