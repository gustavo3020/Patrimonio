using FluentAssertions;
using Patrimonio.Financas.Domain.Common.Entities;

namespace Patrimonio.Financas.Tests.Unit.Common.Domain.Entities;

public sealed class EntidadeBaseTests
{
    [Fact]
    public void Inicializacao_DeveInicializarDataCriacao_QuandoCriada()
    {
        var antes = DateTimeOffset.UtcNow;

        var entidade = new EntidadeTeste();

        var depois = DateTimeOffset.UtcNow;

        entidade.DataCriacao
            .Should()
            .BeOnOrAfter(antes)
            .And.BeOnOrBefore(depois);
    }

    [Fact]
    public void Inicializacao_DeveInicializarDataAlteracao_QuandoCriada()
    {
        var antes = DateTimeOffset.UtcNow;

        var entidade = new EntidadeTeste();

        var depois = DateTimeOffset.UtcNow;

        entidade.DataAlteracao
            .Should()
            .BeOnOrAfter(antes)
            .And.BeOnOrBefore(depois);
    }

    [Fact]
    public void Atualizar_DeveAtualizarDataAlteracao_QuandoAtualizado()
    {
        var entidade = new EntidadeTeste();

        var antes = DateTimeOffset.UtcNow;

        entidade.Atualizar();

        var depois = DateTimeOffset.UtcNow;

        entidade.DataAlteracao
            .Should()
            .BeOnOrAfter(antes)
            .And.BeOnOrBefore(depois);
    }

    private sealed class EntidadeTeste : EntidadeBase
    {
        public void Atualizar()
        {
            RegistrarAlteracao();
        }
    }
}
