using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Patrimonio.Financas.Domain.Cartoes.Entities;
using Patrimonio.Financas.Domain.Instituicoes.Entities;

namespace Patrimonio.Financas.Infrastructure.Cartoes.Configuration;

/// <summary>
/// Configura o mapeamento da entidade <see cref="Cartao"/> para o banco de dados.
/// </summary>
internal sealed class CartaoConfiguration : IEntityTypeConfiguration<Cartao>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<Cartao> builder)
    {
        builder.ToTable("Cartoes");

        builder.HasKey(c => c.Id);

        builder.OwnsOne(c => c.Nome, nome =>
        {
            nome.Property(n => n.Valor)
                .HasColumnName("Nome")
                .HasMaxLength(200);

            nome.Property(n => n.Normalizado)
                .HasColumnName("NomeNormalizado")
                .HasMaxLength(200);

            nome.HasIndex(n => n.Normalizado)
                .IsUnique();
        });

        builder.ComplexProperty(c => c.Limite, valor =>
        {
            valor.Property(d => d.Valor)
                 .HasColumnName("Limite")
                 .HasPrecision(18, 2)
                 .IsRequired();
        });

        builder.ComplexProperty(c => c.DiaFechamento, valor =>
        {
            valor.Property(d => d.Valor)
                 .HasColumnName("DiaFechamento")
                 .IsRequired();
        });

        builder.ComplexProperty(c => c.DiaVencimento, valor =>
        {
            valor.Property(d => d.Valor)
                 .HasColumnName("DiaVencimento")
                 .IsRequired();
        });

        builder.HasOne<Instituicao>()
               .WithMany()
               .HasForeignKey(c => c.InstituicaoId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
