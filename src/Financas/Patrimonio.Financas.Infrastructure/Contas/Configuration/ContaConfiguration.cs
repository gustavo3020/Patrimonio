using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Patrimonio.Financas.Domain.Contas.Entities;
using Patrimonio.Financas.Domain.Instituicoes.Entities;

namespace Patrimonio.Financas.Infrastructure.Contas.Configuration;

/// <summary>
/// Configura o mapeamento da entidade <see cref="Conta"/> para o banco de dados.
/// </summary>
internal sealed class ContaConfiguration : IEntityTypeConfiguration<Conta>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<Conta> builder)
    {
        builder.ToTable("Contas");

        builder.HasKey(c => c.Id);

        builder.ComplexProperty(c => c.Nome, nome =>
        {
            nome.Property(c => c.Valor)
                .HasColumnName("Nome")
                .HasMaxLength(200)
                .IsRequired();

            nome.Property(c => c.Normalizado)
                .HasColumnName("NomeNormalizado")
                .HasMaxLength(200)
                .IsRequired();
        });

        builder.HasIndex(c => c.Nome.Normalizado)
               .IsUnique();

        builder.HasOne<Instituicao>()
               .WithMany()
               .HasForeignKey(c => c.InstituicaoId)
               .IsRequired();
    }
}
