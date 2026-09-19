using Patrimonio.Financas.Domain.Cartoes.ValueObjects;
using Patrimonio.Financas.Domain.Common.Entities;
using Patrimonio.Financas.Domain.Common.ValueObjects;

namespace Patrimonio.Financas.Domain.Cartoes.Entities;

/// <summary>
/// Representa um lançamento financeiro associado a um cartão de crédito.
/// </summary>
public sealed class Lancamento : EntidadeBase
{
    private Lancamento(
        Descricao descricao,
        Dinheiro valor,
        DateOnly dataCompra,
        Estabelecimento estabelecimento,
        Responsavel responsavel,
        Parcelamento parcelamento,
        int faturaId,
        int categoriaId)
    {
        Descricao = descricao;
        Valor = valor;
        DataCompra = dataCompra;
        Estabelecimento = estabelecimento;
        Responsavel = responsavel;
        Parcelamento = parcelamento;
        FaturaId = faturaId;
        CategoriaId = categoriaId;
    }

    private Lancamento()
    {
    }

    // Dados próprios.
    public Descricao Descricao { get; private set; }
    public Dinheiro Valor { get; private set; }
    public DateOnly DataCompra { get; private set; }
    public Estabelecimento Estabelecimento { get; private set; }
    public Responsavel Responsavel { get; private set; }
    public Parcelamento Parcelamento { get; private set; }

    // Relacionamentos.
    public int FaturaId { get; private set; }
    public int CategoriaId { get; private set; }

    /// <summary>
    /// Cria uma nova instância de Lancamento com os dados fornecidos.
    /// </summary>
    /// <param name="descricao">Descrição do lançamento.</param>
    /// <param name="valor">Valor do lançamento.</param>
    /// <param name="dataCompra">Data de compra do lançamento.</param>
    /// <param name="estabelecimento">Estabelecimento onde o lançamento foi realizado.</param>
    /// <param name="responsavel">Responsável pelo lançamento.</param>
    /// <param name="parcelamento">Informações de parcelamento do lançamento.</param>
    /// <param name="faturaId">ID da fatura associada ao lançamento.</param>
    /// <param name="categoriaId">ID da categoria do lançamento.</param>
    /// <returns>A instância de Lancamento criada.</returns>
    public static Lancamento Criar(
        Descricao descricao,
        Dinheiro valor,
        DateOnly dataCompra,
        Estabelecimento estabelecimento,
        Responsavel responsavel,
        Parcelamento parcelamento,
        int faturaId,
        int categoriaId)
    {
        return new Lancamento(
            descricao,
            valor,
            dataCompra,
            estabelecimento,
            responsavel,
            parcelamento,
            faturaId,
            categoriaId);
    }

    /// <summary>
    /// Altera os dados do lançamento financeiro.
    /// </summary>
    /// <param name="descricao">Descrição do lançamento.</param>
    /// <param name="valor">Valor do lançamento.</param>
    /// <param name="dataCompra">Data de compra do lançamento.</param>
    /// <param name="estabelecimento">Estabelecimento onde o lançamento foi realizado.</param>
    /// <param name="responsavel">Responsável pelo lançamento.</param>
    /// <param name="categoriaId">ID da categoria do lançamento.</param>
    public void Alterar(
        Descricao descricao,
        Dinheiro valor,
        DateOnly dataCompra,
        Estabelecimento estabelecimento,
        Responsavel responsavel,
        int categoriaId)
    {
        Descricao = descricao;
        Valor = valor;
        DataCompra = dataCompra;
        Estabelecimento = estabelecimento;
        Responsavel = responsavel;
        CategoriaId = categoriaId;

        RegistrarAlteracao();
    }
}
