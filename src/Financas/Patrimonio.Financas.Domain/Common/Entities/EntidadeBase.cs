namespace Patrimonio.Financas.Domain.Common.Entities;

/// <summary>
/// Representa a base para as entidades do domínio que possuem uma identificação única.
/// </summary>
public abstract class EntidadeBase
{
    public int Id { get; init; }
}
