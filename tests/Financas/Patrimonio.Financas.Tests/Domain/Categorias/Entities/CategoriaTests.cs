using FluentAssertions;
using Patrimonio.Financas.Domain.Categorias.Entities;
using Patrimonio.Financas.Domain.Common.ValueObjects;

namespace Patrimonio.Financas.Tests.Domain.Categorias.Entities;

public sealed class CategoriaTests
{
    [Fact]
    public void DeveCriarCategoria()
    {
        var nome = new Nome("Salário");

        var categoria = Categoria.Criar(nome);

        categoria.Nome.Valor.Should().Be("Salário");
    }

    [Fact]
    public void DeveAlterarNome()
    {
        var nome = new Nome("Alimentação");
        var categoria = Categoria.Criar(nome);

        var novoNome = new Nome("Mercado");
        categoria.AlterarNome(novoNome);

        categoria.Nome.Valor.Should().Be("Mercado");
    }
}
