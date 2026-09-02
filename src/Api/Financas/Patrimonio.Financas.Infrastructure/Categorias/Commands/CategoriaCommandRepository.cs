using Patrimonio.Financas.Application.Categorias.Commands.Abstractions;
using Patrimonio.Financas.Domain.Categorias.Entities;
using Patrimonio.Financas.Infrastructure.Common.Commands;
using Patrimonio.Financas.Infrastructure.Persistence;

namespace Patrimonio.Financas.Infrastructure.Categorias.Commands;

/// <summary>
/// Implementa as operações de persistência de comandos para categorias.
/// </summary>
internal sealed class CategoriaCommandRepository(FinancasDbContext context)
    : CommandRepository<Categoria>(context), ICategoriaCommandRepository
{
}
