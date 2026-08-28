using Microsoft.EntityFrameworkCore;
using Npgsql;
using Patrimonio.Financas.Application.Common.Abstractions;
using Patrimonio.Financas.Domain.Common.Entities;
using Patrimonio.Financas.Infrastructure.Common.Exceptions;
using Patrimonio.Financas.Infrastructure.Persistence;

namespace Patrimonio.Financas.Infrastructure.Common.Commands;

/// <summary>
/// Implementa as operações de persistência comuns aos repositórios de comando.
/// </summary>
/// <typeparam name="TEntity">Tipo da entidade manipulada pelo repositório.</typeparam>
/// <param name="context">Contexto de persistência do módulo financeiro.</param>
/// <param name="translator">Tradutor de exceções provenientes do banco de dados.</param>
internal abstract class CommandRepository<TEntity>(FinancasDbContext context, DatabaseExceptionTranslator translator)
    : ICommandRepository<TEntity> where TEntity : EntidadeBase
{
    /// <inheritdoc/>
    public void Adicionar(TEntity entidade) => context.Set<TEntity>().Add(entidade);

    /// <inheritdoc/>
    public void Remover(TEntity entidade) => context.Set<TEntity>().Remove(entidade);

    /// <inheritdoc/>
    public async Task<TEntity> ObterPorIdAsync(int id, CancellationToken cancellationToken)
    {
        var entidade = await context.Set<TEntity>()
            .FindAsync([id], cancellationToken);

        return entidade ?? throw new RecursoNaoEncontradoException($"Registro com id {id} não encontrado.");
    }

    /// <inheritdoc/>
    public async Task SalvarAsync(CancellationToken cancellationToken)
    {
        try
        {
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex) when (ex.InnerException is PostgresException postgresException)
        {
            throw translator.Traduzir(postgresException);
        }
    }
}
