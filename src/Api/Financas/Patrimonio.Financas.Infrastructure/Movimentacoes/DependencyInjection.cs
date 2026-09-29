using Microsoft.Extensions.DependencyInjection;
using Patrimonio.Financas.Application.Movimentacoes.Commands.Abstractions;
using Patrimonio.Financas.Application.Movimentacoes.Queries.Abstractions;
using Patrimonio.Financas.Infrastructure.Common.Exceptions;
using Patrimonio.Financas.Infrastructure.Movimentacoes.Commands;
using Patrimonio.Financas.Infrastructure.Movimentacoes.Exceptions;
using Patrimonio.Financas.Infrastructure.Movimentacoes.Queries;

namespace Patrimonio.Financas.Infrastructure.Movimentacoes;

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
    public static IServiceCollection AddMovimentacoesInfrastructure(this IServiceCollection services)
    {
        // Repositórios
        services.AddScoped<IMovimentacaoCommandRepository, MovimentacaoCommandRepository>()
                .AddScoped<IMovimentacaoQueryRepository, MovimentacaoQueryRepository>();

        // Tradutor
        services.AddScoped<IConstraintTranslator, MovimentacaoConstraintTranslator>();

        return services;
    }
}
