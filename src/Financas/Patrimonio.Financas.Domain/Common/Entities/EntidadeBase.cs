namespace Patrimonio.Financas.Domain.Common.Entities;

/// <summary>
/// Representa a base para as entidades do domínio que possuem uma identificação única.
/// </summary>
public abstract class EntidadeBase
{
    // Identificação.
    public int Id { get; init; }

    // Campos de auditoria.
    public DateTimeOffset DataCriacao { get; init; } = DateTimeOffset.UtcNow;
    public DateTimeOffset DataAlteracao { get; private set; } = DateTimeOffset.UtcNow;

    /// <summary>
    /// Registra a alteração da entidade, definindo a data de alteração para o dia atual.
    /// </summary>
    protected void RegistrarAlteracao()
    {
        DataAlteracao = DateTimeOffset.UtcNow;
    }
}
