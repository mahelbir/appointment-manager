using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations;

public class ClientConfiguration : IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        builder.ToTable("Clients");

        builder.Property(c => c.Id).HasColumnName("Id").IsRequired();
        builder.Property(c => c.CreatedDate).HasColumnName("CreatedDate").IsRequired();
        builder.Property(c => c.UpdatedDate).HasColumnName("UpdatedDate");
        builder.Property(c => c.DeletedDate).HasColumnName("DeletedDate");
        builder.Property(c => c.FirstName).HasColumnName("FirstName").IsRequired();
        builder.Property(c => c.LastName).HasColumnName("LastName").IsRequired();
        builder.Property(c => c.Contact).HasColumnName("Contact").IsRequired();

        builder.HasMany(c => c.Appointments).WithOne(a => a.Client).HasForeignKey(a => a.ClientId);

        builder.HasQueryFilter(c => !c.DeletedDate.HasValue);
    }
}