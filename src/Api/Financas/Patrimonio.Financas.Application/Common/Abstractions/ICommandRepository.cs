using Patrimonio.Financas.Domain.Common.Entities;

namespace Patrimonio.Financas.Application.Common.Abstractions;

/// <summary>
/// Define as operações de persistência comuns aos repositórios de comando.
/// </summary>
/// <typeparam name="TEntity">Tipo da entidade manipulada pelo repositório.</typeparam>
public interface ICommandRepository<TEntity> where TEntity : EntidadeBase
{
    /// <summary>
    /// Adiciona uma nova entidade ao contexto de persistência.
    /// </summary>
    /// <param name="entidade">Entidade que será adicionada.</param>
    void Adicionar(TEntity entidade);

    /// <summary>
    /// Remove uma entidade do contexto de persistência.
    /// </summary>
    /// <param name="entidade">Entidade que será removida.</param>
    void Remover(TEntity entidade);

    /// <summary>
    /// Obtém uma entidade pelo identificador para alteração ou exclusão.
    /// </summary>
    /// <param name="id">Identificador da entidade.</param>
    /// <param name="cancellationToken">Token utilizado para cancelar a operação.</param>
    /// <returns>A entidade correspondente ao identificador informado.</returns>
    Task<TEntity?> ObterPorIdAsync(int id, CancellationToken cancellationToken);

    /// <summary>
    /// Persiste as alterações realizadas no contexto.
    /// </summary>
    /// <param name="cancellationToken">Token utilizado para cancelar a operação.</param>
    Task SalvarAsync(CancellationToken cancellationToken);
}
