using Patrimonio.Financas.Contracts.Cartoes.Dtos;
using Patrimonio.Financas.Contracts.Cartoes.Services;
using Patrimonio.Financas.Domain.Exceptions;
using Patrimonio.Financas.SharedKernel.Cartoes.Enums;
using Patrimonio.Financas.Tests.Integration.Base;

namespace Patrimonio.Financas.Tests.Integration.Seeders;

/// <summary>
/// Fornece os dados base de cartões necessários aos testes de integração.
/// </summary>
internal sealed class CartaoSeeder(
    ICartaoCommandService commandService,
    ICartaoQueryService queryService)
{
    public async Task SeedAsync(BaseData data)
    {
        CartaoDetalheDto cartao;

        try
        {
            cartao = await queryService.ObterPorIdAsync(1, TestContext.Current.CancellationToken);
        }
        catch (RecursoNaoEncontradoException)
        {
            var cartaoDto = Criar(data.Instituicao.Id);

            cartao = await commandService.CriarAsync(cartaoDto, TestContext.Current.CancellationToken);
        }

        data.Cartao = cartao;
    }

    private static CartaoCriacaoDto Criar(int instituicaoId)
    {
        return new CartaoCriacaoDto
        {
            Nome = "Cartão de teste",
            Bandeira = BandeiraCartao.Mastercard,
            Limite = 5000.00m,
            DiaFechamento = 11,
            DiaVencimento = 15,
            InstituicaoId = instituicaoId
        };
    }
}
