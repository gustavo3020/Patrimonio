using Patrimonio.Financas.Domain.Common.Entities;
using Patrimonio.Financas.Domain.Exceptions;
using Patrimonio.Financas.SharedKernel.Cartoes.Enums;

namespace Patrimonio.Financas.Domain.Cartoes.Entities;

/// <summary>
/// Representa uma fatura de cartão de crédito.
/// </summary>
public sealed class Fatura : EntidadeBase
{
    private Fatura(
        DateOnly dataFechamento,
        DateOnly dataVencimento,
        int cartaoId)
    {
        DataFechamento = dataFechamento;
        DataVencimento = dataVencimento;
        CartaoId = cartaoId;
    }

    private Fatura()
    {
    }

    // Dados próprios.
    public DateOnly DataFechamento { get; private set; }
    public DateOnly DataVencimento { get; private set; }
    public StatusFatura Status { get; private set; } = StatusFatura.Aberta;
    public DateOnly? DataPagamento { get; private set; }

    // Relacionamentos.
    public int CartaoId { get; private set; }

    /// <summary>
    /// Cria uma nova instância de Fatura com os dados fornecidos.
    /// </summary>
    /// <param name="dataFechamento">Data de fechamento da fatura.</param>
    /// <param name="dataVencimento">Data de vencimento da fatura.</param>
    /// <param name="cartaoId">ID do cartão associado.</param>
    /// <returns>A instância de Fatura criada.</returns>
    /// <exception cref="RegraDeNegocioException">
    /// Lançada quando a data de fechamento é posterior à data de vencimento.
    /// </exception>
    public static Fatura Criar(
        DateOnly dataFechamento,
        DateOnly dataVencimento,
        int cartaoId)
    {
        if (dataFechamento > dataVencimento)
            throw new RegraDeNegocioException("A data de fechamento deve ser anterior ou igual à data de vencimento.");

        return new Fatura(dataFechamento, dataVencimento, cartaoId);
    }

    /// <summary>
    /// Altera os dados da fatura.
    /// </summary>
    /// <param name="dataFechamento">Data de fechamento da fatura.</param>
    /// <param name="dataVencimento">Data de vencimento da fatura.</param>
    /// <exception cref="RegraDeNegocioException">
    /// Lançada quando a data de fechamento é posterior à data de vencimento.
    /// </exception>
    public void Alterar(
        DateOnly dataFechamento,
        DateOnly dataVencimento)
    {
        if (Status != StatusFatura.Aberta)
            throw new RegraDeNegocioException("Só é possível alterar uma fatura que esteja aberta.");

        if (dataFechamento > dataVencimento)
            throw new RegraDeNegocioException("A data de fechamento deve ser anterior ou igual à data de vencimento.");

        DataFechamento = dataFechamento;
        DataVencimento = dataVencimento;

        RegistrarAlteracao();
    }

    /// <summary>
    /// Fecha a fatura atual.
    /// </summary>
    /// <exception cref="RegraDeNegocioException">
    /// Lançada quando a fatura não está aberta.
    /// </exception>
    public void Fechar()
    {
        if (Status != StatusFatura.Aberta)
            throw new RegraDeNegocioException("Só é possível fechar uma fatura que esteja aberta.");

        Status = StatusFatura.Fechada;

        RegistrarAlteracao();
    }

    /// <summary>
    /// Paga a fatura atual.
    /// </summary>
    /// <param name="dataPagamento">Data de pagamento da fatura.</param>
    /// <exception cref="RegraDeNegocioException">
    /// Lançada quando a fatura não está fechada ou a data de pagamento é anterior à data de fechamento
    /// </exception>
    public void Pagar(DateOnly dataPagamento)
    {
        if (Status != StatusFatura.Fechada)
            throw new RegraDeNegocioException("Só é possível pagar uma fatura que esteja fechada");

        if (dataPagamento < DataFechamento)
            throw new RegraDeNegocioException("A data de pagamento não pode ser anterior à data de fechamento.");

        Status = StatusFatura.Paga;
        DataPagamento = dataPagamento;

        RegistrarAlteracao();
    }
}
