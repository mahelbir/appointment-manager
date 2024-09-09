using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations;

public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
{
    public void Configure(EntityTypeBuilder<Appointment> builder)
    {
        builder.ToTable("Appointments");
        
        builder.Property(p => p.Id).HasColumnName("Id").IsRequired();
        builder.Property(p => p.CreatedDate).HasColumnName("CreatedDate").IsRequired();
        builder.Property(p => p.CreatedDate).HasColumnName("UpdatedDate");
        builder.Property(p => p.CreatedDate).HasColumnName("DeletedDate");
        builder.Property(p => p.StartDate).HasColumnName("StartDate").IsRequired();
        builder.Property(p => p.EndDate).HasColumnName("EndDate").IsRequired();

        builder.HasQueryFilter(p => !p.DeletedDate.HasValue);
    }
}