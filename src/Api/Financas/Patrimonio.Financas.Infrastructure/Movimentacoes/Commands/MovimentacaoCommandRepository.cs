using Patrimonio.Financas.Application.Movimentacoes.Commands.Abstractions;
using Patrimonio.Financas.Domain.Movimentacoes.Entities;
using Patrimonio.Financas.Infrastructure.Common.Commands;
using Patrimonio.Financas.Infrastructure.Persistence;

namespace Patrimonio.Financas.Infrastructure.Movimentacoes.Commands;

/// <summary>
/// Implementa as operações de persistência de comandos para movimentações.
/// </summary>
internal sealed class MovimentacaoCommandRepository(FinancasDbContext context)
    : CommandRepository<Movimentacao>(context), IMovimentacaoCommandRepository
{
}
