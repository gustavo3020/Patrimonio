using Patrimonio.Financas.Contracts.Cartoes.Dtos;
using Patrimonio.Financas.Contracts.Cartoes.Services;
using Patrimonio.Financas.Domain.Exceptions;
using Patrimonio.Financas.Tests.Integration.Base;

namespace Patrimonio.Financas.Tests.Integration.Seeders;

/// <summary>
/// Fornece os dados base de lançamentos necessários aos testes de integração.
/// </summary>
internal sealed class LancamentoSeeder(
    ILancamentoCommandService commandService,
    ILancamentoQueryService queryService)
{
    public async Task SeedAsync(BaseData data)
    {
        LancamentoDetalheDto lancamento;

        try
        {
            lancamento = await queryService.ObterPorIdAsync(1, CancellationToken.None);
        }
        catch (RecursoNaoEncontradoException)
        {
            var lancamentoDto = Criar(data.Fatura.Id, data.Categoria.Id);

            lancamento = await commandService.CriarAsync(lancamentoDto, CancellationToken.None);
        }

        data.Lancamento = lancamento;
    }

    private static LancamentoCriacaoDto Criar(int faturaId, int categoriaId)
    {
        return new LancamentoCriacaoDto
        {
            Descricao = "Lancamento de teste",
            Valor = 100.00m,
            DataCompra = new DateOnly(2026, 09, 01),
            Estabelecimento = "Estabelecimento de teste",
            Responsavel = "Responsável de teste",
            TotalParcelas = 1,
            FaturaId = faturaId,
            CategoriaId = categoriaId
        };
    }
}
