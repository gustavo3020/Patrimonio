using FluentAssertions;
using Patrimonio.Financas.Domain.Cartoes.ValueObjects;
using Patrimonio.Financas.Domain.Exceptions;

namespace Patrimonio.Financas.Tests.Unit.Cartoes.Domain.ValueObjects;

public sealed class ParcelamentoTests
{
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public void DeveCriarParcelamentoComNumeroValido(int numeroParcela)
    {
        // Arrange
        var grupoId = Guid.NewGuid();
        var totalParcelas = 3;

        // Act
        var parcelamento = new Parcelamento(grupoId, numeroParcela, totalParcelas);

        // Assert
        parcelamento.GrupoId.Should().Be(grupoId);
        parcelamento.NumeroParcela.Should().Be(numeroParcela);
        parcelamento.TotalParcelas.Should().Be(totalParcelas);
    }

    [Fact]
    public void DeveRejeitarGrupoIdVazio()
    {
        // Arrange
        var grupoId = Guid.Empty;
        var numeroParcela = 1;
        var totalParcelas = 3;

        // Act
        var acao = () => new Parcelamento(grupoId, numeroParcela, totalParcelas);

        // Assert
        acao.Should().Throw<RegraDeNegocioException>()
            .WithMessage("O ID do grupo de parcelamento não pode ser vazio.");
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    public void DeveRejeitarNumeroParcelaInvalido(int numeroParcela)
    {
        // Arrange
        var grupoId = Guid.NewGuid();
        var totalParcelas = 3;

        // Act
        var acao = () => new Parcelamento(grupoId, numeroParcela, totalParcelas);

        // Assert
        acao.Should().Throw<RegraDeNegocioException>()
            .WithMessage("O número da parcela deve ser maior que zero.");
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    public void DeveRejeitarTotalParcelasInvalido(int totalParcelas)
    {
        // Arrange
        var grupoId = Guid.NewGuid();
        var numeroParcela = 1;

        // Act
        var acao = () => new Parcelamento(grupoId, numeroParcela, totalParcelas);

        // Assert
        acao.Should().Throw<RegraDeNegocioException>()
            .WithMessage("O número total de parcelas deve ser maior que zero.");
    }

    [Fact]
    public void DeveRejeitarNumeroParcelaMaiorQueTotalParcelas()
    {
        // Arrange
        var grupoId = Guid.NewGuid();
        var numeroParcela = 4;
        var totalParcelas = 3;

        // Act
        var acao = () => new Parcelamento(grupoId, numeroParcela, totalParcelas);

        // Assert
        acao.Should().Throw<RegraDeNegocioException>();
    }
}
