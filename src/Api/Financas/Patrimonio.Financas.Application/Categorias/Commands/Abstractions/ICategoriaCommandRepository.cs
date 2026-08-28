using Patrimonio.Financas.Application.Common.Abstractions;
using Patrimonio.Financas.Domain.Categorias.Entities;

namespace Patrimonio.Financas.Application.Categorias.Commands.Abstractions;

/// <summary>
/// Define as operações de persistência necessárias aos comandos de categorias.
/// </summary>
public interface ICategoriaCommandRepository : ICommandRepository<Categoria>
{
}
