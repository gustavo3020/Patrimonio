using Patrimonio.Financas.Domain.Common.Entities;
using Patrimonio.Financas.Domain.Common.ValueObjects;

namespace Patrimonio.Financas.Domain.Categorias.Entities;

/// <summary>
/// Representa uma categoria utilizada para classificar movimentações financeiras.
/// </summary>
public sealed class Categoria : EntidadeBase
{
    public Nome Nome { get; private set; }

    private Categoria(Nome nome)
    {
        Nome = nome;
    }

    /// <summary>
    /// Cria uma nova categoria.
    /// </summary>
    /// <param name="nome">Nome da categoria.</param>
    /// <returns>Nova categoria criada.</returns>
    public static Categoria Criar(Nome nome)
    {
        return new Categoria(nome);
    }

    /// <summary>
    /// Altera o nome da categoria.
    /// </summary>
    /// <param name="nome">Novo nome da categoria.</param>
    public void AlterarNome(Nome nome)
    {
        Nome = nome;
    }
}
