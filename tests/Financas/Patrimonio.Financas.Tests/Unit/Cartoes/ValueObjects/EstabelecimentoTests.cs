using FluentAssertions;
using Patrimonio.Financas.Domain.Cartoes.ValueObjects;

namespace Patrimonio.Financas.Tests.Unit.Cartoes.ValueObjects;

public sealed class EstabelecimentoTests
{
    [Fact]
    public void DeveCriarEstabelecimentoValido()
    {
        var nome = new Estabelecimento("Estabelecimento Teste");

        nome.Valor.Should().Be("Estabelecimento Teste");
    }

    [Fact]
    public void DoisEstabelecimentosComMesmoValorDevemSerIguais()
    {
        var a = new Estabelecimento("Estabelecimento Teste");
        var b = new Estabelecimento("Estabelecimento Teste");

        a.Should().Be(b);
        a.GetHashCode().Should().Be(b.GetHashCode());
    }

    [Fact]
    public void DoisEstabelecimentosComValoresDiferentesNaoDevemSerIguais()
    {
        var a = new Estabelecimento("Estabelecimento Teste");
        var b = new Estabelecimento("Outro Estabelecimento");

        a.Should().NotBe(b);
    }

    [Fact]
    public void ToStringDeveRetornarValorComoTexto()
    {
        var nome = new Estabelecimento("Estabelecimento Teste");

        nome.ToString().Should().Be("Estabelecimento Teste");
    }
}
