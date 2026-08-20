using Microsoft.Extensions.DependencyInjection;
using Patrimonio.Financas.Application.Movimentacoes.Commands.Services;
using Patrimonio.Financas.Application.Movimentacoes.Queries.Services;
using Patrimonio.Financas.Contracts.Movimentacoes.Services;

namespace Patrimonio.Financas.Application.Movimentacoes;

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
    internal static IServiceCollection AddMovimentacoesApplication(this IServiceCollection services)
    {
        services.AddScoped<IMovimentacaoCommandService, MovimentacaoCommandService>()
                .AddScoped<IMovimentacaoQueryService, MovimentacaoQueryService>();

        return services;
    }
}
