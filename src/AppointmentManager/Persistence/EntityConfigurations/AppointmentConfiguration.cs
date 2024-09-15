using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations;

public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
{
    public void Configure(EntityTypeBuilder<Appointment> builder)
    {
        builder.ToTable("Appointments").HasKey(a => a.Id);

        builder.Property(a => a.Id).HasColumnName("Id").IsRequired();
        builder.Property(a => a.CreatedDate).HasColumnName("CreatedDate").IsRequired();
        builder.Property(a => a.UpdatedDate).HasColumnName("UpdatedDate");
        builder.Property(a => a.DeletedDate).HasColumnName("DeletedDate");
        builder.Property(a => a.ClientId).HasColumnName("ClientId").IsRequired();
        builder.Property(a => a.StartDate).HasColumnName("StartDate").IsRequired();
        builder.Property(a => a.EndDate).HasColumnName("EndDate").IsRequired();
        builder.Property(a => a.Status).HasColumnName("Status").IsRequired();
        builder.Property(a => a.CalendarEventId).HasColumnName("CalendarEventId").IsRequired();
        
        builder.HasIndex(a => a.CalendarEventId).IsUnique();

        builder.HasOne(a => a.Client).WithMany(c => c.Appointments).HasForeignKey(a => a.ClientId);

        builder.HasQueryFilter(a => !a.DeletedDate.HasValue);
    }
}