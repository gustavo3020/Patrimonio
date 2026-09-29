using Patrimonio.Financas.Application.Common.Abstractions;
using Patrimonio.Financas.Domain.Cartoes.Entities;

namespace Patrimonio.Financas.Application.Cartoes.Commands.Abstractions;

/// <summary>
/// Define as operações de persistência necessárias aos comandos de cartões.
/// </summary>
public interface ICartaoCommandRepository : ICommandRepository<Cartao>
{
}
