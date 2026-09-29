using FluentAssertions;
using Patrimonio.Financas.Domain.Cartoes.ValueObjects;

namespace Patrimonio.Financas.Tests.Unit.Cartoes.Domain.ValueObjects;

public sealed class EstabelecimentoTests
{
    [Fact]
    public void Criar_DeveCriarEstabelecimento_QuandoDadosValidos()
    {
        var nome = new Estabelecimento("Estabelecimento Teste");

        nome.Valor.Should().Be("Estabelecimento Teste");
    }

    [Fact]
    public void Igualdade_DeveConsiderarIgual_QuandoMesmoValor()
    {
        var a = new Estabelecimento("Estabelecimento Teste");
        var b = new Estabelecimento("Estabelecimento Teste");

        a.Should().Be(b);
        a.GetHashCode().Should().Be(b.GetHashCode());
    }

    [Fact]
    public void Igualdade_DeveConsiderarDiferente_QuandoValoresDiferentes()
    {
        var a = new Estabelecimento("Estabelecimento Teste");
        var b = new Estabelecimento("Outro Estabelecimento");

        a.Should().NotBe(b);
    }

    [Fact]
    public void ToString_DeveRetornarValorComoTexto_QuandoChamado()
    {
        var nome = new Estabelecimento("Estabelecimento Teste");

        nome.ToString().Should().Be("Estabelecimento Teste");
    }
}
