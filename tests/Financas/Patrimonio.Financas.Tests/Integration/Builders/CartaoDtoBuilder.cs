using Patrimonio.Financas.Contracts.Cartoes.Dtos;
using Patrimonio.Financas.SharedKernel.Cartoes.Enums;

namespace Patrimonio.Financas.Tests.Integration.Builders;

internal static class CartaoDtoBuilder
{
    public static CartaoCriacaoDto Criar(int instituicaoId)
    {
        return new CartaoCriacaoDto
        {
            Nome = $"Cartão {Guid.NewGuid()}",
            Bandeira = BandeiraCartao.Mastercard,
            Limite = 3000,
            DiaFechamento = 25,
            DiaVencimento = 30,
            InstituicaoId = instituicaoId
        };
    }

    public static CartaoAlteracaoDto Alterar()
    {
        return new CartaoAlteracaoDto
        {
            Nome = $"Cartão alterado {Guid.NewGuid()}",
            Bandeira = BandeiraCartao.Visa,
            Limite = 1000,
            DiaFechamento = 10,
            DiaVencimento = 20
        };
    }
}
