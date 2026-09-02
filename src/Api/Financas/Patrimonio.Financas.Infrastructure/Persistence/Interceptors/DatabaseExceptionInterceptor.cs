using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Npgsql;
using Patrimonio.Financas.Infrastructure.Common.Exceptions;

namespace Patrimonio.Financas.Infrastructure.Persistence.Interceptors;

/// <summary>
/// Intercepta falhas ocorridas durante a persistência de alterações no banco de dados
/// e traduz exceções específicas do PostgreSQL para exceções conhecidas da aplicação.
/// </summary>
/// <param name="translator">Tradutor de exceções provenientes do banco de dados.</param>
internal sealed class DatabaseExceptionInterceptor(
    DatabaseConstraintTranslator translator) : SaveChangesInterceptor
{
    /// <inheritdoc/>
    public override Task SaveChangesFailedAsync(
        DbContextErrorEventData eventData,
        CancellationToken cancellationToken = default)
    {
        if (eventData.Exception is DbUpdateException
            {
                InnerException: PostgresException postgresException
            })
        {
            throw translator.Traduzir(postgresException);
        }

        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public override void SaveChangesFailed(
        DbContextErrorEventData eventData)
    {
        if (eventData.Exception is DbUpdateException
            {
                InnerException: PostgresException postgresException
            })
        {
            throw translator.Traduzir(postgresException);
        }
    }
}
