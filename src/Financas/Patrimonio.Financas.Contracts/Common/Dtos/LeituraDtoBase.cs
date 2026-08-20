namespace Patrimonio.Financas.Contracts.Common.Dtos;

/// <summary>
/// Representa os dados comuns a todos os DTOs de leitura,
/// contendo o identificador e as informações de auditoria da entidade.
/// </summary>
public abstract record LeituraDtoBase
{
    public required int Id { get; init; }
    public required DateTimeOffset DataCriacao { get; init; }
    public required DateTimeOffset DataAlteracao { get; init; }
}
