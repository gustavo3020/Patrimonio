namespace Patrimonio.Financas.Application.Common.ReadModels;

/// <summary>
/// Representa os dados comuns a todos os Read Models de leitura,
/// contendo o identificador e as informações de auditoria da entidade.
/// </summary>
public abstract record LeituraReadModelBase
{
    public required int Id { get; init; }
    public required DateTimeOffset DataCriacao { get; init; }
    public required DateTimeOffset DataAlteracao { get; init; }
}
