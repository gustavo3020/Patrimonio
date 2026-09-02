using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Patrimonio.Financas.Domain.Categorias.Entities;
using Patrimonio.Financas.Domain.Contas.Entities;
using Patrimonio.Financas.Domain.Movimentacoes.Entities;

namespace Patrimonio.Financas.Infrastructure.Movimentacoes.Configuration;

/// <summary>
/// Configura o mapeamento da entidade <see cref="Movimentacao"/> para o banco de dados.
/// </summary>
internal sealed class MovimentacaoConfiguration : IEntityTypeConfiguration<Movimentacao>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<Movimentacao> builder)
    {
        builder.ToTable("Movimentacoes");

        builder.HasKey(m => m.Id);

        builder.ComplexProperty(m => m.Valor, valor =>
        {
            valor.Property(d => d.Valor)
                 .HasColumnName("Valor")
                 .HasPrecision(18, 2)
                 .IsRequired();
        });

        builder.Property(m => m.Descricao)
               .HasMaxLength(500);

        builder.HasOne<Categoria>()
               .WithMany()
               .HasForeignKey(m => m.CategoriaId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Conta>()
               .WithMany()
               .HasForeignKey(m => m.ContaId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
