using Patrimonio.Financas.Domain.Common.Entities;
using Patrimonio.Financas.Domain.Common.ValueObjects;
using Patrimonio.Financas.Domain.Exceptions;
using Patrimonio.Financas.SharedKernel.Movimentacoes.Enums;

namespace Patrimonio.Financas.Domain.Movimentacoes.Entities;

/// <summary>
/// Representa uma movimentação financeira de entrada ou saída.
/// </summary>
public sealed class Movimentacao : EntidadeBase
{
    private Movimentacao(
        DateOnly data,
        Dinheiro valor,
        Natureza natureza,
        TipoMovimentacao tipo,
        Descricao? descricao,
        int contaId,
        int categoriaId,
        int? faturaId)
    {
        Data = data;
        Valor = valor;
        Natureza = natureza;
        Tipo = tipo;
        Descricao = descricao;
        ContaId = contaId;
        CategoriaId = categoriaId;
        FaturaId = faturaId;
    }

    private Movimentacao()
    {
    }

    // Dados próprios.
    public DateOnly Data { get; private set; }
    public Dinheiro Valor { get; private set; }
    public Natureza Natureza { get; private set; }
    public TipoMovimentacao Tipo { get; private set; }
    public Descricao? Descricao { get; private set; }

    // Relacionamentos.
    public int ContaId { get; private set; }
    public int CategoriaId { get; private set; }
    public int? FaturaId { get; private set; }

    /// <summary>
    /// Cria uma nova movimentação financeira.
    /// </summary>
    /// <param name="data">Data da movimentação.</param>
    /// <param name="valor">Valor da movimentação.</param>
    /// <param name="natureza">Natureza financeira da movimentação.</param>
    /// <param name="tipo">Tipo da movimentação.</param>
    /// <param name="descricao">Descrição opcional da movimentação.</param>
    /// <param name="contaId">Identificador da conta relacionada.</param>
    /// <param name="categoriaId">Identificador da categoria relacionada.</param>
    /// <param name="faturaId">Identificador da fatura relacionada, quando aplicável.</param>
    /// <returns>Nova movimentação criada.</returns>
    public static Movimentacao Criar(
        DateOnly data,
        Dinheiro valor,
        Natureza natureza,
        TipoMovimentacao tipo,
        Descricao? descricao,
        int contaId,
        int categoriaId,
        int? faturaId = null)
    {
        return new Movimentacao(
            data,
            valor,
            natureza,
            tipo,
            descricao,
            contaId,
            categoriaId,
            faturaId);
    }

    public void Alterar(
        DateOnly data,
        Dinheiro valor,
        Natureza natureza,
        TipoMovimentacao tipo,
        Descricao? descricao,
        int contaId,
        int categoriaId)
    {
        if (FaturaId.HasValue)
            throw new RegraDeNegocioException("Não é possível alterar uma movimentação associada a uma fatura.");

        Data = data;
        Valor = valor;
        Natureza = natureza;
        Tipo = tipo;
        Descricao = descricao;
        ContaId = contaId;
        CategoriaId = categoriaId;
    }
}
