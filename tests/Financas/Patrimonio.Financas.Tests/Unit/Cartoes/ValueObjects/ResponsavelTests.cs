using FluentAssertions;
using Patrimonio.Financas.Domain.Cartoes.ValueObjects;

namespace Patrimonio.Financas.Tests.Unit.Cartoes.ValueObjects;

public sealed class ResponsavelTests
{
    [Fact]
    public void DeveCriarResponsavelValido()
    {
        var nome = new Responsavel("Responsável Teste");

        nome.Valor.Should().Be("Responsável Teste");
    }

    [Fact]
    public void DoisResponsavelsComMesmoValorDevemSerIguais()
    {
        var a = new Responsavel("Responsável Teste");
        var b = new Responsavel("Responsável Teste");

        a.Should().Be(b);
        a.GetHashCode().Should().Be(b.GetHashCode());
    }

    [Fact]
    public void DoisResponsavelsComValoresDiferentesNaoDevemSerIguais()
    {
        var a = new Responsavel("Responsável Teste");
        var b = new Responsavel("Outro Responsável");

        a.Should().NotBe(b);
    }

    [Fact]
    public void ToStringDeveRetornarValorComoTexto()
    {
        var nome = new Responsavel("Responsável Teste");

        nome.ToString().Should().Be("Responsável Teste");
    }
}
