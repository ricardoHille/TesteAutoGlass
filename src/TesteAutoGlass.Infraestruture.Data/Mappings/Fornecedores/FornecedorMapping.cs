using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TesteAutoGlass.Fornecedores.Domain.Entities;

namespace TesteAutoGlass.Infraestruture.Data.Mappings.Fornecedores
{
    public class FornecedorMapping : IEntityTypeConfiguration<Fornecedor>
    {
        public void Configure(EntityTypeBuilder<Fornecedor> builder)
        {
            builder.HasIndex(c => c.Id);

            builder.Property(c => c.Codigo)
                .HasDefaultValueSql("NEXT VALUE FOR FornecedorCodigoSequencial");

            builder.Property(c => c.Cnpj)
                .IsRequired()
                .HasColumnType("varchar(14)");

            builder.Property(c => c.Nome)
                .IsRequired()
                .HasColumnType("varchar(100)");

            builder.Property(c => c.Ativo)
                .IsRequired()
                .HasDefaultValue(true);

            builder.HasIndex(c => new { c.Codigo }).IsUnique();

            builder.ToTable("Fornecedor");
        }
    }
}
