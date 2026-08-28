using Patrimonio.Financas.Application.Common.Abstractions;
using Patrimonio.Financas.Domain.Movimentacoes.Entities;

namespace Patrimonio.Financas.Application.Movimentacoes.Commands.Abstractions;

/// <summary>
/// Define as operações de persistência necessárias aos comandos de movimentações.
/// </summary>
public interface IMovimentacaoCommandRepository : ICommandRepository<Movimentacao>
{
}
