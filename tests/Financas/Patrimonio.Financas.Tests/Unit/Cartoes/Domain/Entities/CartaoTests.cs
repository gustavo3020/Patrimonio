using FluentAssertions;
using Patrimonio.Financas.Domain.Cartoes.Entities;
using Patrimonio.Financas.Domain.Common.ValueObjects;
using Patrimonio.Financas.SharedKernel.Cartoes.Enums;

namespace Patrimonio.Financas.Tests.Unit.Cartoes.Domain.Entities;

public sealed class CartaoTests
{
    [Fact]
    public void DeveCriarCartao()
    {
        // Arrange
        var nome = new Nome("Cartão Teste");
        var bandeira = BandeiraCartao.Mastercard;
        var limite = new Dinheiro(5000);
        var diaFechamento = new DiaDoMes(15);
        var diaVencimento = new DiaDoMes(5);
        int instituicaoId = 1;

        // Act
        var cartao = Cartao.Criar(nome, bandeira, limite, diaFechamento, diaVencimento, instituicaoId);

        // Assert
        cartao.Nome.Should().Be(nome);
        cartao.Bandeira.Should().Be(bandeira);
        cartao.Limite.Should().Be(limite);
        cartao.DiaFechamento.Should().Be(diaFechamento);
        cartao.DiaVencimento.Should().Be(diaVencimento);
        cartao.InstituicaoId.Should().Be(instituicaoId);
    }

    [Fact]
    public void DeveAlterarCampos()
    {
        // Arrange
        var nome = new Nome("Cartão Teste");
        var bandeira = BandeiraCartao.Mastercard;
        var limite = new Dinheiro(5000);
        var diaFechamento = new DiaDoMes(15);
        var diaVencimento = new DiaDoMes(5);
        int instituicaoId = 1;
        var cartao = Cartao.Criar(nome, bandeira, limite, diaFechamento, diaVencimento, instituicaoId);

        // Act
        cartao.Alterar(
            new Nome("Nome Alterado"),
            BandeiraCartao.Visa,
            new Dinheiro(10000),
            new DiaDoMes(20),
            new DiaDoMes(10));

        // Assert
        cartao.Nome.Should().Be(new Nome("Nome Alterado"));
        cartao.Bandeira.Should().Be(BandeiraCartao.Visa);
        cartao.Limite.Should().Be(new Dinheiro(10000));
        cartao.DiaFechamento.Should().Be(new DiaDoMes(20));
        cartao.DiaVencimento.Should().Be(new DiaDoMes(10));
        cartao.InstituicaoId.Should().Be(instituicaoId);
    }
}
