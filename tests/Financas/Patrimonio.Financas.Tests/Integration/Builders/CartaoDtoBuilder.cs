using Patrimonio.Financas.Contracts.Cartoes.Dtos;
using Patrimonio.Financas.SharedKernel.Cartoes.Enums;

namespace Patrimonio.Financas.Tests.Integration.Builders;

internal static class CartaoDtoBuilder
{
    public static CartaoCriacaoDto Criar(int instituicaoId, string? nome = null)
    {
        return new CartaoCriacaoDto
        {
            Nome = nome ?? $"Cartão {Guid.NewGuid()}",
            Bandeira = BandeiraCartao.Mastercard,
            Limite = 3000,
            DiaFechamento = 25,
            DiaVencimento = 30,
            InstituicaoId = instituicaoId
        };
    }

    public static CartaoAlteracaoDto Alterar(string? nome = null)
    {
        return new CartaoAlteracaoDto
        {
            Nome = nome ?? $"Cartão alterado {Guid.NewGuid()}",
            Bandeira = BandeiraCartao.Visa,
            Limite = 1000,
            DiaFechamento = 10,
            DiaVencimento = 20
        };
    }
}
