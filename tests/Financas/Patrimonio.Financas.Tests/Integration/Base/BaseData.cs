using Patrimonio.Financas.Contracts.Cartoes.Dtos;
using Patrimonio.Financas.Contracts.Categorias.Dtos;
using Patrimonio.Financas.Contracts.Contas.Dtos;
using Patrimonio.Financas.Contracts.Instituicoes.Dtos;
using Patrimonio.Financas.Contracts.Movimentacoes.Dtos;

namespace Patrimonio.Financas.Tests.Integration.Base;

/// <summary>
/// Contém os dados base utilizados como pré-condições pelos testes de integração.
/// </summary>
internal sealed class BaseData
{
    public CategoriaDetalheDto Categoria { get; set; } = null!;
    public CartaoDetalheDto Cartao { get; set; } = null!;
    public ContaDetalheDto Conta { get; set; } = null!;
    public FaturaDetalheDto Fatura { get; set; } = null!;
    public InstituicaoDetalheDto Instituicao { get; set; } = null!;
    public LancamentoDetalheDto Lancamento { get; set; } = null!;
    public MovimentacaoDetalheDto Movimentacao { get; set; } = null!;
}
