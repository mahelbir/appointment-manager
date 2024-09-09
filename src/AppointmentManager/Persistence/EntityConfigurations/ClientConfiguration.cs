using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations;

public class ClientConfiguration : IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        builder.ToTable("Clients");

        builder.Property(p => p.Id).HasColumnName("Id").IsRequired();
        builder.Property(p => p.CreatedDate).HasColumnName("CreatedDate").IsRequired();
        builder.Property(p => p.UpdatedDate).HasColumnName("UpdatedDate");
        builder.Property(p => p.DeletedDate).HasColumnName("DeletedDate");
        builder.Property(p => p.FirstName).HasColumnName("FirstName").IsRequired();
        builder.Property(p => p.LastName).HasColumnName("LastName").IsRequired();
        builder.Property(p => p.Contact).HasColumnName("Contact").IsRequired();

        builder.HasMany(c => c.Appointments).WithOne(a => a.Client).HasForeignKey(a => a.ClientId);

        builder.HasQueryFilter(p => !p.DeletedDate.HasValue);
    }
}