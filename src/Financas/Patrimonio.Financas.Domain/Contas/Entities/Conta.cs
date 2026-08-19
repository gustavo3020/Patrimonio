using Patrimonio.Financas.Domain.Common.Entities;
using Patrimonio.Financas.Domain.Common.ValueObjects;

namespace Patrimonio.Financas.Domain.Contas.Entities;

/// <summary>
/// Representa uma conta utilizada para registrar movimentações financeiras.
/// </summary>
public sealed class Conta : EntidadeBase
{
    // Dados próprios.
    public Nome Nome { get; private set; }

    // Relacionamentos.
    public int InstituicaoId { get; private set; }

    private Conta(Nome nome, int instituicaoId)
    {
        Nome = nome;
        InstituicaoId = instituicaoId;
    }

    /// <summary>
    /// Cria uma nova conta.
    /// </summary>
    /// <param name="nome">Nome da conta.</param>
    /// <param name="instituicaoId">Identificador da instituição financeira.</param>
    /// <returns>Nova conta criada.</returns>
    public static Conta Criar(Nome nome, int instituicaoId)
    {
        return new Conta(nome, instituicaoId);
    }

    /// <summary>
    /// Altera o nome da conta.
    /// </summary>
    /// <param name="nome">Novo nome da conta.</param>
    public void AlterarNome(Nome nome)
    {
        Nome = nome;
        RegistrarAtualizacao();
    }

    /// <summary>
    /// Altera a instituição financeira associada à conta.
    /// </summary>
    /// <param name="instituicaoId">Novo identificador da instituição.</param>
    public void AlterarInstituicao(int instituicaoId)
    {
        InstituicaoId = instituicaoId;
        RegistrarAtualizacao();
    }
}
