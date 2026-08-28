using Patrimonio.Financas.Application.Contas.Commands.Abstractions;
using Patrimonio.Financas.Domain.Contas.Entities;
using Patrimonio.Financas.Infrastructure.Common.Commands;
using Patrimonio.Financas.Infrastructure.Common.Exceptions;
using Patrimonio.Financas.Infrastructure.Persistence;

namespace Patrimonio.Financas.Infrastructure.Contas.Commands;

/// <summary>
/// Implementa as operações de persistência de comandos para contas.
/// </summary>
internal sealed class ContaCommandRepository(FinancasDbContext context, DatabaseExceptionTranslator translator)
    : CommandRepository<Conta>(context, translator), IContaCommandRepository
{
}
