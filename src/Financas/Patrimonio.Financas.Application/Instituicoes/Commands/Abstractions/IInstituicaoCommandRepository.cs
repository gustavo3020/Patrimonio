using Patrimonio.Financas.Application.Common.Abstractions;
using Patrimonio.Financas.Domain.Instituicoes.Entities;

namespace Patrimonio.Financas.Application.Instituicoes.Commands.Abstractions;

/// <summary>
/// Define as operações de persistência necessárias aos comandos de instituições.
/// </summary>
public interface IInstituicaoCommandRepository : ICommandRepository<Instituicao>
{
}
