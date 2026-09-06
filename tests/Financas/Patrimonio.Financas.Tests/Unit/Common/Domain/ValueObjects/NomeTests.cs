using FluentAssertions;
using Patrimonio.Financas.Domain.Common.ValueObjects;

namespace Patrimonio.Financas.Tests.Unit.Common.Domain.ValueObjects;

public sealed class NomeTests
{
    [Fact]
    public void DeveCriarNomeValido()
    {
        var nome = new Nome("Nome Teste");

        nome.Valor.Should().Be("Nome Teste");
        nome.Normalizado.Should().Be("NOME TESTE");
    }

    [Fact]
    public void DoisNomesComMesmoValorDevemSerIguais()
    {
        var a = new Nome("Nome Teste");
        var b = new Nome("Nome Teste");

        a.Should().Be(b);
        a.GetHashCode().Should().Be(b.GetHashCode());
    }

    [Fact]
    public void DoisNomesComValoresDiferentesNaoDevemSerIguais()
    {
        var a = new Nome("Nome Teste");
        var b = new Nome("Outro Nome");

        a.Should().NotBe(b);
    }

    [Fact]
    public void ToStringDeveRetornarValorComoTexto()
    {
        var nome = new Nome("Nome Teste");

        nome.ToString().Should().Be("Nome Teste");
    }
}
