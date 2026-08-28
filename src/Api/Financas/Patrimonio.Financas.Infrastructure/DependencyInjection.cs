using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Patrimonio.Financas.Infrastructure.Categorias;
using Patrimonio.Financas.Infrastructure.Common.Exceptions;
using Patrimonio.Financas.Infrastructure.Contas;
using Patrimonio.Financas.Infrastructure.Instituicoes;
using Patrimonio.Financas.Infrastructure.Movimentacoes;
using Patrimonio.Financas.Infrastructure.Persistence;

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
        // DbContext
        var connectionString = configuration.GetConnectionString("DatabaseConnectionString");

        if (string.IsNullOrWhiteSpace(connectionString))
            throw new ConfiguracaoInvalidaException("Connection string 'DatabaseConnectionString' não configurada.");

        services.AddDbContext<FinancasDbContext>(options =>
            options.UseNpgsql(connectionString, b =>
            {
                b.MigrationsHistoryTable("__EFMigrationsHistory", "financas");
            })
        );

        // Funcionalidades
        services.AddCategoriasInfrastructure()
                .AddContasInfrastructure()
                .AddInstituicoesInfrastructure()
                .AddMovimentacoesInfrastructure();

        // Serviços compartilhados
        services.AddScoped<DatabaseExceptionTranslator>();

        return services;
    }
}
