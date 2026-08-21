using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Patrimonio.Financas.Domain.Categorias.Entities;

namespace Patrimonio.Financas.Infrastructure.Categorias.Configuration;

/// <summary>
/// Configura o mapeamento da entidade <see cref="Categoria"/> para o banco de dados.
/// </summary>
internal sealed class CategoriaConfiguration : IEntityTypeConfiguration<Categoria>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<Categoria> builder)
    {
        builder.ToTable("Categorias");

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
    }
}
