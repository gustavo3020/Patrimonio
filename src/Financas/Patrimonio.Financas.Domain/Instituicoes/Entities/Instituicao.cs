using Patrimonio.Financas.Domain.Common.Entities;
using Patrimonio.Financas.Domain.Common.ValueObjects;

namespace Patrimonio.Financas.Domain.Instituicoes.Entities;

/// <summary>
/// Representa uma instituição financeira utilizada no controle das contas.
/// </summary>
public sealed class Instituicao : EntidadeBase
{
    // Dados próprios.
    public Nome Nome { get; private set; }

    private Instituicao(Nome nome)
    {
        Nome = nome;
    }

    /// <summary>
    /// Cria uma nova instituição.
    /// </summary>
    /// <param name="nome">Nome da instituição.</param>
    /// <returns>Nova instituição criada.</returns>
    public static Instituicao Criar(Nome nome)
    {
        return new Instituicao(nome);
    }

    /// <summary>
    /// Altera o nome da instituição.
    /// </summary>
    /// <param name="nome">Novo nome da instituição.</param>
    public void AlterarNome(Nome nome)
    {
        Nome = nome;
    }
}
