namespace Patrimonio.Financas.Domain.Common.Entities;

/// <summary>
/// Representa a base para as entidades do domínio que possuem uma identificação única.
/// </summary>
public abstract class EntidadeBase
{
    // Identificação.
    public int Id { get; init; }

    // Campos de auditoria.
    public DateOnly DataCriacao { get; init; } = DateOnly.FromDateTime(DateTime.Today);
    public DateOnly DataAtualizacao { get; private set; } = DateOnly.FromDateTime(DateTime.Today);

    /// <summary>
    /// Registra a atualização da entidade, definindo a data de atualização para o dia atual.
    /// </summary>
    protected void RegistrarAtualizacao()
    {
        DataAtualizacao = DateOnly.FromDateTime(DateTime.Today);
    }
}
