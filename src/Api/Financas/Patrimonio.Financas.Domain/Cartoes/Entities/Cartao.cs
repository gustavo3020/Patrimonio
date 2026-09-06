using Patrimonio.Financas.Domain.Common.Entities;
using Patrimonio.Financas.Domain.Common.ValueObjects;
using Patrimonio.Financas.SharedKernel.Cartoes.Enums;

namespace Patrimonio.Financas.Domain.Cartoes.Entities;

/// <summary>
/// Representa um cartão de crédito utilizado para registrar movimentações financeiras.
/// </summary>
public sealed class Cartao : EntidadeBase
{
    private Cartao(
        Nome nome,
        BandeiraCartao bandeira,
        Dinheiro limite,
        DiaDoMes diaFechamento,
        DiaDoMes diaVencimento,
        int instituicaoId)
    {
        Nome = nome;
        Bandeira = bandeira;
        Limite = limite;
        DiaFechamento = diaFechamento;
        DiaVencimento = diaVencimento;
        InstituicaoId = instituicaoId;
    }

    private Cartao()
    {
    }

    // Dados próprios.
    public Nome Nome { get; private set; }
    public BandeiraCartao Bandeira { get; private set; }
    public Dinheiro Limite { get; private set; }
    public DiaDoMes DiaFechamento { get; private set; }
    public DiaDoMes DiaVencimento { get; private set; }

    // Relacionamentos.
    public int InstituicaoId { get; private set; }

    /// <summary>
    /// Cria uma nova instância de Cartão com os dados fornecidos.
    /// </summary>
    /// <param name="nome">Nome do cartão.</param>
    /// <param name="bandeira">Bandeira do cartão.</param>
    /// <param name="limite">Limite do cartão.</param>
    /// <param name="diaFechamento">Dia de fechamento do cartão.</param>
    /// <param name="diaVencimento">Dia de vencimento do cartão.</param>
    /// <param name="instituicaoId">ID da instituição financeira.</param>
    /// <returns>A instância de Cartão criada.</returns>
    public static Cartao Criar(
        Nome nome,
        BandeiraCartao bandeira,
        Dinheiro limite,
        DiaDoMes diaFechamento,
        DiaDoMes diaVencimento,
        int instituicaoId)
    {
        return new Cartao(nome, bandeira, limite, diaFechamento, diaVencimento, instituicaoId);
    }

    /// <summary>
    /// Altera os dados do cartão com os novos valores fornecidos.
    /// </summary>
    /// <param name="nome">Nome do cartão.</param>
    /// <param name="bandeira">Bandeira do cartão.</param>
    /// <param name="limite">Limite do cartão.</param>
    /// <param name="diaFechamento">Dia de fechamento do cartão.</param>
    /// <param name="diaVencimento">Dia de vencimento do cartão.</param>
    public void Alterar(
        Nome nome,
        BandeiraCartao bandeira,
        Dinheiro limite,
        DiaDoMes diaFechamento,
        DiaDoMes diaVencimento)
    {
        Nome = nome;
        Bandeira = bandeira;
        Limite = limite;
        DiaFechamento = diaFechamento;
        DiaVencimento = diaVencimento;

        RegistrarAlteracao();
    }
}
