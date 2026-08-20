using Patrimonio.Financas.Domain.Common.Entities;
using Patrimonio.Financas.Domain.Exceptions;
using Patrimonio.Financas.Domain.Movimentacoes.ValueObjects;
using Patrimonio.Financas.SharedKernel.Movimentacoes.Enums;

namespace Patrimonio.Financas.Domain.Movimentacoes.Entities;

/// <summary>
/// Representa uma movimentação financeira de entrada ou saída.
/// </summary>
public sealed class Movimentacao : EntidadeBase
{
    // Dados próprios.
    public DateOnly Data { get; private set; }
    public Dinheiro Valor { get; private set; }
    public Natureza Natureza { get; private set; }
    public TipoMovimentacao Tipo { get; private set; }
    public string? Descricao { get; private set; }

    // Relacionamentos.
    public int ContaId { get; private set; }
    public int CategoriaId { get; private set; }

    private Movimentacao(
        DateOnly data,
        Dinheiro valor,
        Natureza natureza,
        TipoMovimentacao tipo,
        string? descricao,
        int contaId,
        int categoriaId)
    {
        Data = data;
        Valor = valor;
        Natureza = natureza;
        Tipo = tipo;
        Descricao = descricao;
        ContaId = contaId;
        CategoriaId = categoriaId;
    }

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
    /// <returns>Nova movimentação criada.</returns>
    public static Movimentacao Criar(
        DateOnly data,
        Dinheiro valor,
        Natureza natureza,
        TipoMovimentacao tipo,
        string? descricao,
        int contaId,
        int categoriaId)
    {
        return new Movimentacao(
            data,
            valor,
            natureza,
            tipo,
            NormalizarDescricao(descricao),
            contaId,
            categoriaId);
    }

    /// <summary>
    /// Altera a data da movimentação.
    /// </summary>
    /// <param name="data">Nova data da movimentação.</param>
    public void AlterarData(DateOnly data)
    {
        Data = data;
        RegistrarAlteracao();
    }

    /// <summary>
    /// Altera o valor da movimentação.
    /// </summary>
    /// <param name="valor">Novo valor da movimentação.</param>
    public void AlterarValor(Dinheiro valor)
    {
        Valor = valor;
        RegistrarAlteracao();
    }

    /// <summary>
    /// Altera a natureza da movimentação.
    /// </summary>
    /// <param name="natureza">Nova natureza da movimentação.</param>
    public void AlterarNatureza(Natureza natureza)
    {
        Natureza = natureza;
        RegistrarAlteracao();
    }

    /// <summary>
    /// Altera o tipo da movimentação.
    /// </summary>
    /// <param name="tipo">Novo tipo da movimentação.</param>
    public void AlterarTipo(TipoMovimentacao tipo)
    {
        Tipo = tipo;
        RegistrarAlteracao();
    }

    /// <summary>
    /// Altera a descrição da movimentação.
    /// </summary>
    /// <param name="descricao">Nova descrição opcional da movimentação.</param>
    public void AlterarDescricao(string? descricao)
    {
        Descricao = NormalizarDescricao(descricao);
        RegistrarAlteracao();
    }

    /// <summary>
    /// Altera a conta relacionada à movimentação.
    /// </summary>
    /// <param name="contaId">Identificador da nova conta.</param>
    public void AlterarConta(int contaId)
    {
        ContaId = contaId;
        RegistrarAlteracao();
    }

    /// <summary>
    /// Altera a categoria relacionada à movimentação.
    /// </summary>
    /// <param name="categoriaId">Identificador da nova categoria.</param>
    public void AlterarCategoria(int categoriaId)
    {
        CategoriaId = categoriaId;
        RegistrarAlteracao();
    }

    /// <summary>
    /// Normaliza e valida uma descrição de movimentação.
    /// </summary>
    /// <param name="descricao">Descrição a ser normalizada.</param>
    /// <returns>A descrição normalizada ou <see langword="null"/> quando não informada.</returns>
    /// <exception cref="RegraDeNegocioException">
    /// Lançada quando a descrição ultrapassa o tamanho máximo permitido.
    /// </exception>
    private static string? NormalizarDescricao(string? descricao)
    {
        if (string.IsNullOrWhiteSpace(descricao))
            return null;

        descricao = descricao.Trim();

        if (descricao.Length > 500)
            throw new RegraDeNegocioException("A descrição deve possuir no máximo 500 caracteres.");

        return descricao;
    }
}
