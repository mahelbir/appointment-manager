using Domain.Enums;
using NArchitecture.Core.Persistence.Repositories;

namespace Domain.Entities;

public class Appointment : Entity<int>
{
    public int ClientId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public AppointmentStatus Status { get; set; }
    public string CalendarEventId { get; set; }
    
    public virtual Client? Client { get; set; }
}