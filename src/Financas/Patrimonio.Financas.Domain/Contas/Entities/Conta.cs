using Patrimonio.Financas.Domain.Common.Entities;
using Patrimonio.Financas.Domain.Common.ValueObjects;

namespace Patrimonio.Financas.Domain.Contas.Entities;

/// <summary>
/// Representa uma conta utilizada para registrar movimentações financeiras.
/// </summary>
public sealed class Conta : EntidadeBase
{
    public int InstituicaoId { get; private set; }
    public Nome Nome { get; private set; }

    private Conta(int instituicaoId, Nome nome)
    {
        InstituicaoId = instituicaoId;
        Nome = nome;
    }

    /// <summary>
    /// Cria uma nova conta.
    /// </summary>
    /// <param name="instituicaoId">Identificador da instituição financeira.</param>
    /// <param name="nome">Nome da conta.</param>
    /// <returns>Nova conta criada.</returns>
    public static Conta Criar(int instituicaoId, Nome nome)
    {
        return new Conta(instituicaoId, nome);
    }

    /// <summary>
    /// Altera o nome da conta.
    /// </summary>
    /// <param name="nome">Novo nome da conta.</param>
    public void AlterarNome(Nome nome)
    {
        Nome = nome;
    }

    /// <summary>
    /// Altera a instituição financeira associada à conta.
    /// </summary>
    /// <param name="instituicaoId">Novo identificador da instituição.</param>
    public void AlterarInstituicao(int instituicaoId)
    {
        InstituicaoId = instituicaoId;
    }
}
