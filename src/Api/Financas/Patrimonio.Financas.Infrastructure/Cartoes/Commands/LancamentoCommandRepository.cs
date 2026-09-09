using Patrimonio.Financas.Application.Cartoes.Commands.Abstractions;
using Patrimonio.Financas.Domain.Cartoes.Entities;
using Patrimonio.Financas.Infrastructure.Common.Commands;
using Patrimonio.Financas.Infrastructure.Persistence;

namespace Patrimonio.Financas.Infrastructure.Cartoes.Commands;

/// <summary>
/// Implementa as operações de persistência de comandos para lançamentos.
/// </summary>
internal sealed class LancamentoCommandRepository(FinancasDbContext context)
    : CommandRepository<Lancamento>(context), ILancamentoCommandRepository
{
}
