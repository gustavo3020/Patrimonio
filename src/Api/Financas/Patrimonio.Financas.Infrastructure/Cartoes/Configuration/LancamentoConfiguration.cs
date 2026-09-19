using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Patrimonio.Financas.Domain.Cartoes.Entities;
using Patrimonio.Financas.Domain.Categorias.Entities;

namespace Patrimonio.Financas.Infrastructure.Cartoes.Configuration;

/// <summary>
/// Configura o mapeamento da entidade <see cref="Lancamento"/> para o banco de dados.
/// </summary>
internal sealed class LancamentoConfiguration : IEntityTypeConfiguration<Lancamento>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<Lancamento> builder)
    {
        builder.ToTable("Lancamentos");

        builder.HasKey(l => l.Id);

        builder.ComplexProperty(l => l.Descricao, descricao =>
        {
            descricao.Property(d => d.Valor)
                     .HasColumnName("Descricao")
                     .HasMaxLength(500)
                     .IsRequired();
        });

        builder.ComplexProperty(l => l.Valor, valor =>
        {
            valor.Property(d => d.Valor)
                 .HasColumnName("Valor")
                 .HasPrecision(18, 2)
                 .IsRequired();
        });

        builder.ComplexProperty(l => l.Estabelecimento, estabelecimento =>
        {
            estabelecimento.Property(e => e.Valor)
                          .HasColumnName("Estabelecimento")
                          .HasMaxLength(150)
                          .IsRequired();
        });

        builder.ComplexProperty(l => l.Responsavel, responsavel =>
        {
            responsavel.Property(r => r.Valor)
                       .HasColumnName("Responsavel")
                       .HasMaxLength(100)
                       .IsRequired();
        });

        builder.OwnsOne(l => l.Parcelamento, parcelamento =>
        {
            parcelamento.Property(p => p.GrupoId)
                        .HasColumnName("GrupoId")
                        .IsRequired();

            parcelamento.Property(p => p.NumeroParcela)
                        .HasColumnName("NumeroParcela")
                        .IsRequired();

            parcelamento.Property(p => p.TotalParcelas)
                        .HasColumnName("TotalParcelas")
                        .IsRequired();
        });

        builder.HasOne<Fatura>()
               .WithMany()
               .HasForeignKey(l => l.FaturaId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Categoria>()
               .WithMany()
               .HasForeignKey(l => l.CategoriaId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
