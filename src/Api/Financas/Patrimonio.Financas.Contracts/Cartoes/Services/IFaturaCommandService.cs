using Patrimonio.Financas.Contracts.Cartoes.Dtos;

namespace Patrimonio.Financas.Contracts.Cartoes.Services;

/// <summary>
/// Define as operações de escrita disponíveis para faturas.
/// </summary>
public interface IFaturaCommandService
{
    /// <summary>
    /// Cria uma nova fatura.
    /// </summary>
    /// <param name="dto">Dados necessários para a criação da fatura.</param>
    /// <param name="cancellationToken">Token utilizado para cancelar a operação.</param>
    /// <returns>Os dados detalhados da fatura criada.</returns>
    Task<FaturaDetalheDto> CriarAsync(FaturaCriacaoDto dto, CancellationToken cancellationToken);

    /// <summary>
    /// Altera uma fatura existente.
    /// </summary>
    /// <param name="faturaId">Identificador da fatura que será alterada.</param>
    /// <param name="dto">Dados da alteração.</param>
    /// <param name="cancellationToken">Token utilizado para cancelar a operação.</param>
    Task AlterarAsync(int faturaId, FaturaAlteracaoDto dto, CancellationToken cancellationToken);

    /// <summary>
    /// Exclui uma fatura existente.
    /// </summary>
    /// <param name="faturaId">Identificador da fatura que será excluída.</param>
    /// <param name="cancellationToken">Token utilizado para cancelar a operação.</param>
    Task ExcluirAsync(int faturaId, CancellationToken cancellationToken);

    /// <summary>
    /// Fecha uma fatura existente, alterando seu status para "Fechada".
    /// </summary>
    /// <param name="faturaId">Identificador da fatura que será fechada.</param>
    /// <param name="cancellationToken">Token utilizado para cancelar a operação.</param>
    Task FecharAsync(int faturaId, CancellationToken cancellationToken);

    /// <summary>
    /// Marca uma fatura como paga, alterando seu status para "Paga" e registrando a data de pagamento.
    /// </summary>
    /// <param name="faturaId">Identificador da fatura que será marcada como paga.</param>
    /// <param name="dataPagamento">Data em que a fatura foi paga.</param>
    /// <param name="cancellationToken">Token utilizado para cancelar a operação.</param>
    Task PagarAsync(int faturaId, DateOnly dataPagamento, CancellationToken cancellationToken);
}
