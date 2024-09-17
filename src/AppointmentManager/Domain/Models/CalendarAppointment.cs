using Domain.Enums;

namespace Domain.Models;

public class CalendarAppointment
{
    public int Id { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public AppointmentStatus Status { get; set; }
    public AppointmentStatusProps Props { get; set; }
}