using FluentAssertions;
using Patrimonio.Financas.Domain.Cartoes.Entities;
using Patrimonio.Financas.Domain.Exceptions;
using Patrimonio.Financas.SharedKernel.Cartoes.Enums;

namespace Patrimonio.Financas.Tests.Unit.Cartoes.Domain.Entities;

public sealed class FaturaTests
{
    private readonly DateOnly _dataFechamento = new(2026, 8, 11);
    private readonly DateOnly _dataVencimento = new(2026, 8, 15);

    private Fatura CriarFatura()
    {
        return Fatura.Criar(_dataFechamento, _dataVencimento, 1);
    }

    [Fact]
    public void Criar_DeveCriarFatura_QuandoDadosValidos()
    {
        // Act
        var fatura = CriarFatura();

        // Assert
        fatura.DataFechamento.Should().Be(_dataFechamento);
        fatura.DataVencimento.Should().Be(_dataVencimento);
        fatura.CartaoId.Should().Be(1);
        fatura.Status.Should().Be(StatusFatura.Aberta);
        fatura.DataPagamento.Should().BeNull();
    }

    [Fact]
    public void Alterar_DeveAlterarFatura_QuandoDadosValidos()
    {
        // Arrange
        var fatura = CriarFatura();
        var novaDataFechamento = new DateOnly(2026, 8, 12);
        var novaDataVencimento = new DateOnly(2026, 8, 16);

        // Act
        fatura.Alterar(novaDataFechamento, novaDataVencimento);

        // Assert
        fatura.DataFechamento.Should().Be(novaDataFechamento);
        fatura.DataVencimento.Should().Be(novaDataVencimento);
    }

    [Fact]
    public void Fechar_DeveFecharFatura_QuandoDadosValidos()
    {
        // Arrange
        var fatura = CriarFatura();

        // Act
        fatura.Fechar();

        // Assert
        fatura.Status.Should().Be(StatusFatura.Fechada);
    }

    [Fact]
    public void Pagar_DevePagarFatura_QuandoDadosValidos()
    {
        // Arrange
        var fatura = CriarFatura();
        var dataPagamento = new DateOnly(2026, 8, 14);
        fatura.Fechar();

        // Act
        fatura.Pagar(dataPagamento);

        // Assert
        fatura.Status.Should().Be(StatusFatura.Paga);
        fatura.DataPagamento.Should().NotBeNull();
        fatura.DataPagamento.Should().Be(dataPagamento);
    }

    [Fact]
    public void Criar_DeveLancarExcecao_QuandoDataFechamentoPosteriorADataVencimento()
    {
        // Arrange
        var dataFechamento = new DateOnly(2026, 8, 16);
        var dataVencimento = new DateOnly(2026, 8, 15);
        int cartaoId = 1;

        // Act
        var acao = () => Fatura.Criar(dataFechamento, dataVencimento, cartaoId);

        // Assert
        acao.Should().Throw<RegraDeNegocioException>();
    }

    [Fact]
    public void Alterar_DeveLancarExcecao_QuandoDataFechamentoPosteriorADataVencimento()
    {
        // Arrange
        var fatura = CriarFatura();
        var novaDataFechamento = new DateOnly(2026, 8, 16);
        var novaDataVencimento = new DateOnly(2026, 8, 15);

        // Act
        var acao = () => fatura.Alterar(novaDataFechamento, novaDataVencimento);

        // Assert
        acao.Should().Throw<RegraDeNegocioException>();
        fatura.DataFechamento.Should().Be(_dataFechamento);
        fatura.DataVencimento.Should().Be(_dataVencimento);
    }

    [Fact]
    public void Alterar_DeveLancarExcecao_QuandoFaturaFechada()
    {
        // Arrange
        var fatura = CriarFatura();
        fatura.Fechar();

        // Act
        var acao = () => fatura.Alterar(new DateOnly(2026, 8, 12), new DateOnly(2026, 8, 16));

        // Assert
        acao.Should().Throw<RegraDeNegocioException>();
        fatura.DataFechamento.Should().Be(_dataFechamento);
        fatura.DataVencimento.Should().Be(_dataVencimento);
    }

    [Fact]
    public void Excluir_DeveLancarExcecao_QuandoFaturaFechada()
    {
        // Arrange
        var fatura = CriarFatura();
        fatura.Fechar();

        // Act
        var acao = () => fatura.Excluir();

        // Assert
        acao.Should().Throw<RegraDeNegocioException>();
    }

    [Fact]
    public void Excluir_DeveLancarExcecao_QuandoFaturaPaga()
    {
        // Arrange
        var fatura = CriarFatura();
        fatura.Fechar();
        fatura.Pagar(new DateOnly(2026, 8, 14));

        // Act
        var acao = () => fatura.Excluir();

        // Assert
        acao.Should().Throw<RegraDeNegocioException>();
    }

    [Fact]
    public void Fechar_DeveLancarExcecao_QuandoFechada()
    {
        // Arrange
        var fatura = CriarFatura();
        fatura.Fechar();

        // Act
        var acao = () => fatura.Fechar();

        // Assert
        acao.Should().Throw<RegraDeNegocioException>();
    }

    [Fact]
    public void Fechar_DeveLancarExcecao_QuandoPaga()
    {
        // Arrange
        var fatura = CriarFatura();
        fatura.Fechar();
        fatura.Pagar(new DateOnly(2026, 8, 14));

        // Act
        var acao = () => fatura.Fechar();

        // Assert
        acao.Should().Throw<RegraDeNegocioException>();
        fatura.DataPagamento.Should().NotBeNull();
        fatura.Status.Should().Be(StatusFatura.Paga);
    }

    [Fact]
    public void Pagar_DeveLancarExcecao_QuandoAberta()
    {
        // Arrange
        var fatura = CriarFatura();
        var dataPagamento = new DateOnly(2026, 8, 14);

        // Act
        var acao = () => fatura.Pagar(dataPagamento);

        // Assert
        acao.Should().Throw<RegraDeNegocioException>();
        fatura.DataPagamento.Should().BeNull();
        fatura.Status.Should().Be(StatusFatura.Aberta);
    }

    [Fact]
    public void Pagar_DeveLancarExcecao_QuandoPaga()
    {
        // Arrange
        var fatura = CriarFatura();
        var dataPagamento = new DateOnly(2026, 8, 14);
        fatura.Fechar();
        fatura.Pagar(dataPagamento);

        // Act
        var acao = () => fatura.Pagar(dataPagamento);

        // Assert
        acao.Should().Throw<RegraDeNegocioException>();
        fatura.DataPagamento.Should().NotBeNull();
        fatura.Status.Should().Be(StatusFatura.Paga);
    }

    [Fact]
    public void Pagar_DeveLancarExcecao_QuandoDataPagamentoAnteriorADataFechamento()
    {
        // Arrange
        var fatura = CriarFatura();
        var dataPagamento = new DateOnly(2026, 8, 10);
        fatura.Fechar();

        // Act
        var acao = () => fatura.Pagar(dataPagamento);

        // Assert
        acao.Should().Throw<RegraDeNegocioException>();
        fatura.DataPagamento.Should().BeNull();
        fatura.Status.Should().Be(StatusFatura.Fechada);
    }
}
