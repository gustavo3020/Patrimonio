using Patrimonio.Financas.Contracts.Movimentacoes.Dtos;
using Patrimonio.Financas.Contracts.Movimentacoes.Services;
using Patrimonio.Financas.Domain.Exceptions;
using Patrimonio.Financas.SharedKernel.Movimentacoes.Enums;
using Patrimonio.Financas.Tests.Integration.Base;

namespace Patrimonio.Financas.Tests.Integration.Seeders;

/// <summary>
/// Fornece os dados base de movimentações necessários aos testes de integração.
/// </summary>
internal sealed class MovimentacaoSeeder(
    IMovimentacaoCommandService commandService,
    IMovimentacaoQueryService queryService)
{
    public async Task SeedAsync(BaseData data)
    {
        MovimentacaoDetalheDto movimentacao;

        try
        {
            movimentacao = await queryService.ObterPorIdAsync(1, CancellationToken.None);
        }
        catch (RecursoNaoEncontradoException)
        {
            var movimentacaoDto = Criar(data.Categoria.Id, data.Conta.Id);

            movimentacao = await commandService.CriarAsync(movimentacaoDto, CancellationToken.None);
        }

        data.Movimentacao = movimentacao;
    }

    private static MovimentacaoCriacaoDto Criar(int categoriaId, int contaId)
    {
        return new MovimentacaoCriacaoDto
        {
            Data = DateOnly.FromDateTime(DateTime.Now),
            Valor = 100.00m,
            Natureza = Natureza.Entrada,
            Tipo = TipoMovimentacao.Credito,
            Descricao = "Movimentação de teste",
            CategoriaId = categoriaId,
            ContaId = contaId
        };
    }
}
