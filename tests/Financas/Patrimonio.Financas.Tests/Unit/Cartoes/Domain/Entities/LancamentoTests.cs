using FluentAssertions;
using Patrimonio.Financas.Domain.Cartoes.Entities;
using Patrimonio.Financas.Domain.Cartoes.ValueObjects;
using Patrimonio.Financas.Domain.Common.ValueObjects;

namespace Patrimonio.Financas.Tests.Unit.Cartoes.Domain.Entities;

public sealed class LancamentoTests
{
    private static Lancamento CriarLancamento()
    {
        return Lancamento.Criar(
            new Descricao("Descricao Teste"),
            new Dinheiro(100.00m),
            new DateOnly(2026, 8, 10),
            new Estabelecimento("Estabelecimento Teste"),
            new Responsavel("Responsavel Teste"),
            new Parcelamento(Guid.NewGuid(), 1, 3),
            1,
            1);
    }

    [Fact]
    public void DeveCriarLancamento()
    {
        // Arrange
        var descricao = new Descricao("Lancamento Teste");
        var valor = new Dinheiro(150.75m);
        var dataCompra = new DateOnly(2026, 8, 15);
        var estabelecimento = new Estabelecimento("Estabelecimento Teste");
        var responsavel = new Responsavel("Responsavel Teste");
        var parcelamento = new Parcelamento(Guid.NewGuid(), 1, 5);
        var faturaId = 1;
        var categoriaId = 1;

        // Act
        var lancamento = Lancamento.Criar(
            descricao,
            valor,
            dataCompra,
            estabelecimento,
            responsavel,
            parcelamento,
            faturaId,
            categoriaId);

        // Assert
        lancamento.Descricao.Should().Be(descricao);
        lancamento.DataCompra.Should().Be(dataCompra);
        lancamento.Valor.Should().Be(valor);
        lancamento.Estabelecimento.Should().Be(estabelecimento);
        lancamento.Responsavel.Should().Be(responsavel);
        lancamento.Parcelamento.Should().Be(parcelamento);
        lancamento.FaturaId.Should().Be(faturaId);
        lancamento.CategoriaId.Should().Be(categoriaId);
    }

    [Fact]
    public void DeveAlterarLancamento()
    {
        // Arrange
        var lancamento = CriarLancamento();

        // Act
        lancamento.Alterar(
            new Descricao("Descricao Alterada"),
            new Dinheiro(200.00m),
            new DateOnly(2026, 8, 20),
            new Estabelecimento("Estabelecimento Alterado"),
            new Responsavel("Responsavel Alterado"),
            2);

        // Assert
        lancamento.Descricao.Should().Be(new Descricao("Descricao Alterada"));
        lancamento.Valor.Should().Be(new Dinheiro(200.00m));
        lancamento.DataCompra.Should().Be(new DateOnly(2026, 8, 20));
        lancamento.Estabelecimento.Should().Be(new Estabelecimento("Estabelecimento Alterado"));
        lancamento.Responsavel.Should().Be(new Responsavel("Responsavel Alterado"));
        lancamento.CategoriaId.Should().Be(2);
    }
}
