using Patrimonio.Financas.Application.Instituicoes.Commands.Abstractions;
using Patrimonio.Financas.Domain.Instituicoes.Entities;
using Patrimonio.Financas.Infrastructure.Common.Commands;
using Patrimonio.Financas.Infrastructure.Persistence;

namespace Patrimonio.Financas.Infrastructure.Instituicoes.Commands;

/// <summary>
/// Implementa as operações de persistência de comandos para instituições.
/// </summary>
internal sealed class InstituicaoCommandRepository(FinancasDbContext context)
    : CommandRepository<Instituicao>(context), IInstituicaoCommandRepository
{
}
