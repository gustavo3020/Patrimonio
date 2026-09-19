using FluentAssertions;
using Patrimonio.Financas.Domain.Common.ValueObjects;

namespace Patrimonio.Financas.Tests.Unit.Common.Domain.ValueObjects;

public sealed class DescricaoTests
{
    [Fact]
    public void DeveCriarDescricaoValida()
    {
        var descricao = new Descricao("Descrição Teste");

        descricao.Valor.Should().Be("Descrição Teste");
    }

    [Fact]
    public void DoisDescricaosComMesmoValorDevemSerIguais()
    {
        var a = new Descricao("Descrição Teste");
        var b = new Descricao("Descrição Teste");

        a.Should().Be(b);
        a.GetHashCode().Should().Be(b.GetHashCode());
    }

    [Fact]
    public void DoisDescricaosComValoresDiferentesNaoDevemSerIguais()
    {
        var a = new Descricao("Descrição Teste");
        var b = new Descricao("Outra Descrição");

        a.Should().NotBe(b);
    }

    [Fact]
    public void ToStringDeveRetornarValorComoTexto()
    {
        var nome = new Descricao("Descrição Teste");

        nome.ToString().Should().Be("Descrição Teste");
    }
}
