using Patrimonio.Financas.Application.Common.Abstractions;
using Patrimonio.Financas.Domain.Contas.Entities;

namespace Patrimonio.Financas.Application.Contas.Commands.Abstractions;

/// <summary>
/// Define as operações de persistência necessárias aos comandos de contas.
/// </summary>
public interface IContaCommandRepository : ICommandRepository<Conta>
{
}
