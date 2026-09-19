using Patrimonio.Financas.Application.Cartoes.Commands.Abstractions;
using Patrimonio.Financas.Domain.Cartoes.Entities;
using Patrimonio.Financas.Infrastructure.Common.Commands;
using Patrimonio.Financas.Infrastructure.Persistence;

namespace Patrimonio.Financas.Infrastructure.Cartoes.Commands;

/// <summary>
/// Implementa as operações de persistência de comandos para cartões.
/// </summary>
internal sealed class CartaoCommandRepository(FinancasDbContext context)
    : CommandRepository<Cartao>(context), ICartaoCommandRepository
{
}
